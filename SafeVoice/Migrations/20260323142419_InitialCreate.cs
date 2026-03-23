using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeVoice.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ReportingAs = table.Column<int>(type: "INTEGER", nullable: false),
                    VictimName = table.Column<string>(type: "TEXT", nullable: true),
                    VictimAge = table.Column<int>(type: "INTEGER", nullable: true),
                    RelationshipToVictim = table.Column<string>(type: "TEXT", nullable: true),
                    ReporterName = table.Column<string>(type: "TEXT", nullable: true),
                    ReporterContact = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
