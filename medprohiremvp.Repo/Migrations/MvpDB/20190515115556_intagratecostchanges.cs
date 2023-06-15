using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class intagratecostchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientCostChanges",
                columns: table => new
                {
                    ClientCostChanges_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution_ID = table.Column<int>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false),
                    ShiftLabel_ID = table.Column<int>(nullable: false),
                    Cost = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCostChanges", x => x.ClientCostChanges_ID);
                    table.ForeignKey(
                        name: "FK_ClientCostChanges_ClinicalInstitutions_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitutions",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientCostChanges_ShiftLabels_ShiftLabel_ID",
                        column: x => x.ShiftLabel_ID,
                        principalTable: "ShiftLabels",
                        principalColumn: "ShiftLabel_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientCostChanges_Specialities_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Specialities",
                        principalColumn: "Speciality_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientCostChanges_Institution_ID",
                table: "ClientCostChanges",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCostChanges_ShiftLabel_ID",
                table: "ClientCostChanges",
                column: "ShiftLabel_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCostChanges_Speciality_ID",
                table: "ClientCostChanges",
                column: "Speciality_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientCostChanges");
        }
    }
}
