using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeVoice.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingAs",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateResolved",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReviewed",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotes",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHighPriority",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorNotes",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresGardaAttention",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReviewedByUserId",
                table: "Reports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmittedByUserId",
                table: "Reports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    BadgeNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BadgeNumber", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "LastLogin", "LastName", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 4, 3, 0, 30, 49, 604, DateTimeKind.Local).AddTicks(8040), null, "admin@safevoice.ie", "System", true, null, "Administrator", "$2a$11$IbjD.FXDiskxjQE2w2/DR.830E3Ak3y58wx22mtBkZChg8NDcAAi2", "SuperAdmin", "admin" },
                    { 2, "G001", new DateTime(2026, 4, 3, 0, 30, 49, 833, DateTimeKind.Local).AddTicks(8570), "Community Policing", "garda@safevoice.ie", "Garda", true, null, "Administrator", "$2a$11$.MBTQD6PW3KTsGOhF8TNAezKjxUYBEv1sg.l2joZy0vTnEdGzW42q", "Garda", "garda.admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReviewedByUserId",
                table: "Reports",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SubmittedByUserId",
                table: "Reports",
                column: "SubmittedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReviewedByUserId",
                table: "Reports",
                column: "ReviewedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_SubmittedByUserId",
                table: "Reports",
                column: "SubmittedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReviewedByUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_SubmittedByUserId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReviewedByUserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SubmittedByUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DateResolved",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DateReviewed",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InternalNotes",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsHighPriority",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ModeratorNotes",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RequiresGardaAttention",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReviewedByUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SubmittedByUserId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ReportingAs",
                table: "Reports",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
