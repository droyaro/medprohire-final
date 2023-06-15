using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class Applieddays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppliedAlldays",
                table: "ApplicantAppliedShifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppliedDaysList",
                table: "ApplicantAppliedShifts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedAlldays",
                table: "ApplicantAppliedShifts");

            migrationBuilder.DropColumn(
                name: "AppliedDaysList",
                table: "ApplicantAppliedShifts");
        }
    }
}
