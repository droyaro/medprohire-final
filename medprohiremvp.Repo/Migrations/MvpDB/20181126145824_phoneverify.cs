using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class phoneverify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isVerified",
                table: "ApplicationUser",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PhoneVerify",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    VerificationCode = table.Column<string>(nullable: true),
                    isVerified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneVerify", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneVerify");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "isVerified",
                table: "ApplicationUser");
        }
    }
}
