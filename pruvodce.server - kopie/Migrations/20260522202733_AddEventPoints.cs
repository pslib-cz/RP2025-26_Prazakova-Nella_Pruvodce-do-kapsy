using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class AddEventPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Events_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_EventId",
                table: "Points");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Points",
                newName: "AreStudents");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationId",
                table: "Points",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "EventPoints",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPoints", x => new { x.EventId, x.PointId });
                    table.ForeignKey(
                        name: "FK_EventPoints_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPoints_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "TEXT", nullable: false),
                    FloorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_RoomId",
                table: "Points",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPoints_PointId",
                table: "EventPoints",
                column: "PointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Room_RoomId",
                table: "Points",
                column: "RoomId",
                principalTable: "Room",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Room_RoomId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Specializations_SpecializationId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "EventPoints");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Points_RoomId",
                table: "Points");

            migrationBuilder.RenameColumn(
                name: "AreStudents",
                table: "Points",
                newName: "EventId");

            migrationBuilder.AlterColumn<string>(
                name: "SpecializationId",
                table: "Points",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Points_EventId",
                table: "Points",
                column: "EventId");

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
                principalColumn: "SpecializationId");
        }
    }
}
