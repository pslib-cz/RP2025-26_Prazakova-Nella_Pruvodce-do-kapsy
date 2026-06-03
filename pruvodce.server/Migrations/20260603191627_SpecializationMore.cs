using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class SpecializationMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.BuildingId);
                });

            migrationBuilder.Sql("DELETE FROM EventBuildings WHERE BuildingId NOT IN (SELECT BuildingId FROM Building);");

            migrationBuilder.CreateIndex(
                name: "IX_EventBuildings_BuildingId",
                table: "EventBuildings",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventBuildings_Building_BuildingId",
                table: "EventBuildings",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBuildings_Building_BuildingId",
                table: "EventBuildings");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropIndex(
                name: "IX_EventBuildings_BuildingId",
                table: "EventBuildings");

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AdminUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AdminUserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Email",
                table: "AdminUsers",
                column: "Email",
                unique: true);
        }
    }
}
