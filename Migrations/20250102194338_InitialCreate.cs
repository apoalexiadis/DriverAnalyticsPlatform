using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverAnalyticsPlatform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelemetryData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    Acceleration = table.Column<double>(type: "float", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    FuelLevel = table.Column<double>(type: "float", nullable: false),
                    Rpm = table.Column<double>(type: "float", nullable: false),
                    GpsLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GpsLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelemetryData");
        }
    }
}
