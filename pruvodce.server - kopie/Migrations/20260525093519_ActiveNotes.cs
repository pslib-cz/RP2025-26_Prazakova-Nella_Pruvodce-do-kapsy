using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class ActiveNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes");

            migrationBuilder.AddColumn<string>(
                name: "SelectedNoteIds",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActiveNoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActiveNoteStudentNoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ActiveNoteStudentNoteId",
                table: "Subjects",
                column: "ActiveNoteStudentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects",
                column: "ActiveNoteStudentNoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNotes_ActiveNoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ActiveNoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "SelectedNoteIds",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ActiveNoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ActiveNoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_StudentNotes_SubjectId",
                table: "StudentNotes",
                column: "SubjectId",
                unique: true);
        }
    }
}
