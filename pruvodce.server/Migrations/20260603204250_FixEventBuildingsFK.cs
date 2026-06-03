using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pruvodce.server.Migrations
{
    /// <inheritdoc />
    public partial class FixEventBuildingsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = 0;", suppressTransaction: true);

            migrationBuilder.Sql(@"
                CREATE TABLE EventBuildings_new (
                    EventId INTEGER NOT NULL,
                    BuildingId INTEGER NOT NULL,
                    CONSTRAINT PK_EventBuildings PRIMARY KEY (EventId, BuildingId),
                    CONSTRAINT FK_EventBuildings_Events_EventId FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE
                );
            ");

            migrationBuilder.Sql("INSERT INTO EventBuildings_new SELECT EventId, BuildingId FROM EventBuildings;");
            migrationBuilder.Sql("DROP TABLE EventBuildings;");
            migrationBuilder.Sql("ALTER TABLE EventBuildings_new RENAME TO EventBuildings;");

            migrationBuilder.Sql("PRAGMA foreign_keys = 1;", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
