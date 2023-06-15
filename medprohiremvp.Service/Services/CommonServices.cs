using System;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.Service.IServices;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using System.Linq;

namespace medprohiremvp.Service.Services
{
    public class CommonServices : ICommonServices
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IEmailService _emailService;


        public CommonServices(ICommonRepository commonRepository, IEmailService emailService)
        {
            _commonRepository = commonRepository;
            _emailService = emailService;
        }
        public List<Countries> GetCountries(int? id = null)
        {
            return _commonRepository.GetCountries(id);
        }
        public List<States> GetStates(int countryid)
        {
            return _commonRepository.GetStates(countryid);
        }
        public List<States> GetAllStates(int countryid)
        {
            return _commonRepository.GetAllStates(countryid);
        }
        public List<Cities> GetCities(int stateid)
        {
            return _commonRepository.GetCities(stateid);
        }
        public List<Specialities> GetSpecialities()
        {
            return _commonRepository.GetSpecialities();
        }
        public List<CertificateTypes> GetCertificateTypes()
        {
            return _commonRepository.GetCertificateTypes();
        }
        public List<InstitutionTypes> GetInstitutionTypes()
        {
            return _commonRepository.GetInstitutionTypes();
        }
        public List<VisaStatuses> GetVisaStatuses()
        {
            return _commonRepository.GetVisaStatuses();
        }
        public List<Availabilities> GetAvailabilities()
        {
            return _commonRepository.GetAvailabilities();
        }
        public bool SaveApplicant(Applicants applicant, List<ApplicantCertificates> certificates, List<ApplicantSpecialities> specialities, List<ApplicantWorkHistories> workHistory)
        {
            return _commonRepository.SaveApplicant(applicant, certificates, specialities, workHistory);
        }
        public bool SaveClinicalIntitution(ClinicalInstitutions clinicalInstitution)
        {
            return _commonRepository.SaveClinicalIntitution(clinicalInstitution);
        }
        public Applicants FindApplicantByUserID(Guid Id)
        {

            Applicants applicant = _commonRepository.FindApplicantByUserID(Id);
            return applicant;
        }
        public ClinicalInstitutions FindClinicaByUserID(Guid Id)
        {

            ClinicalInstitutions clinical = _commonRepository.FindClinicaByUserID(Id);
            return clinical;
        }
        public bool AddBoardingProcessFileds(Applicants applicant, List<ApplicantReferences> references)
        {
            return _commonRepository.AddBoardingProcessFileds(applicant, references);
        }
        public bool AddClientShift(ClientShifts clientShift, List<ShiftSpecialities> shiftSpecialities)
        {
            return _commonRepository.AddClientShift(clientShift, shiftSpecialities);
        }
        public List<ClientShifts> GetClientShifts(int Institution_ID)
        {
            return _commonRepository.GetClientShifts(Institution_ID);
        }
        public List<int> GetShiftSpecialities(int clientShift_ID)
        {
            return _commonRepository.GetShiftSpecialities(clientShift_ID);
        }
        public Guid InsertEnvelopepId(SignSent envelope)
        {
            return _commonRepository.InsertEnvelopepId(envelope);
        }
        public SignSent GetSignSent(Guid id)
        {
            return _commonRepository.GetSignSent(id);
        }
        public void UpdateSignSended(SignSent sign)
        {
            _commonRepository.UpdateSignSended(sign);
        }
        public void UpdateApplicant(Applicants applicant)
        {
            _commonRepository.UpdateApplicant(applicant);
        }
        public void UpdateClinical(ClinicalInstitutions clinical)
        {
            _commonRepository.UpdateClinical(clinical);
        }
        public List<ClinicalInstitutionBranches> GetLocations(int Institution_ID)
        {
            return _commonRepository.GetLocations(Institution_ID);
        }
        public string GetCityName(int city_id)
        {
            return _commonRepository.GetCityName(city_id);
        }
        public bool AddLocation(ClinicalInstitutionBranches branch)
        {
            return _commonRepository.AddLocation(branch);
        }
        public ClinicalInstitutionBranches GetlocationbyId(int Branch_ID)
        {
            return _commonRepository.GetlocationbyId(Branch_ID);
        }
        public ClientShifts GetClientShiftByID(int ClientShift_ID)
        {
            return _commonRepository.GetClientShiftByID(ClientShift_ID);
        }
        public List<ShiftLabels> GetShiftLabels()
        {
            return _commonRepository.GetShiftLabels();
        }
        public latlong Getlatlng(int id, string tablename)
        {
            return _commonRepository.Getlatlng(id, tablename);
        }
        public latlong GetLatLongByAddress(string Address)
        {
            return _commonRepository.GetLatLongByAddress(Address);
        }
        public List<Applicants> Search( List<int> Specialities, DateTime StartDate, DateTime EndDate, int distance, float latitude, float longitude)
        {
            return _commonRepository.Search(Specialities, StartDate, EndDate, distance, latitude, longitude);
        }
        public List<Applicants> Search( List<int> Specialities,  DateTime StartDate, DateTime EndDate)
        {

            return _commonRepository.Search(Specialities, StartDate,EndDate);
        }
        public List<ApplicantSpecialities> GetApplicantSpecialities(int Applicant_ID)
        {
            return _commonRepository.GetApplicantSpecialities(Applicant_ID);
        }
        public List<ApplicantWorkHistories> GetApplicantWorkHistory(int Applicant_ID)
        {
            return _commonRepository.GetApplicantWorkHistory(Applicant_ID);
        }
        public List<ApplicantCertificates> GetApplicantCertificates(int Applicant_ID)
        {
            return _commonRepository.GetApplicantCertificates(Applicant_ID);
        }
        public List<ApplicantReferences> GetApplicantReferences(int Applicant_ID)
        {
            return _commonRepository.GetApplicantReferences(Applicant_ID);
        }
        public Cities GetCitiesByCityid(int city_id)
        {
            return _commonRepository.GetCitiesByCityid(city_id);
        }

        public Applicants GetApplicantbyId(int Id)
        {
            return _commonRepository.GetApplicantbyId(Id);
        }
        public List<DrugscreenStatuses> GetDrugscreenStatuses()
        {
            return _commonRepository.GetDrugscreenStatuses();
        }
        public bool AddVerificationKey(PhoneVerify verify)
        {
            return _commonRepository.AddVerificationKey(verify);
        }
        public bool RemoveVerificationKey(string phone)
        {
            return _commonRepository.RemoveVerificationKey(phone);

        }
        public PhoneVerify GetVerificationKey(string phone)
        {
            return _commonRepository.GetVerificationKey(phone);
        }
        public bool UpdateVerifiactionKey(PhoneVerify phone)
        {
            return _commonRepository.UpdateVerifiactionKey(phone);
        }
        public int AddNotification(Notifications notifications)
        {


            ApplicationUser user = _commonRepository.GetUserby_ID(notifications.User_ID);
            int not_id = _commonRepository.AddNotification(notifications);
            if (not_id > 0 && user != null)
            {
                NotificationTemplates temp = _commonRepository.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == notifications.NotificationTemplate_ID).First();

                return not_id;
            }
            else return 0;
        }
        public List<Notifications> GetUserNotifications(Guid user_id)
        {
            return _commonRepository.GetUserNotifications(user_id);
        }

        public SignSent GetEmploymentAgreementFile(Guid userid)
        {
            return _commonRepository.GetEmploymentAgreementFile(userid);
        }
        public List<NotificationTemplates> GetNotification_Templates()
        {
            return _commonRepository.GetNotification_Templates();
        }
        public bool RemoveNotifications(int Notification_ID)
        {
            return _commonRepository.RemoveNotifications(Notification_ID);
        }
        public bool UncheckNotifications(int Notification_ID)
        {
            return _commonRepository.UncheckNotifications(Notification_ID);
        }
        public List<Notifications> GetUserOldNotifications(Guid user_id)
        {
            return _commonRepository.GetUserOldNotifications(user_id);
        }
        public bool RemoveShift(int ClientShift_ID)
        {
            return _commonRepository.RemoveShift(ClientShift_ID);
        }
        public bool UpdateShift(ClientShifts ClientShift, List<ShiftSpecialities> shiftSpecialities)
        {
            return _commonRepository.UpdateShift(ClientShift, shiftSpecialities);
        }
        public bool RemoveLocation(int Branch_ID)
        {
            return _commonRepository.RemoveLocation(Branch_ID);
        }
        public bool UpdateLocation(ClinicalInstitutionBranches branches)
        {
            return _commonRepository.UpdateLocation(branches);
        }
        public List<ShiftCategory> GetShiftCategories()
        {
            return _commonRepository.GetShiftCategories();
        }
        public List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserID)
        {
            return _commonRepository.ApplicantSearchShift(cities, specialities, UserID);
        }
        public List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserID, int distance, float longitude, float latitude)
        {
            return _commonRepository.ApplicantSearchShift(cities, specialities, UserID, distance, longitude, latitude);
        }
        public bool AddApplicantAppliedShift(ApplicantAppliedShifts shifts)
        {
            return _commonRepository.AddApplicantAppliedShift(shifts);
        }
        public bool AcceptApplicantAppliedShift(ApplicantAppliedShifts shifts)
        {
            return _commonRepository.AcceptApplicantAppliedShift(shifts);
        }
        public bool AcceptClientShift(ClientShifts shift)
        {
            return _commonRepository.AcceptClientShift(shift);

        }
        public ClinicalInstitutions GetClinicalInstitution_byID(int ID)
        {
            return _commonRepository.GetClinicalInstitution_byID(ID);
        }
        public List<ApplicantAppliedShifts> GetApplicantAppliedShifts(Guid User_ID)
        {
            return _commonRepository.GetApplicantAppliedShifts(User_ID);
        }

        public ApplicantAppliedShifts GetAppliedShift(int AppliedShift_ID)
        {
            return _commonRepository.GetAppliedShift(AppliedShift_ID);
        }
        public bool UpdateApplicantAppliedShift(ApplicantAppliedShifts shift)
        {
            return _commonRepository.UpdateApplicantAppliedShift(shift);
        }
        public bool ConfirmApplicantAppliedShift(ApplicantAppliedShifts shift, List<ApplicantClockInClockOutTime> workingtime)
        {
            return _commonRepository.ConfirmApplicantAppliedShift(shift, workingtime);
        }
        public bool AddClockintime(ApplicantClockInClockOutTime applicantClockinClockOutTime)
        {
            return _commonRepository.AddClockintime(applicantClockinClockOutTime);
        }
        public List<ApplicantClockInClockOutTime> GetTodaysClockinShifts(Guid userID)
        {
            return _commonRepository.GetTodaysClockinShifts(userID);
        }
        public List<ApplicantClockInClockOutTime> GetNotClockinShifts(Guid userID)
        {
            return _commonRepository.GetNotClockinShifts(userID);

        }
        public bool UpdateClockin(ApplicantClockInClockOutTime clockin, Applicants app)
        {
            return _commonRepository.UpdateClockin(clockin, app);
        }
        public ApplicantClockInClockOutTime GetApplicantAcitveShift(Guid userID)
        {
            return _commonRepository.GetApplicantAcitveShift(userID);
        }
        public List<ApplicantClockInClockOutTime> GetAppliedShiftClockinClockouttimes(int AppliedShift_ID)
        {
            return _commonRepository.GetAppliedShiftClockinClockouttimes(AppliedShift_ID);
        }
        public List<ApplicantAppliedShifts> GetAppliedShiftsbyClientShift_ID(int ClientShift_ID)
        {
            return _commonRepository.GetAppliedShiftsbyClientShift_ID(ClientShift_ID);
        }
        public Administrators GetAdministratorbyID(Guid Admin_ID)
        {
            return _commonRepository.GetAdministratorbyID(Admin_ID);
        }
        public bool AddAdminChanges(AdminChanges changes)
        {
            return _commonRepository.AddAdminChanges(changes);
        }
        public List<ClientShifts> GetAllShifts()
        {
            return _commonRepository.GetAllShifts();
        }
        public List<ApplicantAppliedShifts> GetAllActiveShifts()
        {
            return _commonRepository.GetAllActiveShifts();
        }
        public int CountofCompletedShift(int AppliedShift_ID)
        {
            return _commonRepository.CountofCompletedShift(AppliedShift_ID);
        }
        public TimeSpan CountofWorkHours(int AppliedShift_ID)
        {
            return _commonRepository.CountofWorkHours(AppliedShift_ID);
        }
        public bool AddApplicantSpecialities(List<ApplicantSpecialities> specialities)
        {
            return _commonRepository.AddApplicantSpecialities(specialities);
        }
        public bool AddApplicantWorkHistory(List<ApplicantWorkHistories> workHistories)
        {
            return _commonRepository.AddApplicantWorkHistory(workHistories);
        }
        public bool AddApplicantCeritificates(List<ApplicantCertificates> certificates)
        {
            return _commonRepository.AddApplicantCeritificates(certificates);
        }
        public bool UpdateWorkHistory(ApplicantWorkHistories workHistory)
        {
            return _commonRepository.UpdateWorkHistory(workHistory);
        }
        public int GetAllLocationsCount()
        {
            return _commonRepository.GetAllLocationsCount();
        }
        public int GetContractorsCount(int Institution_ID)
        {
            return _commonRepository.GetContractorsCount(Institution_ID);
        }
        public int GetAllContractorsCount()
        {
            return _commonRepository.GetAllContractorsCount();
        }
        public int GetAllOnGoingShiftsCount()
        {
            return _commonRepository.GetAllOnGoingShiftsCount();

        }
        public int GetOnGoingShiftCount(int Institution_ID)
        {
            return _commonRepository.GetOnGoingShiftCount(Institution_ID);
        }
        public bool Add_PayCheck(PayChecks payCheck)
        {
           return _commonRepository.Add_PayCheck(payCheck);
        }
        public List<PayChecks> GetApplicantPayChecks(int Applicant_ID)
        {
            return _commonRepository.GetApplicantPayChecks(Applicant_ID);
        }
        public List<Applicants> GetContractors()
        {
            return _commonRepository.GetContractors();
        }
        public EmailTemplates GetEmailTemplates(int EmailTemplate_ID)
        {
            return _commonRepository.GetEmailTemplates(EmailTemplate_ID);
        }
        public bool AddEmploymentAgreement(EmploymentAgreements employmentAgreement)
        {
            return _commonRepository.AddEmploymentAgreement(employmentAgreement);
        }
        public EmploymentAgreements GetEmploymentAgreements(Guid User_ID)
        {
            return _commonRepository.GetEmploymentAgreements(User_ID);
        }
        public List<Tickets> GetUserTickets(Guid User_ID)
        {
            return _commonRepository.GetUserTickets(User_ID);
        }
        public List<TicketCategories> GetTicketCategories(bool IsClient)
        {
            return _commonRepository.GetTicketCategories(IsClient);
        }
        public bool AddTicket(Tickets ticket, TicketContents ticketContent)
        {
            return _commonRepository.AddTicket(ticket, ticketContent);
        }
        public List<TicketContents> GetTicketContents(int Ticket_ID)
        {
            return _commonRepository.GetTicketContents(Ticket_ID);
        }
        public TicketContents GetTicketContentByID(int TicketContent_ID)
        {
            return _commonRepository.GetTicketContentByID(TicketContent_ID);
        }
        public bool UpdateTicketContent(TicketContents TicketContent)
        {
            return _commonRepository.UpdateTicketContent(TicketContent);
        }
        public bool AddTicketContent(TicketContents ticketContent, Tickets ticket)
        {
            return _commonRepository.AddTicketContent(ticketContent, ticket);
        }
        public List<Tickets> GetAllTickets()
        {
          return  _commonRepository.GetAllTickets();
        }
        public Tickets GetTicketByID(int Ticket_ID)
        {
            return _commonRepository.GetTicketByID(Ticket_ID);
        }
        public List<Cities> GetShiftCities()
        {
            return _commonRepository.GetShiftCities();
        }
        public List<Cities> GetApplicantCities()
        {
            return _commonRepository.GetApplicantCities();
        }
        public List<Notifications> GetUserAllNotifications(Guid user_id)
        {
            return _commonRepository.GetUserAllNotifications(user_id);
        }
        public List<ApplicantAvailables> GetApplicantAvailables(int Applicant_ID)
        {
            return _commonRepository.GetApplicantAvailables(Applicant_ID);
        }
       public List<ApplicantAvailableTypes> GetAvailableTypes()
        {
            return _commonRepository.GetAvailableTypes();
        }
        public bool RemoveAllApplicantAvailables(int Applicant_ID)
        {
            return _commonRepository.RemoveAllApplicantAvailables(Applicant_ID);
        }
        public bool RemoveApplicantAvailables(int ApplicantAvailable_ID)
        {
            return _commonRepository.RemoveApplicantAvailables(ApplicantAvailable_ID);
        }
        public bool AddApplicantAvailables(List<ApplicantAvailables> applicantAvailables)
        {
            return _commonRepository.AddApplicantAvailables(applicantAvailables);
        }
        public bool AddApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            return _commonRepository.AddApplicantAvailable(applicantAvailables);
        }
        public bool UpdateApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            return _commonRepository.UpdateApplicantAvailable(applicantAvailables);
        }
        public bool RemoveApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            return _commonRepository.RemoveApplicantAvailable(applicantAvailables);
        }
        public List<ClientShifts> GetAvailableShiftsforApplicant(int Applicant_ID, int Institution_ID)
        {
            return _commonRepository.GetAvailableShiftsforApplicant(Applicant_ID, Institution_ID);
        }
        public bool UpdateTicket(Tickets ticket)
        {
            return _commonRepository.UpdateTicket(ticket);
        }
        public bool SendPhoneVerificationCode(string phonenumber)
        {
            return _commonRepository.SendPhoneVerificationCode(phonenumber);
        }
        public bool CheckFullApiKey(Guid ApiKey)
        {
            return _commonRepository.CheckFullApiKey(ApiKey);
        }
        public bool UpdateClientShift(ClientShifts shift)
        {
            return _commonRepository.UpdateClientShift(shift);
        }
        public bool UpdateClientShifts(List<ClientShifts> shifts)
        {
            return _commonRepository.UpdateClientShifts(shifts);
        }
        public bool UpdateApplicantSpecialtiesStatus(List<int> Specalities_ID, int Applicant_ID)
        {
            return _commonRepository.UpdateApplicantSpecialtiesStatus(Specalities_ID, Applicant_ID);
        }
         public List<Specialities> GetClientSpecialties(int Institution_ID)
        {
          return  _commonRepository.GetClientSpecialties(Institution_ID);
        }
        public List<ClientSpecialties> GetClientSpecialtiesList(int Institution_ID)
        {
            return _commonRepository.GetClientSpecialtiesList(Institution_ID);
        }
        public bool AddClientSpecialtiesCost(List<ClientSpecialtiesCosts> costs)
        {
            return _commonRepository.AddClientSpecialtiesCost(costs);
        }
        public bool AddClientSpecialites(List<ClientSpecialties> specialties)
        {
            return _commonRepository.AddClientSpecialites(specialties);
        }
        public List<ClientSpecialtiesCosts> GetClientSpecialtiesCostbyId(int ClientSpecialty_ID)
        {
            return _commonRepository.GetClientSpecialtiesCostbyId(ClientSpecialty_ID);
        }
        public bool AddClientCostChanges(List<ClientCostChanges> costChanges)
        {
            return _commonRepository.AddClientCostChanges(costChanges);
        }
        public List<ClientCostChanges> GetClientCostChanges(int Institution_ID)
        {
            return _commonRepository.GetClientCostChanges(Institution_ID);
        }
        public void UpdateAllExpiredShifts()
        {
             _commonRepository.UpdateAllExpiredShifts();
        }
      public  void UpdateClientExpiredShifts(int Institution_ID)
        {
            _commonRepository.UpdateClientExpiredShifts(Institution_ID);

        }
        public void Dispose()
        {
            _commonRepository.Dispose();
            GC.SuppressFinalize(this);
        }
        public List<Applicants> GetApplicantsPicked()
        {
            return _commonRepository.GetApplicantsPicked();
        }
        public List<Specialities> GetApplicantSpeciality()
        {
            return _commonRepository.GetApplicantSpeciality();
        }
        public List<Cities> GetApplicantCityName()
        {
            return _commonRepository.GetApplicantCityName();
        }
        public List<ApplicationUser> GetApplicantZipCode()
        {
            return _commonRepository.GetApplicantZipCode();
        }
        public List<Applicants> GetApplicantsShiftById()
        {
            return _commonRepository.GetApplicantsShiftById();
        }
        public IEnumerable<ApplicantAppliedShifts> GetApplicantsById()
        {
            return _commonRepository.AppApliendShifts();
        }
        public IEnumerable<ՕperatingApplicants> GetOperatingApplicants()
        {
            return _commonRepository.GetOperatingApplicants();
        }
        public bool SaveOperatingApplicants(ՕperatingApplicants օperate)
        {
            return _commonRepository.SaveOperatingApplicants(օperate);
        }
        //
        public ApplicantAppliedShifts GetAppApliedShift (int Applicant_Id ,int ClientShift_Id)
        {
            return _commonRepository.GetAppApliedShift(Applicant_Id, ClientShift_Id);
        }
        public IEnumerable<ApplicantAppliedShifts> GetAppApliedShifts()
        {
            return _commonRepository.GetAppApliedShifts();
        }
        public IEnumerable<ClientSpecialtiesCosts> GetClientSpecialityCost(int id)
        {
            return _commonRepository.GetClientSpecialityCost(id);
        }
    }
}
