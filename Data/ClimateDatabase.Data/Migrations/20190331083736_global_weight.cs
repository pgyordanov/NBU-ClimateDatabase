using Microsoft.EntityFrameworkCore.Migrations;

namespace ClimateDatabase.Data.Migrations
{
    public partial class global_weight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "ClimateStations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ClimateStations");
        }
    }
}
