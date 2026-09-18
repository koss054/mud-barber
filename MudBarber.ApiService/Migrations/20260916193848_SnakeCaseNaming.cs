using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBarber.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Barbers_BarberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Barbers",
                table: "Barbers");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "services");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "bookings");

            migrationBuilder.RenameTable(
                name: "Barbers",
                newName: "barbers");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "services",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "services",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "services",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RetiredAt",
                table: "services",
                newName: "retired_at");

            migrationBuilder.RenameColumn(
                name: "EstimatedMinutes",
                table: "services",
                newName: "estimated_minutes");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "bookings",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "bookings",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bookings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "bookings",
                newName: "service_id");

            migrationBuilder.RenameColumn(
                name: "EstimatedMinutes",
                table: "bookings",
                newName: "estimated_minutes");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "bookings",
                newName: "customer_name");

            migrationBuilder.RenameColumn(
                name: "BarberId",
                table: "bookings",
                newName: "barber_id");

            migrationBuilder.RenameColumn(
                name: "ActualMinutes",
                table: "bookings",
                newName: "actual_minutes");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ServiceId",
                table: "bookings",
                newName: "ix_bookings_service_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_BarberId",
                table: "bookings",
                newName: "ix_bookings_barber_id");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "barbers",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "barbers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RetiredAt",
                table: "barbers",
                newName: "retired_at");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "barbers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "barbers",
                newName: "first_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_services",
                table: "services",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bookings",
                table: "bookings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_barbers",
                table: "barbers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_barbers_barber_id",
                table: "bookings",
                column: "barber_id",
                principalTable: "barbers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_barbers_barber_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_services_service_id",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_services",
                table: "services");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bookings",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_barbers",
                table: "barbers");

            migrationBuilder.RenameTable(
                name: "services",
                newName: "Services");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "barbers",
                newName: "Barbers");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Services",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Services",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Services",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "retired_at",
                table: "Services",
                newName: "RetiredAt");

            migrationBuilder.RenameColumn(
                name: "estimated_minutes",
                table: "Services",
                newName: "EstimatedMinutes");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "Bookings",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Bookings",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bookings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "service_id",
                table: "Bookings",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "estimated_minutes",
                table: "Bookings",
                newName: "EstimatedMinutes");

            migrationBuilder.RenameColumn(
                name: "customer_name",
                table: "Bookings",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "barber_id",
                table: "Bookings",
                newName: "BarberId");

            migrationBuilder.RenameColumn(
                name: "actual_minutes",
                table: "Bookings",
                newName: "ActualMinutes");

            migrationBuilder.RenameIndex(
                name: "ix_bookings_service_id",
                table: "Bookings",
                newName: "IX_Bookings_ServiceId");

            migrationBuilder.RenameIndex(
                name: "ix_bookings_barber_id",
                table: "Bookings",
                newName: "IX_Bookings_BarberId");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Barbers",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Barbers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "retired_at",
                table: "Barbers",
                newName: "RetiredAt");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Barbers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Barbers",
                newName: "FirstName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Barbers",
                table: "Barbers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Barbers_BarberId",
                table: "Bookings",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
