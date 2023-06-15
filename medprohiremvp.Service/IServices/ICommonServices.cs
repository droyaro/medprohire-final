﻿using System;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;

namespace medprohiremvp.Service.IServices
{
    public interface ICommonServices : IDisposable
    {
        List<Countries> GetCountries(int? country_id = null);
        List<States> GetStates(int country_id);
        List<States> GetAllStates(int country_id);
        List<Cities> GetCities(int state_id);
        List<Specialities> GetSpecialities();
        List<CertificateTypes> GetCertificateTypes();
        List<InstitutionTypes> GetInstitutionTypes();
        List<VisaStatuses> GetVisaStatuses();
        List<Availabilities> GetAvailabilities();
        bool SaveApplicant(Applicants applicant, List<ApplicantCertificates> certificates, List<ApplicantSpecialities> specialities, List<ApplicantWorkHistories> workHistory);
        bool SaveClinicalIntitution(ClinicalInstitutions clinicalInstitution);
        Applicants FindApplicantByUserID(Guid Id);
        ClinicalInstitutions FindClinicaByUserID(Guid id);
        bool AddBoardingProcessFileds(Applicants applicant, List<ApplicantReferences> references);
        bool AddClientShift(ClientShifts clientShift, List<ShiftSpecialities> shiftSpecialities);
        List<ClientShifts> GetClientShifts(int Institution_ID);
        List<int> GetShiftSpecialities(int clientShift_ID);
        Guid InsertEnvelopepId(SignSent envelope);
        SignSent GetSignSent(Guid id);
        void UpdateSignSended(SignSent sign);
        void UpdateApplicant(Applicants applicant);
        void UpdateClinical(ClinicalInstitutions clinical);
        List<ClinicalInstitutionBranches> GetLocations(int Institution_ID);
        string GetCityName(int city_id);
        bool AddLocation(ClinicalInstitutionBranches branch);
        ClinicalInstitutionBranches GetlocationbyId(int Branch_ID);
        ClientShifts GetClientShiftByID(int ClientShift_ID);
        List<ShiftLabels> GetShiftLabels();
        latlong Getlatlng(int id, string tablename);
        latlong GetLatLongByAddress(string Address);
        List<Applicants> Search(List<int> Specialities, DateTime StartDate, DateTime EndDate, int distance, float latitude, float longitude);
        List<Applicants> Search(List<int> Specialities, DateTime StartDate, DateTime EndDate);
        List<ApplicantSpecialities> GetApplicantSpecialities(int Applicant_ID);
        List<ApplicantWorkHistories> GetApplicantWorkHistory(int Applicant_ID);
        List<ApplicantCertificates> GetApplicantCertificates(int Applicant_ID);
        List<ApplicantReferences> GetApplicantReferences(int Applicant_ID);
        Cities GetCitiesByCityid(int city_id);
        Applicants GetApplicantbyId(int Id);
        List<DrugscreenStatuses> GetDrugscreenStatuses();
        bool AddVerificationKey(PhoneVerify verify);
        bool RemoveVerificationKey(string phone);
        PhoneVerify GetVerificationKey(string phone);
        bool UpdateVerifiactionKey(PhoneVerify phone);
        int AddNotification(Notifications notifications);
        List<Notifications> GetUserNotifications(Guid user_id);
        SignSent GetEmploymentAgreementFile(Guid userid);
        List<NotificationTemplates> GetNotification_Templates();
        bool RemoveNotifications(int Notification_ID);
        bool UncheckNotifications(int Notification_ID);
        List<Notifications> GetUserOldNotifications(Guid user_id);
        bool RemoveShift(int ClientShift_ID);
        bool UpdateShift(ClientShifts ClientShift, List<ShiftSpecialities> shiftSpecialities);
        bool RemoveLocation(int Branch_ID);
        bool UpdateLocation(ClinicalInstitutionBranches branches);
        List<ShiftCategory> GetShiftCategories();
        List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserId);
        List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserId, int distance, float longitude, float latitude);
        bool AddApplicantAppliedShift(ApplicantAppliedShifts shifts);
        bool AcceptApplicantAppliedShift(ApplicantAppliedShifts shifts);
        bool AcceptClientShift(ClientShifts shift);
        ClinicalInstitutions GetClinicalInstitution_byID(int ID);
        List<ApplicantAppliedShifts> GetApplicantAppliedShifts(Guid User_ID);
        ApplicantAppliedShifts GetAppliedShift(int AppliedShift_ID);
        bool UpdateApplicantAppliedShift(ApplicantAppliedShifts shift);
        bool ConfirmApplicantAppliedShift(ApplicantAppliedShifts shift, List<ApplicantClockInClockOutTime> workingtime);
        bool AddClockintime(ApplicantClockInClockOutTime applicantClockinClockOutTime);
        List<ApplicantClockInClockOutTime> GetTodaysClockinShifts(Guid userID);
        List<ApplicantClockInClockOutTime> GetNotClockinShifts(Guid userID);
        bool UpdateClockin(ApplicantClockInClockOutTime clockin, Applicants app);
        ApplicantClockInClockOutTime GetApplicantAcitveShift(Guid userID);
        List<ApplicantClockInClockOutTime> GetAppliedShiftClockinClockouttimes(int AppliedShift_ID);
        List<ApplicantAppliedShifts> GetAppliedShiftsbyClientShift_ID(int ClientShift_ID);
        Administrators GetAdministratorbyID(Guid Admin_ID);
        bool AddAdminChanges(AdminChanges changes);
        List<ClientShifts> GetAllShifts();
        List<ApplicantAppliedShifts> GetAllActiveShifts();
        int CountofCompletedShift(int AppliedShift_ID);
        TimeSpan CountofWorkHours(int AppliedShift_ID);
        bool AddApplicantSpecialities(List<ApplicantSpecialities> specialities);
        bool AddApplicantWorkHistory(List<ApplicantWorkHistories> workHistories);
        bool AddApplicantCeritificates(List<ApplicantCertificates> certificates);
        bool UpdateWorkHistory(ApplicantWorkHistories workHistory);
        int GetAllLocationsCount();
        int GetContractorsCount(int Institution_ID);
        int GetAllContractorsCount();
        int GetAllOnGoingShiftsCount();
        int GetOnGoingShiftCount(int Institution_ID);
        bool Add_PayCheck(PayChecks payCheck);
        List<PayChecks> GetApplicantPayChecks(int Applicant_ID);
        List<Applicants> GetContractors();
        EmailTemplates GetEmailTemplates(int EmailTemplate_ID);
        bool AddEmploymentAgreement(EmploymentAgreements employmentAgreement);
        EmploymentAgreements GetEmploymentAgreements(Guid User_ID);
        List<Tickets> GetUserTickets(Guid User_ID);
        List<TicketCategories> GetTicketCategories(bool IsClient);
        bool AddTicket(Tickets ticket, TicketContents ticketContent);
        List<TicketContents> GetTicketContents(int Ticket_ID);
        TicketContents GetTicketContentByID(int TicketContent_ID);
        bool UpdateTicketContent(TicketContents TicketContent);
        bool AddTicketContent(TicketContents ticketContent, Tickets ticket);
        List<Tickets> GetAllTickets();
        Tickets GetTicketByID(int Ticket_ID);
        List<Cities> GetShiftCities();
        List<Cities> GetApplicantCities();
        List<Notifications> GetUserAllNotifications(Guid user_id);
        List<ApplicantAvailables> GetApplicantAvailables(int Applicant_ID);
        List<ApplicantAvailableTypes> GetAvailableTypes();
        bool RemoveAllApplicantAvailables(int Applicant_ID);
        bool AddApplicantAvailables(List<ApplicantAvailables> applicantAvailables);
        bool AddApplicantAvailable(ApplicantAvailables applicantAvailables);
        bool UpdateApplicantAvailable(ApplicantAvailables applicantAvailables);
        bool RemoveApplicantAvailable(ApplicantAvailables applicantAvailables);
        bool RemoveApplicantAvailables(int ApplicantAvailable_ID);
        List<ClientShifts> GetAvailableShiftsforApplicant(int Applicant_ID, int Institution_ID);
        bool UpdateTicket(Tickets ticket);
        bool SendPhoneVerificationCode(string phonenumber);
        bool CheckFullApiKey(Guid ApiKey);
        bool UpdateClientShift(ClientShifts shift);
        bool UpdateClientShifts(List<ClientShifts> shifts);
        bool UpdateApplicantSpecialtiesStatus(List<int> Specalities_ID, int Applicant_ID);
        List<Specialities> GetClientSpecialties(int Institution_ID);
        List<ClientSpecialties> GetClientSpecialtiesList(int Institution_ID);
        bool AddClientSpecialtiesCost(List<ClientSpecialtiesCosts> costs);
        bool AddClientSpecialites(List<ClientSpecialties> specialties);
        List<ClientSpecialtiesCosts> GetClientSpecialtiesCostbyId(int ClientSpecialty_ID);

        bool AddClientCostChanges(List<ClientCostChanges> costChanges);
        List<ClientCostChanges> GetClientCostChanges(int Institution_ID);
        void UpdateAllExpiredShifts();
        void UpdateClientExpiredShifts(int Institution_ID);
        List<Applicants> GetApplicantsPicked();
        List<Specialities> GetApplicantSpeciality();
        List<Cities> GetApplicantCityName();
        List<ApplicationUser> GetApplicantZipCode();
        List<Applicants> GetApplicantsShiftById();
        IEnumerable<ApplicantAppliedShifts> GetApplicantsById();
        IEnumerable<ՕperatingApplicants> GetOperatingApplicants();
        bool SaveOperatingApplicants(ՕperatingApplicants օperate);
        //
        ApplicantAppliedShifts GetAppApliedShift(int Applicant_Id, int ClientShift_Id);
        IEnumerable<ApplicantAppliedShifts> GetAppApliedShifts();
        IEnumerable<ClientSpecialtiesCosts> GetClientSpecialityCost(int id);
    }
}
