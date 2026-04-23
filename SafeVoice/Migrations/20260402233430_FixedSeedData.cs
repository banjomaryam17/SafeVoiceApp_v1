using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeVoice.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$zVXcgddvx0J6kC0uPVYqMuAp.ou9gQM6WcOjAFa.fUy7JREBxVs0W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$uoMtfWT451UVbX/O7Ocs8OIxxb1OSfm2qPs3kpwGyO.k.0Gp65d.C" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 3, 0, 30, 49, 604, DateTimeKind.Local).AddTicks(8040), "$2a$11$IbjD.FXDiskxjQE2w2/DR.830E3Ak3y58wx22mtBkZChg8NDcAAi2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 3, 0, 30, 49, 833, DateTimeKind.Local).AddTicks(8570), "$2a$11$.MBTQD6PW3KTsGOhF8TNAezKjxUYBEv1sg.l2joZy0vTnEdGzW42q" });
        }
    }
}
