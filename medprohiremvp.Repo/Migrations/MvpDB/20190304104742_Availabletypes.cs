using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class Availabletypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicantAvailableType_ID",
                table: "Applicants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Applicants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApplicantAvailableTypes",
                columns: table => new
                {
                    ApplicantAvailableType_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicantAvailableType_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAvailableTypes", x => x.ApplicantAvailableType_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantAvailableTypes");

            migrationBuilder.DropColumn(
                name: "ApplicantAvailableType_ID",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Applicants");
        }
    }
}
