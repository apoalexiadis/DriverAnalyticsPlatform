using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DriverAnalyticsPlatform.Data
{
    /// <summary>
    /// Factory class for creating instances of the DriverAnalyticsPlatformDbContext at design time.
    /// Required for EF Core migrations and scaffolding.
    /// </summary>
    public class DriverAnalyticsPlatformDbContextFactory : IDesignTimeDbContextFactory<DriverAnalyticsPlatformDbContext>
    {
        /// <summary>
        /// Creates a new instance of the database context using configuration settings.
        /// </summary>
        /// <param name="args">Command-line arguments, if any.</param>
        /// <returns>An instance of DriverAnalyticsPlatformDbContext.</returns>
        public DriverAnalyticsPlatformDbContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set base path to the current directory
                .AddJsonFile("appsettings.json") // Load configuration from appsettings.json
                .Build();

            // Configure database connection using the connection string
            var optionsBuilder = new DbContextOptionsBuilder<DriverAnalyticsPlatformDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // Return the configured database context
            return new DriverAnalyticsPlatformDbContext(optionsBuilder.Options);
        }
    }
}
