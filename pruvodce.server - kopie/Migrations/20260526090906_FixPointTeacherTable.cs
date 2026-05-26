using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class FixPointTeacherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "PointTeacher",
                columns: table => new
                {
                    PointTeacherId = table.Column<string>(type: "TEXT", nullable: false),
                    PointId = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTeacher", x => x.PointTeacherId);
                    table.ForeignKey(
                        name: "FK_PointTeacher_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointTeacher_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_PointTeacher_PointId",
                table: "PointTeacher",
                column: "PointId");
            
            migrationBuilder.CreateIndex(
                name: "IX_PointTeacher_TeacherId",
                table: "PointTeacher",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
