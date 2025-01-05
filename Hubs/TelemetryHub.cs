using Microsoft.AspNetCore.SignalR;

namespace DriverAnalyticsPlatform.Hubs
{
    /// <summary>
    /// SignalR hub for managing real-time telemetry and alert notifications.
    /// </summary>
    public class TelemetryHub : Hub
    {
        /// <summary>
        /// Sends a telemetry update to all connected clients.
        /// </summary>
        /// <param name="message">The telemetry update message to send.</param>
        public async Task SendTelemetryUpdate(string message)
        {
            // Broadcasts telemetry update to all clients
            await Clients.All.SendAsync("ReceiveTelemetryUpdate", message);
        }

        /// <summary>
        /// Sends an alert notification to all connected clients.
        /// </summary>
        /// <param name="alertMessage">The alert message to send.</param>
        public async Task SendAlert(string alertMessage)
        {
            // Broadcasts alert message to all clients
            await Clients.All.SendAsync("ReceiveAlert", alertMessage);
        }
    }
}
