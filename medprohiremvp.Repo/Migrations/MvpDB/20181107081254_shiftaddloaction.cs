using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class shiftaddloaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Branch_ID",
                table: "ClientShift",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Branch_ID",
                table: "ClientShift",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
