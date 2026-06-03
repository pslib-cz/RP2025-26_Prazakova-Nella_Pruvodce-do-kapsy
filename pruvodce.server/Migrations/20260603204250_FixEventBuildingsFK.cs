using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class FixEventBuildingsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBuildings_Building_BuildingId",
                table: "EventBuildings");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropIndex(
                name: "IX_EventBuildings_BuildingId",
                table: "EventBuildings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
