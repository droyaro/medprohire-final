using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.Repo.Context;
using medprohiremvp.DATA.Entity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Geolocation;
using medprohiremvp.DATA.IdentityModels;
using System.Globalization;
using System.Net;
using System.IO;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.AspNetCore.Identity;

namespace medprohiremvp.Repo.Repository
{
    public class CommonRepository : ICommonRepository
    {
        protected readonly MvpDBContext _dbcontext;
        protected readonly AppUserContext _dbidentity;
        protected readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommonRepository(MvpDBContext Context, IConfiguration configuration, AppUserContext identity, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = Context;
            _configuration = configuration;
            _dbidentity = identity;
            _userManager = userManager;

        }
        public List<Countries> GetCountries(int? county_id = null)
        {
            return _dbcontext.Countries.ToList();
        }
        public List<States> GetStates(int countyid)
        {
            return _dbcontext.States.Where(x => x.country_id == countyid && x.id == 3970).ToList();
        }
        public List<States> GetAllStates(int countyid)
        {
            return _dbcontext.States.Where(x => x.country_id == countyid).ToList();
        }
        public List<Cities> GetCities(int stateid)
        {
            return _dbcontext.Cities.Where(x => x.state_id == stateid).ToList();
        }
        public List<Specialities> GetSpecialities()
        {
            return _dbcontext.Specialities.ToList();
        }
        public List<CertificateTypes> GetCertificateTypes()
        {
            return _dbcontext.CertificateTypes.ToList();
        }
        public List<InstitutionTypes> GetInstitutionTypes()
        {
            return _dbcontext.InstitutionTypes.ToList();
        }
        public List<VisaStatuses> GetVisaStatuses()
        {
            return _dbcontext.VisaStatuses.ToList();
        }
        public List<Availabilities> GetAvailabilities()
        {
            return _dbcontext.Availabilities.ToList();
        }
        public bool SaveApplicant(Applicants applicant, List<ApplicantCertificates> certificates, List<ApplicantSpecialities> specialities, List<ApplicantWorkHistories> workHistory)
        {
            using (var dbContextTransaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    _dbcontext.Applicants.Add(applicant);
                    _dbcontext.SaveChanges();
                    applicant.PreferredID = applicant.FirstName.First() + applicant.LastName.First() + "0000" + applicant.Applicant_ID; if (certificates != null)
                    {
                        certificates.Select(x => { return x.Applicant_ID = applicant.Applicant_ID; }).ToList();
                        certificates.Select(x => { return x.CertificateType_ID = 1; }).ToList();
                        _dbcontext.ApplicantCertificates.AddRange(certificates);
                    }
                    specialities.Select(x => { return x.Applicant_ID = applicant.Applicant_ID; }).ToList();
                    _dbcontext.ApplicantSpecialities.AddRange(specialities);
                    if (workHistory != null)
                    {
                        workHistory.Select(x => { return x.Applicant_ID = applicant.Applicant_ID; }).ToList();
                        _dbcontext.ApplicantWorkHistories.AddRange(workHistory);
                    }
                    _dbcontext.SaveChanges();

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                { return false; }
            }
        }
        public bool SaveClinicalIntitution(ClinicalInstitutions clinicalInstitution)
        {
            try
            {
                _dbcontext.ClinicalInstitutions.Add(clinicalInstitution);
                _dbcontext.SaveChanges();
                //specialties.Select(x => { return x.Institution_ID = clinicalInstitution.Institution_ID; }).ToList();
                //_dbcontext.ClientSpecialties.AddRange(specialties);
                //_dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool AddBoardingProcessFileds(Applicants applicant, List<ApplicantReferences> references)
        {
            using (var dbContextTransaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    _dbcontext.Applicants.Update(applicant);
                    references.Select(x => { return x.Applicant_ID = applicant.Applicant_ID; }).ToList();
                    _dbcontext.ApplicantReferences.AddRange(references);
                    _dbcontext.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                { return false; }
            }

        }
        public Applicants FindApplicantByUserID(Guid Id)
        {

            return _dbcontext.Applicants.FirstOrDefault(x => x.User_ID == Id);
            
        }
        public ClinicalInstitutions FindClinicaByUserID(Guid Id)
        {
            return _dbcontext.ClinicalInstitutions.FirstOrDefault(x => x.User_ID == Id);
        }
        public bool AddClientShift(ClientShifts clientShift, List<ShiftSpecialities> shiftSpecialities)
        {
            using (var dbContextTransaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    clientShift.Category_ID = 1;
                    clientShift.Available = true;
                    _dbcontext.ClientShifts.Add(clientShift);
                    _dbcontext.SaveChanges();
                    shiftSpecialities.Select(x => { return x.ClientShift_ID = clientShift.ClientShift_ID; }).ToList();
                    _dbcontext.ShiftSpecialities.AddRange(shiftSpecialities);
                    _dbcontext.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                { return false; }
            }
        }
        public List<ClientShifts> GetClientShifts(int institution_ID)
        {
            List<ClientShifts> clientShiftlist = new List<ClientShifts>();
            clientShiftlist = _dbcontext.ClientShifts.Where(x => x.Institution_ID == institution_ID).ToList();
            return clientShiftlist;
        }
        public List<int> GetShiftSpecialities(int clientShift_ID)
        {
            List<int> shiftSpecialities = new List<int>();
            shiftSpecialities = _dbcontext.ShiftSpecialities.Where(x => x.ClientShift_ID == clientShift_ID).Select(s => s.Speciality_ID).ToList();
            return shiftSpecialities;
        }
        public Guid InsertEnvelopepId(SignSent envelope)
        {
            try
            {
                Guid id = new Guid();
                envelope.SignSended_ID = id;
                if (envelope.FileType.Contains("_adminsign"))
                {
                    _dbcontext.SignSent.RemoveRange(_dbcontext.SignSent.Where(x => x.User_ID == envelope.User_ID && x.FileType.Contains(envelope.FileType.Substring(0, envelope.FileType.LastIndexOf('_')))).ToList());
                    switch (envelope.FileType.Substring(0, envelope.FileType.LastIndexOf('_')))
                    {
                        case "1":
                            Applicants app = _dbcontext.Applicants.Where(x => x.User_ID == envelope.User_ID).First();
                            app.Employment_agreement = null;
                            _dbcontext.Applicants.Update(app);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    _dbcontext.SignSent.RemoveRange(_dbcontext.SignSent.Where(x => x.User_ID == envelope.User_ID && x.FileType == envelope.FileType).ToList());
                }
                _dbcontext.SaveChanges();
                _dbcontext.SignSent.Add(envelope);
                _dbcontext.SaveChanges();
                return envelope.SignSended_ID;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }
        public SignSent GetSignSent(Guid id)
        {
            return _dbcontext.SignSent.Where(x => x.SignSended_ID == id).FirstOrDefault();
        }
        public void UpdateSignSended(SignSent sign)
        {
            _dbcontext.SignSent.Update(sign);
            _dbcontext.SaveChanges();
        }
        public void UpdateApplicant(Applicants applicant)
        {
            _dbcontext.Applicants.Update(applicant);
            _dbcontext.SaveChanges();
        }
        public void UpdateClinical(ClinicalInstitutions clinical)
        {
            _dbcontext.ClinicalInstitutions.Update(clinical);
            _dbcontext.SaveChanges();
        }
        public List<ClinicalInstitutionBranches> GetLocations(int Institution_ID)
        {
            return _dbcontext.ClinicalInstitutionBranches.Where(x => x.Institution_ID == Institution_ID).ToList();
        }
        public string GetCityName(int city_id)
        {
            string cityName = "";
            var list = _dbcontext.Cities.Include(s => s.state.state_name).Include(c => c.country.shortname).Where(x => x.id == city_id).Select(x => new { x.city_name, x.country.shortname, x.state.state_name }).FirstOrDefault();
            cityName = list.city_name + ", " + list.state_name + ", " + list.shortname;
            return cityName;
        }
        public bool AddLocation(ClinicalInstitutionBranches branch)
        {
            bool answer = false;
            try
            {
                _dbcontext.ClinicalInstitutionBranches.Add(branch);
                _dbcontext.SaveChanges();
                answer = true;
            }
            catch
            {
                answer = false;
            }
            return answer;
        }
        public ClinicalInstitutionBranches GetlocationbyId(int Branch_ID)
        {
            return _dbcontext.ClinicalInstitutionBranches.Where(x => x.Branch_ID == Branch_ID).FirstOrDefault();
        }
        public ClientShifts GetClientShiftByID(int ClientShift_ID)
        {
            return _dbcontext.ClientShifts.Where(x => x.ClientShift_ID == ClientShift_ID).FirstOrDefault();
        }
        public List<ShiftLabels> GetShiftLabels()
        {
            return _dbcontext.ShiftLabels.ToList();
        }
        public latlong Getlatlng(int id, string tablename)
        {
            latlong latlng = new latlong();
            if (tablename.ToLower() == "city")
            {
                latlng = new latlong()
                {
                    Latitude = _dbcontext.Cities.Where(X => X.id == id).Select(x => x.Latitude).FirstOrDefault(),
                    Longitude = _dbcontext.Cities.Where(X => X.id == id).Select(x => x.Longitude).FirstOrDefault(),
                };
            }
            return latlng;

        }
        public latlong GetLatLongByAddress(string Address)
        {
            latlong latlong = new latlong();
            if (!String.IsNullOrEmpty(Address))
            {
                string key = "AlzQYgoKdeKQfiLzXDkBdKQp2_oCQnWug-wNqdhpsjtFpZs2-S7BTi_chzUQLMQX";
                string query = Address;
                float latitude = 0;
                float longitude = 0;
                string zipcode = "";
                string text;
                try
                {
                    Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}&o=xml", query, key));
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(geocodeRequest);
                    using (WebResponse response = webRequest.GetResponse())
                    {
                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            text = sr.ReadToEnd();

                        }
                        string point = text.Substring(text.IndexOf("<Point>") + "<Point>".Length, text.IndexOf("</Point>") - text.IndexOf("<Point>") - "<Point>".Length);
                        zipcode = text.Substring(text.IndexOf("<PostalCode>") + "<PostalCode>".Length, text.IndexOf("</PostalCode>") - text.IndexOf("<PostalCode>") - "<PostalCode>".Length);
                        float.TryParse(point.Substring(point.IndexOf("<Latitude>") + "<Latitude>".Length, point.IndexOf("</Latitude>") - point.IndexOf("<Latitude>") - "<Latitude>".Length), out latitude);
                        float.TryParse(point.Substring(point.IndexOf("<Longitude>") + "<Longitude>".Length, point.IndexOf("</Longitude>") - point.IndexOf("<Longitude>") - "<Longitude>".Length), out longitude);
                        latlong.Latitude = latitude;
                        latlong.Longitude = longitude;
                        latlong.ZipCode = zipcode;
                    }
                }
                catch { }
            }
            return latlong;
        }
        public List<Applicants> Search(List<int> Specialities, DateTime StartDate, DateTime EndDate)
        {

            List<Applicants> applicants = new List<Applicants>();
            var userid = _dbidentity.UserRoles.Where(x => x.RoleId == _dbidentity.Roles.Where(xr => xr.Name == "Applicant").Select(xr => xr.Id).FirstOrDefault()).Select(x => x.UserId).ToList();
            if (userid != null)
            {
                applicants = _dbcontext.Applicants.
                    Where(x => x.IsAvailable)
                .Where(x => x.BoardingProcess == 3 && x.Employment_agreement != null)
                .Where(x => userid.Contains(x.User_ID)).ToList();
                int count = applicants.Count;
                List<Applicants> valid = new List<Applicants>();
                if (applicants.Count > 0)
                {

                    for (int i = 0; i < count; i++)
                    {

                        if (Specialities != null)
                        {
                            try
                            {

                                var appspec = _dbcontext.ApplicantSpecialities.Where(x => x.Applicant_ID == applicants[i].Applicant_ID && x.Status == 1).Where(x => Specialities.Contains(x.Speciality_ID)).Select(x => x.Applicant_ID).ToList();
                                if (appspec.Count == 0)
                                {
                                    valid.Add(applicants[i]);
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (StartDate != DateTime.MinValue && EndDate!=DateTime.MinValue)
                        {
                            try
                            {

                                var availapp = _dbcontext.ApplicantAvailables.Where(x => x.Applicant_ID == applicants[i].Applicant_ID && x.AvailableDay>=StartDate && x.AvailableDay<=EndDate).Select(x => x.Applicant_ID).ToList();
                                if (availapp.Count == 0)
                                {
                                    valid.Add(applicants[i]);
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    if (valid.Count > 0)
                    {
                        foreach (Applicants invalid in valid)
                        {
                            applicants.Remove(invalid);
                        }
                    }
                }
            }

            return applicants;
        }
        public List<Applicants> Search(List<int> Specialities, DateTime StartDate, DateTime EndDate, int distance, float latitude, float longitude)
        {
            List<Applicants> applicants = new List<Applicants>();
            List<Applicants> invalids = new List<Applicants>();
            if (distance > 0)
            {
                applicants = Search(Specialities, StartDate,EndDate);

                if (applicants.Count > 0)
                {

                    for (int i = 0; i < applicants.Count; i++)
                    {
                        int dist = Convert.ToInt32(GeoCalculator.GetDistance(latitude, longitude, _dbidentity.Users.Where(x => x.Id == applicants[i].User_ID).First().Latitude, _dbidentity.Users.Where(x => x.Id == applicants[i].User_ID).First().Longitude, 2, distanceUnit: DistanceUnit.Miles));

                        if (dist >= distance)
                        {
                            invalids.Add(applicants[i]);
                        }
                    }

                }
            }
            if (invalids.Count > 0)
            {
                foreach (Applicants invalid in invalids)
                {
                    applicants.Remove(invalid);
                }
            }
            return applicants;
        }

        public List<ApplicantSpecialities> GetApplicantSpecialities(int Applicant_ID)
        {

            List<ApplicantSpecialities> applicantSpecialities = _dbcontext.ApplicantSpecialities.Where(x => x.Applicant_ID == Applicant_ID).ToList();

            return applicantSpecialities;
        }

        public List<ApplicantWorkHistories> GetApplicantWorkHistory(int Applicant_ID)
        {
            List<ApplicantWorkHistories> workHistories = new List<ApplicantWorkHistories>();
            workHistories = _dbcontext.ApplicantWorkHistories.Where(x => x.Applicant_ID == Applicant_ID).ToList();
            return workHistories;

        }
        public List<ApplicantCertificates> GetApplicantCertificates(int Applicant_ID)
        {
            List<ApplicantCertificates> certificates = new List<ApplicantCertificates>();
            certificates = _dbcontext.ApplicantCertificates.Where(x => x.Applicant_ID == Applicant_ID).ToList();
            return certificates;
        }
        public List<ApplicantReferences> GetApplicantReferences(int Applicant_ID)
        {
            List<ApplicantReferences> applicantReferences = new List<ApplicantReferences>();
            applicantReferences = _dbcontext.ApplicantReferences.Where(x => x.Applicant_ID == Applicant_ID).ToList();
            return applicantReferences;
        }

        public Cities GetCitiesByCityid(int city_id)
        {
            return _dbcontext.Cities.Where(x => x.id == city_id).FirstOrDefault();
        }
        public Applicants GetApplicantbyId(int Id)
        {
            Applicants applicant = new Applicants();
            applicant = _dbcontext.Applicants.FirstOrDefault(x => x.Applicant_ID == Id);
            return applicant;
        }

        public List<DrugscreenStatuses> GetDrugscreenStatuses()
        {
            return _dbcontext.DrugscreenStatuses.ToList();
        }
        public bool AddVerificationKey(PhoneVerify verify)
        {
            try
            {
                _dbcontext.PhoneVerify.AddAsync(verify);
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public PhoneVerify GetVerificationKey(string phone)
        {
            return _dbcontext.PhoneVerify.Where(x => x.PhoneNumber == phone).FirstOrDefault();
        }
        public bool UpdateVerifiactionKey(PhoneVerify phone)
        {
            try
            {
                _dbcontext.PhoneVerify.Update(phone);
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool RemoveVerificationKey(string phone)
        {
            try
            {
                _dbcontext.PhoneVerify.RemoveRange(_dbcontext.PhoneVerify.Where(x => x.PhoneNumber == phone));
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public int AddNotification(Notifications notifications)
        {
            notifications.Status = 1;
            try
            {
                _dbcontext.Notifications.Add(notifications);
                _dbcontext.SaveChanges();
                return notifications.Notification_ID;
            }
            catch { }
            return 0;

        }
        public ApplicationUser GetUserby_ID(Guid user_id)
        {
            return _dbidentity.Users.Where(x => x.Id == user_id).FirstOrDefault();
        }
        public List<Notifications> GetUserNotifications(Guid user_id)
        {

            List<Notifications> notifications = _dbcontext.Notifications.Where(c => c.User_ID == user_id && c.Status == 1).ToList();

            return notifications;
        }
        public List<Notifications> GetUserOldNotifications(Guid user_id)
        {

            List<Notifications> notifications = _dbcontext.Notifications.Where(c => c.User_ID == user_id && c.Status == 0).ToList();

            return notifications;
        }

        public SignSent GetEmploymentAgreementFile(Guid userid)
        {
            SignSent file = new SignSent();
            List<SignSent> emplist = _dbcontext.SignSent.Where(x => x.User_ID == userid && x.Status == "downloaded" && x.FileType.Substring(0, x.FileType.LastIndexOf('_')) == "1").ToList();
            if (emplist.Count > 0)
            {
                if (emplist.Count == 1)
                {
                    file = emplist[0];
                }
                else
                {
                    file = emplist.Where(x => x.FileType.Contains("_usersign")).FirstOrDefault();
                }
            }
            return file;
        }
        public List<NotificationTemplates> GetNotification_Templates()
        {
            return _dbcontext.NotificationTemplates.ToList();
        }

        public bool RemoveNotifications(int Notification_ID)
        {
            try
            {
                List<Notifications> notes = _dbcontext.Notifications.Where(x => x.Notification_ID == Notification_ID).ToList();
                if (notes != null)
                {
                    foreach (Notifications note in notes)
                    {
                        note.Status = 2;
                    }
                }
                _dbcontext.Notifications.UpdateRange(notes);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }
        public bool UncheckNotifications(int Notification_ID)
        {
            try
            {
                List<Notifications> notes = _dbcontext.Notifications.Where(x => x.Notification_ID == Notification_ID).ToList();
                if (notes != null)
                {
                    foreach (Notifications note in notes)
                    {
                        note.Status = 0;
                    }
                }
                _dbcontext.Notifications.UpdateRange(notes);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }
        public bool RemoveShift(int ClientShift_ID)
        {
            try
            {
                _dbcontext.ClientShifts.Remove(_dbcontext.ClientShifts.Where(x => x.ClientShift_ID == ClientShift_ID).First());
                _dbcontext.ShiftSpecialities.RemoveRange(_dbcontext.ShiftSpecialities.Where(x => x.ClientShift_ID == ClientShift_ID));
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool UpdateShift(ClientShifts ClientShift, List<ShiftSpecialities> shiftSpecialities)
        {
            try
            {
                _dbcontext.ClientShifts.Update(ClientShift);
                _dbcontext.ShiftSpecialities.RemoveRange(_dbcontext.ShiftSpecialities.Where(x => x.ClientShift_ID == ClientShift.ClientShift_ID));
                shiftSpecialities.Select(x => { return x.ClientShift_ID = ClientShift.ClientShift_ID; }).ToList();
                _dbcontext.ShiftSpecialities.AddRange(shiftSpecialities);
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool RemoveLocation(int Branch_ID)
        {
            try
            {
                _dbcontext.ClinicalInstitutionBranches.Remove(_dbcontext.ClinicalInstitutionBranches.Where(x => x.Branch_ID == Branch_ID).First());
                //List<ClientShift> shifts=_dbcontext.ClientShift.Where(x => x.Branch_ID == Branch_ID).ToList();
                //if(shifts.Count>0)
                //{
                //    foreach(var shift in shifts)
                //    {
                //        shift.Branch_ID = null;
                //    }
                //    _dbcontext.ClientShift.UpdateRange(shifts);
                //}
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateLocation(ClinicalInstitutionBranches branches)
        {
            try
            {
                _dbcontext.ClinicalInstitutionBranches.Update(branches);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ShiftCategory> GetShiftCategories()
        {
            return _dbcontext.ShiftCategories.ToList();
        }

        public List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserId)
        {
            List<ClientShifts> shifts = new List<ClientShifts>();
            List<int> clientshiftid = new List<int>();
            clientshiftid = GetApplicantAppliedShifts(UserId).Select(x => x.ClientShift_ID).ToList();
            shifts = _dbcontext.ClientShifts.Where(x => x.Available == true && !(clientshiftid.Contains(x.ClientShift_ID)) && x.ShiftExpirationDate>=DateTime.Now).ToList();
            if (shifts.Count > 0)
            {
                if (cities != null)
                {

                    shifts = shifts.Where(X =>
                     (X.Branch_ID != null && cities.Contains(_dbcontext.ClinicalInstitutionBranches.Where(c => c.Branch_ID == X.Branch_ID).Select(c => c.CityId).First())) //search by shift branch cities
                     ||
                     (X.Branch_ID == null && cities.Contains(GetUserby_ID(_dbcontext.ClinicalInstitutions.Where(a => a.Institution_ID == X.Institution_ID).Select(a => a.User_ID).First()).City_ID))).ToList(); //search by institution cities if branch=null
                }
                if (specialities != null)
                {
                    int count = shifts.Count;
                    List<ClientShifts> removableshifts = new List<ClientShifts>();
                    for (int i = 0; i < count; i++)
                    {
                        if (_dbcontext.ShiftSpecialities.Where(x => x.ClientShift_ID == shifts[i].ClientShift_ID).Where(s => specialities.Contains(s.Speciality_ID)).Select(x => x.ClientShift_ID).ToList().Count == 0)
                        {
                            removableshifts.Add(shifts[i]);
                        }
                    }
                    if (removableshifts.Count > 0)
                    {
                        foreach (ClientShifts remove in removableshifts)
                        {
                            shifts.Remove(remove);
                        }
                    }

                }

            }
            return shifts;
        }
        public List<ClientShifts> ApplicantSearchShift(List<int> cities, List<int> specialities, Guid UserID, int distance, float longitude, float latitude)
        {
            List<ClientShifts> shifts = ApplicantSearchShift(cities, specialities, UserID);
            List<ClientShifts> invalids = new List<ClientShifts>();
            if (shifts.Count > 0 && distance > 0)
            {
                for (int i = 0; i < shifts.Count; i++)
                {
                    Guid user_ID = _dbcontext.ClinicalInstitutions.Where(x => x.Institution_ID == shifts[i].Institution_ID).Select(x => x.User_ID).FirstOrDefault();
                    float shiftlong = shifts[i].Branch_ID == null ? _dbidentity.Users.Where(x => x.Id == user_ID).Select(x => x.Longitude).FirstOrDefault() :
                        _dbcontext.ClinicalInstitutionBranches.Where(x => x.Branch_ID == shifts[i].Branch_ID).Select(x => x.Longitude).FirstOrDefault();
                    float shiftlat = shifts[i].Branch_ID == null ? _dbidentity.Users.Where(x => x.Id == user_ID).Select(x => x.Latitude).FirstOrDefault() :
                       _dbcontext.ClinicalInstitutionBranches.Where(x => x.Branch_ID == shifts[i].Branch_ID).Select(x => x.Latitude).FirstOrDefault();
                    int dist = Convert.ToInt32(GeoCalculator.GetDistance(latitude, longitude, shiftlat, shiftlong, 2, distanceUnit: DistanceUnit.Miles));

                    if (dist >= distance)
                    {
                        invalids.Add(shifts[i]);
                    }
                }
                if (invalids.Count > 0)
                {
                    foreach (ClientShifts invalid in invalids)
                    {
                        shifts.Remove(invalid);
                    }
                }

            }
            return shifts;

        }

        public bool AddApplicantAppliedShift(ApplicantAppliedShifts shifts)
        {

            try
            {
                _dbcontext.ApplicantAppliedShifts.Add(shifts);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AcceptApplicantAppliedShift(ApplicantAppliedShifts shifts)
        {
            try
            {


                shifts.Accepted = true;
                _dbcontext.ApplicantAppliedShifts.Update(shifts);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AcceptClientShift(ClientShifts shift)
        {
            try
            {
                shift.Available = true;
                _dbcontext.ClientShifts.Update(shift);
                _dbcontext.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<ApplicantAppliedShifts> GetApplicantAppliedShifts(Guid User_ID)
        {
            var v =  _dbcontext.ApplicantAppliedShifts.Where(x => x.Applicant_ID == _dbcontext.Applicants.Where(a => a.User_ID == User_ID).Select(a => a.Applicant_ID).First()).ToList();
           // _dbcontext.SaveChanges();
            return v;
        }
        public ClinicalInstitutions GetClinicalInstitution_byID(int ID)
        {
            return _dbcontext.ClinicalInstitutions.Where(x => x.Institution_ID == ID).FirstOrDefault();
        }
        public ApplicantAppliedShifts GetAppliedShift(int AppliedShift_ID)
        {
            return _dbcontext.ApplicantAppliedShifts.Where(x => x.AppliedShift_ID == AppliedShift_ID).First();
        }
        public bool UpdateApplicantAppliedShift(ApplicantAppliedShifts shift)
        {
            try
            {
                _dbcontext.ApplicantAppliedShifts.Update(shift);
                _dbcontext.SaveChanges();
                return true;

            }
            catch (Exception ex) { return false; }
        }
        public bool ConfirmApplicantAppliedShift(ApplicantAppliedShifts shift, List<ApplicantClockInClockOutTime> workingtime)
        {
            using (var dbContextTransaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {

                    _dbcontext.ApplicantAppliedShifts.Update(shift);
                    _dbcontext.ApplicantClockInClockOutTime.AddRange(workingtime);
                    ClientShifts clientShift = GetClientShiftByID(shift.ClientShift_ID);
                    clientShift.Category_ID = 2;
                    _dbcontext.ClientShifts.Update(clientShift);
                    _dbcontext.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }
        public bool AddClockintime(ApplicantClockInClockOutTime applicantClockinClockOutTime)
        {
            try
            {
                _dbcontext.Update(applicantClockinClockOutTime);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ApplicantClockInClockOutTime> GetTodaysClockinShifts(Guid userID)
        {
            ApplicantClockInClockOutTime activeshift = GetApplicantAcitveShift(userID);
            int Applicant_ID = _dbcontext.Applicants.Where(x => x.User_ID == userID).Select(x => x.Applicant_ID).FirstOrDefault();
            if (Applicant_ID > 0)
            {
                List<int> applicantshift_id = _dbcontext.ApplicantAppliedShifts
                .Where(sh => sh.Applicant_ID == Applicant_ID && sh.Accepted == true)
                .Select(sh => sh.AppliedShift_ID).ToList();
                List<ApplicantClockInClockOutTime> clockOutTimes = _dbcontext.ApplicantClockInClockOutTime.Where(x => applicantshift_id.Contains(x.AppliedShift_ID) && x.ClockInTime == DateTime.MinValue).ToList();
                if (clockOutTimes.Count > 0 && activeshift != null)
                {
                    if (clockOutTimes.Where(x => x.ClockinClockOutTime_ID == activeshift.ClockinClockOutTime_ID).FirstOrDefault() != null)
                    {
                        clockOutTimes.Remove(clockOutTimes.Where(x => x.ClockinClockOutTime_ID == activeshift.ClockinClockOutTime_ID).First());
                    }
                }
                return clockOutTimes;
            }
            return null;
        }
        public List<ApplicantClockInClockOutTime> GetNotClockinShifts(Guid userID)
        {
            int Applicant_ID = _dbcontext.Applicants.Where(x => x.User_ID == userID).Select(x => x.Applicant_ID).FirstOrDefault();
            if (Applicant_ID > 0)
            {
                List<int> applicantshift_id = _dbcontext.ApplicantAppliedShifts
                .Where(sh => sh.Applicant_ID == Applicant_ID && sh.Accepted == true)
                .Select(sh => sh.AppliedShift_ID).ToList();
                List<ApplicantClockInClockOutTime> clockOutTimes = _dbcontext.ApplicantClockInClockOutTime.Where(x => applicantshift_id.Contains(x.AppliedShift_ID) && x.ClockInTime == DateTime.MinValue).ToList();
                List<ApplicantClockInClockOutTime> valid = new List<ApplicantClockInClockOutTime>();
                foreach (ApplicantClockInClockOutTime time in clockOutTimes)
                {
                    DateTime worktime = time.WorkingDay.Add(time.WorkStartTime.TimeOfDay);
                    if ((worktime - DateTime.Now).Days < 0 || ((worktime - DateTime.Now).Days == 0 && (worktime - DateTime.Now).Hours < 0))
                    {
                        valid.Add(time);
                    }
                }
                return valid;
            }
            return null;
        }
        public ApplicantClockInClockOutTime GetApplicantAcitveShift(Guid userID)
        {
            Applicants Applicant = _dbcontext.Applicants.Where(x => x.User_ID == userID).FirstOrDefault();
            if (Applicant.Applicant_ID > 0 && Applicant.Atwork)
            {
                List<int> applicantshift_id = _dbcontext.ApplicantAppliedShifts
                .Where(sh => sh.Applicant_ID == Applicant.Applicant_ID && sh.Accepted == true)
                .Select(sh => sh.AppliedShift_ID).ToList();
                ApplicantClockInClockOutTime clockinShift = _dbcontext.ApplicantClockInClockOutTime.Where(x => applicantshift_id.Contains(x.AppliedShift_ID) && x.ClockInTime != DateTime.MinValue && x.ClockOutTime == DateTime.MinValue).FirstOrDefault();

                return clockinShift;
            }
            return null;
        }
        public bool UpdateClockin(ApplicantClockInClockOutTime clockin, Applicants app)
        {
            using (var dbContextTransaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    _dbcontext.Applicants.Update(app);
                    _dbcontext.ApplicantClockInClockOutTime.Update(clockin);
                    dbContextTransaction.Commit();
                    return true;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }
        public List<ApplicantClockInClockOutTime> GetAppliedShiftClockinClockouttimes(int AppliedShift_ID)
        {
            List<ApplicantClockInClockOutTime> times = new List<ApplicantClockInClockOutTime>();

            times = _dbcontext.ApplicantClockInClockOutTime.Where(x => x.AppliedShift_ID == AppliedShift_ID).ToList();

            return times;
        }
        public List<ApplicantAppliedShifts> GetAppliedShiftsbyClientShift_ID(int ClientShift_ID)
        {
            List<ApplicantAppliedShifts> appliedShifts = new List<ApplicantAppliedShifts>();
            appliedShifts = _dbcontext.ApplicantAppliedShifts.Where(x => x.ClientShift_ID == ClientShift_ID).ToList();
            return appliedShifts;
        }
        public Administrators GetAdministratorbyID(Guid Admin_ID)
        {
            return _dbcontext.Administrators.Where(x => x.Admin_ID == Admin_ID).FirstOrDefault();
        }
        public bool AddAdminChanges(AdminChanges changes)
        {
            try
            {
                _dbcontext.AdminChanges.Add(changes);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ClientShifts> GetAllShifts()
        {
            return _dbcontext.ClientShifts.Where(x => x.Available == true).ToList();

        }
        public List<ApplicantAppliedShifts> GetAllActiveShifts()
        {
            return _dbcontext.ApplicantAppliedShifts.Where(x => x.Accepted == true && x.Status == 1).ToList();
        }
        public int CountofCompletedShift(int AppliedShift_ID)
        {
            List<ApplicantClockInClockOutTime> t = _dbcontext.ApplicantClockInClockOutTime.Where(x => x.AppliedShift_ID == AppliedShift_ID && x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
            if (t != null)
            {
                return t.Count;
            }
            return 0;
        }
        public TimeSpan CountofWorkHours(int AppliedShift_ID)
        {
            TimeSpan time = TimeSpan.Zero;
            List<ApplicantClockInClockOutTime> t = _dbcontext.ApplicantClockInClockOutTime.Where(x => x.AppliedShift_ID == AppliedShift_ID && x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
            if (t != null)
            {
                foreach (ApplicantClockInClockOutTime clockinClockOutTime in t)
                {
                    time += (clockinClockOutTime.ClockOutTime - clockinClockOutTime.ClockInTime).Duration();
                }
            }
            return time;
        }
        public bool AddApplicantSpecialities(List<ApplicantSpecialities> specialities)
        {
            try
            {
                _dbcontext.ApplicantSpecialities.AddRange(specialities);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddApplicantWorkHistory(List<ApplicantWorkHistories> workHistories)
        {
            try
            {
                _dbcontext.ApplicantWorkHistories.AddRange(workHistories);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AddApplicantCeritificates(List<ApplicantCertificates> certificates)
        {
            try
            {
                certificates.Select(x => { return x.CertificateType_ID = 1; }).ToList();
                _dbcontext.ApplicantCertificates.AddRange(certificates);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateWorkHistory(ApplicantWorkHistories workHistory)
        {
            try
            {
                _dbcontext.ApplicantWorkHistories.Update(workHistory);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetAllLocationsCount()
        {
            return _dbcontext.ClinicalInstitutionBranches.Count() + _dbcontext.ClinicalInstitutions.Count();
        }
        public int GetContractorsCount(int Institution_ID)
        {
            if (GetClientShifts(Institution_ID).Count != 0)
            {
                List<int> Clientshiftid = GetClientShifts(Institution_ID).Select(c => c.ClientShift_ID).ToList();
                List<ApplicantAppliedShifts> appliedshifts = _dbcontext.ApplicantAppliedShifts.Where(x => Clientshiftid.Contains(x.ClientShift_ID) && x.Accepted && x.Status == 1).ToList();
                List<int> applicant_ID = appliedshifts != null ? appliedshifts.Select(x => x.Applicant_ID).ToList() : new List<int>();
                if (applicant_ID.Count != 0)
                {
                    return applicant_ID.Distinct().ToList().Count;
                }
            }

            return 0;
        }
        public int GetAllContractorsCount()
        {
            return _dbcontext.Applicants.Where(x => x.BoardingProcess == 3 && x.Employment_agreement != null).Count();
        }
        public int GetAllOnGoingShiftsCount()
        {
            return _dbcontext.ApplicantAppliedShifts.Where(x => x.Accepted == true && x.Status == 1).Select(x => x.ClientShift_ID).Distinct().ToList().Count;

        }
        public int GetOnGoingShiftCount(int Institution_ID)
        {
            List<ClientShifts> shifts = GetClientShifts(Institution_ID);
            if (shifts.Count != 0)
            {
                List<int> shiftid = shifts.Select(c => c.ClientShift_ID).ToList();
                List<ApplicantAppliedShifts> clientshiftid = _dbcontext.ApplicantAppliedShifts.Where(x => shiftid.Contains(x.ClientShift_ID) && x.Accepted && x.Status == 1).ToList();
                if (clientshiftid.Count != 0)
                {
                    return clientshiftid.Select(x => x.ClientShift_ID).Distinct().ToList().Count;
                }
            }

            return 0;
        }
        public bool Add_PayCheck(PayChecks payCheck)
        {
            try
            {
                _dbcontext.PayChecks.Add(payCheck);
                _dbcontext.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<PayChecks> GetApplicantPayChecks(int Applicant_ID)
        {
            List<PayChecks> payChecks = new List<PayChecks>();
            payChecks = _dbcontext.PayChecks.Where(x => x.Applicant_ID == Applicant_ID).ToList();
            return payChecks;
        }
        public List<Applicants> GetContractors()
        {
            return _dbcontext.Applicants.Where(x => x.BoardingProcess == 3 && x.Employment_agreement != null).ToList();
        }
        public EmailTemplates GetEmailTemplates(int EmailTemplate_ID)
        {
            return _dbcontext.EmailTemplates.Where(x => x.EmailTemplate_ID == 1).FirstOrDefault();
        }
        public bool AddEmploymentAgreement(EmploymentAgreements employmentAgreement)
        {
            try
            {
                _dbcontext.Add(employmentAgreement);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public EmploymentAgreements GetEmploymentAgreements(Guid User_ID)
        {
            return _dbcontext.EmploymentAgreements.Where(x => x.User_ID == User_ID).LastOrDefault();
        }
        public List<Tickets> GetUserTickets(Guid User_ID)
        {
            return _dbcontext.Tickets.Where(x => x.User_ID == User_ID).ToList();
        }
        public List<TicketCategories> GetTicketCategories(bool IsClient)
        {
            return _dbcontext.TicketCategories.Where(x => x.IsClient == IsClient).ToList();
        }
        public bool AddTicket(Tickets ticket, TicketContents ticketContent)
        {
            try
            {
                _dbcontext.Tickets.Add(ticket);
                _dbcontext.SaveChanges();
                ticketContent.Ticket_ID = ticket.Ticket_ID;
                _dbcontext.TicketContents.Add(ticketContent);
                _dbcontext.SaveChanges();
                return true;

            }

            catch (Exception ex)
            {
                return false;
            }
        }
        public List<TicketContents> GetTicketContents(int Ticket_ID)
        {
            return _dbcontext.TicketContents.Where(x => x.Ticket_ID == Ticket_ID).OrderBy(x => x.InsertDate).ToList();
        }
        public TicketContents GetTicketContentByID(int TicketContent_ID)
        {
            return _dbcontext.TicketContents.Where(x => x.TicketContent_ID == TicketContent_ID).FirstOrDefault();
        }

        public bool UpdateTicketContent(TicketContents ticketContents)
        {
            try
            {

                _dbcontext.TicketContents.Update(ticketContents);
                _dbcontext.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public bool AddTicketContent(TicketContents ticketContent, Tickets ticket)
        {
            try
            {
                _dbcontext.TicketContents.Add(ticketContent);
                _dbcontext.Tickets.Update(ticket);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Tickets> GetAllTickets()
        {
            return _dbcontext.Tickets.ToList();
        }
        public Tickets GetTicketByID(int Ticket_ID)
        {
            return _dbcontext.Tickets.Where(x => x.Ticket_ID == Ticket_ID).FirstOrDefault();
        }
        public List<Cities> GetShiftCities()
        {
            List<ClientShifts> shiftsList = _dbcontext.ClientShifts.Where(x => x.Available == true).ToList();
            var CitiesList = new List<Cities>();
            if (shiftsList != null)
            {
                foreach (ClientShifts shift in shiftsList)
                {
                    if (shift.Branch_ID != null)
                    {
                        ClinicalInstitutionBranches branches = _dbcontext.ClinicalInstitutionBranches.Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                        CitiesList.Add(_dbcontext.Cities.Where(x => branches != null ? x.id == branches.CityId : false).FirstOrDefault());
                    }
                    else
                    {
                        ClinicalInstitutions clinical = _dbcontext.ClinicalInstitutions.Where(i => i.Institution_ID == shift.Institution_ID).FirstOrDefault();
                        if (clinical != null)
                        {
                            ApplicationUser user = _dbidentity.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                            CitiesList.Add(_dbcontext.Cities.Where(x => user != null ? x.id == user.City_ID : false).FirstOrDefault());
                        }

                    }
                }
            }
            if (CitiesList != null)
            {
                return CitiesList.Distinct().ToList();
            }
            return CitiesList;
        }
        public List<Cities> GetApplicantCities()
        {
            var CitiesList = new List<Cities>();
            var role_id = _dbidentity.Roles.Where(x => x.Name == "Applicant").Select(x => x.Id).FirstOrDefault();
            var useridlist = _dbidentity.UserRoles.Where(x => x.RoleId == role_id).Select(x => x.UserId).ToList();
            var contractorsidlist = _dbcontext.Applicants.Where(x => x.Employment_agreement != null).Select(x => x.User_ID).ToList();
            var citiesidlist = _dbidentity.Users.Where(x => useridlist.Contains(x.Id) && contractorsidlist.Contains(x.Id)).Select(x => x.City_ID).Distinct().ToList();
            CitiesList = _dbcontext.Cities.Where(x => citiesidlist.Contains(x.id)).ToList();
            return CitiesList;
        }
        public List<Notifications> GetUserAllNotifications(Guid user_id)
        {

            List<Notifications> notifications = _dbcontext.Notifications.Where(c => c.User_ID == user_id).ToList();

            return notifications;
        }
        public List<ApplicantAvailables> GetApplicantAvailables(int Applicant_ID)
        {
            return _dbcontext.ApplicantAvailables.Where(x => x.Applicant_ID == Applicant_ID && (x.AvailableDay-DateTime.Now).Days>-2).ToList();
        }
        public List<ApplicantAvailableTypes> GetAvailableTypes()
        {
            return _dbcontext.ApplicantAvailableTypes.ToList();
        }
        public bool RemoveAllApplicantAvailables(int Applicant_ID)
        {
            try
            {
                var applicantAvailables = _dbcontext.ApplicantAvailables.Where(x => x.Applicant_ID == Applicant_ID).ToList();
                _dbcontext.ApplicantAvailables.RemoveRange(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool RemoveApplicantAvailables(int ApplicantAvailable_ID)
        {
            try
            {
                var applicantAvailables = _dbcontext.ApplicantAvailables.Where(x => x.ApplicantAvailable_ID == ApplicantAvailable_ID).ToList();
                _dbcontext.ApplicantAvailables.RemoveRange(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool AddApplicantAvailables(List<ApplicantAvailables> applicantAvailables)
        {
            try
            {
                _dbcontext.AddRange(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            try
            {
                _dbcontext.Add(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            try
            {
                _dbcontext.Update(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveApplicantAvailable(ApplicantAvailables applicantAvailables)
        {
            try
            {
                _dbcontext.Remove(applicantAvailables);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ClientShifts> GetAvailableShiftsforApplicant(int Applicant_ID, int Institution_ID)
        {
            Applicants applicant = _dbcontext.Applicants.Where(x => x.Applicant_ID == Applicant_ID).FirstOrDefault();
            if (applicant != null)
            {
                List<ClientShifts> AvailableClientShift = new List<ClientShifts>();
                if (applicant.IsAvailable)
                {
                    List<int> appSpec = _dbcontext.ApplicantSpecialities.Where(x => x.Applicant_ID == applicant.Applicant_ID && x.Status == 1).Select(x => x.Speciality_ID).ToList();
                    List<ApplicantAvailables> applicantAvailables = _dbcontext.ApplicantAvailables.Where(x => x.Applicant_ID == Applicant_ID).ToList();
                    
                        var days = applicantAvailables.Select(x=>x.AvailableDay.ToString("MM/dd/yyyy")).ToList();

                        List<ClientShifts> clientShifts = _dbcontext.ClientShifts.Where(x => x.Institution_ID == Institution_ID && x.Category_ID <= 2 ).ToList();
                        foreach (ClientShifts clientShift in clientShifts)
                        {
                            var specCount = _dbcontext.ShiftSpecialities.Where(x => x.ClientShift_ID == clientShift.ClientShift_ID && appSpec.Contains(x.Speciality_ID)).Count();
                            if (specCount > 0)
                            {
                            var shiftdays= clientShift.ShiftsDates.Split(',');
                                foreach (var date in shiftdays)
                                {
                                    if (days.Contains(date))
                                    {
                                        AvailableClientShift.Add(clientShift);
                                        break;
                                    }
                                }
                            }

                        }
                    
                    if (AvailableClientShift.Count > 0)
                    {
                        return (AvailableClientShift);
                    }
                    else
                    {
                        return (null);
                    }
                }
            }
            return (null);
        }
        public bool UpdateTicket(Tickets ticket)

        {
            try
            {
                _dbcontext.Tickets.Update(ticket);
                _dbcontext.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool SendPhoneVerificationCode(string phonenumber)
        {
            try
            {
                Random random = new Random();
                string VerificationCode = random.Next(111111, 999999).ToString();
                const string accountSid = "ACa82cd9df891fc540407af7f6b9d8e6cf";
                const string authToken = "67b6b7ff066d3e6de38b0e6918a9416d";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    body: "Your verification code for Medprohire is " + VerificationCode,
                    from: new PhoneNumber("+12013080024"),
                    to: new PhoneNumber(phonenumber)
                );

                Console.WriteLine(message.Sid);
                Guid id = new Guid();
                phonenumber = "+1" + phonenumber;
                PhoneVerify phone = new PhoneVerify { PhoneVerify_ID = id, PhoneNumber = phonenumber, VerificationCode = VerificationCode, IsVerified = false };
                RemoveVerificationKey(phonenumber);
                AddVerificationKey(phone);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckFullApiKey(Guid ApiKey)
        {
            var ApiKeys = _dbcontext.ApiKeys.Where(x => x.ApiKey == ApiKey && x.Role == "Full").FirstOrDefault();
            if (ApiKeys != null)
            {
                return true;
            }
            return false;
        }
        public bool UpdateClientShift(ClientShifts shift)
        {
            try
            {
                _dbcontext.ClientShifts.Update(shift);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateClientShifts(List<ClientShifts> shifts)
        {
            try
            {
                _dbcontext.ClientShifts.UpdateRange(shifts);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateApplicantSpecialtiesStatus(List<int> Specalities_ID, int Applicant_ID)
        {
            var appspec = _dbcontext.ApplicantSpecialities.Where(x => x.Applicant_ID == Applicant_ID && Specalities_ID.Contains(x.Speciality_ID)).ToList();
            if (appspec != null)
            {
                appspec.Select(x => { return x.Status = 1; }).ToList();
                _dbcontext.ApplicantSpecialities.UpdateRange(appspec);
                _dbcontext.SaveChanges();
                return true;
            }
            return false;
        }
        public List<Specialities> GetClientSpecialties(int Institution_ID)
        {
            var clientspecialties = _dbcontext.ClientSpecialties.Where(x => x.Institution_ID == Institution_ID).Select(x => x.Speciality_ID).ToList();
            return _dbcontext.Specialities.Where(x => clientspecialties.Contains(x.Speciality_ID)).ToList();
        }
        public List<ClientSpecialties> GetClientSpecialtiesList(int Institution_ID)
        {
            return _dbcontext.ClientSpecialties.Where(x => x.Institution_ID == Institution_ID).ToList();
        }
        public bool AddClientSpecialtiesCost(List<ClientSpecialtiesCosts> costs)
        {
            if (costs != null)
            {
                try
                {
                    _dbcontext.ClientSpecialtiesCosts.AddRange(costs);
                    _dbcontext.SaveChanges();
                    var clintid = _dbcontext.ClientSpecialties.Where(x => x.ClientSpeciality_ID == costs[0].ClientSpeciality_ID).Select(x => x.Institution_ID).FirstOrDefault();
                    var client = _dbcontext.ClinicalInstitutions.Where(x => x.Institution_ID == clintid).FirstOrDefault();
                    client.Status = 3;
                    _dbcontext.ClinicalInstitutions.Update(client);
                    _dbcontext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public bool AddClientSpecialites(List<ClientSpecialties> specialties)
        {
            try
            {
                _dbcontext.ClientSpecialties.AddRange(specialties);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ClientSpecialtiesCosts> GetClientSpecialtiesCostbyId(int ClientSpecialty_ID)
        {
            return _dbcontext.ClientSpecialtiesCosts.Where(x => x.ClientSpeciality_ID == ClientSpecialty_ID).ToList();
        }
        public bool AddClientCostChanges(List<ClientCostChanges> costChanges)
        {

            // Making old datas about Client cost status 0(not valid), then adding changes
            if (costChanges != null)
            {
                if (costChanges.Count > 0)
                {
                    try
                    {
                        List<ClientCostChanges> oldchanges = _dbcontext.ClientCostChanges.Where(x => x.Institution_ID == costChanges[0].Institution_ID).ToList();

                        if (oldchanges != null)
                        {
                            if (oldchanges.Count > 0)
                            {
                                oldchanges.Select(x => { return x.status =false; }).ToList();
                                _dbcontext.ClientCostChanges.UpdateRange(oldchanges);
                                _dbcontext.SaveChanges();
                            }
                        }
                     
                        _dbcontext.ClientCostChanges.AddRange(costChanges);
                        _dbcontext.SaveChanges();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public List<ClientCostChanges> GetClientCostChanges(int Institution_ID)
        {
            return _dbcontext.ClientCostChanges.Where(x => x.Institution_ID == Institution_ID && x.status==true).ToList();
        }
        public void UpdateAllExpiredShifts()
        {
            var expired=_dbcontext.ClientShifts.Where(x => x.Category_ID <=2 && x.ShiftExpirationDate < DateTime.Now).ToList();
            expired.Select(x => { return x.Category_ID = 5; }).ToList();
            if (expired.Count > 0)
            {
                _dbcontext.ClientShifts.UpdateRange(expired);
                _dbcontext.SaveChanges();
            }

        }
        public void UpdateClientExpiredShifts(int Institution_ID)
        {
            var expired = _dbcontext.ClientShifts.Where(x => x.Category_ID <=2 && x.ShiftExpirationDate < DateTime.Now && x.Institution_ID==Institution_ID).ToList();
            expired.Select(x => { return x.Category_ID = 5; }).ToList();
            if (expired.Count > 0)
            {
                _dbcontext.ClientShifts.UpdateRange(expired);
                _dbcontext.SaveChanges();
            }

        }
        public void Dispose()
        {
            _dbcontext.Dispose();
            _dbidentity.Dispose();
            GC.SuppressFinalize(this);
        }
        public List<Applicants> GetApplicantsPicked()
        {
            return _dbcontext.Applicants.Where(x => x.Picked == true).ToList();
        }

        public List<Specialities> GetApplicantSpeciality()
        {
            List<Applicants> applicants = GetApplicantsPicked();
            var specialities = (from speciality in _dbcontext.Specialities
                                join appspeciality in _dbcontext.ApplicantSpecialities on speciality.Speciality_ID equals appspeciality.Speciality_ID
                                join applic in applicants on appspeciality.Applicant_ID equals applic.Applicant_ID
                                select new Specialities
                                {
                                    SpecialityName = speciality.SpecialityName
                                }).ToList();
            return specialities;
        }

        public List<Cities> GetApplicantCityName()
        {
            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
            List<Applicants> applicants = GetApplicantsPicked();
            var cityNames = (from appuser in appusers
                             join city in _dbcontext.Cities on appuser.City_ID equals city.id
                             join applicant in applicants on appuser.Id equals applicant.User_ID
                             select new Cities
                             {
                                 city_name = city.city_name
                             }).ToList();
            return cityNames;
        }

        public List<ApplicationUser> GetApplicantZipCode()
        {
            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
            List<Applicants> applicants = GetApplicantsPicked();
            var zipCode = (from appuser in appusers
                           join ap in applicants on appuser.Id equals ap.User_ID
                           select new ApplicationUser
                           {
                               ZipCode = appuser.ZipCode
                           }).ToList();
            return zipCode;

        }

        public List<Applicants> GetApplicantsShiftById()
        {

            var applied = AppApliendShifts();
            var s = (from applicAppliend in applied
                     join applicant in _dbcontext.Applicants on applicAppliend.Applicant_ID equals applicant.Applicant_ID
                     select new Applicants
                     {
                         FirstName = applicant.FirstName,
                         LastName = applicant.LastName,
                         ProfileImage = applicant.ProfileImage
                     }).ToList();
            return s;
        }

        public IEnumerable<ApplicantAppliedShifts> AppApliendShifts()
        {
            return _dbcontext.ApplicantAppliedShifts.ToList();
        }


        public IEnumerable<ՕperatingApplicants> GetOperatingApplicants()
        {

            var appliendShift = AppApliendShifts();
            List<Applicants> applicantPicked = GetApplicantsPicked();
            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();

            var city = (from applicantApplShift in _dbcontext.ApplicantAppliedShifts
                        join picked in applicantPicked on applicantApplShift.Applicant_ID equals picked.Applicant_ID
                        join appuser in appusers on picked.User_ID equals appuser.Id
                        join cities in _dbcontext.Cities on appuser.City_ID equals cities.id
                        select new ՕperatingApplicants
                        {
                            Applicant_ID = picked.Applicant_ID,
                            City = cities.city_name,
                            LastName = picked.LastName,
                            FirstName = picked.FirstName,
                            ClientShift_ID = applicantApplShift.ClientShift_ID,
                        }).ToList();

            return city;
        }

        public bool SaveOperatingApplicants(ՕperatingApplicants օperate)
        {

            try
            {
                _dbcontext.ՕperatingApplicants.Add(օperate);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //

        public ApplicantAppliedShifts GetAppApliedShift(int Applicant_Id, int ClientShift_Id)
        {
            return _dbcontext.ApplicantAppliedShifts.FirstOrDefault(x => x.Applicant_ID == Applicant_Id && x.ClientShift_ID == ClientShift_Id);
        }

        public IEnumerable<ApplicantAppliedShifts> GetAppApliedShifts()
        {
            return _dbcontext.ApplicantAppliedShifts.Where(x => x.Applicant_ID != 0).ToList();
        }

        public IEnumerable<ClientSpecialtiesCosts> GetClientSpecialityCost(int id)
        {
            return _dbcontext.ClientSpecialtiesCosts.Where(c => c.ClientSpeciality_ID == id);
        }
    }
}
