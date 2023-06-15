using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class Changeslochking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangesCount",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangesEnabledTime",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ChangesLocked",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangesCount",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "ChangesEnabledTime",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "ChangesLocked",
                table: "ApplicationUser");
        }
    }
}
