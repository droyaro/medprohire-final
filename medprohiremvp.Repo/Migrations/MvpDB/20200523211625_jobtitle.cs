using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class jobtitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantWorkHistory_Speciality_Specialty_ID",
                table: "ApplicantWorkHistories");


            migrationBuilder.DropColumn(
                name: "Speciality_ID",
                table: "ApplicantWorkHistories");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "ApplicantWorkHistories",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "ApplicantWorkHistories");

            migrationBuilder.AddColumn<int>(
                name: "Speciality_ID",
                table: "ApplicantWorkHistories",
                nullable: false,
                defaultValue: 0);

          

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantWorkHistory_Speciality_Specialty_ID",
                table: "ApplicantWorkHistories",
                column: "Speciality_ID",
                principalTable: "Specialities",
                principalColumn: "Speciality_ID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
