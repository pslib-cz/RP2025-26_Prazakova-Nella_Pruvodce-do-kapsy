using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    public partial class AddStudentNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Subjects");

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Points");

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Points",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentNote",
                columns: table => new
                {
                    StudentNoteId = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    StudentField = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentNote", x => x.StudentNoteId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_NoteId",
                table: "Teachers",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_NoteId",
                table: "Subjects",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_NoteId",
                table: "Points",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_StudentNote_NoteId",
                table: "Points",
                column: "NoteId",
                principalTable: "StudentNote",
                principalColumn: "StudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNote_NoteId",
                table: "Subjects",
                column: "NoteId",
                principalTable: "StudentNote",
                principalColumn: "StudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_StudentNote_NoteId",
                table: "Teachers",
                column: "NoteId",
                principalTable: "StudentNote",
                principalColumn: "StudentNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_StudentNote_NoteId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNote_NoteId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_StudentNote_NoteId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_NoteId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_NoteId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Points_NoteId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "StudentNote");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Points",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "AdminUsers",
                type: "TEXT",
                nullable: true);
        }
    }
}