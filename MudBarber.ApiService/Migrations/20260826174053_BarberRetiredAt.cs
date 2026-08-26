using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBarber.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class BarberRetiredAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add before dropping, so the retired flag can be carried across.
            // The scaffolded order dropped IsActive first and lost it.
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RetiredAt",
                table: "Barbers",
                type: "timestamp with time zone",
                nullable: true);

            // The bool never recorded when someone retired, so the migration
            // time is the best available stand-in for already-retired barbers.
            migrationBuilder.Sql(
                @"UPDATE ""Barbers"" SET ""RetiredAt"" = NOW() WHERE NOT ""IsActive"";");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Barbers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Mirror of Up: add, carry the data back, then drop.
            // Defaults to true so a rollback does not retire everyone.
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Barbers",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql(
                @"UPDATE ""Barbers"" SET ""IsActive"" = (""RetiredAt"" IS NULL);");

            migrationBuilder.DropColumn(
                name: "RetiredAt",
                table: "Barbers");
        }
    }
}
