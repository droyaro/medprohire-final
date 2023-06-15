using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class cities_lnglat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift");

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Cities",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Cities",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift",
                column: "ShiftLabel_ID",
                principalTable: "ShiftLabels",
                principalColumn: "ShiftLabel_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Cities");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift",
                column: "ShiftLabel_ID",
                principalTable: "ShiftLabels",
                principalColumn: "ShiftLabel_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
