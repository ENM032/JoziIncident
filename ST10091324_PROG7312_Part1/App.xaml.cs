using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ST10091324_PROG7312_Part1.Infrastructure;

namespace ST10091324_PROG7312_Part1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ConfigureServices(IServiceCollection services)
        {
            // Register database service
            services.AddSingleton<DatabaseService>();
            
            // Register cache service
            services.AddSingleton<CacheService>();
            
            // Register performance monitor
            services.AddSingleton<PerformanceMonitor>();
            
            // Register error handler
            services.AddSingleton<ErrorHandler>();
            
            // Register connection pool manager
            services.AddSingleton<ConnectionPoolManager>();
            
            // Phase 3 Services
            services.AddSingleton<RealTimeDataSyncService>();
            services.AddSingleton<DatabaseHealthMonitor>();
            services.AddSingleton<DataValidationService>();
            services.AddSingleton<ConcurrencyOptimizationService>();
            
            // Register logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            });
        }
    }
}
