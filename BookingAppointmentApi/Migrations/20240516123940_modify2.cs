using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingAppointmentApi.Migrations
{
    public partial class modify2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsultantSpecialty",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultantSpecialty",
                table: "Appointments");
        }
    }
}
