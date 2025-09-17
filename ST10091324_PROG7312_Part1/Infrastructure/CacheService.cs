using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ST10091324_PROG7312_Part1.Model;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Provides advanced caching functionality with invalidation strategies for improved performance
    /// </summary>
    public class CacheService : IDisposable
    {
        private readonly MemoryCache _cache;
        private readonly ConcurrentDictionary<string, DateTime> _cacheMetrics;
        private readonly ConcurrentDictionary<string, List<string>> _taggedKeys;
        private readonly ConcurrentDictionary<string, CacheItemPolicy> _policies;
        private readonly Timer _cleanupTimer;
        private readonly object _lockObject = new object();
        private bool _isDisposed = false;
        
        // Cache statistics (using fields for thread-safe operations)
        private int _cacheHits;
        private int _cacheMisses;
        private int _cacheEvictions;
        
        public int CacheHits => _cacheHits;
        public int CacheMisses => _cacheMisses;
        public int CacheEvictions => _cacheEvictions;
        public double HitRatio => CacheHits + CacheMisses > 0 ? (double)CacheHits / (CacheHits + CacheMisses) : 0;
        public long TotalMemoryUsage => GC.GetTotalMemory(false);
        
        // Cache configuration
        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
        public long MaxMemoryUsage { get; set; } = 100 * 1024 * 1024; // 100MB
        
        public CacheService()
        {
            _cache = MemoryCache.Default;
            _cacheMetrics = new ConcurrentDictionary<string, DateTime>();
            _taggedKeys = new ConcurrentDictionary<string, List<string>>();
            _policies = new ConcurrentDictionary<string, CacheItemPolicy>();
            
            // Setup cleanup timer to run every 5 minutes
            _cleanupTimer = new Timer(PerformCleanup, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Gets cached value or executes factory function if not found
        /// </summary>
        /// <typeparam name="T">Type of cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Function to create value if not cached</param>
        /// <param name="ttl">Time to live for cached item</param>
        /// <returns>Cached or newly created value</returns>
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Cache key cannot be null or empty", nameof(key));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            // Try to get from cache first
            if (_cache.TryGetValue(key, out var cacheItem) && !cacheItem.IsExpired)
            {
                cacheItem.UpdateLastAccessed();
                return (T)cacheItem.Value;
            }

            // Not in cache or expired, create new value
            var value = await factory().ConfigureAwait(false);
            var expiresAt = DateTime.UtcNow.Add(ttl ?? _defaultTtl);
            
            Set(key, value, expiresAt);
            return value;
        }

        /// <summary>
        /// Gets a cached item asynchronously
        /// </summary>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                Interlocked.Increment(ref _cacheMisses);
                return null;
            }

            var item = _cache.Get(key) as T;
            if (item != null)
            {
                Interlocked.Increment(ref _cacheHits);
                _cacheMetrics.TryAdd(key, DateTime.UtcNow);
                return item;
            }
            
            Interlocked.Increment(ref _cacheMisses);
            return null;
        }
        
        /// <summary>
        /// Gets a cached item synchronously
        /// </summary>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                Interlocked.Increment(ref _cacheMisses);
                return null;
            }

            var item = _cache.Get(key) as T;
            if (item != null)
            {
                Interlocked.Increment(ref _cacheHits);
                _cacheMetrics.TryAdd(key, DateTime.UtcNow);
                return item;
            }
            
            Interlocked.Increment(ref _cacheMisses);
            return null;
        }

        /// <summary>
        /// Sets a cached item asynchronously with optional tags and custom expiration
        /// </summary>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, params string[] tags) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.Add(expiration ?? DefaultExpiration),
                Priority = CacheItemPriority.Default,
                RemovedCallback = OnCacheItemRemoved
            };

            _cache.Set(key, value, policy);
            _policies.TryAdd(key, policy);
            _cacheMetrics.TryAdd(key, DateTime.UtcNow);
            
            // Handle tags for cache invalidation
            if (tags != null && tags.Length > 0)
            {
                foreach (var tag in tags)
                {
                    _taggedKeys.AddOrUpdate(tag, new List<string> { key }, (k, v) => 
                    {
                        if (!v.Contains(key))
                            v.Add(key);
                        return v;
                    });
                }
            }
        }
        
        /// <summary>
        /// Sets a cached item synchronously with optional tags and custom expiration
        /// </summary>
        public void Set<T>(string key, T value, TimeSpan? expiration = null, params string[] tags) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.Add(expiration ?? DefaultExpiration),
                Priority = CacheItemPriority.Default,
                RemovedCallback = OnCacheItemRemoved
            };

            _cache.Set(key, value, policy);
            _policies.TryAdd(key, policy);
            _cacheMetrics.TryAdd(key, DateTime.UtcNow);
            
            // Handle tags for cache invalidation
            if (tags != null && tags.Length > 0)
            {
                foreach (var tag in tags)
                {
                    _taggedKeys.AddOrUpdate(tag, new List<string> { key }, (k, v) => 
                    {
                        if (!v.Contains(key))
                            v.Add(key);
                        return v;
                    });
                }
            }
        }

        /// <summary>
        /// Removes a specific item from cache
        /// </summary>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;

            _cache.Remove(key);
            _cacheMetrics.TryRemove(key, out _);
            _policies.TryRemove(key, out _);
            
            // Remove from tagged keys
            foreach (var taggedKey in _taggedKeys)
            {
                taggedKey.Value.Remove(key);
            }
        }
        
        /// <summary>
        /// Invalidates cache entries by tag
        /// </summary>
        public void InvalidateByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return;
                
            if (_taggedKeys.TryGetValue(tag, out var keys))
            {
                foreach (var key in keys.ToList())
                {
                    Remove(key);
                }
                _taggedKeys.TryRemove(tag, out _);
            }
        }
        
        /// <summary>
        /// Invalidates cache entries matching a pattern
        /// </summary>
        public void InvalidateByPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return;
                
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var keysToRemove = _cacheMetrics.Keys.Where(key => regex.IsMatch(key)).ToList();
            
            foreach (var key in keysToRemove)
            {
                Remove(key);
            }
        }
        
        /// <summary>
        /// Clears all cache entries
        /// </summary>
        public void Clear()
        {
            foreach (var key in _cacheMetrics.Keys.ToList())
            {
                _cache.Remove(key);
            }
            
            _cacheMetrics.Clear();
            _policies.Clear();
            _taggedKeys.Clear();
            
            // Reset statistics
            _cacheHits = 0;
            _cacheMisses = 0;
            _cacheEvictions = 0;
        }

        /// <summary>
        /// Gets cache statistics
        /// </summary>
        /// <returns>Cache statistics</returns>
        public CacheStatistics GetStatistics()
        {
            var totalItems = _cache.Count;
            var expiredItems = _cache.Values.Count(item => item.IsExpired);
            var activeItems = totalItems - expiredItems;

            return new CacheStatistics
            {
                TotalItems = totalItems,
                ActiveItems = activeItems,
                ExpiredItems = expiredItems,
                MaxCacheSize = _maxCacheSize,
                CacheUtilization = totalItems / (double)_maxCacheSize
            };
        }

        /// <summary>
        /// Evicts least recently used items to make space
        /// </summary>
        private void EvictLeastRecentlyUsed()
        {
            var itemsToEvict = Math.Max(1, _maxCacheSize / 10); // Evict 10% of cache
            
            var oldestItems = _cache
                .OrderBy(kvp => kvp.Value.LastAccessed)
                .Take(itemsToEvict)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in oldestItems)
            {
                _cache.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// Performs periodic cleanup of expired items and memory management
        /// </summary>
        private void PerformCleanup(object state)
        {
            try
            {
                // Check memory usage and trigger cleanup if needed
                if (TotalMemoryUsage > MaxMemoryUsage)
                {
                    var itemsToRemove = _cacheMetrics
                        .OrderBy(kvp => kvp.Value)
                        .Take(_cacheMetrics.Count / 4) // Remove 25% of oldest items
                        .Select(kvp => kvp.Key)
                        .ToList();
                        
                    foreach (var key in itemsToRemove)
                    {
                        Remove(key);
                        Interlocked.Increment(ref _cacheEvictions);
                    }
                }
                
                // Clean up empty tag collections
                var emptyTags = _taggedKeys
                    .Where(kvp => kvp.Value.Count == 0)
                    .Select(kvp => kvp.Key)
                    .ToList();
                    
                foreach (var tag in emptyTags)
                {
                    _taggedKeys.TryRemove(tag, out _);
                }
                
                // Force garbage collection if memory usage is still high
                if (TotalMemoryUsage > MaxMemoryUsage * 0.8)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                // Log cleanup errors but don't throw
                System.Diagnostics.Debug.WriteLine($"Cache cleanup error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Callback when cache item is removed
        /// </summary>
        private void OnCacheItemRemoved(CacheEntryRemovedArguments arguments)
        {
            _cacheMetrics.TryRemove(arguments.CacheItem.Key, out _);
            _policies.TryRemove(arguments.CacheItem.Key, out _);
            
            if (arguments.RemovedReason == CacheEntryRemovedReason.Evicted)
            {
                Interlocked.Increment(ref _cacheEvictions);
            }
        }

        /// <summary>
        /// Gets or sets cache item with automatic expiration
        /// </summary>
        public T GetOrSet<T>(string key, Func<T> factory, TimeSpan? expiration = null, params string[] tags) where T : class
        {
            var item = Get<T>(key);
            if (item != null)
                return item;
                
            var value = factory();
            if (value != null)
            {
                Set(key, value, expiration, tags);
            }
            
            return value;
        }
        
        /// <summary>
        /// Disposes the cache service and releases resources
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _cleanupTimer?.Dispose();
                Clear();
                _isDisposed = true;
            }
        }
    }

    /// <summary>
    /// Cache item wrapper with expiration and access tracking
    /// </summary>
    internal class CacheItem
    {
        public object Value { get; }
        public DateTime ExpiresAt { get; }
        public DateTime LastAccessed { get; private set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public CacheItem(object value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
            LastAccessed = DateTime.UtcNow;
        }

        public void UpdateLastAccessed()
        {
            LastAccessed = DateTime.UtcNow;
        }
    }



    /// <summary>
    /// Specialized cache service for database entities with predefined cache keys
    /// </summary>
    public class EntityCacheService : IDisposable
    {
        private readonly CacheService _cacheService;
        private bool _disposed = false;

        // Cache key constants
        private const string USER_KEY_PREFIX = "user:";
        private const string INCIDENT_KEY_PREFIX = "incident:";
        private const string EVENT_KEY_PREFIX = "event:";
        private const string SERVICE_REQUEST_KEY_PREFIX = "service_request:";
        private const string USER_LIST_KEY = "users:all";
        private const string INCIDENT_LIST_KEY = "incidents:all";
        private const string EVENT_LIST_KEY = "events:all";
        private const string SERVICE_REQUEST_LIST_KEY = "service_requests:all";

        public EntityCacheService()
        {
            _cacheService = new CacheService(maxCacheSize: 2000, defaultTtl: TimeSpan.FromMinutes(15));
        }

        // User caching methods
        public async Task<User> GetUserAsync(int userId, Func<Task<User>> factory)
        {
            return await _cacheService.GetOrSetAsync($"{USER_KEY_PREFIX}{userId}", factory, TimeSpan.FromMinutes(30));
        }

        public async Task<List<User>> GetAllUsersAsync(Func<Task<List<User>>> factory)
        {
            return await _cacheService.GetOrSetAsync(USER_LIST_KEY, factory, TimeSpan.FromMinutes(10));
        }

        public void InvalidateUser(int userId)
        {
            _cacheService.Remove($"{USER_KEY_PREFIX}{userId}");
            _cacheService.Remove(USER_LIST_KEY); // Invalidate list cache too
        }

        // Incident caching methods
        public async Task<Incident> GetIncidentAsync(int incidentId, Func<Task<Incident>> factory)
        {
            return await _cacheService.GetOrSetAsync($"{INCIDENT_KEY_PREFIX}{incidentId}", factory, TimeSpan.FromMinutes(20));
        }

        public async Task<List<Incident>> GetAllIncidentsAsync(Func<Task<List<Incident>>> factory)
        {
            return await _cacheService.GetOrSetAsync(INCIDENT_LIST_KEY, factory, TimeSpan.FromMinutes(5));
        }

        public void InvalidateIncident(int incidentId)
        {
            _cacheService.Remove($"{INCIDENT_KEY_PREFIX}{incidentId}");
            _cacheService.Remove(INCIDENT_LIST_KEY);
        }

        // Local Event caching methods
        public async Task<LocalEvent> GetEventAsync(int eventId, Func<Task<LocalEvent>> factory)
        {
            return await _cacheService.GetOrSetAsync($"{EVENT_KEY_PREFIX}{eventId}", factory, TimeSpan.FromMinutes(25));
        }

        public async Task<List<LocalEvent>> GetAllEventsAsync(Func<Task<List<LocalEvent>>> factory)
        {
            return await _cacheService.GetOrSetAsync(EVENT_LIST_KEY, factory, TimeSpan.FromMinutes(15));
        }

        public void InvalidateEvent(int eventId)
        {
            _cacheService.Remove($"{EVENT_KEY_PREFIX}{eventId}");
            _cacheService.Remove(EVENT_LIST_KEY);
        }

        // Service Request caching methods
        public async Task<ServiceRequest> GetServiceRequestAsync(int serviceRequestId, Func<Task<ServiceRequest>> factory)
        {
            return await _cacheService.GetOrSetAsync($"{SERVICE_REQUEST_KEY_PREFIX}{serviceRequestId}", factory, TimeSpan.FromMinutes(20));
        }

        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync(Func<Task<List<ServiceRequest>>> factory)
        {
            return await _cacheService.GetOrSetAsync(SERVICE_REQUEST_LIST_KEY, factory, TimeSpan.FromMinutes(10));
        }

        public void InvalidateServiceRequest(int serviceRequestId)
        {
            _cacheService.Remove($"{SERVICE_REQUEST_KEY_PREFIX}{serviceRequestId}");
            _cacheService.Remove(SERVICE_REQUEST_LIST_KEY);
        }

        // Bulk invalidation methods
        public void InvalidateAllUsers()
        {
            _cacheService.Remove(USER_LIST_KEY);
            // Could also remove individual user caches if needed
        }

        public void InvalidateAllIncidents()
        {
            _cacheService.Remove(INCIDENT_LIST_KEY);
        }

        public void InvalidateAllEvents()
        {
            _cacheService.Remove(EVENT_LIST_KEY);
        }

        public void InvalidateAllServiceRequests()
        {
            _cacheService.Remove(SERVICE_REQUEST_LIST_KEY);
        }

        public void ClearAllCache()
        {
            _cacheService.Clear();
        }

        public CacheStatistics GetStatistics()
        {
            return _cacheService.GetStatistics();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cacheService?.Dispose();
                _disposed = true;
            }
        }
    }
}