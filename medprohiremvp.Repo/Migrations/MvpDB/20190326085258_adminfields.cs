using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class adminfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Administrators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Administrators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Administrators");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Administrators");
        }
    }
}
