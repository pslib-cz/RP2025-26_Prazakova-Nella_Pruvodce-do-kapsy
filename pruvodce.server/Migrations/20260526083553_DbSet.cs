using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class DbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointTeacher",
                table: "PointTeacher");

            migrationBuilder.RenameTable(
                name: "PointTeacher",
                newName: "PointTeachers");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeacher_TeacherId1",
                table: "PointTeachers",
                newName: "IX_PointTeachers_TeacherId1");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeacher_TeacherId",
                table: "PointTeachers",
                newName: "IX_PointTeachers_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers",
                columns: new[] { "PointId", "TeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeachers_Points_PointId",
                table: "PointTeachers",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId",
                table: "PointTeachers",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId1",
                table: "PointTeachers",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTeachers_Points_PointId",
                table: "PointTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId",
                table: "PointTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId1",
                table: "PointTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers");

            migrationBuilder.RenameTable(
                name: "PointTeachers",
                newName: "PointTeacher");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeachers_TeacherId1",
                table: "PointTeacher",
                newName: "IX_PointTeacher_TeacherId1");

            migrationBuilder.RenameIndex(
                name: "IX_PointTeachers_TeacherId",
                table: "PointTeacher",
                newName: "IX_PointTeacher_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointTeacher",
                table: "PointTeacher",
                columns: new[] { "PointId", "TeacherId" });

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
        }
    }
}
