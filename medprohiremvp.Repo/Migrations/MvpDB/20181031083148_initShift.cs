using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class initShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialty_Name",
                table: "Speciality",
                newName: "Speciality_Name");

            migrationBuilder.RenameColumn(
                name: "Specialty_ID",
                table: "Speciality",
                newName: "Speciality_ID");

            migrationBuilder.CreateTable(
                name: "ClientShift",
                columns: table => new
                {
                    ClientShift_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution_ID = table.Column<int>(nullable: false),
                    ContractorCount = table.Column<int>(nullable: false),
                    HourlyRate = table.Column<int>(nullable: false),
                    ClockInTime = table.Column<DateTime>(nullable: false),
                    ClockOutTime = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ShiftDescription = table.Column<string>(nullable: false),
                    Responsibility = table.Column<string>(nullable: false),
                    DateOfShift = table.Column<DateTime>(nullable: false),
                    ShiftExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientShift", x => x.ClientShift_ID);
                    table.ForeignKey(
                        name: "FK_ClientShift_ClinicalInstitution_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitution",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftSpecialities",
                columns: table => new
                {
                    ShiftSpeciality_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientShift_ID = table.Column<int>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftSpecialities", x => x.ShiftSpeciality_ID);
                    table.ForeignKey(
                        name: "FK_ShiftSpecialities_ClientShift_ClientShift_ID",
                        column: x => x.ClientShift_ID,
                        principalTable: "ClientShift",
                        principalColumn: "ClientShift_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftSpecialities_Speciality_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Speciality",
                        principalColumn: "Speciality_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Institution_ID",
                table: "ClientShift",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSpecialities_ClientShift_ID",
                table: "ShiftSpecialities",
                column: "ClientShift_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSpecialities_Speciality_ID",
                table: "ShiftSpecialities",
                column: "Speciality_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftSpecialities");

            migrationBuilder.DropTable(
                name: "ClientShift");

            migrationBuilder.RenameColumn(
                name: "Speciality_Name",
                table: "Speciality",
                newName: "Specialty_Name");

            migrationBuilder.RenameColumn(
                name: "Speciality_ID",
                table: "Speciality",
                newName: "Specialty_ID");
        }
    }
}
