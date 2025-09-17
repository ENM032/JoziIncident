using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ST10091324_PROG7312_Part1.Models;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Monitors database health, performance metrics, and connection status
    /// </summary>
    public class DatabaseHealthMonitor : IDisposable
    {
        private readonly Timer _healthCheckTimer;
        private readonly object _lockObject = new object();
        private bool _isDisposed = false;
        
        // Health metrics
        private readonly List<HealthCheckResult> _healthHistory;
        private readonly Dictionary<string, PerformanceCounter> _performanceCounters;
        
        // Configuration
        public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
        public int MaxHistoryEntries { get; set; } = 100;
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);
        
        // Events
        public event EventHandler<DatabaseHealthEventArgs> HealthStatusChanged;
        public event EventHandler<PerformanceAlertEventArgs> PerformanceAlert;
        
        public DatabaseHealthMonitor()
        {
            _healthHistory = new List<HealthCheckResult>();
            _performanceCounters = new Dictionary<string, PerformanceCounter>();
            
            InitializePerformanceCounters();
            
            // Start health check timer
            _healthCheckTimer = new Timer(PerformHealthCheck, null, 
                TimeSpan.Zero, HealthCheckInterval);
        }
        
        /// <summary>
        /// Gets the current health status of the database
        /// </summary>
        public DatabaseHealthStatus CurrentStatus { get; private set; } = DatabaseHealthStatus.Unknown;
        
        /// <summary>
        /// Gets the latest health check result
        /// </summary>
        public HealthCheckResult LatestHealthCheck
        {
            get
            {
                lock (_lockObject)
                {
                    return _healthHistory.LastOrDefault();
                }
            }
        }
        
        /// <summary>
        /// Gets health history for analysis
        /// </summary>
        public IReadOnlyList<HealthCheckResult> GetHealthHistory(int count = 10)
        {
            lock (_lockObject)
            {
                return _healthHistory.TakeLast(count).ToList().AsReadOnly();
            }
        }
        
        /// <summary>
        /// Performs a comprehensive health check
        /// </summary>
        public async Task<HealthCheckResult> PerformHealthCheckAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new HealthCheckResult
            {
                Timestamp = DateTime.UtcNow,
                CheckId = Guid.NewGuid().ToString()
            };
            
            try
            {
                // Test database connectivity
                await TestDatabaseConnectivity(result);
                
                // Check query performance
                await CheckQueryPerformance(result);
                
                // Monitor connection pool
                CheckConnectionPool(result);
                
                // Check disk space and resources
                CheckSystemResources(result);
                
                // Determine overall health status
                result.OverallStatus = DetermineOverallStatus(result);
                result.ResponseTime = stopwatch.Elapsed;
                
                // Update current status
                var previousStatus = CurrentStatus;
                CurrentStatus = result.OverallStatus;
                
                // Raise event if status changed
                if (previousStatus != CurrentStatus)
                {
                    HealthStatusChanged?.Invoke(this, new DatabaseHealthEventArgs
                    {
                        PreviousStatus = previousStatus,
                        CurrentStatus = CurrentStatus,
                        HealthResult = result
                    });
                }
                
                // Check for performance alerts
                CheckPerformanceAlerts(result);
            }
            catch (Exception ex)
            {
                result.OverallStatus = DatabaseHealthStatus.Critical;
                result.Errors.Add($"Health check failed: {ex.Message}");
                result.ResponseTime = stopwatch.Elapsed;
            }
            
            // Store result in history
            lock (_lockObject)
            {
                _healthHistory.Add(result);
                
                // Maintain history size limit
                while (_healthHistory.Count > MaxHistoryEntries)
                {
                    _healthHistory.RemoveAt(0);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Tests basic database connectivity
        /// </summary>
        private async Task TestDatabaseConnectivity(HealthCheckResult result)
        {
            var connectivityStopwatch = Stopwatch.StartNew();
            
            try
            {
                using (var context = new JoziIncidentContext())
                {
                    context.Database.CommandTimeout = (int)ConnectionTimeout.TotalSeconds;
                    
                    // Simple connectivity test
                    var canConnect = await context.Database.Connection.OpenAsync()
                        .ContinueWith(t => !t.IsFaulted, TaskContinuationOptions.ExecuteSynchronously);
                    
                    if (canConnect)
                    {
                        result.DatabaseConnectivity = DatabaseHealthStatus.Healthy;
                        result.ConnectionTime = connectivityStopwatch.Elapsed;
                        
                        // Test a simple query
                        var userCount = await context.Users.CountAsync();
                        result.Metrics["UserCount"] = userCount;
                    }
                    else
                    {
                        result.DatabaseConnectivity = DatabaseHealthStatus.Unhealthy;
                        result.Errors.Add("Failed to establish database connection");
                    }
                }
            }
            catch (Exception ex)
            {
                result.DatabaseConnectivity = DatabaseHealthStatus.Critical;
                result.Errors.Add($"Database connectivity error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Checks query performance with sample operations
        /// </summary>
        private async Task CheckQueryPerformance(HealthCheckResult result)
        {
            try
            {
                using (var context = new JoziIncidentContext())
                {
                    var performanceStopwatch = Stopwatch.StartNew();
                    
                    // Test various query types
                    var tasks = new List<Task<TimeSpan>>
                    {
                        MeasureQueryTime(() => context.Users.Take(10).ToListAsync()),
                        MeasureQueryTime(() => context.ServiceRequests.Take(10).ToListAsync()),
                        MeasureQueryTime(() => context.LocalEvents.Take(10).ToListAsync())
                    };
                    
                    var queryTimes = await Task.WhenAll(tasks);
                    var averageQueryTime = TimeSpan.FromMilliseconds(queryTimes.Average(t => t.TotalMilliseconds));
                    
                    result.AverageQueryTime = averageQueryTime;
                    result.Metrics["AverageQueryTimeMs"] = averageQueryTime.TotalMilliseconds;
                    
                    // Determine query performance status
                    if (averageQueryTime.TotalMilliseconds < 100)
                        result.QueryPerformance = DatabaseHealthStatus.Healthy;
                    else if (averageQueryTime.TotalMilliseconds < 500)
                        result.QueryPerformance = DatabaseHealthStatus.Warning;
                    else
                        result.QueryPerformance = DatabaseHealthStatus.Unhealthy;
                }
            }
            catch (Exception ex)
            {
                result.QueryPerformance = DatabaseHealthStatus.Critical;
                result.Errors.Add($"Query performance check failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Measures the execution time of a query
        /// </summary>
        private async Task<TimeSpan> MeasureQueryTime<T>(Func<Task<T>> queryFunc)
        {
            var stopwatch = Stopwatch.StartNew();
            await queryFunc();
            return stopwatch.Elapsed;
        }
        
        /// <summary>
        /// Checks connection pool status
        /// </summary>
        private void CheckConnectionPool(HealthCheckResult result)
        {
            try
            {
                // Get connection pool metrics if available
                if (_performanceCounters.TryGetValue("ConnectionPool", out var counter))
                {
                    var poolSize = counter.NextValue();
                    result.Metrics["ConnectionPoolSize"] = poolSize;
                    
                    // Determine pool health based on usage
                    if (poolSize < 80) // Less than 80% utilization
                        result.ConnectionPoolStatus = DatabaseHealthStatus.Healthy;
                    else if (poolSize < 95)
                        result.ConnectionPoolStatus = DatabaseHealthStatus.Warning;
                    else
                        result.ConnectionPoolStatus = DatabaseHealthStatus.Unhealthy;
                }
                else
                {
                    result.ConnectionPoolStatus = DatabaseHealthStatus.Unknown;
                }
            }
            catch (Exception ex)
            {
                result.ConnectionPoolStatus = DatabaseHealthStatus.Unknown;
                result.Warnings.Add($"Connection pool check failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Checks system resources affecting database performance
        /// </summary>
        private void CheckSystemResources(HealthCheckResult result)
        {
            try
            {
                // Check memory usage
                var totalMemory = GC.GetTotalMemory(false);
                result.Metrics["MemoryUsageBytes"] = totalMemory;
                
                // Check CPU usage if performance counter is available
                if (_performanceCounters.TryGetValue("CPU", out var cpuCounter))
                {
                    var cpuUsage = cpuCounter.NextValue();
                    result.Metrics["CPUUsagePercent"] = cpuUsage;
                    
                    if (cpuUsage < 70)
                        result.SystemResourceStatus = DatabaseHealthStatus.Healthy;
                    else if (cpuUsage < 90)
                        result.SystemResourceStatus = DatabaseHealthStatus.Warning;
                    else
                        result.SystemResourceStatus = DatabaseHealthStatus.Unhealthy;
                }
                else
                {
                    result.SystemResourceStatus = DatabaseHealthStatus.Unknown;
                }
            }
            catch (Exception ex)
            {
                result.SystemResourceStatus = DatabaseHealthStatus.Unknown;
                result.Warnings.Add($"System resource check failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Determines overall health status based on individual checks
        /// </summary>
        private DatabaseHealthStatus DetermineOverallStatus(HealthCheckResult result)
        {
            var statuses = new[]
            {
                result.DatabaseConnectivity,
                result.QueryPerformance,
                result.ConnectionPoolStatus,
                result.SystemResourceStatus
            };
            
            if (statuses.Any(s => s == DatabaseHealthStatus.Critical))
                return DatabaseHealthStatus.Critical;
            
            if (statuses.Any(s => s == DatabaseHealthStatus.Unhealthy))
                return DatabaseHealthStatus.Unhealthy;
            
            if (statuses.Any(s => s == DatabaseHealthStatus.Warning))
                return DatabaseHealthStatus.Warning;
            
            if (statuses.All(s => s == DatabaseHealthStatus.Healthy))
                return DatabaseHealthStatus.Healthy;
            
            return DatabaseHealthStatus.Unknown;
        }
        
        /// <summary>
        /// Checks for performance alerts and raises events
        /// </summary>
        private void CheckPerformanceAlerts(HealthCheckResult result)
        {
            var alerts = new List<string>();
            
            // Check response time alert
            if (result.ResponseTime.TotalSeconds > 10)
            {
                alerts.Add($"Health check took {result.ResponseTime.TotalSeconds:F2} seconds");
            }
            
            // Check query performance alert
            if (result.AverageQueryTime.TotalMilliseconds > 1000)
            {
                alerts.Add($"Average query time is {result.AverageQueryTime.TotalMilliseconds:F0}ms");
            }
            
            // Check memory usage alert
            if (result.Metrics.TryGetValue("MemoryUsageBytes", out var memoryUsage) && 
                memoryUsage > 500 * 1024 * 1024) // 500MB
            {
                alerts.Add($"High memory usage: {memoryUsage / (1024 * 1024):F0}MB");
            }
            
            // Raise performance alert if any issues found
            if (alerts.Any())
            {
                PerformanceAlert?.Invoke(this, new PerformanceAlertEventArgs
                {
                    Timestamp = DateTime.UtcNow,
                    Alerts = alerts,
                    HealthResult = result
                });
            }
        }
        
        /// <summary>
        /// Initializes performance counters for monitoring
        /// </summary>
        private void InitializePerformanceCounters()
        {
            try
            {
                // Initialize CPU counter
                _performanceCounters["CPU"] = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                
                // Initialize other counters as available
                // Note: Some counters may not be available in all environments
            }
            catch (Exception ex)
            {
                // Performance counters may not be available in all environments
                System.Diagnostics.Debug.WriteLine($"Performance counter initialization failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Timer callback for periodic health checks
        /// </summary>
        private async void PerformHealthCheck(object state)
        {
            try
            {
                await PerformHealthCheckAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Scheduled health check failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Disposes the health monitor and releases resources
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _healthCheckTimer?.Dispose();
                
                foreach (var counter in _performanceCounters.Values)
                {
                    counter?.Dispose();
                }
                
                _performanceCounters.Clear();
                _isDisposed = true;
            }
        }
    }
    
    /// <summary>
    /// Database health status enumeration
    /// </summary>
    public enum DatabaseHealthStatus
    {
        Unknown,
        Healthy,
        Warning,
        Unhealthy,
        Critical
    }
    
    /// <summary>
    /// Health check result containing detailed metrics
    /// </summary>
    public class HealthCheckResult
    {
        public string CheckId { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public DatabaseHealthStatus OverallStatus { get; set; }
        
        // Individual component statuses
        public DatabaseHealthStatus DatabaseConnectivity { get; set; }
        public DatabaseHealthStatus QueryPerformance { get; set; }
        public DatabaseHealthStatus ConnectionPoolStatus { get; set; }
        public DatabaseHealthStatus SystemResourceStatus { get; set; }
        
        // Performance metrics
        public TimeSpan ConnectionTime { get; set; }
        public TimeSpan AverageQueryTime { get; set; }
        public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();
        
        // Issues and warnings
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
    
    /// <summary>
    /// Event arguments for database health status changes
    /// </summary>
    public class DatabaseHealthEventArgs : EventArgs
    {
        public DatabaseHealthStatus PreviousStatus { get; set; }
        public DatabaseHealthStatus CurrentStatus { get; set; }
        public HealthCheckResult HealthResult { get; set; }
    }
    
    /// <summary>
    /// Event arguments for performance alerts
    /// </summary>
    public class PerformanceAlertEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; }
        public List<string> Alerts { get; set; }
        public HealthCheckResult HealthResult { get; set; }
    }
}