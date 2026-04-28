using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class EditModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Floors_Buildings_BuildingId",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Floors_FloorId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_Subject_SubjectId",
                table: "Specializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Points_PointId",
                table: "Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Rooms_RoomId",
                table: "Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Teachers_TeacherId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_FloorId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Floors_BuildingId",
                table: "Floors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DeleteData(
                table: "Points",
                keyColumn: "PointId",
                keyValue: "P1");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: "A214");

            migrationBuilder.DeleteData(
                table: "Floors",
                keyColumn: "FloorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LabelX",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LabelY",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SvgData",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Floors");

            migrationBuilder.DropColumn(
                name: "SvgOutline",
                table: "Floors");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Buildings");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_TeacherId",
                table: "Subjects",
                newName: "IX_Subjects_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_RoomId",
                table: "Subjects",
                newName: "IX_Subjects_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_PointId",
                table: "Subjects",
                newName: "IX_Subjects_PointId");

            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "Floors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_Subjects_SubjectId",
                table: "Specializations",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Points_PointId",
                table: "Subjects",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Rooms_RoomId",
                table: "Subjects",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_Subjects_SubjectId",
                table: "Specializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Points_PointId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Rooms_RoomId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "Floors");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subject",
                newName: "IX_Subject_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_RoomId",
                table: "Subject",
                newName: "IX_Subject_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_PointId",
                table: "Subject",
                newName: "IX_Subject_PointId");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LabelX",
                table: "Rooms",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LabelY",
                table: "Rooms",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Rooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SvgData",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Floors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SvgOutline",
                table: "Floors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Buildings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Buildings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "SubjectId");

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "Address", "Name" },
                values: new object[] { 1, "Školní 1", "Hlavní budova" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Description", "EndDate", "IsActive", "Name", "StartDate" },
                values: new object[] { 1, null, new DateTime(2026, 4, 17, 0, 30, 15, 677, DateTimeKind.Local).AddTicks(3171), true, "DOD 2026 Leden", new DateTime(2026, 4, 16, 0, 30, 15, 675, DateTimeKind.Local).AddTicks(7536) });

            migrationBuilder.InsertData(
                table: "Floors",
                columns: new[] { "FloorId", "BuildingId", "Name", "SvgOutline" },
                values: new object[] { 1, 1, "Přízemí", "M 0 0 L 100 100" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "FloorId", "Label", "LabelX", "LabelY", "Note", "SvgData", "Type" },
                values: new object[] { "A214", 1, "Laboratoř IT", null, null, null, "...", 1 });

            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "PointId", "Description", "EventId", "Icon", "Label", "LabelX", "LabelY", "Note", "RoomId", "TeacherId" },
                values: new object[] { "P1", null, 1, null, "Stanoviště robotiky", 50.0, 50.0, null, "A214", null });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FloorId",
                table: "Rooms",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_BuildingId",
                table: "Floors",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_Buildings_BuildingId",
                table: "Floors",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Floors_FloorId",
                table: "Rooms",
                column: "FloorId",
                principalTable: "Floors",
                principalColumn: "FloorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_Subject_SubjectId",
                table: "Specializations",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Points_PointId",
                table: "Subject",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Rooms_RoomId",
                table: "Subject",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Teachers_TeacherId",
                table: "Subject",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId");
        }
    }
}
