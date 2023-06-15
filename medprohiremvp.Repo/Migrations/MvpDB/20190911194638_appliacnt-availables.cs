using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class appliacntavailables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAvailables_ApplicantAvailableTypes_ApplicantAvailableType_ID",
                table: "ApplicantAvailables");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantAvailables_ApplicantAvailableType_ID",
                table: "ApplicantAvailables");

            migrationBuilder.DropColumn(
                name: "ApplicantAvailableType_ID",
                table: "ApplicantAvailables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicantAvailableType_ID",
                table: "ApplicantAvailables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAvailables_ApplicantAvailableType_ID",
                table: "ApplicantAvailables",
                column: "ApplicantAvailableType_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAvailables_ApplicantAvailableTypes_ApplicantAvailableType_ID",
                table: "ApplicantAvailables",
                column: "ApplicantAvailableType_ID",
                principalTable: "ApplicantAvailableTypes",
                principalColumn: "ApplicantAvailableType_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
