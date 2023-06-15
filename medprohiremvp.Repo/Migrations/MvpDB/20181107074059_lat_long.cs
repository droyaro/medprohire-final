using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class lat_long : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "ClinicalInstitutionBranches",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "ClinicalInstitutionBranches",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "ClinicalInstitution",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "ClinicalInstitution",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ClinicalInstitutionBranches");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ClinicalInstitutionBranches");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ClinicalInstitution");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ClinicalInstitution");
        }
    }
}
