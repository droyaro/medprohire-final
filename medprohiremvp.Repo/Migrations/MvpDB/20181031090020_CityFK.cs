using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class CityFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "country_id",
                table: "Cities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_country_id",
                table: "Cities",
                column: "country_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_country_id",
                table: "Cities",
                column: "country_id",
                principalTable: "Countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_country_id",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_country_id",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "country_id",
                table: "Cities");
        }
    }
}
