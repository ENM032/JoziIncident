using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ST10091324_PROG7312_Part1.Model
{
    /// <summary>
    /// Enhanced DbContext with connection pooling and performance optimizations
    /// </summary>
    public class UserDbContext : DbContext
    {
        private static readonly object ConfigLock = new object();
        private static bool _databaseInitialized = false;

        /// <summary>
        /// Constructor with optimized configuration for async operations and connection pooling
        /// </summary>
        public UserDbContext() : base("name=UserDbContext")
        {
            InitializeDatabase();
            ConfigureContext();
        }

        /// <summary>
        /// Constructor with custom connection string for testing scenarios
        /// </summary>
        /// <param name="connectionString">Custom connection string</param>
        public UserDbContext(string connectionString) : base(connectionString)
        {
            ConfigureContext();
        }

        private void ConfigureContext()
        {
            // Optimize for async operations
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true; // Enable for write operations
            Configuration.ValidateOnSaveEnabled = true;
            
            // Connection pooling optimizations
            Database.CommandTimeout = 30; // 30 seconds timeout
            
            // Enable async operations
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 30;
        }

        private void InitializeDatabase()
        {
            if (!_databaseInitialized)
            {
                lock (ConfigLock)
                {
                    if (!_databaseInitialized)
                    {
                        // Ensure database is created and migrations are applied
                        Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserDbContext, Migrations.Configuration>());
                        _databaseInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Configure context for read-only operations
        /// </summary>
        public void ConfigureForReadOnly()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// Configure context for write operations
        /// </summary>
        public void ConfigureForWrite()
        {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
        }

        // DbSet represents the "Users" table in the database
        public DbSet<User> Users { get; set; }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<LocalEvent> LocalEvents { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure indexes for performance optimization
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .HasName("IX_Users_Email");
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .HasName("IX_Users_Username");
                
            modelBuilder.Entity<ServiceRequest>()
                .HasIndex(sr => sr.UserId)
                .HasName("IX_ServiceRequests_UserId");
                
            modelBuilder.Entity<Incident>()
                .HasIndex(i => i.UserId)
                .HasName("IX_Incidents_UserId");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Ensure proper cleanup of resources
                SaveChanges();
            }
            base.Dispose(disposing);
        }
    }
}
