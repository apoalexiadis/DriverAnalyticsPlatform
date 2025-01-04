namespace DriverAnalyticsPlatform.Models
{
    /// <summary>
    /// Represents an email request containing details for sending an email.
    /// </summary>
    public class EmailRequest
    {
        /// <summary>
        /// Unique identifier for the email request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email address of the recipient.
        /// </summary>
        public string RecipientEmail { get; set; } // Recipient

        /// <summary>
        /// Subject of the email.
        /// </summary>
        public string Subject { get; set; } // Subject

        /// <summary>
        /// Body of the email message.
        /// </summary>
        public string Message { get; set; } // Message
    }
}