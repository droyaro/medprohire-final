using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class drugscreen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_DrugscreenStatuses_DrugscreenStatus_ID",
                table: "Applicant");

            migrationBuilder.AlterColumn<int>(
                name: "DrugscreenStatus_ID",
                table: "Applicant",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_DrugscreenStatuses_DrugscreenStatus_ID",
                table: "Applicant",
                column: "DrugscreenStatus_ID",
                principalTable: "DrugscreenStatuses",
                principalColumn: "DrugscreenStatus_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_DrugscreenStatuses_DrugscreenStatus_ID",
                table: "Applicant");

            migrationBuilder.AlterColumn<int>(
                name: "DrugscreenStatus_ID",
                table: "Applicant",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_DrugscreenStatuses_DrugscreenStatus_ID",
                table: "Applicant",
                column: "DrugscreenStatus_ID",
                principalTable: "DrugscreenStatuses",
                principalColumn: "DrugscreenStatus_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
