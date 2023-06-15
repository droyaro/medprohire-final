using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class renameingfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Notification_Templates_Notification_Template_ID",
                table: "Notifications");

          

            migrationBuilder.RenameColumn(
                name: "Speciality_Name",
                table: "Speciality",
                newName: "SpecialityName");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "SignSended",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "SignSended",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "filetype",
                table: "SignSended",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "filepath",
                table: "SignSended",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "emp_yposition",
                table: "SignSended",
                newName: "Emp_YPosition");

            migrationBuilder.RenameColumn(
                name: "emp_xposition",
                table: "SignSended",
                newName: "Emp_XPosition");

            migrationBuilder.RenameColumn(
                name: "emp_pagenumber",
                table: "SignSended",
                newName: "Emp_PageNumber");

            migrationBuilder.RenameColumn(
                name: "EnvelopeId",
                table: "SignSended",
                newName: "Envelope_ID");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SignSended",
                newName: "SignSended_ID");

            migrationBuilder.RenameColumn(
                name: "isVerified",
                table: "PhoneVerify",
                newName: "IsVerified");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PhoneVerify",
                newName: "PhoneVerify_ID");

            migrationBuilder.RenameColumn(
                name: "PaycheckFile",
                table: "PayChecks",
                newName: "PayCheckFile");

            migrationBuilder.RenameColumn(
                name: "Net_pay",
                table: "PayChecks",
                newName: "Net_Pay");

            migrationBuilder.RenameColumn(
                name: "Paycheck_ID",
                table: "PayChecks",
                newName: "PayCheck_ID");

            migrationBuilder.RenameColumn(
                name: "user_ID",
                table: "Notifications",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "Notification_Title",
                table: "Notifications",
                newName: "NotificationTitle");

            migrationBuilder.RenameColumn(
                name: "Notification_Template_ID",
                table: "Notifications",
                newName: "NotificationTemplate_ID");

            migrationBuilder.RenameColumn(
                name: "Notification_Body",
                table: "Notifications",
                newName: "NotificationBody");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_Notification_Template_ID",
                table: "Notifications",
                newName: "IX_Notifications_NotificationTemplate_ID");

            migrationBuilder.RenameColumn(
                name: "Notification_sm_Body",
                table: "Notification_Templates",
                newName: "NotificationTitle");

            migrationBuilder.RenameColumn(
                name: "Notification_controller",
                table: "Notification_Templates",
                newName: "NotificationSmBody");

            migrationBuilder.RenameColumn(
                name: "Notification_Title",
                table: "Notification_Templates",
                newName: "NotificationController");

            migrationBuilder.RenameColumn(
                name: "Notification_Body",
                table: "Notification_Templates",
                newName: "NotificationBody");

            migrationBuilder.RenameColumn(
                name: "Notification_Action",
                table: "Notification_Templates",
                newName: "NotificationAction");

            migrationBuilder.RenameColumn(
                name: "Notification_Template_ID",
                table: "Notification_Templates",
                newName: "NotificationTemplate_ID");

            migrationBuilder.RenameColumn(
                name: "InstitutionTypeNames",
                table: "InstitutionTypes",
                newName: "InstitutionTypeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ClinicalInstitutionBranches",
                newName: "BranchName");

            migrationBuilder.RenameColumn(
                name: "Certificate_TypeName",
                table: "CertificateType",
                newName: "CertificateTypeName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ApplicantSpecialities",
                newName: "ApplicantSpeciality_ID");

            migrationBuilder.RenameColumn(
                name: "AccpetedByClient",
                table: "ApplicantClockinClockOutTime",
                newName: "AcceptedByClient");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ApplicantClockinClockOutTime",
                newName: "ClockinClockOutTime_ID");

            migrationBuilder.RenameColumn(
                name: "CeritifcationImg",
                table: "ApplicantCertificates",
                newName: "CeritificationImg");

            migrationBuilder.RenameColumn(
                name: "AppliedAlldays",
                table: "ApplicantAppliedShifts",
                newName: "AppliedAllDays");

      

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Notification_Templates_NotificationTemplate_ID",
                table: "Notifications",
                column: "NotificationTemplate_ID",
                principalTable: "Notification_Templates",
                principalColumn: "NotificationTemplate_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Notification_Templates_NotificationTemplate_ID",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "ShiftCategories");

            migrationBuilder.RenameColumn(
                name: "SpecialityName",
                table: "Speciality",
                newName: "Speciality_Name");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "SignSended",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SignSended",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "SignSended",
                newName: "filetype");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "SignSended",
                newName: "filepath");

            migrationBuilder.RenameColumn(
                name: "Emp_YPosition",
                table: "SignSended",
                newName: "emp_yposition");

            migrationBuilder.RenameColumn(
                name: "Emp_XPosition",
                table: "SignSended",
                newName: "emp_xposition");

            migrationBuilder.RenameColumn(
                name: "Emp_PageNumber",
                table: "SignSended",
                newName: "emp_pagenumber");

            migrationBuilder.RenameColumn(
                name: "Envelope_ID",
                table: "SignSended",
                newName: "EnvelopeId");

            migrationBuilder.RenameColumn(
                name: "SignSended_ID",
                table: "SignSended",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "PhoneVerify",
                newName: "isVerified");

            migrationBuilder.RenameColumn(
                name: "PhoneVerify_ID",
                table: "PhoneVerify",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PayCheckFile",
                table: "PayChecks",
                newName: "PaycheckFile");

            migrationBuilder.RenameColumn(
                name: "Net_Pay",
                table: "PayChecks",
                newName: "Net_pay");

            migrationBuilder.RenameColumn(
                name: "PayCheck_ID",
                table: "PayChecks",
                newName: "Paycheck_ID");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "Notifications",
                newName: "user_ID");

            migrationBuilder.RenameColumn(
                name: "NotificationTitle",
                table: "Notifications",
                newName: "Notification_Title");

            migrationBuilder.RenameColumn(
                name: "NotificationTemplate_ID",
                table: "Notifications",
                newName: "Notification_Template_ID");

            migrationBuilder.RenameColumn(
                name: "NotificationBody",
                table: "Notifications",
                newName: "Notification_Body");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotificationTemplate_ID",
                table: "Notifications",
                newName: "IX_Notifications_Notification_Template_ID");

            migrationBuilder.RenameColumn(
                name: "NotificationTitle",
                table: "Notification_Templates",
                newName: "Notification_sm_Body");

            migrationBuilder.RenameColumn(
                name: "NotificationSmBody",
                table: "Notification_Templates",
                newName: "Notification_controller");

            migrationBuilder.RenameColumn(
                name: "NotificationController",
                table: "Notification_Templates",
                newName: "Notification_Title");

            migrationBuilder.RenameColumn(
                name: "NotificationBody",
                table: "Notification_Templates",
                newName: "Notification_Body");

            migrationBuilder.RenameColumn(
                name: "NotificationAction",
                table: "Notification_Templates",
                newName: "Notification_Action");

            migrationBuilder.RenameColumn(
                name: "NotificationTemplate_ID",
                table: "Notification_Templates",
                newName: "Notification_Template_ID");

            migrationBuilder.RenameColumn(
                name: "InstitutionTypeName",
                table: "InstitutionTypes",
                newName: "InstitutionTypeNames");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                table: "ClinicalInstitutionBranches",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CertificateTypeName",
                table: "CertificateType",
                newName: "Certificate_TypeName");

            migrationBuilder.RenameColumn(
                name: "ApplicantSpeciality_ID",
                table: "ApplicantSpecialities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AcceptedByClient",
                table: "ApplicantClockinClockOutTime",
                newName: "AccpetedByClient");

            migrationBuilder.RenameColumn(
                name: "ClockinClockOutTime_ID",
                table: "ApplicantClockinClockOutTime",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CeritificationImg",
                table: "ApplicantCertificates",
                newName: "CeritifcationImg");

            migrationBuilder.RenameColumn(
                name: "AppliedAllDays",
                table: "ApplicantAppliedShifts",
                newName: "AppliedAlldays");

          

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Notification_Templates_Notification_Template_ID",
                table: "Notifications",
                column: "Notification_Template_ID",
                principalTable: "Notification_Templates",
                principalColumn: "Notification_Template_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
