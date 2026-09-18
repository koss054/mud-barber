using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBarber.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class RenameServiceDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_services",
                table: "services");

            migrationBuilder.RenameTable(
                name: "services",
                newName: "barber_services");

            migrationBuilder.AddPrimaryKey(
                name: "pk_barber_services",
                table: "barber_services",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_barber_services_service_id",
                table: "bookings",
                column: "service_id",
                principalTable: "barber_services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_barber_services_service_id",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_barber_services",
                table: "barber_services");

            migrationBuilder.RenameTable(
                name: "barber_services",
                newName: "services");

            migrationBuilder.AddPrimaryKey(
                name: "pk_services",
                table: "services",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
