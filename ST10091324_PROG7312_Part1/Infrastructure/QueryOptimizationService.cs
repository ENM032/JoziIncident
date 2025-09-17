using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ST10091324_PROG7312_Part1.Model;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Service for optimizing Entity Framework queries and providing indexing recommendations
    /// </summary>
    public class QueryOptimizationService
    {
        private readonly PerformanceMonitor _performanceMonitor;
        private readonly List<IndexRecommendation> _indexRecommendations;

        public QueryOptimizationService(PerformanceMonitor performanceMonitor = null)
        {
            _performanceMonitor = performanceMonitor;
            _indexRecommendations = GenerateIndexRecommendations();
        }

        /// <summary>
        /// Optimized query for getting users with pagination and filtering
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="searchTerm">Optional search term for filtering</param>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paginated and filtered users</returns>
        public async Task<PagedResult<User>> GetUsersOptimizedAsync(
            UserDbContext context, 
            string searchTerm = null, 
            int pageNumber = 1, 
            int pageSize = 20)
        {
            if (_performanceMonitor != null)
            {
                return await _performanceMonitor.TrackOperationAsync(
                    "GetUsersOptimized",
                    () => ExecuteGetUsersOptimizedAsync(context, searchTerm, pageNumber, pageSize));
            }

            return await ExecuteGetUsersOptimizedAsync(context, searchTerm, pageNumber, pageSize);
        }

        private async Task<PagedResult<User>> ExecuteGetUsersOptimizedAsync(
            UserDbContext context, 
            string searchTerm, 
            int pageNumber, 
            int pageSize)
        {
            var query = context.Users.AsQueryable();

            // Apply filtering if search term is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // Optimized search using indexed columns
                query = query.Where(u => 
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination with optimized ordering
            var users = await query
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        /// <summary>
        /// Optimized query for getting incidents with related data
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="status">Optional status filter</param>
        /// <param name="categoryId">Optional category filter</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paginated incidents with optimized loading</returns>
        public async Task<PagedResult<Incident>> GetIncidentsOptimizedAsync(
            UserDbContext context,
            string status = null,
            int? categoryId = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            if (_performanceMonitor != null)
            {
                return await _performanceMonitor.TrackOperationAsync(
                    "GetIncidentsOptimized",
                    () => ExecuteGetIncidentsOptimizedAsync(context, status, categoryId, pageNumber, pageSize));
            }

            return await ExecuteGetIncidentsOptimizedAsync(context, status, categoryId, pageNumber, pageSize);
        }

        private async Task<PagedResult<Incident>> ExecuteGetIncidentsOptimizedAsync(
            UserDbContext context,
            string status,
            int? categoryId,
            int pageNumber,
            int pageSize)
        {
            var query = context.Incidents.AsQueryable();

            // Apply filters using indexed columns
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status == status);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination with optimized ordering by indexed date column
            var incidents = await query
                .OrderByDescending(i => i.DateReported)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Incident>
            {
                Items = incidents,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        /// <summary>
        /// Optimized query for getting events by date range
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="startDate">Start date filter</param>
        /// <param name="endDate">End date filter</param>
        /// <param name="category">Optional category filter</param>
        /// <returns>Events in date range</returns>
        public async Task<List<LocalEvent>> GetEventsByDateRangeOptimizedAsync(
            UserDbContext context,
            DateTime startDate,
            DateTime endDate,
            string category = null)
        {
            if (_performanceMonitor != null)
            {
                return await _performanceMonitor.TrackOperationAsync(
                    "GetEventsByDateRangeOptimized",
                    () => ExecuteGetEventsByDateRangeOptimizedAsync(context, startDate, endDate, category));
            }

            return await ExecuteGetEventsByDateRangeOptimizedAsync(context, startDate, endDate, category);
        }

        private async Task<List<LocalEvent>> ExecuteGetEventsByDateRangeOptimizedAsync(
            UserDbContext context,
            DateTime startDate,
            DateTime endDate,
            string category)
        {
            var query = context.LocalEvents
                .Where(e => e.Date >= startDate && e.Date <= endDate);

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }

            return await query
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Optimized query for getting service requests with status tracking
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="userId">Optional user ID filter</param>
        /// <param name="status">Optional status filter</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paginated service requests</returns>
        public async Task<PagedResult<ServiceRequest>> GetServiceRequestsOptimizedAsync(
            UserDbContext context,
            int? userId = null,
            string status = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            if (_performanceMonitor != null)
            {
                return await _performanceMonitor.TrackOperationAsync(
                    "GetServiceRequestsOptimized",
                    () => ExecuteGetServiceRequestsOptimizedAsync(context, userId, status, pageNumber, pageSize));
            }

            return await ExecuteGetServiceRequestsOptimizedAsync(context, userId, status, pageNumber, pageSize);
        }

        private async Task<PagedResult<ServiceRequest>> ExecuteGetServiceRequestsOptimizedAsync(
            UserDbContext context,
            int? userId,
            string status,
            int pageNumber,
            int pageSize)
        {
            var query = context.ServiceRequests.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(sr => sr.UserId == userId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(sr => sr.Status == status);
            }

            var totalCount = await query.CountAsync();

            var serviceRequests = await query
                .OrderByDescending(sr => sr.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ServiceRequest>
            {
                Items = serviceRequests,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        /// <summary>
        /// Gets database indexing recommendations
        /// </summary>
        /// <returns>List of recommended indexes</returns>
        public List<IndexRecommendation> GetIndexRecommendations()
        {
            return _indexRecommendations.ToList();
        }

        /// <summary>
        /// Generates SQL scripts for creating recommended indexes
        /// </summary>
        /// <returns>SQL scripts for index creation</returns>
        public List<string> GenerateIndexCreationScripts()
        {
            var scripts = new List<string>();

            foreach (var recommendation in _indexRecommendations)
            {
                var script = $"CREATE {(recommendation.IsUnique ? "UNIQUE " : "")}INDEX {recommendation.IndexName} " +
                           $"ON {recommendation.TableName} ({string.Join(", ", recommendation.Columns)})";
                
                if (recommendation.IncludeColumns?.Any() == true)
                {
                    script += $" INCLUDE ({string.Join(", ", recommendation.IncludeColumns)})";
                }

                scripts.Add(script);
            }

            return scripts;
        }

        /// <summary>
        /// Analyzes query performance and suggests optimizations
        /// </summary>
        /// <param name="queryName">Name of the query to analyze</param>
        /// <param name="executionTime">Query execution time</param>
        /// <returns>Optimization suggestions</returns>
        public List<string> AnalyzeQueryPerformance(string queryName, TimeSpan executionTime)
        {
            var suggestions = new List<string>();

            if (executionTime.TotalMilliseconds > 5000)
            {
                suggestions.Add("Query execution time exceeds 5 seconds. Consider adding appropriate indexes.");
                suggestions.Add("Review WHERE clause conditions and ensure they use indexed columns.");
                suggestions.Add("Consider implementing pagination for large result sets.");
            }
            else if (executionTime.TotalMilliseconds > 1000)
            {
                suggestions.Add("Query execution time exceeds 1 second. Consider optimization.");
                suggestions.Add("Review JOIN operations and ensure foreign key indexes exist.");
            }

            // Query-specific suggestions
            switch (queryName.ToLower())
            {
                case "getusers":
                case "getusersoptimized":
                    suggestions.Add("Ensure indexes exist on FirstName, LastName, and Email columns for search operations.");
                    break;
                case "getincidents":
                case "getincidentsoptimized":
                    suggestions.Add("Ensure indexes exist on Status, CategoryId, and DateReported columns.");
                    break;
                case "getevents":
                case "geteventsbydaterangeoptimized":
                    suggestions.Add("Ensure index exists on Date column for date range queries.");
                    break;
                case "getservicerequests":
                case "getservicerequestsoptimized":
                    suggestions.Add("Ensure indexes exist on UserId, Status, and RequestDate columns.");
                    break;
            }

            return suggestions;
        }

        /// <summary>
        /// Generates index recommendations based on common query patterns
        /// </summary>
        /// <returns>List of index recommendations</returns>
        private List<IndexRecommendation> GenerateIndexRecommendations()
        {
            return new List<IndexRecommendation>
            {
                // User table indexes
                new IndexRecommendation
                {
                    TableName = "Users",
                    IndexName = "IX_Users_Email",
                    Columns = new[] { "Email" },
                    IsUnique = true,
                    Reason = "Unique constraint and login lookups"
                },
                new IndexRecommendation
                {
                    TableName = "Users",
                    IndexName = "IX_Users_Name_Search",
                    Columns = new[] { "LastName", "FirstName" },
                    Reason = "Name-based searches and sorting"
                },
                new IndexRecommendation
                {
                    TableName = "Users",
                    IndexName = "IX_Users_LastName",
                    Columns = new[] { "LastName" },
                    IncludeColumns = new[] { "FirstName", "Email" },
                    Reason = "Covering index for user listings"
                },

                // Incident table indexes
                new IndexRecommendation
                {
                    TableName = "Incidents",
                    IndexName = "IX_Incidents_Status_DateReported",
                    Columns = new[] { "Status", "DateReported" },
                    Reason = "Status filtering with date sorting"
                },
                new IndexRecommendation
                {
                    TableName = "Incidents",
                    IndexName = "IX_Incidents_CategoryId",
                    Columns = new[] { "CategoryId" },
                    Reason = "Category-based filtering"
                },
                new IndexRecommendation
                {
                    TableName = "Incidents",
                    IndexName = "IX_Incidents_DateReported",
                    Columns = new[] { "DateReported" },
                    Reason = "Date-based sorting and filtering"
                },
                new IndexRecommendation
                {
                    TableName = "Incidents",
                    IndexName = "IX_Incidents_UserId",
                    Columns = new[] { "UserId" },
                    Reason = "User-specific incident lookups"
                },

                // LocalEvent table indexes
                new IndexRecommendation
                {
                    TableName = "LocalEvents",
                    IndexName = "IX_LocalEvents_Date",
                    Columns = new[] { "Date" },
                    Reason = "Date range queries and chronological sorting"
                },
                new IndexRecommendation
                {
                    TableName = "LocalEvents",
                    IndexName = "IX_LocalEvents_Category_Date",
                    Columns = new[] { "Category", "Date" },
                    Reason = "Category filtering with date sorting"
                },
                new IndexRecommendation
                {
                    TableName = "LocalEvents",
                    IndexName = "IX_LocalEvents_Location",
                    Columns = new[] { "Location" },
                    Reason = "Location-based event searches"
                },

                // ServiceRequest table indexes
                new IndexRecommendation
                {
                    TableName = "ServiceRequests",
                    IndexName = "IX_ServiceRequests_UserId_Status",
                    Columns = new[] { "UserId", "Status" },
                    Reason = "User-specific requests with status filtering"
                },
                new IndexRecommendation
                {
                    TableName = "ServiceRequests",
                    IndexName = "IX_ServiceRequests_Status_RequestDate",
                    Columns = new[] { "Status", "RequestDate" },
                    Reason = "Status filtering with date sorting"
                },
                new IndexRecommendation
                {
                    TableName = "ServiceRequests",
                    IndexName = "IX_ServiceRequests_RequestDate",
                    Columns = new[] { "RequestDate" },
                    Reason = "Chronological sorting and date filtering"
                }
            };
        }
    }

    /// <summary>
    /// Represents a paginated result set
    /// </summary>
    /// <typeparam name="T">Type of items in the result</typeparam>
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    /// <summary>
    /// Represents a database index recommendation
    /// </summary>
    public class IndexRecommendation
    {
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public string[] Columns { get; set; }
        public string[] IncludeColumns { get; set; }
        public bool IsUnique { get; set; }
        public string Reason { get; set; }
    }
}