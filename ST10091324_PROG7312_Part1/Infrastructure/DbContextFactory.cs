using System;
using System.Data.Entity;
using ST10091324_PROG7312_Part1.Model;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Factory for creating and managing DbContext instances with proper lifecycle management
    /// </summary>
    public class DbContextFactory : IDisposable
    {
        private static readonly object LockObject = new object();
        private static DbContextFactory _instance;
        private bool _disposed = false;

        /// <summary>
        /// Singleton instance for centralized context management
        /// </summary>
        public static DbContextFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new DbContextFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        private DbContextFactory()
        {
            // Private constructor for singleton pattern
        }

        /// <summary>
        /// Creates a new UserDbContext with optimized configuration
        /// </summary>
        /// <returns>Configured UserDbContext instance</returns>
        public UserDbContext CreateContext()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DbContextFactory));
            }

            var context = new UserDbContext();
            
            // Configure for optimal performance and async operations
            context.Database.CommandTimeout = 30; // 30 seconds timeout
            
            return context;
        }

        /// <summary>
        /// Creates a context specifically for read-only operations
        /// </summary>
        /// <returns>Read-only optimized UserDbContext</returns>
        public UserDbContext CreateReadOnlyContext()
        {
            var context = CreateContext();
            
            // Additional optimizations for read-only scenarios
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            
            return context;
        }

        /// <summary>
        /// Executes a database operation with proper context lifecycle management
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operation">Database operation to execute</param>
        /// <returns>Operation result</returns>
        public async Task<T> ExecuteAsync<T>(Func<UserDbContext, Task<T>> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            using (var context = CreateContext())
            {
                try
                {
                    return await operation(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Log the exception (implement logging as needed)
                    throw new DatabaseOperationException("Database operation failed", ex);
                }
            }
        }

        /// <summary>
        /// Executes a database operation without return value
        /// </summary>
        /// <param name="operation">Database operation to execute</param>
        public async Task ExecuteAsync(Func<UserDbContext, Task> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            using (var context = CreateContext())
            {
                try
                {
                    await operation(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Log the exception (implement logging as needed)
                    throw new DatabaseOperationException("Database operation failed", ex);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _instance = null;
            }
        }
    }

    /// <summary>
    /// Custom exception for database operation failures
    /// </summary>
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }
        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}