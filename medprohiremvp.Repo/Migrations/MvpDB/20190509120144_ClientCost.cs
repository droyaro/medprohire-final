using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ClientCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientSpecialties",
                columns: table => new
                {
                    ClientSpeciality_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution_ID = table.Column<int>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSpecialties", x => x.ClientSpeciality_ID);
                    table.ForeignKey(
                        name: "FK_ClientSpecialties_ClinicalInstitutions_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitutions",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientSpecialties_Specialities_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Specialities",
                        principalColumn: "Speciality_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSpecialtiesCosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientSpeciality_ID = table.Column<int>(nullable: false),
                    ShiftLabel_ID = table.Column<int>(nullable: false),
                    Cost = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSpecialtiesCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSpecialtiesCosts_ClientSpecialties_ClientSpeciality_ID",
                        column: x => x.ClientSpeciality_ID,
                        principalTable: "ClientSpecialties",
                        principalColumn: "ClientSpeciality_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientSpecialtiesCosts_ShiftLabels_ShiftLabel_ID",
                        column: x => x.ShiftLabel_ID,
                        principalTable: "ShiftLabels",
                        principalColumn: "ShiftLabel_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecialties_Institution_ID",
                table: "ClientSpecialties",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecialties_Speciality_ID",
                table: "ClientSpecialties",
                column: "Speciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecialtiesCosts_ClientSpeciality_ID",
                table: "ClientSpecialtiesCosts",
                column: "ClientSpeciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecialtiesCosts_ShiftLabel_ID",
                table: "ClientSpecialtiesCosts",
                column: "ShiftLabel_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSpecialtiesCosts");

            migrationBuilder.DropTable(
                name: "ClientSpecialties");
        }
    }
}
