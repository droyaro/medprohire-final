using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class appliedshiftnotificremove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notification_ID",
                table: "ApplicantAppliedShifts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Notification_ID",
                table: "ApplicantAppliedShifts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
