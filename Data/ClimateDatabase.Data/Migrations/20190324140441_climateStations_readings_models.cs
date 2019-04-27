using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateDatabase.Data.Migrations
{
    public partial class climateStations_readings_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClimateStations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClimateStationReadings",
                columns: table => new
                {
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ClimateStationId = table.Column<string>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    ClimateStationIntervalWeight = table.Column<double>(nullable: false),
                    AverageTemperature = table.Column<double>(nullable: true),
                    TemperatureDeviation = table.Column<double>(nullable: true),
                    MaximumTemperature = table.Column<double>(nullable: true),
                    MaximumTemperatureDay = table.Column<int>(nullable: true),
                    MinimumTemperature = table.Column<double>(nullable: true),
                    MinimumTemperatureDay = table.Column<int>(nullable: true),
                    RainSum = table.Column<double>(nullable: true),
                    RainRatio = table.Column<double>(nullable: true),
                    MaximumRain = table.Column<double>(nullable: true),
                    MaximumRainDay = table.Column<int>(nullable: true),
                    DaysWithRainMoreThan1mm = table.Column<int>(nullable: true),
                    DaysWithRainMoreThan10mm = table.Column<int>(nullable: true),
                    DaysWithWindFasterThan14ms = table.Column<int>(nullable: true),
                    DaysWithThunder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateStationReadings", x => new { x.ClimateStationId, x.Year, x.Month });
                    table.ForeignKey(
                        name: "FK_ClimateStationReadings_ClimateStations_ClimateStationId",
                        column: x => x.ClimateStationId,
                        principalTable: "ClimateStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateStationReadings_IsDeleted",
                table: "ClimateStationReadings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateStations_IsDeleted",
                table: "ClimateStations",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClimateStationReadings");

            migrationBuilder.DropTable(
                name: "ClimateStations");
        }
    }
}
