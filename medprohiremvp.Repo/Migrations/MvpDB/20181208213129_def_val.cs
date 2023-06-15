using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class def_val : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift");

            migrationBuilder.AlterColumn<int>(
                name: "Category_ID",
                table: "ClientShift",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift",
                column: "Category_ID",
                principalTable: "Shift_Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift");

            migrationBuilder.AlterColumn<int>(
                name: "Category_ID",
                table: "ClientShift",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift",
                column: "Category_ID",
                principalTable: "Shift_Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
