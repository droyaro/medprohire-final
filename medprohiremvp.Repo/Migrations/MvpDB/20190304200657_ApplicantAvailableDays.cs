using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ApplicantAvailableDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTodayAvailableOnly",
                table: "Applicants");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantAvailableDays",
                table: "Applicants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantAvailableDays",
                table: "Applicants");

            migrationBuilder.AddColumn<bool>(
                name: "IsTodayAvailableOnly",
                table: "Applicants",
                nullable: false,
                defaultValue: false);
        }
    }
}
