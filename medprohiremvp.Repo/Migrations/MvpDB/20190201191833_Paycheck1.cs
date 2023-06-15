using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class Paycheck1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayChecks",
                columns: table => new
                {
                    Paycheck_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    AppliedShift_ID = table.Column<int>(nullable: false),
                    PayBeginDate = table.Column<DateTime>(nullable: false),
                    PayEndDate = table.Column<DateTime>(nullable: false),
                    Net_pay = table.Column<string>(nullable: false),
                    PaycheckFile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayChecks", x => x.Paycheck_ID);
                    table.ForeignKey(
                        name: "FK_PayChecks_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayChecks_ApplicantAppliedShifts_AppliedShift_ID",
                        column: x => x.AppliedShift_ID,
                        principalTable: "ApplicantAppliedShifts",
                        principalColumn: "AppliedShift_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayChecks_Applicant_ID",
                table: "PayChecks",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PayChecks_AppliedShift_ID",
                table: "PayChecks",
                column: "AppliedShift_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayChecks");
        }
    }
}
