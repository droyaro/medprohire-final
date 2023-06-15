using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class labelconnect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftLabel_ID",
                table: "ClientShift",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_ShiftLabel_ID",
                table: "ClientShift",
                column: "ShiftLabel_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift",
                column: "ShiftLabel_ID",
                principalTable: "ShiftLabels",
                principalColumn: "ShiftLabel_ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                table: "ClientShift");

            migrationBuilder.DropIndex(
                name: "IX_ClientShift_ShiftLabel_ID",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "ShiftLabel_ID",
                table: "ClientShift");
        }
    }
}
