using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Performance monitoring service for tracking database operations and system metrics
    /// </summary>
    public class PerformanceMonitor : IDisposable
    {
        private readonly ConcurrentQueue<PerformanceMetric> _metrics;
        private readonly Timer _reportingTimer;
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

        // Performance counters
        private long _totalOperations = 0;
        private long _successfulOperations = 0;
        private long _failedOperations = 0;
        private readonly ConcurrentDictionary<string, OperationStats> _operationStats;

        public PerformanceMonitor(string logDirectory = null)
        {
            _metrics = new ConcurrentQueue<PerformanceMetric>();
            _operationStats = new ConcurrentDictionary<string, OperationStats>();
            
            // Set up log file path
            var baseDirectory = logDirectory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(baseDirectory);
            _logFilePath = Path.Combine(baseDirectory, $"performance_{DateTime.Now:yyyyMMdd}.log");

            // Report metrics every 5 minutes
            _reportingTimer = new Timer(ReportMetrics, null, 
                TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Tracks execution time and result of a database operation
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="operation">Operation to execute and monitor</param>
        /// <returns>Operation result</returns>
        public async Task<T> TrackOperationAsync<T>(string operationName, Func<Task<T>> operation)
        {
            if (string.IsNullOrEmpty(operationName))
                throw new ArgumentException("Operation name cannot be null or empty", nameof(operationName));

            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var stopwatch = Stopwatch.StartNew();
            var startTime = DateTime.UtcNow;
            Exception operationException = null;
            T result = default(T);

            try
            {
                result = await operation().ConfigureAwait(false);
                Interlocked.Increment(ref _successfulOperations);
                return result;
            }
            catch (Exception ex)
            {
                operationException = ex;
                Interlocked.Increment(ref _failedOperations);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Interlocked.Increment(ref _totalOperations);

                var metric = new PerformanceMetric
                {
                    OperationName = operationName,
                    StartTime = startTime,
                    Duration = stopwatch.Elapsed,
                    Success = operationException == null,
                    ErrorMessage = operationException?.Message,
                    MemoryUsage = GC.GetTotalMemory(false)
                };

                _metrics.Enqueue(metric);
                UpdateOperationStats(operationName, stopwatch.Elapsed, operationException == null);

                // Log slow operations immediately
                if (stopwatch.ElapsedMilliseconds > 5000) // 5 seconds threshold
                {
                    LogSlowOperation(metric);
                }
            }
        }

        /// <summary>
        /// Tracks execution time of a void operation
        /// </summary>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="operation">Operation to execute and monitor</param>
        public async Task TrackOperationAsync(string operationName, Func<Task> operation)
        {
            await TrackOperationAsync(operationName, async () =>
            {
                await operation().ConfigureAwait(false);
                return true; // Dummy return value
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Records a custom metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="value">Metric value</param>
        /// <param name="unit">Unit of measurement</param>
        public void RecordMetric(string metricName, double value, string unit = null)
        {
            var metric = new PerformanceMetric
            {
                OperationName = metricName,
                StartTime = DateTime.UtcNow,
                Duration = TimeSpan.Zero,
                Success = true,
                CustomValue = value,
                Unit = unit
            };

            _metrics.Enqueue(metric);
        }

        /// <summary>
        /// Gets current performance statistics
        /// </summary>
        /// <returns>Performance statistics</returns>
        public PerformanceStatistics GetStatistics()
        {
            var totalOps = Interlocked.Read(ref _totalOperations);
            var successfulOps = Interlocked.Read(ref _successfulOperations);
            var failedOps = Interlocked.Read(ref _failedOperations);

            var recentMetrics = GetRecentMetrics(TimeSpan.FromMinutes(15));
            var avgResponseTime = recentMetrics.Any() 
                ? recentMetrics.Average(m => m.Duration.TotalMilliseconds) 
                : 0;

            var slowOperations = recentMetrics
                .Where(m => m.Duration.TotalMilliseconds > 1000)
                .Count();

            return new PerformanceStatistics
            {
                TotalOperations = totalOps,
                SuccessfulOperations = successfulOps,
                FailedOperations = failedOps,
                SuccessRate = totalOps > 0 ? (double)successfulOps / totalOps * 100 : 0,
                AverageResponseTime = avgResponseTime,
                SlowOperationsCount = slowOperations,
                CurrentMemoryUsage = GC.GetTotalMemory(false),
                OperationBreakdown = _operationStats.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone())
            };
        }

        /// <summary>
        /// Gets metrics from a specific time period
        /// </summary>
        /// <param name="timeSpan">Time period to look back</param>
        /// <returns>Metrics from the specified period</returns>
        public List<PerformanceMetric> GetRecentMetrics(TimeSpan timeSpan)
        {
            var cutoffTime = DateTime.UtcNow.Subtract(timeSpan);
            return _metrics.Where(m => m.StartTime >= cutoffTime).ToList();
        }

        /// <summary>
        /// Gets the slowest operations in the specified time period
        /// </summary>
        /// <param name="timeSpan">Time period to analyze</param>
        /// <param name="count">Number of slowest operations to return</param>
        /// <returns>Slowest operations</returns>
        public List<PerformanceMetric> GetSlowestOperations(TimeSpan timeSpan, int count = 10)
        {
            return GetRecentMetrics(timeSpan)
                .OrderByDescending(m => m.Duration)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Updates operation statistics
        /// </summary>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="duration">Operation duration</param>
        /// <param name="success">Whether operation was successful</param>
        private void UpdateOperationStats(string operationName, TimeSpan duration, bool success)
        {
            _operationStats.AddOrUpdate(operationName,
                new OperationStats(operationName, duration, success),
                (key, existing) =>
                {
                    existing.AddExecution(duration, success);
                    return existing;
                });
        }

        /// <summary>
        /// Logs slow operations immediately
        /// </summary>
        /// <param name="metric">Performance metric for slow operation</param>
        private void LogSlowOperation(PerformanceMetric metric)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SLOW OPERATION: {metric.OperationName} " +
                          $"took {metric.Duration.TotalMilliseconds:F0}ms";
            
            if (!metric.Success)
            {
                logEntry += $" and FAILED: {metric.ErrorMessage}";
            }

            WriteToLog(logEntry);
        }

        /// <summary>
        /// Timer callback for periodic metric reporting
        /// </summary>
        /// <param name="state">Timer state</param>
        private void ReportMetrics(object state)
        {
            try
            {
                var stats = GetStatistics();
                var report = GeneratePerformanceReport(stats);
                WriteToLog(report);

                // Clean up old metrics (keep only last 24 hours)
                CleanupOldMetrics();
            }
            catch (Exception ex)
            {
                WriteToLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR generating performance report: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a formatted performance report
        /// </summary>
        /// <param name="stats">Performance statistics</param>
        /// <returns>Formatted report string</returns>
        private string GeneratePerformanceReport(PerformanceStatistics stats)
        {
            var report = $"\n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] PERFORMANCE REPORT\n" +
                        $"Total Operations: {stats.TotalOperations}\n" +
                        $"Success Rate: {stats.SuccessRate:F1}%\n" +
                        $"Average Response Time: {stats.AverageResponseTime:F0}ms\n" +
                        $"Slow Operations (>1s): {stats.SlowOperationsCount}\n" +
                        $"Memory Usage: {stats.CurrentMemoryUsage / 1024 / 1024:F1} MB\n";

            if (stats.OperationBreakdown.Any())
            {
                report += "\nOperation Breakdown:\n";
                foreach (var op in stats.OperationBreakdown.OrderByDescending(kvp => kvp.Value.TotalExecutions))
                {
                    report += $"  {op.Key}: {op.Value.TotalExecutions} calls, " +
                             $"avg {op.Value.AverageResponseTime:F0}ms, " +
                             $"success rate {op.Value.SuccessRate:F1}%\n";
                }
            }

            return report;
        }

        /// <summary>
        /// Removes metrics older than 24 hours
        /// </summary>
        private void CleanupOldMetrics()
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-24);
            var metricsToKeep = new ConcurrentQueue<PerformanceMetric>();

            while (_metrics.TryDequeue(out var metric))
            {
                if (metric.StartTime >= cutoffTime)
                {
                    metricsToKeep.Enqueue(metric);
                }
            }

            // Replace the queue with cleaned metrics
            while (metricsToKeep.TryDequeue(out var metric))
            {
                _metrics.Enqueue(metric);
            }
        }

        /// <summary>
        /// Writes log entry to file
        /// </summary>
        /// <param name="logEntry">Log entry to write</param>
        private void WriteToLog(string logEntry)
        {
            try
            {
                lock (_lockObject)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Fallback to console if file logging fails
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine(logEntry);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _reportingTimer?.Dispose();
                
                // Final report before disposal
                try
                {
                    var finalStats = GetStatistics();
                    var finalReport = GeneratePerformanceReport(finalStats);
                    WriteToLog($"\n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FINAL PERFORMANCE REPORT (Application Shutdown)");
                    WriteToLog(finalReport);
                }
                catch (Exception ex)
                {
                    WriteToLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR generating final report: {ex.Message}");
                }

                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Individual performance metric data
    /// </summary>
    public class PerformanceMetric
    {
        public string OperationName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public long MemoryUsage { get; set; }
        public double? CustomValue { get; set; }
        public string Unit { get; set; }
    }

    /// <summary>
    /// Aggregated performance statistics
    /// </summary>
    public class PerformanceStatistics
    {
        public long TotalOperations { get; set; }
        public long SuccessfulOperations { get; set; }
        public long FailedOperations { get; set; }
        public double SuccessRate { get; set; }
        public double AverageResponseTime { get; set; }
        public int SlowOperationsCount { get; set; }
        public long CurrentMemoryUsage { get; set; }
        public Dictionary<string, OperationStats> OperationBreakdown { get; set; }
    }

    /// <summary>
    /// Statistics for individual operation types
    /// </summary>
    public class OperationStats
    {
        public string OperationName { get; }
        public int TotalExecutions { get; private set; }
        public int SuccessfulExecutions { get; private set; }
        public int FailedExecutions { get; private set; }
        public double AverageResponseTime { get; private set; }
        public double SuccessRate => TotalExecutions > 0 ? (double)SuccessfulExecutions / TotalExecutions * 100 : 0;
        public TimeSpan MinResponseTime { get; private set; } = TimeSpan.MaxValue;
        public TimeSpan MaxResponseTime { get; private set; } = TimeSpan.MinValue;

        private readonly object _lockObject = new object();
        private double _totalResponseTime = 0;

        public OperationStats(string operationName, TimeSpan initialDuration, bool initialSuccess)
        {
            OperationName = operationName;
            AddExecution(initialDuration, initialSuccess);
        }

        public void AddExecution(TimeSpan duration, bool success)
        {
            lock (_lockObject)
            {
                TotalExecutions++;
                if (success)
                    SuccessfulExecutions++;
                else
                    FailedExecutions++;

                _totalResponseTime += duration.TotalMilliseconds;
                AverageResponseTime = _totalResponseTime / TotalExecutions;

                if (duration < MinResponseTime)
                    MinResponseTime = duration;
                if (duration > MaxResponseTime)
                    MaxResponseTime = duration;
            }
        }

        public OperationStats Clone()
        {
            lock (_lockObject)
            {
                return new OperationStats(OperationName, TimeSpan.Zero, true)
                {
                    TotalExecutions = this.TotalExecutions,
                    SuccessfulExecutions = this.SuccessfulExecutions,
                    FailedExecutions = this.FailedExecutions,
                    AverageResponseTime = this.AverageResponseTime,
                    MinResponseTime = this.MinResponseTime,
                    MaxResponseTime = this.MaxResponseTime,
                    _totalResponseTime = this._totalResponseTime
                };
            }
        }
    }
}