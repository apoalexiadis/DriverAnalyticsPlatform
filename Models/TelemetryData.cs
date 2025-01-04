using System.ComponentModel.DataAnnotations;

namespace DriverAnalyticsPlatform.Models
{
    /// <summary>
    /// Represents telemetry data collected from a vehicle.
    /// </summary>
    public class TelemetryData
    {
        /// <summary>
        /// Unique identifier for the telemetry data entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Timestamp when the data was recorded.
        /// This field is required.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Vehicle speed in km/h.
        /// Must be between 0 and 300 km/h.
        /// </summary>
        [Range(0, 300, ErrorMessage = "Speed must be between 0 and 300 km/h.")]
        public double Speed { get; set; }

        /// <summary>
        /// Vehicle acceleration in m/s².
        /// Must be between -10 and 10 m/s².
        /// </summary>
        [Range(-10, 10, ErrorMessage = "Acceleration must be between -10 and 10 m/s².")]
        public double Acceleration { get; set; }

        /// <summary>
        /// Distance traveled in kilometers.
        /// Must be between 0 and 1000 km.
        /// </summary>
        [Range(0, 1000, ErrorMessage = "Distance must be between 0 and 1000 km.")]
        public double Distance { get; set; }

        /// <summary>
        /// Fuel level as a percentage.
        /// Must be between 0 and 100%.
        /// </summary>
        [Range(0, 100, ErrorMessage = "Fuel level must be between 0 and 100%.")]
        public double FuelLevel { get; set; }

        /// <summary>
        /// Engine revolutions per minute (RPM).
        /// Must be between 0 and 8000 RPM.
        /// </summary>
        [Range(0, 8000, ErrorMessage = "RPM must be between 0 and 8000.")]
        public double Rpm { get; set; }

        /// <summary>
        /// GPS latitude coordinates.
        /// This field is required.
        /// </summary>
        [Required]
        public string GpsLatitude { get; set; }

        /// <summary>
        /// GPS longitude coordinates.
        /// This field is required.
        /// </summary>
        [Required]
        public string GpsLongitude { get; set; }
    }
}
