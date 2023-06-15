using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations
{
    public partial class phoneverify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isVerified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isVerified",
                table: "AspNetUsers");

           
        }
    }
}
