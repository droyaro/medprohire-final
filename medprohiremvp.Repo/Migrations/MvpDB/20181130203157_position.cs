using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "emp_pagenumber",
                table: "SignSended",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "emp_xposition",
                table: "SignSended",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "emp_yposition",
                table: "SignSended",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emp_pagenumber",
                table: "SignSended");

            migrationBuilder.DropColumn(
                name: "emp_xposition",
                table: "SignSended");

            migrationBuilder.DropColumn(
                name: "emp_yposition",
                table: "SignSended");
        }
    }
}
