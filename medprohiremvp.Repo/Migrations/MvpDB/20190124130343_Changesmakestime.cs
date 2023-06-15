using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class Changesmakestime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangesEnabledTime",
                table: "ApplicationUser",
                newName: "ChangesMakedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangesMakedTime",
                table: "ApplicationUser",
                newName: "ChangesEnabledTime");
        }
    }
}
