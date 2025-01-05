using Microsoft.AspNetCore.Mvc;
using DriverAnalyticsPlatform.Models;
using DriverAnalyticsPlatform.Data;
using Microsoft.EntityFrameworkCore;
using DriverAnalyticsPlatform.Services;
using DriverAnalyticsPlatform.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DriverAnalyticsPlatform.Controllers
{
    /// <summary>
    /// Controller for managing telemetry data and alerts.
    /// Provides endpoints for retrieving, adding, and exporting data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : ControllerBase
    {
        private readonly DriverAnalyticsPlatformDbContext _context;
        private readonly IHubContext<TelemetryHub> _hubContext;

        // Constructor to initialize dependencies
        public TelemetryController(DriverAnalyticsPlatformDbContext context, IHubContext<TelemetryHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Retrieves a list of alerts ordered by the most recent.
        /// </summary>
        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _context.Alerts
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return Ok(alerts);
        }

        /// <summary>
        /// Retrieves telemetry data history along with calculated KPIs.
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetTelemetryHistory()
        {
            // Fetch the last 50 records sorted by timestamp
            var history = await _context.TelemetryData
                .OrderByDescending(t => t.Timestamp)
                .Take(50)
                .ToListAsync();

            // KPI Calculations
            var maxSpeed = history.Max(t => t.Speed);
            var minFuelLevel = history.Min(t => t.FuelLevel);
            var alertCount = history.Count(t => t.Speed > 120 || t.FuelLevel < 20);

            return Ok(new
            {
                History = history,
                MaxSpeed = maxSpeed,
                MinFuelLevel = minFuelLevel,
                AlertCount = alertCount
            });
        }

        /// <summary>
        /// Adds telemetry data, checks for alerts, and notifies clients in real time.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTelemetryData([FromBody] TelemetryData data, [FromServices] EmailService emailService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Save the telemetry data to the database
            _context.TelemetryData.Add(data);
            await _context.SaveChangesAsync();

            // Check for alerts
            List<string> alertMessages = new List<string>();

            if (data.Speed > 120)
            {
                alertMessages.Add($"High Speed Alert: {data.Speed} km/h");
            }

            if (data.FuelLevel < 20)
            {
                alertMessages.Add($"Low Fuel Alert: {data.FuelLevel}%");
            }

            foreach (var message in alertMessages)
            {
                // Save alert to the database
                var alert = new Alert
                {
                    Message = message,
                    Timestamp = DateTime.Now
                };
                _context.Alerts.Add(alert);

                // Send alert via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", message);
            }

            if (alertMessages.Count > 0)
            {
                await _context.SaveChangesAsync();
            }

            // Notify clients in real time with the telemetry update
            await _hubContext.Clients.All.SendAsync("ReceiveTelemetryUpdate", new
            {
                data.Timestamp,
                data.Speed,
                data.FuelLevel,
                data.Acceleration,
                data.GpsLatitude,
                data.GpsLongitude
            });

            return CreatedAtAction(nameof(GetAllTelemetryData), new { id = data.Id }, data);
        }

        /// <summary>
        /// Exports telemetry data to PDF format.
        /// </summary>
        [HttpGet("export/pdf")]
        public IActionResult ExportToPdf()
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new iTextSharp.text.Paragraph("Telemetry Data Report"));
                document.Add(new iTextSharp.text.Paragraph("--------------------------------------"));

                var data = _context.TelemetryData.ToList();
                foreach (var item in data)
                {
                    document.Add(new iTextSharp.text.Paragraph($"ID: {item.Id}, Speed: {item.Speed}, Time: {item.Timestamp}"));
                }

                document.Close();
                return File(memoryStream.ToArray(), "application/pdf", "telemetry_report.pdf");
            }
        }

        /// <summary>
        /// Exports telemetry data to CSV format.
        /// </summary>
        [HttpGet("export/csv")]
        public IActionResult ExportToCsv()
        {
            var records = _context.TelemetryData.ToList();

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
                writer.Flush();
                return File(memoryStream.ToArray(), "text/csv", "telemetry_data.csv");
            }
        }

        /// <summary>
        /// Computes and returns general statistics based on telemetry data.
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var totalRecords = await _context.TelemetryData.CountAsync();
            var averageSpeed = await _context.TelemetryData.AverageAsync(d => d.Speed);
            var maxSpeed = await _context.TelemetryData.MaxAsync(d => d.Speed);
            var dangerousBrakes = await _context.TelemetryData.CountAsync(d => d.Acceleration < -5);

            return Ok(new
            {
                TotalRecords = totalRecords,
                AverageSpeed = averageSpeed,
                MaxSpeed = maxSpeed,
                DangerousBrakes = dangerousBrakes
            });
        }

        /// <summary>
        /// Retrieves all telemetry data.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTelemetryData()
        {
            var data = await _context.TelemetryData.ToListAsync();
            return Ok(data);
        }
    }
}
