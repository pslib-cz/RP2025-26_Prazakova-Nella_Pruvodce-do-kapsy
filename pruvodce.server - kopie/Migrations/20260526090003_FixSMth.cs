using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class FixSMth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId1",
                table: "PointTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers");

            migrationBuilder.DropIndex(
                name: "IX_PointTeachers_TeacherId1",
                table: "PointTeachers");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "PointTeachers");

            migrationBuilder.AlterColumn<string>(
                name: "PointTeacherId",
                table: "PointTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers",
                column: "PointTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTeachers_PointId",
                table: "PointTeachers",
                column: "PointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers");

            migrationBuilder.DropIndex(
                name: "IX_PointTeachers_PointId",
                table: "PointTeachers");

            migrationBuilder.AlterColumn<string>(
                name: "PointTeacherId",
                table: "PointTeachers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId1",
                table: "PointTeachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PointTeachers",
                table: "PointTeachers",
                columns: new[] { "PointId", "TeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_PointTeachers_TeacherId1",
                table: "PointTeachers",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTeachers_Teachers_TeacherId1",
                table: "PointTeachers",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "TeacherId");
        }
    }
}
