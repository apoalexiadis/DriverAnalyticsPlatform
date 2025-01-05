using Microsoft.EntityFrameworkCore;
using DriverAnalyticsPlatform.Models;

namespace DriverAnalyticsPlatform.Data
{
    /// <summary>
    /// Database context class for the Driver Analytics Platform.
    /// Provides access to database tables and configurations using Entity Framework Core.
    /// </summary>
    public class DriverAnalyticsPlatformDbContext : DbContext
    {
        /// <summary>
        /// Constructor to initialize the database context with specified options.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public DriverAnalyticsPlatformDbContext(DbContextOptions<DriverAnalyticsPlatformDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the TelemetryData table in the database.
        /// Stores telemetry information such as speed, acceleration, fuel levels, etc.
        /// </summary>
        public DbSet<TelemetryData> TelemetryData { get; set; }

        /// <summary>
        /// Represents the EmailRequest table in the database.
        /// Stores email request records, including recipient details and message content.
        /// </summary>
        public DbSet<EmailRequest> EmailRequest { get; set; }

        /// <summary>
        /// Represents the Alerts table in the database.
        /// Stores alerts generated based on telemetry data, such as high speed or low fuel warnings.
        /// </summary>
        public DbSet<Alert> Alerts { get; set; }
    }
}
