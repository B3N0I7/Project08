using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultantCalendarApi.Migrations
{
    public partial class customizeConsultantCalendarDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatienId",
                table: "ConsultantCalendars",
                newName: "ConsultantId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantCalendars_Consultants_ConsultantId",
                table: "ConsultantCalendars");

            migrationBuilder.DropIndex(
                name: "IX_ConsultantCalendars_ConsultantId",
                table: "ConsultantCalendars");

            migrationBuilder.RenameColumn(
                name: "ConsultantId",
                table: "ConsultantCalendars",
                newName: "PatienId");
        }
    }
}
