using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class datemodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClinicalInstitutionBranches",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ClinicalInstitutionBranches",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClinicalInstitution",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ClinicalInstitution",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClientShift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ClientShift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Applicant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClinicalInstitutionBranches");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ClinicalInstitutionBranches");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClinicalInstitution");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ClinicalInstitution");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ClientShift");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Applicant");
        }
    }
}
