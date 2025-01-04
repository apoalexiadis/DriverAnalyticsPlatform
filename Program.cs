using DriverAnalyticsPlatform.Data;
using DriverAnalyticsPlatform.Hubs;
using DriverAnalyticsPlatform.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Configure Culture Settings
// ---------------------------
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// ---------------------------
// Configure Services
// ---------------------------

// Configure DbContext with SQL Server connection
builder.Services.AddDbContext<DriverAnalyticsPlatformDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Controllers
builder.Services.AddControllers();

// Add API Explorer and Swagger for documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR for real-time communication
builder.Services.AddSignalR();

// Add EmailService for dependency injection
builder.Services.AddTransient<EmailService>();

// ---------------------------
// Configure CORS Policy
// ---------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()  // Allow any header
               .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyOrigin(); // Allow requests from any origin
    });
});

var app = builder.Build();

// ---------------------------
// Configure Middleware
// ---------------------------

// Enable Swagger UI only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS policy
app.UseCors("AllowAll");

// Enforce HTTPS
app.UseHttpsRedirection();

// Enable serving static files (e.g., HTML, CSS, JS)
app.UseStaticFiles();

// Enable Authorization middleware
app.UseAuthorization();

// ---------------------------
// Map Endpoints
// ---------------------------

// Map API Controllers
app.MapControllers();

// Map SignalR Hub for telemetry updates
app.MapHub<TelemetryHub>("/telemetryHub");

// Enable detailed error pages during development
app.UseDeveloperExceptionPage();

// Start the application
app.Run();
