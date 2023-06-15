using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class availabledays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantAvailableDays",
                table: "Applicants");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantAvailableDays",
                table: "ApplicantAvailables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantAvailableDays",
                table: "ApplicantAvailables");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantAvailableDays",
                table: "Applicants",
                nullable: true);
        }
    }
}
