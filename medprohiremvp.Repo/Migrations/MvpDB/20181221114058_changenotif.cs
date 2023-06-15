using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class changenotif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notification_Body",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notification_Title",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notification_Body",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Notification_Title",
                table: "Notifications");
        }
    }
}
