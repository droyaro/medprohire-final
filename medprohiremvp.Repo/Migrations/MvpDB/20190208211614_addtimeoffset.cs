using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class addtimeoffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<int>(
                name: "TimeOffset",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "ApplicationUser",
                nullable: true);
        }
    }
}
