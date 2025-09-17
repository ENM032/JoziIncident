using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ST10091324_PROG7312_Part1.Model;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// In-memory caching service for frequently accessed data with TTL and LRU eviction
    /// </summary>
    public class CacheService : IDisposable
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache;
        private readonly Timer _cleanupTimer;
        private readonly object _lockObject = new object();
        private readonly int _maxCacheSize;
        private readonly TimeSpan _defaultTtl;
        private bool _disposed = false;

        public CacheService(int maxCacheSize = 1000, TimeSpan? defaultTtl = null)
        {
            _cache = new ConcurrentDictionary<string, CacheItem>();
            _maxCacheSize = maxCacheSize;
            _defaultTtl = defaultTtl ?? TimeSpan.FromMinutes(30);
            
            // Cleanup expired items every 5 minutes
            _cleanupTimer = new Timer(CleanupExpiredItems, null, 
                TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
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
        /// Gets cached value synchronously
        /// </summary>
        /// <typeparam name="T">Type of cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or default</returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);

            if (_cache.TryGetValue(key, out var cacheItem) && !cacheItem.IsExpired)
            {
                cacheItem.UpdateLastAccessed();
                return (T)cacheItem.Value;
            }

            return default(T);
        }

        /// <summary>
        /// Sets a value in cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="expiresAt">Expiration time</param>
        public void Set(string key, object value, DateTime? expiresAt = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Cache key cannot be null or empty", nameof(key));

            var expiration = expiresAt ?? DateTime.UtcNow.Add(_defaultTtl);
            var newItem = new CacheItem(value, expiration);

            lock (_lockObject)
            {
                // Check cache size and evict if necessary
                if (_cache.Count >= _maxCacheSize && !_cache.ContainsKey(key))
                {
                    EvictLeastRecentlyUsed();
                }

                _cache.AddOrUpdate(key, newItem, (k, existing) => newItem);
            }
        }

        /// <summary>
        /// Removes item from cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if item was removed</returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// Clears all cached items
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
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
        /// Cleanup timer callback to remove expired items
        /// </summary>
        /// <param name="state">Timer state</param>
        private void CleanupExpiredItems(object state)
        {
            var expiredKeys = _cache
                .Where(kvp => kvp.Value.IsExpired)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cleanupTimer?.Dispose();
                _cache.Clear();
                _disposed = true;
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
    /// Cache statistics for monitoring
    /// </summary>
    public class CacheStatistics
    {
        public int TotalItems { get; set; }
        public int ActiveItems { get; set; }
        public int ExpiredItems { get; set; }
        public int MaxCacheSize { get; set; }
        public double CacheUtilization { get; set; }
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