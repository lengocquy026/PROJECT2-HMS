using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Data.Migrations
{
    public partial class Dec2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Accomodations_AccomodationID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_AccomodationID",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "AccomodationID",
                table: "Booking",
                newName: "NoOfChildren");

            migrationBuilder.AddColumn<int>(
                name: "AccomodationPackageID",
                table: "Booking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfAdults",
                table: "Booking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Booking",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AccomodationPackageID",
                table: "Booking",
                column: "AccomodationPackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AccomodationPackages_AccomodationPackageID",
                table: "Booking",
                column: "AccomodationPackageID",
                principalTable: "AccomodationPackages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AccomodationPackages_AccomodationPackageID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_AccomodationPackageID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AccomodationPackageID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "NoOfAdults",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "NoOfChildren",
                table: "Booking",
                newName: "AccomodationID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AccomodationID",
                table: "Booking",
                column: "AccomodationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Accomodations_AccomodationID",
                table: "Booking",
                column: "AccomodationID",
                principalTable: "Accomodations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
