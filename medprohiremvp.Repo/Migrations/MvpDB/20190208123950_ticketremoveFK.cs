using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ticketremoveFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ApplicationUser_User_ID",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_User_ID",
                table: "Tickets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_User_ID",
                table: "Tickets",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ApplicationUser_User_ID",
                table: "Tickets",
                column: "User_ID",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
