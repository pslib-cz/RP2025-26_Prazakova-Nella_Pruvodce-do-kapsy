using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class StudentNotesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_StudentNotes_NoteId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_StudentNotes_NoteId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_StudentNotes_NoteId",
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
                name: "StudentField",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "StudentYear",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Points");

            migrationBuilder.AddColumn<string>(
                name: "NoteStudentNoteId",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteStudentNoteId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "StudentNotes",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                table: "StudentNotes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentClass",
                table: "StudentNotes",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "StudentNotes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "StudentNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "StudentNotes",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "NoteStudentNoteId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NoteStudentNoteId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "StudentClass",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "StudentNotes");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "StudentNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "StudentNotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                table: "StudentNotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "StudentField",
                table: "StudentNotes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentYear",
                table: "StudentNotes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Points",
                type: "TEXT",
                nullable: true);

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
                name: "FK_Points_StudentNotes_NoteId",
                table: "Points",
                column: "NoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_StudentNotes_NoteId",
                table: "Subjects",
                column: "NoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_StudentNotes_NoteId",
                table: "Teachers",
                column: "NoteId",
                principalTable: "StudentNotes",
                principalColumn: "StudentNoteId");
        }
    }
}
