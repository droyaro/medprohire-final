using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class clockdays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EndStatus",
                table: "ApplicantClockinClockOutTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkEndTime",
                table: "ApplicantClockinClockOutTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkStartTime",
                table: "ApplicantClockinClockOutTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDay",
                table: "ApplicantClockinClockOutTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndStatus",
                table: "ApplicantClockinClockOutTime");

            migrationBuilder.DropColumn(
                name: "WorkEndTime",
                table: "ApplicantClockinClockOutTime");

            migrationBuilder.DropColumn(
                name: "WorkStartTime",
                table: "ApplicantClockinClockOutTime");

            migrationBuilder.DropColumn(
                name: "WorkingDay",
                table: "ApplicantClockinClockOutTime");
        }
    }
}
