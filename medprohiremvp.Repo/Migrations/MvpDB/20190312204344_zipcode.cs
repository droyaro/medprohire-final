using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class zipcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaxZipCode",
                table: "Cities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinZipCode",
                table: "Cities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxZipCode",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "MinZipCode",
                table: "Cities");
        }
    }
}
