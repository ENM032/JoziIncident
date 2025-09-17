using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Provides high-concurrency optimization services for database operations
    /// </summary>
    public class ConcurrencyOptimizationService : IDisposable
    {
        private readonly ILogger<ConcurrencyOptimizationService> _logger;
        private readonly SemaphoreSlim _connectionSemaphore;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _resourceSemaphores;
        private readonly Channel<BatchOperation> _batchChannel;
        private readonly ChannelWriter<BatchOperation> _batchWriter;
        private readonly ChannelReader<BatchOperation> _batchReader;
        private readonly Timer _batchProcessingTimer;
        private readonly ConcurrentQueue<OperationMetrics> _metricsQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _batchProcessingTask;
        
        private readonly int _maxConcurrentConnections;
        private readonly int _batchSize;
        private readonly TimeSpan _batchTimeout;
        private bool _disposed;
        
        public ConcurrencyOptimizationService(
            ILogger<ConcurrencyOptimizationService> logger,
            int maxConcurrentConnections = 50,
            int batchSize = 100,
            TimeSpan? batchTimeout = null)
        {
            _logger = logger;
            _maxConcurrentConnections = maxConcurrentConnections;
            _batchSize = batchSize;
            _batchTimeout = batchTimeout ?? TimeSpan.FromMilliseconds(500);
            
            _connectionSemaphore = new SemaphoreSlim(maxConcurrentConnections, maxConcurrentConnections);
            _resourceSemaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
            _metricsQueue = new ConcurrentQueue<OperationMetrics>();
            _cancellationTokenSource = new CancellationTokenSource();
            
            // Create channel for batch operations
            var channelOptions = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };
            
            _batchChannel = Channel.CreateBounded<BatchOperation>(channelOptions);
            _batchWriter = _batchChannel.Writer;
            _batchReader = _batchChannel.Reader;
            
            // Start batch processing task
            _batchProcessingTask = Task.Run(ProcessBatchOperationsAsync, _cancellationTokenSource.Token);
            
            // Setup periodic metrics cleanup
            _batchProcessingTimer = new Timer(CleanupMetrics, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }
        
        /// <summary>
        /// Executes an operation with connection throttling
        /// </summary>
        public async Task<T> ExecuteWithThrottlingAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            string resourceKey = null,
            int maxConcurrentForResource = 10,
            CancellationToken cancellationToken = default)
        {
            var startTime = DateTime.UtcNow;
            var operationId = Guid.NewGuid().ToString();
            
            try
            {
                // Acquire connection semaphore
                await _connectionSemaphore.WaitAsync(cancellationToken);
                
                SemaphoreSlim resourceSemaphore = null;
                if (!string.IsNullOrEmpty(resourceKey))
                {
                    resourceSemaphore = _resourceSemaphores.GetOrAdd(
                        resourceKey,
                        _ => new SemaphoreSlim(maxConcurrentForResource, maxConcurrentForResource));
                    
                    await resourceSemaphore.WaitAsync(cancellationToken);
                }
                
                try
                {
                    var result = await operation(cancellationToken);
                    
                    // Record successful operation metrics
                    RecordOperationMetrics(operationId, startTime, true, resourceKey);
                    
                    return result;
                }
                finally
                {
                    resourceSemaphore?.Release();
                }
            }
            catch (Exception ex)
            {
                // Record failed operation metrics
                RecordOperationMetrics(operationId, startTime, false, resourceKey, ex);
                throw;
            }
            finally
            {
                _connectionSemaphore.Release();
            }
        }
        
        /// <summary>
        /// Executes multiple operations in parallel with optimal concurrency
        /// </summary>
        public async Task<T[]> ExecuteParallelAsync<T>(
            IEnumerable<Func<CancellationToken, Task<T>>> operations,
            int? maxDegreeOfParallelism = null,
            CancellationToken cancellationToken = default)
        {
            var operationsList = operations.ToList();
            var maxParallelism = maxDegreeOfParallelism ?? Math.Min(_maxConcurrentConnections, operationsList.Count);
            
            var semaphore = new SemaphoreSlim(maxParallelism, maxParallelism);
            var tasks = operationsList.Select(async operation =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    return await ExecuteWithThrottlingAsync(operation, cancellationToken: cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            
            return await Task.WhenAll(tasks);
        }
        
        /// <summary>
        /// Queues an operation for batch processing
        /// </summary>
        public async Task<T> QueueBatchOperationAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            string batchKey,
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<T>();
            var batchOperation = new BatchOperation
            {
                Id = Guid.NewGuid().ToString(),
                BatchKey = batchKey,
                Operation = async (ct) =>
                {
                    try
                    {
                        var result = await operation(ct);
                        tcs.SetResult(result);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                        throw;
                    }
                },
                QueuedAt = DateTime.UtcNow
            };
            
            await _batchWriter.WriteAsync(batchOperation, cancellationToken);
            return await tcs.Task;
        }
        
        /// <summary>
        /// Executes operations with retry logic and exponential backoff
        /// </summary>
        public async Task<T> ExecuteWithRetryAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            int maxRetries = 3,
            TimeSpan? baseDelay = null,
            CancellationToken cancellationToken = default)
        {
            var delay = baseDelay ?? TimeSpan.FromMilliseconds(100);
            var attempt = 0;
            
            while (attempt <= maxRetries)
            {
                try
                {
                    return await ExecuteWithThrottlingAsync(operation, cancellationToken: cancellationToken);
                }
                catch (Exception ex) when (attempt < maxRetries && IsRetriableException(ex))
                {
                    attempt++;
                    var backoffDelay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                    
                    _logger.LogWarning($"Operation failed (attempt {attempt}/{maxRetries + 1}), retrying in {backoffDelay.TotalMilliseconds}ms. Error: {ex.Message}");
                    
                    await Task.Delay(backoffDelay, cancellationToken);
                }
            }
            
            // Final attempt without retry
            return await ExecuteWithThrottlingAsync(operation, cancellationToken: cancellationToken);
        }
        
        /// <summary>
        /// Gets current concurrency metrics
        /// </summary>
        public ConcurrencyMetrics GetMetrics()
        {
            var recentMetrics = _metricsQueue.Where(m => m.Timestamp > DateTime.UtcNow.AddMinutes(-5)).ToList();
            
            return new ConcurrencyMetrics
            {
                CurrentConnections = _maxConcurrentConnections - _connectionSemaphore.CurrentCount,
                MaxConnections = _maxConcurrentConnections,
                TotalOperations = recentMetrics.Count,
                SuccessfulOperations = recentMetrics.Count(m => m.Success),
                FailedOperations = recentMetrics.Count(m => !m.Success),
                AverageExecutionTime = recentMetrics.Any() ? 
                    TimeSpan.FromMilliseconds(recentMetrics.Average(m => m.ExecutionTime.TotalMilliseconds)) : 
                    TimeSpan.Zero,
                ResourceUtilization = _resourceSemaphores.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new ResourceMetrics
                    {
                        CurrentUsage = kvp.Value.CurrentCount,
                        MaxCapacity = _maxConcurrentConnections // This would be resource-specific in real implementation
                    })
            };
        }
        
        /// <summary>
        /// Processes batch operations
        /// </summary>
        private async Task ProcessBatchOperationsAsync()
        {
            var batchBuffer = new List<BatchOperation>();
            var lastProcessTime = DateTime.UtcNow;
            
            try
            {
                await foreach (var operation in _batchReader.ReadAllAsync(_cancellationTokenSource.Token))
                {
                    batchBuffer.Add(operation);
                    
                    // Process batch if we hit size limit or timeout
                    if (batchBuffer.Count >= _batchSize || 
                        DateTime.UtcNow - lastProcessTime >= _batchTimeout)
                    {
                        await ProcessBatch(batchBuffer);
                        batchBuffer.Clear();
                        lastProcessTime = DateTime.UtcNow;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in batch processing task");
            }
            finally
            {
                // Process remaining operations
                if (batchBuffer.Any())
                {
                    await ProcessBatch(batchBuffer);
                }
            }
        }
        
        /// <summary>
        /// Processes a batch of operations
        /// </summary>
        private async Task ProcessBatch(List<BatchOperation> operations)
        {
            if (!operations.Any()) return;
            
            try
            {
                // Group operations by batch key for optimal processing
                var groupedOperations = operations.GroupBy(op => op.BatchKey);
                
                var batchTasks = groupedOperations.Select(async group =>
                {
                    var groupOperations = group.ToList();
                    _logger.LogDebug($"Processing batch of {groupOperations.Count} operations for key: {group.Key}");
                    
                    // Execute operations in the group with controlled concurrency
                    var tasks = groupOperations.Select(op => 
                        ExecuteWithThrottlingAsync(
                            op.Operation, 
                            resourceKey: op.BatchKey,
                            cancellationToken: _cancellationTokenSource.Token));
                    
                    await Task.WhenAll(tasks);
                });
                
                await Task.WhenAll(batchTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing batch of {operations.Count} operations");
            }
        }
        
        /// <summary>
        /// Records operation metrics
        /// </summary>
        private void RecordOperationMetrics(string operationId, DateTime startTime, bool success, string resourceKey, Exception exception = null)
        {
            var metrics = new OperationMetrics
            {
                OperationId = operationId,
                Timestamp = startTime,
                ExecutionTime = DateTime.UtcNow - startTime,
                Success = success,
                ResourceKey = resourceKey,
                ErrorMessage = exception?.Message
            };
            
            _metricsQueue.Enqueue(metrics);
            
            // Log significant operations
            if (!success || metrics.ExecutionTime > TimeSpan.FromSeconds(5))
            {
                var level = success ? LogLevel.Warning : LogLevel.Error;
                _logger.Log(level, $"Operation {operationId} completed in {metrics.ExecutionTime.TotalMilliseconds}ms. Success: {success}. Resource: {resourceKey}");
            }
        }
        
        /// <summary>
        /// Determines if an exception is retriable
        /// </summary>
        private bool IsRetriableException(Exception ex)
        {
            // Add specific exception types that should be retried
            return ex is TimeoutException ||
                   ex is TaskCanceledException ||
                   (ex.Message?.Contains("timeout", StringComparison.OrdinalIgnoreCase) == true) ||
                   (ex.Message?.Contains("connection", StringComparison.OrdinalIgnoreCase) == true);
        }
        
        /// <summary>
        /// Cleans up old metrics
        /// </summary>
        private void CleanupMetrics(object state)
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-1);
            var metricsToKeep = new List<OperationMetrics>();
            
            while (_metricsQueue.TryDequeue(out var metric))
            {
                if (metric.Timestamp > cutoffTime)
                {
                    metricsToKeep.Add(metric);
                }
            }
            
            foreach (var metric in metricsToKeep)
            {
                _metricsQueue.Enqueue(metric);
            }
        }
        
        /// <summary>
        /// Disposes resources
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            
            try
            {
                _batchWriter?.Complete();
                _cancellationTokenSource?.Cancel();
                
                _batchProcessingTask?.Wait(TimeSpan.FromSeconds(5));
                
                _connectionSemaphore?.Dispose();
                
                foreach (var semaphore in _resourceSemaphores.Values)
                {
                    semaphore?.Dispose();
                }
                
                _batchProcessingTimer?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during ConcurrencyOptimizationService disposal");
            }
        }
    }
    
    /// <summary>
    /// Represents a batch operation
    /// </summary>
    public class BatchOperation
    {
        public string Id { get; set; }
        public string BatchKey { get; set; }
        public Func<CancellationToken, Task<object>> Operation { get; set; }
        public DateTime QueuedAt { get; set; }
    }
    
    /// <summary>
    /// Operation metrics for monitoring
    /// </summary>
    public class OperationMetrics
    {
        public string OperationId { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public bool Success { get; set; }
        public string ResourceKey { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// Concurrency metrics for monitoring
    /// </summary>
    public class ConcurrencyMetrics
    {
        public int CurrentConnections { get; set; }
        public int MaxConnections { get; set; }
        public int TotalOperations { get; set; }
        public int SuccessfulOperations { get; set; }
        public int FailedOperations { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public Dictionary<string, ResourceMetrics> ResourceUtilization { get; set; }
        
        public double SuccessRate => TotalOperations > 0 ? (double)SuccessfulOperations / TotalOperations : 0;
        public double ConnectionUtilization => MaxConnections > 0 ? (double)CurrentConnections / MaxConnections : 0;
    }
    
    /// <summary>
    /// Resource-specific metrics
    /// </summary>
    public class ResourceMetrics
    {
        public int CurrentUsage { get; set; }
        public int MaxCapacity { get; set; }
        public double UtilizationRate => MaxCapacity > 0 ? (double)CurrentUsage / MaxCapacity : 0;
    }
}