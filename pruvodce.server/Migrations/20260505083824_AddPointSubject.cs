using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class AddPointSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointSubject");

            migrationBuilder.CreateTable(
                name: "PointSubjects",
                columns: table => new
                {
                    PointId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSubjects", x => new { x.PointId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_PointSubjects_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointSubjects_SubjectId",
                table: "PointSubjects",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointSubjects");

            migrationBuilder.CreateTable(
                name: "PointSubject",
                columns: table => new
                {
                    PointsPointId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectsSubjectId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSubject", x => new { x.PointsPointId, x.SubjectsSubjectId });
                    table.ForeignKey(
                        name: "FK_PointSubject_Points_PointsPointId",
                        column: x => x.PointsPointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointSubject_Subjects_SubjectsSubjectId",
                        column: x => x.SubjectsSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointSubject_SubjectsSubjectId",
                table: "PointSubject",
                column: "SubjectsSubjectId");
        }
    }
}
