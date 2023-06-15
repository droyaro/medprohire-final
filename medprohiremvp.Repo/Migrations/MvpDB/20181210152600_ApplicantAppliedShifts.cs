using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ApplicantAppliedShifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "ClientShift",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApplicantAppliedShifts",
                columns: table => new
                {
                    AppliedShift_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    ClientShift_ID = table.Column<int>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAppliedShifts", x => x.AppliedShift_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantAppliedShifts_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApplicantAppliedShifts_ClientShift_ClientShift_ID",
                        column: x => x.ClientShift_ID,
                        principalTable: "ClientShift",
                        principalColumn: "ClientShift_ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAppliedShifts_Applicant_ID",
                table: "ApplicantAppliedShifts",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAppliedShifts_ClientShift_ID",
                table: "ApplicantAppliedShifts",
                column: "ClientShift_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantAppliedShifts");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "ClientShift");
        }
    }
}
