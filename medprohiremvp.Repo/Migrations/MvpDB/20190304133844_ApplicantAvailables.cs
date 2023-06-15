using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ApplicantAvailables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantAvailableType_ID",
                table: "Applicants");

            migrationBuilder.CreateTable(
                name: "ApplicantAvailables",
                columns: table => new
                {
                    ApplicantAvailable_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    ApplicantAvailableType_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAvailables", x => x.ApplicantAvailable_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantAvailables_ApplicantAvailableTypes_ApplicantAvailableType_ID",
                        column: x => x.ApplicantAvailableType_ID,
                        principalTable: "ApplicantAvailableTypes",
                        principalColumn: "ApplicantAvailableType_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantAvailables_Applicants_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicants",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAvailables_ApplicantAvailableType_ID",
                table: "ApplicantAvailables",
                column: "ApplicantAvailableType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAvailables_Applicant_ID",
                table: "ApplicantAvailables",
                column: "Applicant_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantAvailables");

            migrationBuilder.AddColumn<int>(
                name: "ApplicantAvailableType_ID",
                table: "Applicants",
                nullable: false,
                defaultValue: 0);
        }
    }
}
