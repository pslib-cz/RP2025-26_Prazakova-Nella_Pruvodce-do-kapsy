using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class AddValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Specializations");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Acronym",
                table: "Subjects",
                column: "Acronym",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_Name_StartDate",
                table: "Events",
                columns: new[] { "Name", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Email",
                table: "AdminUsers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_Acronym",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Events_Name_StartDate",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_AdminUsers_Email",
                table: "AdminUsers");

            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "Specializations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Points",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");
        }
    }
}
