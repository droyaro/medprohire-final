using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class shiftcategoryforignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category_ID",
                table: "ClientShift",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Category_ID",
                table: "ClientShift",
                column: "Category_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift",
                column: "Category_ID",
                
                principalTable: "Shift_Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientShift_Shift_Categories_Category_ID",
                table: "ClientShift");

            migrationBuilder.DropIndex(
                name: "IX_ClientShift_Category_ID",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "Category_ID",
                
                table: "ClientShift");
        }
    }
}
