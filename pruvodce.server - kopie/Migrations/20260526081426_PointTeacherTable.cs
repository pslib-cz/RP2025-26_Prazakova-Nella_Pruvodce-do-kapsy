using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class PointTeacherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTeacher_Points_PointsPointId",
                table: "PointTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTeacher_Teachers_TeachersTeacherId",
                table: "PointTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ActiveNoteId",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "TeachersTeacherId",
                table: "PointTeacher",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "PointsPointId",
                table: "PointTeacher",
                newName: "PointId");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeacher_TeachersTeacherId",
                table: "PointTeacher",
                newName: "IX_PointTeacher_TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "PointTeacherId",
                table: "PointTeacher",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId1",
                table: "PointTeacher",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointTeacher_TeacherId1",
                table: "PointTeacher",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeacher_Points_PointId",
                table: "PointTeacher",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeacher_Teachers_TeacherId",
                table: "PointTeacher",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeacher_Teachers_TeacherId1",
                table: "PointTeacher",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects",
                column: "ActiveNoteStudentNoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTeacher_Points_PointId",
                table: "PointTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTeacher_Teachers_TeacherId",
                table: "PointTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTeacher_Teachers_TeacherId1",
                table: "PointTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_PointTeacher_TeacherId1",
                table: "PointTeacher");

            migrationBuilder.DropColumn(
                name: "PointTeacherId",
                table: "PointTeacher");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "PointTeacher");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "PointTeacher",
                newName: "TeachersTeacherId");

            migrationBuilder.RenameColumn(
                name: "PointId",
                table: "PointTeacher",
                newName: "PointsPointId");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeacher_TeacherId",
                table: "PointTeacher",
                newName: "IX_PointTeacher_TeachersTeacherId");

            migrationBuilder.AddColumn<string>(
                name: "ActiveNoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeacher_Points_PointsPointId",
                table: "PointTeacher",
                column: "PointsPointId",
                principalTable: "Points",
                principalColumn: "PointId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeacher_Teachers_TeachersTeacherId",
                table: "PointTeacher",
                column: "TeachersTeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects",
                column: "ActiveNoteStudentNoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");
        }
    }
}
