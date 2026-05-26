using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class StructureFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Teachers_TeacherId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_Subjects_SubjectId",
                table: "Specializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Points_PointId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Rooms_RoomId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_PointId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_RoomId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_SubjectId",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Points_TeacherId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "PointId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "LabelX",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "LabelY",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Points");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Specializations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PointId",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "SpecializationId",
                table: "Points",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PointSubject",
                columns: table => new
                {
                    PointsPointId = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "PointTeacher",
                columns: table => new
                {
                    PointsPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeachersTeacherId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTeacher", x => new { x.PointsPointId, x.TeachersTeacherId });
                    table.ForeignKey(
                        name: "FK_PointTeacher_Points_PointsPointId",
                        column: x => x.PointsPointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointTeacher_Teachers_TeachersTeacherId",
                        column: x => x.TeachersTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeacher",
                columns: table => new
                {
                    SubjectsSubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    TeachersTeacherId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacher", x => new { x.SubjectsSubjectId, x.TeachersTeacherId });
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_Subjects_SubjectsSubjectId",
                        column: x => x.SubjectsSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_Teachers_TeachersTeacherId",
                        column: x => x.TeachersTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_SpecializationId",
                table: "Points",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_BuildingId",
                table: "Events",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_PointSubject_SubjectsSubjectId",
                table: "PointSubject",
                column: "SubjectsSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTeacher_TeachersTeacherId",
                table: "PointTeacher",
                column: "TeachersTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacher_TeachersTeacherId",
                table: "SubjectTeacher",
                column: "TeachersTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Buildings_BuildingId",
                table: "Events",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "SpecializationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Buildings_BuildingId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "PointSubject");

            migrationBuilder.DropTable(
                name: "PointTeacher");

            migrationBuilder.DropTable(
                name: "SubjectTeacher");

            migrationBuilder.DropIndex(
                name: "IX_Points_SpecializationId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Events_BuildingId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "PointId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "Specializations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Points",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "PointId",
                table: "Points",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Points",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LabelX",
                table: "Points",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LabelY",
                table: "Points",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Points",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_PointId",
                table: "Subjects",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_RoomId",
                table: "Subjects",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_SubjectId",
                table: "Specializations",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_TeacherId",
                table: "Points",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Teachers_TeacherId",
                table: "Points",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_Subjects_SubjectId",
                table: "Specializations",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Points_PointId",
                table: "Subjects",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Rooms_RoomId",
                table: "Subjects",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId");
        }
    }
}
