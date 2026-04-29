using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class IDK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Buildings_BuildingId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Rooms_RoomId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_RoomId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Events_BuildingId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "PointsPointId",
                table: "PointTeacher",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "PointsPointId",
                table: "PointSubject",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationId",
                table: "Points",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "SpecializationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "PointsPointId",
                table: "PointTeacher",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "PointsPointId",
                table: "PointSubject",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationId",
                table: "Points",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Points_RoomId",
                table: "Points",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_BuildingId",
                table: "Events",
                column: "BuildingId");

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
                name: "FK_Points_Rooms_RoomId",
                table: "Points",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "SpecializationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
