using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class ticketupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Applicants_Applicant_ID",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Applicant_ID",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Applicant_ID",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "User_ID",
                table: "Tickets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "TicketContents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_User_ID",
                table: "Tickets",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ApplicationUser_User_ID",
                table: "Tickets",
                column: "User_ID",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ApplicationUser_User_ID",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_User_ID",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "TicketContents");

            migrationBuilder.AddColumn<int>(
                name: "Applicant_ID",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Applicant_ID",
                table: "Tickets",
                column: "Applicant_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Applicants_Applicant_ID",
                table: "Tickets",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
