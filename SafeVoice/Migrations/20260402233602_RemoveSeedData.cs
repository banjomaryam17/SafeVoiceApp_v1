using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeVoice.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BadgeNumber", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastLogin", "LastName", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@safevoice.ie", "System", true, null, "Administrator", "$2a$11$zVXcgddvx0J6kC0uPVYqMuAp.ou9gQM6WcOjAFa.fUy7JREBxVs0W", "SuperAdmin", "admin" },
                    { 2, "G001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Community Policing", "garda@safevoice.ie", "Garda", true, null, "Administrator", "$2a$11$uoMtfWT451UVbX/O7Ocs8OIxxb1OSfm2qPs3kpwGyO.k.0Gp65d.C", "Garda", "garda.admin" }
                });
        }
    }
}
