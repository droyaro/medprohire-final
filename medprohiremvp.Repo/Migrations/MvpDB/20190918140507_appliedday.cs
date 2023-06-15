using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class appliedday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicantAppliedShiftsDays",
                columns: table => new
                {
                    AppliedShiftDay_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppliedShift_ID = table.Column<int>(nullable: false),
                    Day = table.Column<DateTime>(nullable: false),
                    ClockInTime = table.Column<DateTime>(nullable: false),
                    ClockOutTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAppliedShiftsDays", x => x.AppliedShiftDay_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantAppliedShiftsDays_ApplicantAppliedShifts_AppliedShift_ID",
                        column: x => x.AppliedShift_ID,
                        principalTable: "ApplicantAppliedShifts",
                        principalColumn: "AppliedShift_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAppliedShiftsDays_AppliedShift_ID",
                table: "ApplicantAppliedShiftsDays",
                column: "AppliedShift_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantAppliedShiftsDays");
        }
    }
}
