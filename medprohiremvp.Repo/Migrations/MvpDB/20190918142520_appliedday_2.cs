using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class appliedday_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicantAppliedShiftsDays_AppliedShift_ID",
                table: "ApplicantAppliedShiftsDays");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAppliedShiftsDays_AppliedShift_ID",
                table: "ApplicantAppliedShiftsDays",
                column: "AppliedShift_ID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicantAppliedShiftsDays_AppliedShift_ID",
                table: "ApplicantAppliedShiftsDays");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAppliedShiftsDays_AppliedShift_ID",
                table: "ApplicantAppliedShiftsDays",
                column: "AppliedShift_ID");
        }
    }
}
