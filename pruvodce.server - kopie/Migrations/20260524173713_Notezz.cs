using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class Notezz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNotes_NoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_StudentNotes_NoteStudentNoteId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_NoteStudentNoteId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_NoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NoteStudentNoteId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "NoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes",
                column: "SubjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_TeacherId",
                table: "StudentNotes",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentNotes_Subjects_SubjectId",
                table: "StudentNotes",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentNotes_Teachers_TeacherId",
                table: "StudentNotes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentNotes_Subjects_SubjectId",
                table: "StudentNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentNotes_Teachers_TeacherId",
                table: "StudentNotes");

            migrationBuilder.DropIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes");

            migrationBuilder.DropIndex(
                name: "IX_StudentNotes_TeacherId",
                table: "StudentNotes");

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteStudentNoteId",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteStudentNoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_NoteStudentNoteId",
                table: "Teachers",
                column: "NoteStudentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_NoteStudentNoteId",
                table: "Subjects",
                column: "NoteStudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNotes_NoteStudentNoteId",
                table: "Subjects",
                column: "NoteStudentNoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_StudentNotes_NoteStudentNoteId",
                table: "Teachers",
                column: "NoteStudentNoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");
        }
    }
}
