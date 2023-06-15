using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class empagreement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmploymentAgreements",
                columns: table => new
                {
                    EmploymentAgreement_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_ID = table.Column<Guid>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    EmployeeAddress = table.Column<string>(nullable: true),
                    HourlyRate = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentAgreements", x => x.EmploymentAgreement_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmploymentAgreements");
        }
    }
}
