using System;

namespace DriverAnalyticsPlatform.Models
{
    /// <summary>
    /// Represents an alert message containing details about alerts.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// Unique identifier for the alert.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The alert message content.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Timestamp indicating when the alert was created.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
