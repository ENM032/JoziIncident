using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ST10091324_PROG7312_Part1.Model;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Provides real-time data synchronization and automatic updates for the application
    /// </summary>
    public class RealTimeDataSyncService : INotifyPropertyChanged, IDisposable
    {
        private readonly Timer _syncTimer;
        private readonly object _lockObject = new object();
        private readonly Dictionary<Type, DateTime> _lastSyncTimes;
        private readonly CacheService _cacheService;
        private readonly PerformanceMonitor _performanceMonitor;
        private bool _isDisposed = false;
        
        // Events for real-time notifications
        public event EventHandler<DataSyncEventArgs> DataUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        
        // Sync intervals (in milliseconds)
        private const int DEFAULT_SYNC_INTERVAL = 30000; // 30 seconds
        private const int FAST_SYNC_INTERVAL = 5000;     // 5 seconds for critical data
        
        public bool IsSyncEnabled { get; private set; }
        public DateTime LastSyncTime { get; private set; }
        public int SyncInterval { get; set; } = DEFAULT_SYNC_INTERVAL;
        
        public RealTimeDataSyncService(CacheService cacheService, PerformanceMonitor performanceMonitor)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _performanceMonitor = performanceMonitor ?? throw new ArgumentNullException(nameof(performanceMonitor));
            _lastSyncTimes = new Dictionary<Type, DateTime>();
            
            // Initialize timer for periodic sync
            _syncTimer = new Timer(OnSyncTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        }
        
        /// <summary>
        /// Starts the real-time synchronization service
        /// </summary>
        public void StartSync()
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(RealTimeDataSyncService));
            
            lock (_lockObject)
            {
                if (!IsSyncEnabled)
                {
                    IsSyncEnabled = true;
                    _syncTimer.Change(SyncInterval, SyncInterval);
                    OnPropertyChanged(nameof(IsSyncEnabled));
                }
            }
        }
        
        /// <summary>
        /// Stops the real-time synchronization service
        /// </summary>
        public void StopSync()
        {
            if (_isDisposed) return;
            
            lock (_lockObject)
            {
                if (IsSyncEnabled)
                {
                    IsSyncEnabled = false;
                    _syncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    OnPropertyChanged(nameof(IsSyncEnabled));
                }
            }
        }
        
        /// <summary>
        /// Manually triggers a synchronization for all data types
        /// </summary>
        public async Task SyncAllDataAsync()
        {
            if (_isDisposed) return;
            
            var syncTasks = new List<Task>
            {
                SyncUsersAsync(),
                SyncServiceRequestsAsync(),
                SyncIncidentsAsync(),
                SyncLocalEventsAsync()
            };
            
            await Task.WhenAll(syncTasks).ConfigureAwait(false);
            
            LastSyncTime = DateTime.UtcNow;
            OnPropertyChanged(nameof(LastSyncTime));
        }
        
        /// <summary>
        /// Synchronizes user data and detects changes
        /// </summary>
        public async Task SyncUsersAsync()
        {
            if (_isDisposed) return;
            
            var stopwatch = _performanceMonitor.StartOperation("SyncUsers");
            
            try
            {
                using (var context = DbContextFactory.CreateContext())
                {
                    var lastSync = GetLastSyncTime<User>();
                    
                    // Get users modified since last sync
                    var modifiedUsers = await context.Users
                        .Where(u => u.LastModified > lastSync)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    
                    if (modifiedUsers.Any())
                    {
                        // Update cache
                        foreach (var user in modifiedUsers)
                        {
                            _cacheService.Set($"user_{user.Id}", user, TimeSpan.FromMinutes(30));
                        }
                        
                        // Notify subscribers
                        OnDataUpdated(new DataSyncEventArgs
                        {
                            DataType = typeof(User),
                            UpdatedItems = modifiedUsers.Cast<object>().ToList(),
                            SyncTime = DateTime.UtcNow
                        });
                    }
                    
                    UpdateLastSyncTime<User>();
                }
            }
            catch (Exception ex)
            {
                _performanceMonitor.RecordError("SyncUsers", ex);
                throw;
            }
            finally
            {
                _performanceMonitor.EndOperation(stopwatch);
            }
        }
        
        /// <summary>
        /// Synchronizes service request data and detects changes
        /// </summary>
        public async Task SyncServiceRequestsAsync()
        {
            if (_isDisposed) return;
            
            var stopwatch = _performanceMonitor.StartOperation("SyncServiceRequests");
            
            try
            {
                using (var context = DbContextFactory.CreateContext())
                {
                    var lastSync = GetLastSyncTime<ServiceRequest>();
                    
                    // Get service requests modified since last sync
                    var modifiedRequests = await context.ServiceRequests
                        .Where(sr => sr.LastModified > lastSync)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    
                    if (modifiedRequests.Any())
                    {
                        // Update cache with invalidation
                        _cacheService.InvalidatePattern("servicerequest_*");
                        
                        foreach (var request in modifiedRequests)
                        {
                            _cacheService.Set($"servicerequest_{request.Id}", request, TimeSpan.FromMinutes(15));
                        }
                        
                        // Notify subscribers
                        OnDataUpdated(new DataSyncEventArgs
                        {
                            DataType = typeof(ServiceRequest),
                            UpdatedItems = modifiedRequests.Cast<object>().ToList(),
                            SyncTime = DateTime.UtcNow
                        });
                    }
                    
                    UpdateLastSyncTime<ServiceRequest>();
                }
            }
            catch (Exception ex)
            {
                _performanceMonitor.RecordError("SyncServiceRequests", ex);
                throw;
            }
            finally
            {
                _performanceMonitor.EndOperation(stopwatch);
            }
        }
        
        /// <summary>
        /// Synchronizes incident data and detects changes
        /// </summary>
        public async Task SyncIncidentsAsync()
        {
            if (_isDisposed) return;
            
            var stopwatch = _performanceMonitor.StartOperation("SyncIncidents");
            
            try
            {
                using (var context = DbContextFactory.CreateContext())
                {
                    var lastSync = GetLastSyncTime<Incident>();
                    
                    // Get incidents modified since last sync
                    var modifiedIncidents = await context.Incidents
                        .Where(i => i.LastModified > lastSync)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    
                    if (modifiedIncidents.Any())
                    {
                        // Update cache
                        foreach (var incident in modifiedIncidents)
                        {
                            _cacheService.Set($"incident_{incident.Id}", incident, TimeSpan.FromMinutes(20));
                        }
                        
                        // Notify subscribers
                        OnDataUpdated(new DataSyncEventArgs
                        {
                            DataType = typeof(Incident),
                            UpdatedItems = modifiedIncidents.Cast<object>().ToList(),
                            SyncTime = DateTime.UtcNow
                        });
                    }
                    
                    UpdateLastSyncTime<Incident>();
                }
            }
            catch (Exception ex)
            {
                _performanceMonitor.RecordError("SyncIncidents", ex);
                throw;
            }
            finally
            {
                _performanceMonitor.EndOperation(stopwatch);
            }
        }
        
        /// <summary>
        /// Synchronizes local event data and detects changes
        /// </summary>
        public async Task SyncLocalEventsAsync()
        {
            if (_isDisposed) return;
            
            var stopwatch = _performanceMonitor.StartOperation("SyncLocalEvents");
            
            try
            {
                using (var context = DbContextFactory.CreateContext())
                {
                    var lastSync = GetLastSyncTime<LocalEvent>();
                    
                    // Get local events modified since last sync
                    var modifiedEvents = await context.LocalEvents
                        .Where(le => le.LastModified > lastSync)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    
                    if (modifiedEvents.Any())
                    {
                        // Update cache
                        foreach (var localEvent in modifiedEvents)
                        {
                            _cacheService.Set($"localevent_{localEvent.Id}", localEvent, TimeSpan.FromMinutes(60));
                        }
                        
                        // Notify subscribers
                        OnDataUpdated(new DataSyncEventArgs
                        {
                            DataType = typeof(LocalEvent),
                            UpdatedItems = modifiedEvents.Cast<object>().ToList(),
                            SyncTime = DateTime.UtcNow
                        });
                    }
                    
                    UpdateLastSyncTime<LocalEvent>();
                }
            }
            catch (Exception ex)
            {
                _performanceMonitor.RecordError("SyncLocalEvents", ex);
                throw;
            }
            finally
            {
                _performanceMonitor.EndOperation(stopwatch);
            }
        }
        
        private void OnSyncTimerElapsed(object state)
        {
            if (_isDisposed || !IsSyncEnabled) return;
            
            // Run sync on background thread
            Task.Run(async () =>
            {
                try
                {
                    await SyncAllDataAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _performanceMonitor.RecordError("AutoSync", ex);
                }
            });
        }
        
        private DateTime GetLastSyncTime<T>()
        {
            return _lastSyncTimes.TryGetValue(typeof(T), out var lastSync) 
                ? lastSync 
                : DateTime.MinValue;
        }
        
        private void UpdateLastSyncTime<T>()
        {
            _lastSyncTimes[typeof(T)] = DateTime.UtcNow;
        }
        
        private void OnDataUpdated(DataSyncEventArgs args)
        {
            DataUpdated?.Invoke(this, args);
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public void Dispose()
        {
            if (!_isDisposed)
            {
                StopSync();
                _syncTimer?.Dispose();
                _isDisposed = true;
            }
        }
    }
    
    /// <summary>
    /// Event arguments for data synchronization events
    /// </summary>
    public class DataSyncEventArgs : EventArgs
    {
        public Type DataType { get; set; }
        public List<object> UpdatedItems { get; set; }
        public DateTime SyncTime { get; set; }
    }
}