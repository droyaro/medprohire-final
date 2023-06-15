using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class shiftadditionals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Consecutive_Occurrences",
                table: "ClientShift",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HolidayShift",
                table: "ClientShift",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Occurrences",
                table: "ClientShift",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShiftLabels",
                columns: table => new
                {
                    ShiftLabel_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShiftLabelName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftLabels", x => x.ShiftLabel_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftLabels");

            migrationBuilder.DropColumn(
                name: "Consecutive_Occurrences",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "HolidayShift",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "Occurrences",
                table: "ClientShift");
        }
    }
}
