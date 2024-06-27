using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultantCalendarApi.Migrations
{
    public partial class columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantCalendars_Consultants_ConsultantId",
                table: "ConsultantCalendars");

            migrationBuilder.DropIndex(
                name: "IX_ConsultantCalendars_ConsultantId",
                table: "ConsultantCalendars");

            migrationBuilder.AddColumn<string>(
                name: "ConsultantName",
                table: "ConsultantCalendars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConsultantSpecialty",
                table: "ConsultantCalendars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultantName",
                table: "ConsultantCalendars");

            migrationBuilder.DropColumn(
                name: "ConsultantSpecialty",
                table: "ConsultantCalendars");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantCalendars_ConsultantId",
                table: "ConsultantCalendars",
                column: "ConsultantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantCalendars_Consultants_ConsultantId",
                table: "ConsultantCalendars",
                column: "ConsultantId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
