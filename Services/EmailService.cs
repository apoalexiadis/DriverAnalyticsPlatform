using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace DriverAnalyticsPlatform.Services
{
    /// <summary>
    /// Service for sending emails using SMTP.
    /// </summary>
    public class EmailService
    {
        // Configuration to access email settings from appsettings.json
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the EmailService class.
        /// </summary>
        /// <param name="configuration">Configuration to retrieve email settings.</param>
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="recipientEmail">Email address of the recipient.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="message">Body of the email.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            // Retrieve email settings from configuration
            var emailSettings = _configuration.GetSection("EmailSettings");

            // Create the email message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Driver Analytics", emailSettings["SenderEmail"])); // Sender information
            email.To.Add(new MailboxAddress("", recipientEmail)); // Recipient information
            email.Subject = subject; // Subject line

            // Set the email body as plain text
            email.Body = new TextPart("plain")
            {
                Text = message
            };

            // Create an SMTP client for sending the email
            using var client = new SmtpClient();

            try
            {
                // Connect to the SMTP server
                await client.ConnectAsync(
                    emailSettings["SMTPServer"],
                    int.Parse(emailSettings["Port"]),
                    false // Set to false for insecure connections, update for SSL/TLS as required
                );

                // Authenticate using credentials
                await client.AuthenticateAsync(
                    emailSettings["SenderEmail"],
                    emailSettings["Password"]
                );

                // Send the email
                await client.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions that occur during email sending
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
            finally
            {
                // Ensure the client disconnects properly
                await client.DisconnectAsync(true);
            }
        }
    }
}
