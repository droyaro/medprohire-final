using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class renameing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "ApplicantWorkHistory");

            migrationBuilder.DropTable(
                name: "CertificateType");

            migrationBuilder.DropTable(
                name: "ClientShift");

            migrationBuilder.DropTable(
                name: "Notification_Templates");

            migrationBuilder.DropTable(
                name: "SignSended");

            migrationBuilder.DropTable(
                name: "Applicant");

            migrationBuilder.DropTable(
                name: "Speciality");

            migrationBuilder.DropTable(
                name: "ClinicalInstitution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantClockinClockOutTime",
                table: "ApplicantClockinClockOutTime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adminchanges",
                table: "Adminchanges");

            migrationBuilder.RenameTable(
                name: "ApplicantClockinClockOutTime",
                newName: "ApplicantClockInClockOutTime");

            migrationBuilder.RenameTable(
                name: "Adminchanges",
                newName: "AdminChanges");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantClockinClockOutTime_AppliedShift_ID",
                table: "ApplicantClockInClockOutTime",
                newName: "IX_ApplicantClockInClockOutTime_AppliedShift_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantClockInClockOutTime",
                table: "ApplicantClockInClockOutTime",
                column: "ClockinClockOutTime_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminChanges",
                table: "AdminChanges",
                column: "AdminChanges_ID");

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Admin_ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ProfileImg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Admin_ID);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Applicant_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Availability_ID = table.Column<int>(nullable: false),
                    BackgroundCheck = table.Column<bool>(nullable: false),
                    Drugscreen = table.Column<bool>(nullable: false),
                    DrugscreenStatus_ID = table.Column<int>(nullable: true),
                    WorkAuth = table.Column<bool>(nullable: false),
                    VisaStatus_ID = table.Column<int>(nullable: false),
                    IsEligible = table.Column<bool>(nullable: false),
                    PreferredID = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(nullable: true),
                    TIN = table.Column<string>(nullable: true),
                    CEU = table.Column<bool>(nullable: false),
                    ProfileImage = table.Column<string>(nullable: true),
                    Status_ID = table.Column<int>(nullable: false),
                    I_9 = table.Column<string>(nullable: true),
                    W_4 = table.Column<string>(nullable: true),
                    Contract = table.Column<string>(nullable: true),
                    Employment_agreement = table.Column<string>(nullable: true),
                    Sub_specialities = table.Column<string>(nullable: true),
                    E_verify = table.Column<bool>(nullable: false),
                    BoardingProcess = table.Column<int>(nullable: false),
                    Atwork = table.Column<bool>(nullable: false),
                    Resume = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Applicant_ID);
                    table.ForeignKey(
                        name: "FK_Applicants_Availabilities_Availability_ID",
                        column: x => x.Availability_ID,
                        principalTable: "Availabilities",
                        principalColumn: "Availability_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_DrugscreenStatuses_DrugscreenStatus_ID",
                        column: x => x.DrugscreenStatus_ID,
                        principalTable: "DrugscreenStatuses",
                        principalColumn: "DrugscreenStatus_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_ApplicationUser_User_ID",
                        column: x => x.User_ID,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_VisaStatuses_VisaStatus_ID",
                        column: x => x.VisaStatus_ID,
                        principalTable: "VisaStatuses",
                        principalColumn: "VisaStatus_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTypes",
                columns: table => new
                {
                    Certificate_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CertificateTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Certificate_ID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalInstitutions",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
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
                    table.PrimaryKey("PK_ClinicalInstitutions", x => x.Institution_ID);
                    table.ForeignKey(
                        name: "FK_ClinicalInstitutions_InstitutionTypes_InstitutionType_ID",
                        column: x => x.InstitutionType_ID,
                        principalTable: "InstitutionTypes",
                        principalColumn: "InstitutionType_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicalInstitutions_ApplicationUser_User_ID",
                        column: x => x.User_ID,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    NotificationTemplate_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationTitle = table.Column<string>(nullable: true),
                    NotificationSmBody = table.Column<string>(nullable: true),
                    NotificationBody = table.Column<string>(nullable: true),
                    NotificationAction = table.Column<string>(nullable: true),
                    NotificationController = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.NotificationTemplate_ID);
                });

            migrationBuilder.CreateTable(
                name: "SignSent",
                columns: table => new
                {
                    SignSended_ID = table.Column<Guid>(nullable: false),
                    User_ID = table.Column<Guid>(nullable: false),
                    Envelope_ID = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Emp_XPosition = table.Column<int>(nullable: false),
                    Emp_YPosition = table.Column<int>(nullable: false),
                    Emp_PageNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignSent", x => x.SignSended_ID);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Speciality_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SpecialityName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Speciality_ID);
                });

            migrationBuilder.CreateTable(
                name: "ClientShifts",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ClientShift_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution_ID = table.Column<int>(nullable: false),
                    ContractorCount = table.Column<int>(nullable: false),
                    HourlyRate = table.Column<int>(nullable: false),
                    ClockInTime = table.Column<DateTime>(nullable: false),
                    ClockOutTime = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ShiftDescription = table.Column<string>(nullable: false),
                    Responsibility = table.Column<string>(nullable: false),
                    DateOfShift = table.Column<DateTime>(nullable: false),
                    ShiftExpirationDate = table.Column<DateTime>(nullable: false),
                    Branch_ID = table.Column<int>(nullable: true),
                    Occurrences = table.Column<int>(nullable: false),
                    ShiftLabel_ID = table.Column<int>(nullable: false),
                    Consecutive_Occurrences = table.Column<bool>(nullable: false),
                    HolidayShift = table.Column<bool>(nullable: false),
                    Category_ID = table.Column<int>(nullable: false),
                    Available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientShifts", x => x.ClientShift_ID);
                    table.ForeignKey(
                        name: "FK_ClientShifts_ClinicalInstitutionBranches_Branch_ID",
                        column: x => x.Branch_ID,
                        principalTable: "ClinicalInstitutionBranches",
                        principalColumn: "Branch_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientShifts_ShiftCategories_Category_ID",
                        column: x => x.Category_ID,
                        principalTable: "ShiftCategories",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientShifts_ClinicalInstitutions_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitutions",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientShifts_ShiftLabels_ShiftLabel_ID",
                        column: x => x.ShiftLabel_ID,
                        principalTable: "ShiftLabels",
                        principalColumn: "ShiftLabel_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantWorkHistories",
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
                    table.PrimaryKey("PK_ApplicantWorkHistories", x => x.WorkHistory_ID);
                    table.ForeignKey(
                        name: "FK_ApplicantWorkHistories_Applicants_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicants",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantWorkHistories_Specialities_Speciality_ID",
                        column: x => x.Speciality_ID,
                        principalTable: "Specialities",
                        principalColumn: "Speciality_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_Availability_ID",
                table: "Applicants",
                column: "Availability_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_DrugscreenStatus_ID",
                table: "Applicants",
                column: "DrugscreenStatus_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_User_ID",
                table: "Applicants",
                column: "User_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_VisaStatus_ID",
                table: "Applicants",
                column: "VisaStatus_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkHistories_Applicant_ID",
                table: "ApplicantWorkHistories",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkHistories_Speciality_ID",
                table: "ApplicantWorkHistories",
                column: "Speciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShifts_Branch_ID",
                table: "ClientShifts",
                column: "Branch_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShifts_Category_ID",
                table: "ClientShifts",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShifts_Institution_ID",
                table: "ClientShifts",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShifts_ShiftLabel_ID",
                table: "ClientShifts",
                column: "ShiftLabel_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitutions_InstitutionType_ID",
                table: "ClinicalInstitutions",
                column: "InstitutionType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitutions_User_ID",
                table: "ClinicalInstitutions",
                column: "User_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAppliedShifts_Applicants_Applicant_ID",
                table: "ApplicantAppliedShifts",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAppliedShifts_ClientShifts_ClientShift_ID",
                table: "ApplicantAppliedShifts",
                column: "ClientShift_ID",
                principalTable: "ClientShifts",
                principalColumn: "ClientShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertificates_Applicants_Applicant_ID",
                table: "ApplicantCertificates",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertificates_CertificateTypes_CertificateType_ID",
                table: "ApplicantCertificates",
                column: "CertificateType_ID",
                principalTable: "CertificateTypes",
                principalColumn: "Certificate_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantClockInClockOutTime_ApplicantAppliedShifts_AppliedShift_ID",
                table: "ApplicantClockInClockOutTime",
                column: "AppliedShift_ID",
                principalTable: "ApplicantAppliedShifts",
                principalColumn: "AppliedShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantReferences_Applicants_Applicant_ID",
                table: "ApplicantReferences",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSpecialities_Applicants_Applicant_ID",
                table: "ApplicantSpecialities",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSpecialities_Specialities_Speciality_ID",
                table: "ApplicantSpecialities",
                column: "Speciality_ID",
                principalTable: "Specialities",
                principalColumn: "Speciality_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalInstitutionBranches_ClinicalInstitutions_Institution_ID",
                table: "ClinicalInstitutionBranches",
                column: "Institution_ID",
                principalTable: "ClinicalInstitutions",
                principalColumn: "Institution_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTemplates_NotificationTemplate_ID",
                table: "Notifications",
                column: "NotificationTemplate_ID",
                principalTable: "NotificationTemplates",
                principalColumn: "NotificationTemplate_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayChecks_Applicants_Applicant_ID",
                table: "PayChecks",
                column: "Applicant_ID",
                principalTable: "Applicants",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialities_ClientShifts_ClientShift_ID",
                table: "ShiftSpecialities",
                column: "ClientShift_ID",
                principalTable: "ClientShifts",
                principalColumn: "ClientShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialities_Specialities_Speciality_ID",
                table: "ShiftSpecialities",
                column: "Speciality_ID",
                principalTable: "Specialities",
                principalColumn: "Speciality_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAppliedShifts_Applicants_Applicant_ID",
                table: "ApplicantAppliedShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAppliedShifts_ClientShifts_ClientShift_ID",
                table: "ApplicantAppliedShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantCertificates_Applicants_Applicant_ID",
                table: "ApplicantCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantCertificates_CertificateTypes_CertificateType_ID",
                table: "ApplicantCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantClockInClockOutTime_ApplicantAppliedShifts_AppliedShift_ID",
                table: "ApplicantClockInClockOutTime");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantReferences_Applicants_Applicant_ID",
                table: "ApplicantReferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSpecialities_Applicants_Applicant_ID",
                table: "ApplicantSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSpecialities_Specialities_Speciality_ID",
                table: "ApplicantSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalInstitutionBranches_ClinicalInstitutions_Institution_ID",
                table: "ClinicalInstitutionBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTemplates_NotificationTemplate_ID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PayChecks_Applicants_Applicant_ID",
                table: "PayChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSpecialities_ClientShifts_ClientShift_ID",
                table: "ShiftSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSpecialities_Specialities_Speciality_ID",
                table: "ShiftSpecialities");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "ApplicantWorkHistories");

            migrationBuilder.DropTable(
                name: "CertificateTypes");

            migrationBuilder.DropTable(
                name: "ClientShifts");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "SignSent");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "ClinicalInstitutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantClockInClockOutTime",
                table: "ApplicantClockInClockOutTime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminChanges",
                table: "AdminChanges");

            migrationBuilder.RenameTable(
                name: "ApplicantClockInClockOutTime",
                newName: "ApplicantClockinClockOutTime");

            migrationBuilder.RenameTable(
                name: "AdminChanges",
                newName: "Adminchanges");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantClockInClockOutTime_AppliedShift_ID",
                table: "ApplicantClockinClockOutTime",
                newName: "IX_ApplicantClockinClockOutTime_AppliedShift_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantClockinClockOutTime",
                table: "ApplicantClockinClockOutTime",
                column: "ClockinClockOutTime_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adminchanges",
                table: "Adminchanges",
                column: "AdminChanges_ID");

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Admin_ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ProfileImg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Admin_ID);
                });

            migrationBuilder.CreateTable(
                name: "Applicant",
                columns: table => new
                {
                    Applicant_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Atwork = table.Column<bool>(nullable: false),
                    Availability_ID = table.Column<int>(nullable: false),
                    BackgroundCheck = table.Column<bool>(nullable: false),
                    BoardingProcess = table.Column<int>(nullable: false),
                    CEU = table.Column<bool>(nullable: false),
                    Contract = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Drugscreen = table.Column<bool>(nullable: false),
                    DrugscreenStatus_ID = table.Column<int>(nullable: true),
                    E_verify = table.Column<bool>(nullable: false),
                    Employment_agreement = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    I_9 = table.Column<string>(nullable: true),
                    IsEligible = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    PreferredID = table.Column<string>(nullable: true),
                    ProfileImage = table.Column<string>(nullable: true),
                    Resume = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(nullable: true),
                    Status_ID = table.Column<int>(nullable: false),
                    Sub_specialities = table.Column<string>(nullable: true),
                    TIN = table.Column<string>(nullable: true),
                    User_ID = table.Column<Guid>(nullable: false),
                    VisaStatus_ID = table.Column<int>(nullable: false),
                    W_4 = table.Column<string>(nullable: true),
                    WorkAuth = table.Column<bool>(nullable: false)
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
                name: "CertificateType",
                columns: table => new
                {
                    Certificate_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CertificateTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateType", x => x.Certificate_ID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalInstitution",
                columns: table => new
                {
                    Institution_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactPerson = table.Column<string>(nullable: false),
                    ContactTitle = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    InstitutionDescription = table.Column<string>(nullable: true),
                    InstitutionName = table.Column<string>(nullable: false),
                    InstitutionTaxId = table.Column<string>(nullable: false),
                    InstitutionType_ID = table.Column<int>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    User_ID = table.Column<Guid>(nullable: false)
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
                name: "Notification_Templates",
                columns: table => new
                {
                    NotificationTemplate_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationAction = table.Column<string>(nullable: true),
                    NotificationBody = table.Column<string>(nullable: true),
                    NotificationController = table.Column<string>(nullable: true),
                    NotificationSmBody = table.Column<string>(nullable: true),
                    NotificationTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification_Templates", x => x.NotificationTemplate_ID);
                });

            migrationBuilder.CreateTable(
                name: "SignSended",
                columns: table => new
                {
                    SignSended_ID = table.Column<Guid>(nullable: false),
                    Emp_PageNumber = table.Column<int>(nullable: false),
                    Emp_XPosition = table.Column<int>(nullable: false),
                    Emp_YPosition = table.Column<int>(nullable: false),
                    Envelope_ID = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    User_ID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignSended", x => x.SignSended_ID);
                });

            migrationBuilder.CreateTable(
                name: "Speciality",
                columns: table => new
                {
                    Speciality_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SpecialityName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speciality", x => x.Speciality_ID);
                });

            migrationBuilder.CreateTable(
                name: "ClientShift",
                columns: table => new
                {
                    ClientShift_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    Branch_ID = table.Column<int>(nullable: true),
                    Category_ID = table.Column<int>(nullable: false),
                    ClockInTime = table.Column<DateTime>(nullable: false),
                    ClockOutTime = table.Column<DateTime>(nullable: false),
                    Consecutive_Occurrences = table.Column<bool>(nullable: false),
                    ContractorCount = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    DateOfShift = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    HolidayShift = table.Column<bool>(nullable: false),
                    HourlyRate = table.Column<int>(nullable: false),
                    Institution_ID = table.Column<int>(nullable: false),
                    Occurrences = table.Column<int>(nullable: false),
                    Responsibility = table.Column<string>(nullable: false),
                    ShiftDescription = table.Column<string>(nullable: false),
                    ShiftExpirationDate = table.Column<DateTime>(nullable: false),
                    ShiftLabel_ID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientShift", x => x.ClientShift_ID);
                    table.ForeignKey(
                        name: "FK_ClientShift_ClinicalInstitutionBranches_Branch_ID",
                        column: x => x.Branch_ID,
                        principalTable: "ClinicalInstitutionBranches",
                        principalColumn: "Branch_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientShift_ShiftCategories_Category_ID",
                        column: x => x.Category_ID,
                        principalTable: "ShiftCategories",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientShift_ClinicalInstitution_Institution_ID",
                        column: x => x.Institution_ID,
                        principalTable: "ClinicalInstitution",
                        principalColumn: "Institution_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientShift_ShiftLabels_ShiftLabel_ID",
                        column: x => x.ShiftLabel_ID,
                        principalTable: "ShiftLabels",
                        principalColumn: "ShiftLabel_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantWorkHistory",
                columns: table => new
                {
                    WorkHistory_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    PlaceName = table.Column<string>(nullable: false),
                    Speciality_ID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
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
                        principalColumn: "Speciality_ID",
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
                name: "IX_ApplicantWorkHistory_Applicant_ID",
                table: "ApplicantWorkHistory",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantWorkHistory_Speciality_ID",
                table: "ApplicantWorkHistory",
                column: "Speciality_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Branch_ID",
                table: "ClientShift",
                column: "Branch_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Category_ID",
                table: "ClientShift",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_Institution_ID",
                table: "ClientShift",
                column: "Institution_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientShift_ShiftLabel_ID",
                table: "ClientShift",
                column: "ShiftLabel_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitution_InstitutionType_ID",
                table: "ClinicalInstitution",
                column: "InstitutionType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalInstitution_User_ID",
                table: "ClinicalInstitution",
                column: "User_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAppliedShifts_Applicant_Applicant_ID",
                table: "ApplicantAppliedShifts",
                column: "Applicant_ID",
                principalTable: "Applicant",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAppliedShifts_ClientShift_ClientShift_ID",
                table: "ApplicantAppliedShifts",
                column: "ClientShift_ID",
                principalTable: "ClientShift",
                principalColumn: "ClientShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertificates_Applicant_Applicant_ID",
                table: "ApplicantCertificates",
                column: "Applicant_ID",
                principalTable: "Applicant",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertificates_CertificateType_CertificateType_ID",
                table: "ApplicantCertificates",
                column: "CertificateType_ID",
                principalTable: "CertificateType",
                principalColumn: "Certificate_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantClockinClockOutTime_ApplicantAppliedShifts_AppliedShift_ID",
                table: "ApplicantClockinClockOutTime",
                column: "AppliedShift_ID",
                principalTable: "ApplicantAppliedShifts",
                principalColumn: "AppliedShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantReferences_Applicant_Applicant_ID",
                table: "ApplicantReferences",
                column: "Applicant_ID",
                principalTable: "Applicant",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSpecialities_Applicant_Applicant_ID",
                table: "ApplicantSpecialities",
                column: "Applicant_ID",
                principalTable: "Applicant",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSpecialities_Speciality_Speciality_ID",
                table: "ApplicantSpecialities",
                column: "Speciality_ID",
                principalTable: "Speciality",
                principalColumn: "Speciality_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalInstitutionBranches_ClinicalInstitution_Institution_ID",
                table: "ClinicalInstitutionBranches",
                column: "Institution_ID",
                principalTable: "ClinicalInstitution",
                principalColumn: "Institution_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Notification_Templates_NotificationTemplate_ID",
                table: "Notifications",
                column: "NotificationTemplate_ID",
                principalTable: "Notification_Templates",
                principalColumn: "NotificationTemplate_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayChecks_Applicant_Applicant_ID",
                table: "PayChecks",
                column: "Applicant_ID",
                principalTable: "Applicant",
                principalColumn: "Applicant_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialities_ClientShift_ClientShift_ID",
                table: "ShiftSpecialities",
                column: "ClientShift_ID",
                principalTable: "ClientShift",
                principalColumn: "ClientShift_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSpecialities_Speciality_Speciality_ID",
                table: "ShiftSpecialities",
                column: "Speciality_ID",
                principalTable: "Speciality",
                principalColumn: "Speciality_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
