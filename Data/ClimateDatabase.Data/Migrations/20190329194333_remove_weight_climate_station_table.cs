using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateDatabase.Data.Migrations
{
    public partial class remove_weight_climate_station_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ClimateStations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "ClimateStations",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
