using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class clockinclockout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicantClockinClockOutTime",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppliedShift_ID = table.Column<int>(nullable: false),
                    ClockInTime = table.Column<DateTime>(nullable: false),
                    ClockOutTime = table.Column<DateTime>(nullable: false),
                    Manually = table.Column<bool>(nullable: false),
                    AccpetedByClient = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantClockinClockOutTime", x => x.id);
                    table.ForeignKey(
                        name: "FK_ApplicantClockinClockOutTime_ApplicantAppliedShifts_AppliedShift_ID",
                        column: x => x.AppliedShift_ID,
                        principalTable: "ApplicantAppliedShifts",
                        principalColumn: "AppliedShift_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantClockinClockOutTime_AppliedShift_ID",
                table: "ApplicantClockinClockOutTime",
                column: "AppliedShift_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantClockinClockOutTime");
        }
    }
}
