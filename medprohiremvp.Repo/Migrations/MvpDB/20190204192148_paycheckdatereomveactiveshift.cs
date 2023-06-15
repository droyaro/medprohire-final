using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class paycheckdatereomveactiveshift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayChecks_ApplicantAppliedShifts_AppliedShift_ID",
                table: "PayChecks");

            migrationBuilder.DropIndex(
                name: "IX_PayChecks_AppliedShift_ID",
                table: "PayChecks");

            migrationBuilder.DropColumn(
                name: "AppliedShift_ID",
                table: "PayChecks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppliedShift_ID",
                table: "PayChecks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PayChecks_AppliedShift_ID",
                table: "PayChecks",
                column: "AppliedShift_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PayChecks_ApplicantAppliedShifts_AppliedShift_ID",
                table: "PayChecks",
                column: "AppliedShift_ID",
                principalTable: "ApplicantAppliedShifts",
                principalColumn: "AppliedShift_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
