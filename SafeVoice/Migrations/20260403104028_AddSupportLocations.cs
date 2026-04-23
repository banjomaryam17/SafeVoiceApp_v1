using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeVoice.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    OpeningHours = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Services = table.Column<string>(type: "TEXT", nullable: true),
                    Is24Hour = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEmergency = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StationCode = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportLocations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SupportLocations",
                columns: new[] { "Id", "Address", "CreatedAt", "Description", "District", "Email", "Is24Hour", "IsActive", "IsEmergency", "Latitude", "Longitude", "Name", "OpeningHours", "Phone", "Services", "StationCode", "Type", "Website" },
                values: new object[,]
                {
                    { 1, "Park Street, Dundalk, Co. Louth", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Louth", null, true, true, true, 53.999400000000001, -6.4019000000000004, "Dundalk Garda Station", "24/7", "042-9388400", null, "DUN", "GardaStation", null },
                    { 2, "Crowe Street, Dundalk, Co. Louth", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Child and Family Agency services", null, "dundalk@tusla.ie", false, true, false, 54.001399999999997, -6.4050000000000002, "Tusla Dundalk", "Mon-Fri 9:00-17:00", "042-9394700", "Child protection, family support, welfare services", null, "Tusla", null },
                    { 3, "Stapleton Place, Dundalk, Co. Louth", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Health and social care services", null, null, false, true, false, 54.004100000000001, -6.3981000000000003, "HSE Louth Community Services", "Mon-Fri 9:00-17:00", "041-6850000", "Mental health, community care, counselling", null, "HSE", null },
                    { 4, "National Service", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Free 24/7 listening service for children", null, null, true, true, false, 53.349800000000002, -6.2603, "Childline", "24/7", "1800-666666", "Confidential support for children and young people", null, "Helpline", "https://childline.ie" },
                    { 5, "Drogheda, Co. Louth", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emergency medical services", null, null, true, true, true, 53.717500000000001, -6.3476999999999997, "Our Lady of Lourdes Hospital", "24/7 Emergency", "041-9874000", "Emergency department, paediatric care", null, "Hospital", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportLocations");
        }
    }
}
