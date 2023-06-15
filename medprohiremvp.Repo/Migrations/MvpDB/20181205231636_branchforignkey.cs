using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class branchforignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Branch_ID",
                table: "ClientShift",
                column: "Branch_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_ClinicalInstitutionBranches_Branch_ID",
                table: "ClientShift",
                column: "Branch_ID",
                principalTable: "ClinicalInstitutionBranches",
                principalColumn: "Branch_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_ClinicalInstitutionBranches_Branch_ID",
                table: "ClientShift");

            migrationBuilder.DropIndex(
                name: "IX_ClientShift_Branch_ID",
                table: "ClientShift");
        }
    }
}
