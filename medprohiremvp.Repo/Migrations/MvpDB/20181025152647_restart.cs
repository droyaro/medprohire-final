using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class restart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Availability_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Availability = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Availability_ID);
                });

            migrationBuilder.CreateTable(
                name: "CertificateType",
                columns: table => new
                {
                    Certificate_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Certificate_TypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateType", x => x.Certificate_ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    shortname = table.Column<string>(maxLength: 3, nullable: false),
                    name = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DrugscreenStatuses",
                columns: table => new
                {
                    DrugscreenStatus_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DrugscreenStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugscreenStatuses", x => x.DrugscreenStatus_ID);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTypes",
                columns: table => new
                {
                    InstitutionType_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstitutionTypeNames = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTypes", x => x.InstitutionType_ID);
                });

            migrationBuilder.CreateTable(
                name: "Speciality",
                columns: table => new
                {
                    Specialty_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Specialty_Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speciality", x => x.Specialty_ID);
                });

            migrationBuilder.CreateTable(
                name: "VisaStatuses",
                columns: table => new
                {
                    VisaStatus_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisaStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaStatuses", x => x.VisaStatus_ID);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    country_id = table.Column<int>(nullable: false),
                    state_name = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.id);
                    table.ForeignKey(
                        name: "FK_States_Countries_country_id",
                        column: x => x.country_id,
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    city_name = table.Column<string>(maxLength: 150, nullable: false),
                    state_id = table.Column<int>(nullable: false),
                    country_id = table.Column<int>(nullable: false),
                    Countriesid = table.Column<int>(nullable: true),
                    Statesid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_Countriesid",
                        column: x => x.Countriesid,
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_States_Statesid",
                        column: x => x.Statesid,
                        principalTable: "States",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    AccessFailedCount = table.Column<int>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    City_ID = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Cities_City_ID",
                        column: x => x.City_ID,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicant",
                columns: table => new
                {
                    Applicant_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Availability_ID = table.Column<int>(nullable: false),
                    BackgroundCheck = table.Column<bool>(nullable: false),
                    Drugscreen = table.Column<bool>(nullable: false),
                    DrugscreenStatus_ID = table.Column<int>(nullable: true),
                    WorkAuth = table.Column<bool>(nullable: false),
                    VisaStatus_ID = table.Column<int>(nullable: false),
                    PreferredID = table.Column<string>(nullable: false),
                    SSN = table.Column<string>(nullable: false),
                    TIN = table.Column<string>(nullable: false),
                    CEU = table.Column<bool>(nullable: false),
                    ProfileImage = table.Column<string>(nullable: true),
                    Status_ID = table.Column<int>(nullable: false),
                    I_9 = table.Column<string>(nullable: true),
                    W_4 = table.Column<string>(nullable: true),
                    Contract = table.Column<string>(nullable: true),
                    Employemen_agreement = table.Column<string>(nullable: true),
                    Sub_specialities = table.Column<string>(nullable: true),
                    E_verify = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant", x => x.Applicant_ID);
                    table.ForeignKey(
                        name: "FK_Applicant_Availabilities_Availability_ID",
                        column: x => x.Availability_ID,
                        principalTable: "Availabilities",
                        principalColumn: "Availability_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicant_DrugscreenStatuses_DrugscreenStatus_ID",
                        column: x => x.DrugscreenStatus_ID,
                        principalTable: "DrugscreenStatuses",
                        principalColumn: "DrugscreenStatus_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicant_ApplicationUser_User_ID",
                        column: x => x.User_ID,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicant_VisaStatuses_VisaStatus_ID",
                        column: x => x.VisaStatus_ID,
                        principalTable: "VisaStatuses",
                        principalColumn: "VisaStatus_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalInstitution",
                columns: table => new
                {
                    Institution_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_ID = table.Column<Guid>(nullable: false),
                    InstitutionType_ID = table.Column<int>(nullable: false),
                    InstitutionName = table.Column<string>(nullable: false),
                    ContactPerson = table.Column<string>(nullable: false),
                    ContactTitle = table.Column<string>(nullable: false),
                    InstitutionTaxId = table.Column<string>(nullable: false),
                    InstitutionDescription = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalInstitution", x => x.Institution_ID);
                    table.ForeignKey(
                        name: "FK_ClinicalInstitution_InstitutionTypes_InstitutionType_ID",
                        column: x => x.InstitutionType_ID,
                        principalTable: "InstitutionTypes",
                        principalColumn: "InstitutionType_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicalInstitution_ApplicationUser_User_ID",
                        column: x => x.User_ID,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantCertificates",
                columns: table => new
                {
                    Ceritification_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    CertificateType_ID = table.Column<int>(nullable: false),
                    CeritifcationImg = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantCertificates", x => x.Ceritification_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantCertificates_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantCertificates_CertificateType_CertificateType_ID",
                        column: x => x.CertificateType_ID,
                        principalTable: "CertificateType",
                        principalColumn: "Certificate_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantReferences",
                columns: table => new
                {
                    Reference_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    Company = table.Column<string>(nullable: false),
                    Position = table.Column<string>(nullable: false),
                    ContactName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantReferences", x => x.Reference_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantReferences_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false),
                    License = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantSpecialities_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantSpecialities_Speciality_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Speciality",
                        principalColumn: "Specialty_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantWorkHistory",
                columns: table => new
                {
                    WorkHistory_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false),
                    PlaceName = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantWorkHistory", x => x.WorkHistory_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantWorkHistory_Applicant_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicant",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantWorkHistory_Speciality_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Speciality",
                        principalColumn: "Specialty_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalInstitutionBranches",
                columns: table => new
                {
                    Branch_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution_ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    CityId = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalInstitutionBranches", x => x.Branch_ID);
                    table.ForeignKey(
                        name: "FK_ClinicalInstitutionBranches_ClinicalInstitution_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitution",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_Availability_ID",
                table: "Applicant",
                column: "Availability_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_DrugscreenStatus_ID",
                table: "Applicant",
                column: "DrugscreenStatus_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_User_ID",
                table: "Applicant",
                column: "User_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_VisaStatus_ID",
                table: "Applicant",
                column: "VisaStatus_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantCertificates_Applicant_ID",
                table: "ApplicantCertificates",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantCertificates_CertificateType_ID",
                table: "ApplicantCertificates",
                column: "CertificateType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantReferences_Applicant_ID",
                table: "ApplicantReferences",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSpecialities_Applicant_ID",
                table: "ApplicantSpecialities",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSpecialities_Speciality_ID",
                table: "ApplicantSpecialities",
                column: "Speciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkHistory_Applicant_ID",
                table: "ApplicantWorkHistory",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkHistory_Speciality_ID",
                table: "ApplicantWorkHistory",
                column: "Speciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_City_ID",
                table: "ApplicationUser",
                column: "City_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Countriesid",
                table: "Cities",
                column: "Countriesid");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Statesid",
                table: "Cities",
                column: "Statesid");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitution_InstitutionType_ID",
                table: "ClinicalInstitution",
                column: "InstitutionType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitution_User_ID",
                table: "ClinicalInstitution",
                column: "User_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitutionBranches_Institution_ID",
                table: "ClinicalInstitutionBranches",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_States_country_id",
                table: "States",
                column: "country_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantCertificates");

            migrationBuilder.DropTable(
                name: "ApplicantReferences");

            migrationBuilder.DropTable(
                name: "ApplicantSpecialities");

            migrationBuilder.DropTable(
                name: "ApplicantWorkHistory");

            migrationBuilder.DropTable(
                name: "ClinicalInstitutionBranches");

            migrationBuilder.DropTable(
                name: "CertificateType");

            migrationBuilder.DropTable(
                name: "Applicant");

            migrationBuilder.DropTable(
                name: "Speciality");

            migrationBuilder.DropTable(
                name: "ClinicalInstitution");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropTable(
                name: "DrugscreenStatuses");

            migrationBuilder.DropTable(
                name: "VisaStatuses");

            migrationBuilder.DropTable(
                name: "InstitutionTypes");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
