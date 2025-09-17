using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Comprehensive error handling and retry policy implementation for database operations
    /// </summary>
    public class DatabaseErrorHandler
    {
        private const int DefaultMaxRetryAttempts = 3;
        private const int DefaultBaseDelayMs = 1000;
        private static readonly Random Random = new Random();

        /// <summary>
        /// Executes a database operation with retry policy for transient failures
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operation">Database operation to execute</param>
        /// <param name="maxRetryAttempts">Maximum number of retry attempts</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Operation result</returns>
        public static async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetryAttempts = DefaultMaxRetryAttempts,
            CancellationToken cancellationToken = default)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            Exception lastException = null;

            for (int attempt = 0; attempt <= maxRetryAttempts; attempt++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return await operation().ConfigureAwait(false);
                }
                catch (Exception ex) when (IsTransientError(ex) && attempt < maxRetryAttempts)
                {
                    lastException = ex;
                    var delay = CalculateDelay(attempt);
                    
                    // Log retry attempt (implement logging as needed)
                    LogRetryAttempt(attempt + 1, maxRetryAttempts, delay, ex);
                    
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Non-transient error or max retries exceeded
                    throw new DatabaseOperationException(
                        $"Database operation failed after {attempt + 1} attempts", ex);
                }
            }

            // This should never be reached, but included for completeness
            throw new DatabaseOperationException(
                $"Database operation failed after {maxRetryAttempts + 1} attempts", lastException);
        }

        /// <summary>
        /// Executes a database operation without return value with retry policy
        /// </summary>
        /// <param name="operation">Database operation to execute</param>
        /// <param name="maxRetryAttempts">Maximum number of retry attempts</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static async Task ExecuteWithRetryAsync(
            Func<Task> operation,
            int maxRetryAttempts = DefaultMaxRetryAttempts,
            CancellationToken cancellationToken = default)
        {
            await ExecuteWithRetryAsync(async () =>
            {
                await operation().ConfigureAwait(false);
                return true; // Dummy return value
            }, maxRetryAttempts, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines if an exception represents a transient error that should be retried
        /// </summary>
        /// <param name="exception">Exception to evaluate</param>
        /// <returns>True if the error is transient and should be retried</returns>
        private static bool IsTransientError(Exception exception)
        {
            // SQL Server transient error codes
            if (exception is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2:     // Timeout
                    case 53:    // Network path not found
                    case 121:   // Semaphore timeout
                    case 1205:  // Deadlock
                    case 1222:  // Lock request timeout
                    case 8645:  // Timeout waiting for memory resource
                    case 8651:  // Low memory condition
                    case 20:    // Instance not available
                    case 64:    // Connection failed
                    case 233:   // Connection forcibly closed
                    case 10053: // Connection aborted
                    case 10054: // Connection reset
                    case 10060: // Connection timeout
                    case 40197: // Service busy
                    case 40501: // Service busy
                    case 40613: // Database unavailable
                        return true;
                }
            }

            // Entity Framework specific transient errors
            if (exception is EntityException ||
                exception is TimeoutException ||
                exception is TaskCanceledException)
            {
                return true;
            }

            // Check inner exceptions
            if (exception.InnerException != null)
            {
                return IsTransientError(exception.InnerException);
            }

            return false;
        }

        /// <summary>
        /// Calculates delay for retry attempt using exponential backoff with jitter
        /// </summary>
        /// <param name="attempt">Current attempt number (0-based)</param>
        /// <returns>Delay in milliseconds</returns>
        private static TimeSpan CalculateDelay(int attempt)
        {
            // Exponential backoff: 1s, 2s, 4s, 8s, etc.
            var exponentialDelay = DefaultBaseDelayMs * Math.Pow(2, attempt);
            
            // Add jitter to prevent thundering herd
            var jitter = Random.NextDouble() * 0.1 * exponentialDelay; // ±10% jitter
            
            var totalDelay = exponentialDelay + jitter;
            
            // Cap at 30 seconds
            return TimeSpan.FromMilliseconds(Math.Min(totalDelay, 30000));
        }

        /// <summary>
        /// Logs retry attempt information
        /// </summary>
        /// <param name="currentAttempt">Current attempt number</param>
        /// <param name="maxAttempts">Maximum attempts</param>
        /// <param name="delay">Delay before next attempt</param>
        /// <param name="exception">Exception that caused the retry</param>
        private static void LogRetryAttempt(int currentAttempt, int maxAttempts, TimeSpan delay, Exception exception)
        {
            // TODO: Implement proper logging framework
            // For now, we'll use console output for debugging
            Console.WriteLine($"Database operation failed (attempt {currentAttempt}/{maxAttempts + 1}). " +
                            $"Retrying in {delay.TotalSeconds:F1}s. Error: {exception.Message}");
        }

        /// <summary>
        /// Wraps an exception with additional context for database operations
        /// </summary>
        /// <param name="operation">Name of the operation that failed</param>
        /// <param name="originalException">Original exception</param>
        /// <returns>Wrapped exception with context</returns>
        public static DatabaseOperationException WrapException(string operation, Exception originalException)
        {
            var message = $"Database operation '{operation}' failed: {originalException.Message}";
            return new DatabaseOperationException(message, originalException);
        }

        /// <summary>
        /// Validates database connection and throws appropriate exception if invalid
        /// </summary>
        /// <param name="context">Database context to validate</param>
        public static void ValidateConnection(UserDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                // Test connection by accessing database
                var canConnect = context.Database.Exists();
                if (!canConnect)
                {
                    throw new DatabaseOperationException("Database connection is not available");
                }
            }
            catch (Exception ex) when (!(ex is DatabaseOperationException))
            {
                throw new DatabaseOperationException("Failed to validate database connection", ex);
            }
        }
    }

    /// <summary>
    /// Exception types for categorizing database errors
    /// </summary>
    public enum DatabaseErrorType
    {
        ConnectionFailure,
        TimeoutError,
        DeadlockError,
        ValidationError,
        ConcurrencyError,
        UnknownError
    }

    /// <summary>
    /// Enhanced database operation exception with error categorization
    /// </summary>
    public class DatabaseOperationException : Exception
    {
        public DatabaseErrorType ErrorType { get; }
        public bool IsTransient { get; }

        public DatabaseOperationException(string message) : base(message)
        {
            ErrorType = DatabaseErrorType.UnknownError;
            IsTransient = false;
        }

        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorType = DetermineErrorType(innerException);
            IsTransient = DatabaseErrorHandler.IsTransientError(innerException);
        }

        public DatabaseOperationException(string message, DatabaseErrorType errorType, bool isTransient = false) : base(message)
        {
            ErrorType = errorType;
            IsTransient = isTransient;
        }

        private static DatabaseErrorType DetermineErrorType(Exception exception)
        {
            if (exception is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2:
                    case 121:
                    case 8645:
                        return DatabaseErrorType.TimeoutError;
                    case 1205:
                        return DatabaseErrorType.DeadlockError;
                    case 2627:
                    case 2601:
                        return DatabaseErrorType.ValidationError;
                    case 53:
                    case 20:
                    case 64:
                        return DatabaseErrorType.ConnectionFailure;
                    default:
                        return DatabaseErrorType.UnknownError;
                }
            }

            if (exception is TimeoutException)
                return DatabaseErrorType.TimeoutError;

            if (exception is EntityException)
                return DatabaseErrorType.ConnectionFailure;

            return DatabaseErrorType.UnknownError;
        }
    }
}