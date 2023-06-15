using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using System.IO;
using medprohiremvp.Models;
using medprohiremvp.Service.SignSend;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Net.Http;
using medprohiremvp.Models.Applicant;
using medprohiremvp.Models.ClinicalInstitution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Geolocation;
using System.Net;
using System.Diagnostics;
using medprohiremvp.Service.EmailServices;

namespace medprohiremvp.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantController : Controller
    {
        private readonly ICommonServices _commonService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISignature _signature;
        private readonly IHostingEnvironment _environment;
        private readonly TimespanModel _timespanModel;
        private readonly IEmailService _emailService;
        private readonly IAppliedShiftServices _appliedShiftServices;

        private readonly RootPath _rootpath;
        private readonly string savefiletypeprefix = "usersign";
        public ApplicantController(UserManager<ApplicationUser> userManager, ICommonServices commonServices, IHostingEnvironment environment, ISignature signature, IOptions<TimespanModel> timespanModel, IOptions<RootPath> rootPath, IEmailService emailService, IAppliedShiftServices appliedShiftServices)
        {
            _userManager = userManager;
            _commonService = commonServices;
            _signature = signature;
            _environment = environment;
            _timespanModel = timespanModel.Value;
            _rootpath = rootPath.Value;
            _emailService = emailService;
            _appliedShiftServices = appliedShiftServices;


        }

        [HttpGet]
        public IActionResult Employment_Agreement()
        {
            string button = "";
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                Applicants applicant = _commonService.FindApplicantByUserID(userId);
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (applicant.Employment_agreement == null)
                {
                    button = "Sign Document";
                }
                else
                {
                    button = "Download";
                }
            }

            return View("Employment_Agreement", button);
        }
        [HttpPost]
        public async Task<IActionResult> Employment_Agreement_Sign()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                Applicants applicant = _commonService.FindApplicantByUserID(userId);
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (applicant.Employment_agreement == null)
                {
                    SignSent signsend = _commonService.GetEmploymentAgreementFile(userId);

                    if (signsend.FilePath != null)
                    {
                        string file = _rootpath.AdminRoot + signsend.FilePath;
                        var callbackUrl = Url.Action("SignCompleted", "SignDocument", new { }, Request.Scheme);
                        string url = _signature.Geturlsignature("1_" + savefiletypeprefix, file, user.Email, applicant.LastName + " " + applicant.FirstName, userId, callbackUrl, signsend.Emp_XPosition, signsend.Emp_YPosition, signsend.Emp_PageNumber, signsend.Emp_XPosition, signsend.Emp_YPosition, signsend.Emp_PageNumber);
                        return Redirect(url);
                    }
                }
                else
                {

                    string filepath = _rootpath.UserRoot + applicant.Employment_agreement;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                    return File(fileBytes, "application/x-msdownload", "Employment_agreement.pdf");
                }

            }
            return RedirectToAction("Employment_Agreement", "Applicant");
        }
        public async Task Employment_Agreement_File()
        {

            string file = "";
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                SignSent signsend = _commonService.GetEmploymentAgreementFile(userId);

                if (signsend.FilePath != null)
                {
                    byte[] array;
                    string root = signsend.FileType.Contains("admin") ? _rootpath.AdminRoot : _rootpath.UserRoot;
                    file = root + signsend.FilePath;
                    HttpContext.Response.OnStarting(state =>
                    {
                        using (HttpContext.Response.Body)
                        {

                            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                array = new Byte[stream.Length];
                                stream.Read(array, 0, array.Length);

                            }
                            HttpContext.Response.Headers.Clear();
                            HttpContext.Response.Headers.Add("Content-Disposition", $"inline; filename=Employment_agreement.pdf");
                            HttpContext.Response.ContentType = "application/pdf";
                            HttpContext.Response.Headers.ContentLength = array.Length;
                            HttpContext.Response.Body.WriteAsync(array, 0, array.Length);
                            HttpContext.Response.Body.FlushAsync();
                            return Task.CompletedTask;
                        }
                    }, null);

                }
            }

        }


        [HttpGet]
        [Route("Applicant/AvailableShifts")]
        public IActionResult Applicant_ShiftSearch()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Applicants applicant = new Applicants();
            ViewBag.ZipCode = user.ZipCode;
            applicant.Picked = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Status == 0 && x.Accepted == false).Count() > 0 ? true : false;
            AppShiftSearchViewModel model = new AppShiftSearchViewModel();
            model.Search = new ShiftSearchViewModel();
            model.Shifts = new List<AppliedShifts>();
            model.picked = applicant.Picked;
            model.PickedShifts = new List<AppliedShifts>();

            var ShiftCitiesList = _commonService.GetShiftCities();

            List<ClientShifts> clientShifts = _commonService.ApplicantSearchShift(null, null, userId);

            if (clientShifts.Count > 0)
            {
                foreach (ClientShifts clientShift in clientShifts)
                {
                    string LocationName = "";
                    string InstitutionName = "";
                    string Location = "";

                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                    InstitutionName = institution.InstitutionName;
                    if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;

                    }
                    else
                    {

                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                    }
                    int dist = Convert.ToInt32(GeoCalculator.GetDistance(user.Latitude, user.Longitude, clinical.Latitude, clinical.Longitude, 2, distanceUnit: DistanceUnit.Miles));
                    model.Shifts.Add(new AppliedShifts()
                    {
                        Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                        ClientShift_ID = clientShift.ClientShift_ID,
                        ClockInTime = clientShift.ClockInTime,
                        ClockOutTime = clientShift.ClockOutTime,
                        BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                        Consecutive_Occurrences = clientShift.Consecutive_Occurrences,

                        DateOfShift = clientShift.DateOfShift,
                        EndDate = clientShift.EndDate,


                        Institution_ID = clientShift.Institution_ID,
                        Occurrences = clientShift.Occurrences,
                        Responsibility = clientShift.Responsibility,
                        ShiftDescription = clientShift.ShiftDescription,
                        ShiftExpirationDate = clientShift.ShiftExpirationDate,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                        ShiftLabel_ID = clientShift.ShiftLabel_ID,
                        StartDate = clientShift.StartDate,
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                        Applied = false,
                        Location = Location,
                        Distance = dist,
                        AppliedAllDays = false,
                        AppliedDaysList = "",
                        InstitutionName = institution.InstitutionName,
                        ShiftsDates = clientShift.ShiftsDates



                    });
                }
            }
            List<ApplicantAppliedShifts> appliedshifts = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Accepted == false && x.Status == 0).ToList();
            if (appliedshifts != null)
            {
                string LocationName = "";
                string Location = "";
                foreach (ApplicantAppliedShifts appliedshift in appliedshifts)
                {
                    ClientShifts shift = _commonService.GetClientShiftByID(appliedshift.ClientShift_ID);
                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                    if (shift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;

                    }
                    else
                    {
                        LocationName = institution.InstitutionName;
                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;


                    }
                    int dist = Convert.ToInt32(GeoCalculator.GetDistance(user.Latitude, user.Longitude, clinical.Latitude, clinical.Longitude, 2, distanceUnit: DistanceUnit.Miles));
                    model.PickedShifts.Add(new AppliedShifts()
                    {
                        ClockInTime = shift.ClockInTime,
                        ClockOutTime = shift.ClockOutTime,

                        StartDate = shift.StartDate,
                        EndDate = shift.EndDate,
                        Responsibility = shift.Responsibility,
                        ShiftDescription = shift.ShiftDescription,
                        DateOfShift = shift.DateOfShift,
                        ShiftExpirationDate = shift.ShiftExpirationDate,
                        Institution_ID = shift.Institution_ID,
                        Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                    Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                    .Select(x => x.SpecialityName).ToList()),
                        Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                        BranchName = LocationName,
                        Location = Location,
                        ShiftLabel_ID = shift.ShiftLabel_ID,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                        Occurrences = shift.Occurrences,
                        Consecutive_Occurrences = shift.Consecutive_Occurrences,
                        ClientShift_ID = shift.ClientShift_ID,
                        AppliedAllDays = appliedshift.AppliedAllDays,
                        AppliedDaysList = appliedshift.AppliedDaysList,
                        AppliedShift_ID = appliedshift.AppliedShift_ID,
                        InstitutionName = institution.InstitutionName,
                        Applied = true,
                        Distance = dist,
                        ShiftsDates = shift.ShiftsDates



                    });
                }
            }
            applicant = _commonService.FindApplicantByUserID(user.Id);
            var specIds = _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(x => x.Speciality_ID).ToList();
            var specialities = new SelectList(_commonService.GetSpecialities().Where(x => specIds.Contains(x.Speciality_ID)), "Speciality_ID", "SpecialityName");
            var city = _commonService.GetCitiesByCityid(user.City_ID);
            var states = new SelectList(_commonService.GetStates(city.country_id).Where(x => ShiftCitiesList.Count > 0 ? ShiftCitiesList.Select(c => c.state_id).ToList().Contains(x.id) : true), "id", "state_name");
            var cities = new SelectList(ShiftCitiesList, "id", "city_name");
            foreach (var state in states)
            {
                state.Selected = true;
            }
            //states.Where(x => x.Value == city.state_id.ToString()).First().Selected = true;

            //cities.Where(x => x.Value == city.id.ToString()).First().Selected = true;
            ViewBag.Cities = cities;
            ViewBag.States = states;
            ViewBag.Speciality = specialities;

            return View(model);
        }

        [HttpPost]
        [Route("Applicant/AvailableShifts")]
        public IActionResult Applicant_ShiftSearch(AppShiftSearchViewModel model)
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.ZipCode = user.ZipCode;
            model.Shifts = new List<AppliedShifts>();
            List<int> cities_list = model.Search.Cities;
            var ShiftCitiesList = _commonService.GetShiftCities();
            if (cities_list == null)
            {
                if (model.Search.States != null)
                {
                    cities_list = new List<int>();
                    foreach (int state_id in model.Search.States)
                    {
                        cities_list.AddRange(_commonService.GetCities(state_id).Select(x => x.id).ToList());
                    }
                }
            }
            List<ClientShifts> clientShifts = new List<ClientShifts>();
            if (model.Search.Distance > 0)
            {
                clientShifts = _commonService.ApplicantSearchShift(cities_list, model.Search.Specialities, userId, model.Search.Distance, user.Longitude, user.Latitude);
            }
            else
            {
                clientShifts = _commonService.ApplicantSearchShift(cities_list, model.Search.Specialities, userId);
            }
            if (clientShifts.Count > 0)
            {

                foreach (ClientShifts clientShift in clientShifts)
                {
                    string LocationName = "";
                    string InstitutionName = "";
                    string Location = "";
                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                    InstitutionName = institution.InstitutionName;
                    if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                        ShiftCitiesList.Add(_commonService.GetCitiesByCityid(branch.CityId));
                    }
                    else
                    {

                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;
                        ShiftCitiesList.Add(_commonService.GetCitiesByCityid(clinical.City_ID));
                    }
                    model.Shifts.Add(new AppliedShifts()
                    {
                        Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                        ClientShift_ID = clientShift.ClientShift_ID,
                        ClockInTime = clientShift.ClockInTime,
                        ClockOutTime = clientShift.ClockOutTime,
                        BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                        Consecutive_Occurrences = clientShift.Consecutive_Occurrences,

                        DateOfShift = clientShift.DateOfShift,
                        EndDate = clientShift.EndDate,

                        Institution_ID = clientShift.Institution_ID,
                        Occurrences = clientShift.Occurrences,
                        Responsibility = clientShift.Responsibility,
                        ShiftDescription = clientShift.ShiftDescription,
                        ShiftExpirationDate = clientShift.ShiftExpirationDate,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                        ShiftLabel_ID = clientShift.ShiftLabel_ID,
                        StartDate = clientShift.StartDate,
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                        Applied = false,
                        Location = Location,
                        ShiftsDates = clientShift.ShiftsDates

                    });
                }
            }
            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            var city = _commonService.GetCitiesByCityid(user.City_ID);
            var states = new SelectList(_commonService.GetStates(city.country_id).Where(x => ShiftCitiesList.Count > 0 ? ShiftCitiesList.Select(c => c.state_id).ToList().Contains(x.id) : true), "id", "state_name");
            var cityviewlist = new SelectList(ShiftCitiesList, "id", "city_name");
            if (model.Search.States != null)
            {

                for (int i = 0; i < model.Search.States.Count; i++)
                {
                    states.Where(x => x.Value == model.Search.States[i].ToString()).First().Selected = true;

                }

                if (model.Search.Cities != null)
                {
                    foreach (int c in model.Search.Cities)
                    {
                        cityviewlist.Where(x => x.Value == c.ToString()).First().Selected = true;
                    }
                }
                ViewBag.Cities = cityviewlist;
            }

            ViewBag.States = states;
            ViewBag.Speciality = specialities;

            return View(model);
        }
        public IActionResult ShiftView(int ClientShift_ID)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ClientShifts clientShift = _commonService.GetClientShiftByID(ClientShift_ID);

            if (clientShift != null)
            {
                string LocationName = "";
                string InstitutionName = "";
                string Location = "";
                ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                InstitutionName = institution.InstitutionName;
                if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                {
                    ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                    LocationName = branch.BranchName;
                    Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                }
                else
                {

                    Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                }
                AppliedShifts model = new AppliedShifts()
                {
                    Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                    ClientShift_ID = clientShift.ClientShift_ID,
                    ClockInTime = clientShift.ClockInTime,
                    ClockOutTime = clientShift.ClockOutTime,
                    BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                    Consecutive_Occurrences = clientShift.Consecutive_Occurrences,

                    DateOfShift = clientShift.DateOfShift,
                    EndDate = clientShift.EndDate,


                    Institution_ID = clientShift.Institution_ID,
                    Occurrences = clientShift.Occurrences,
                    Responsibility = clientShift.Responsibility,
                    ShiftDescription = clientShift.ShiftDescription,
                    ShiftExpirationDate = clientShift.ShiftExpirationDate,
                    ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                    ShiftLabel_ID = clientShift.ShiftLabel_ID,
                    StartDate = clientShift.StartDate,
                    SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                    Applied = _commonService.GetApplicantAppliedShifts(userId).Select(x => x.ClientShift_ID).Contains(clientShift.ClientShift_ID) ? true : false,
                    Location = Location,
                    ShiftsDates = clientShift.ShiftsDates

                };
                return View(model);
            }
            return View();
        }
        public IActionResult ApplytoShift(AppliedShifts model, int id)
        {
            if (String.IsNullOrEmpty(model.AppliedDaysList) && String.IsNullOrEmpty(model.ShiftsDates))
            {
                return RedirectToAction("AvailableShifts", "Applicant");
            }
            else
            {

                var shiftdays = model.ShiftsDates.Split(',');
                var applieddays = model.AppliedDaysList.Split(',');
                if (shiftdays.Length == applieddays.Length)
                {
                    model.AppliedAllDays = true;
                }
                else
                {
                    model.AppliedAllDays = false;
                }
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                Applicants app = _commonService.FindApplicantByUserID(userId);
                app.Picked = true;
                if (app != null && _commonService.GetClientShiftByID(id).Available)
                {
                    ApplicantAppliedShifts appliedShifts = new ApplicantAppliedShifts()
                    {
                        Applicant_ID = app.Applicant_ID,
                        ClientShift_ID = id,
                        AppliedAllDays = model.AppliedAllDays,
                        AppliedDaysList = model.AppliedDaysList
                    };
                    _commonService.AddApplicantAppliedShift(appliedShifts);

                }
                return RedirectToAction("AvailableShifts");

            }
        }

        public IActionResult ConfirmAcceptedShift(int nid)
        {
            List<AppliedShifts> model = new List<AppliedShifts>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (nid != 0)
            {
                Notifications not = _commonService.GetUserAllNotifications(userId).Where(x => x.Notification_ID == nid && x.NotificationTemplate_ID == 3).FirstOrDefault();
                if (not != null)
                {

                    ApplicantAppliedShifts confirmed = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == false && x.Status == 1 && x.AppliedShift_ID == not.Special_ID).FirstOrDefault();
                    if (confirmed != null)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(confirmed.ClientShift_ID);
                        ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        ApplicationUser user = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                        model.Add(new AppliedShifts()
                        {
                            ClockInTime = shift.ClockInTime,
                            ClockOutTime = shift.ClockOutTime,

                            StartDate = shift.StartDate,
                            EndDate = shift.EndDate,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            DateOfShift = shift.DateOfShift,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            Institution_ID = shift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                            Occurrences = shift.Occurrences,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ClientShift_ID = shift.ClientShift_ID,
                            AppliedAllDays = confirmed.AppliedAllDays,
                            AppliedDaysList = confirmed.AppliedDaysList,
                            AppliedShift_ID = confirmed.AppliedShift_ID,
                            Remarks = confirmed.Remarks,
                            PhoneNumber = user != null ? user.PhoneNumber : "",
                            Applied = not.Status == 1 ? false : true,
                            ShiftsDates = shift.ShiftsDates


                        });

                    }
                }
            }
            return View(model);
        }
        public IActionResult InvitedShift(int nid)
        {
            AppliedShifts model = new AppliedShifts();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (nid != 0)
            {
                Notifications not = _commonService.GetUserAllNotifications(userId).Where(x => x.Notification_ID == nid && x.NotificationTemplate_ID == 6).FirstOrDefault();
                if (not != null)
                {
                    ApplicantAppliedShifts confirmed = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == false && x.Status == 1 && x.AppliedShift_ID == not.Special_ID).FirstOrDefault();
                    List<ApplicantAppliedShiftsDays> days = _appliedShiftServices.GetAppliedShiftDays(confirmed.AppliedShift_ID);
                    if (confirmed != null)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(confirmed.ClientShift_ID);
                        ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        ApplicationUser user = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                        model.ClockInTime = shift.ClockInTime;
                        model.ClockOutTime = shift.ClockOutTime;

                        model.StartDate = shift.StartDate;
                        model.EndDate = shift.EndDate;
                        model.Responsibility = shift.Responsibility;
                        model.ShiftDescription = shift.ShiftDescription;
                        model.DateOfShift = shift.DateOfShift;
                        model.ShiftExpirationDate = shift.ShiftExpirationDate;
                        model.Institution_ID = shift.Institution_ID;
                        model.Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID);
                        model.SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                    Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                    .Select(x => x.SpecialityName).ToList());
                        model.Branch_ID = shift.Branch_ID.GetValueOrDefault();
                        model.BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;
                        model.ShiftLabel_ID = shift.ShiftLabel_ID;
                        model.ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault();
                        model.Occurrences = shift.Occurrences;
                        model.Consecutive_Occurrences = shift.Consecutive_Occurrences;
                        model.ClientShift_ID = shift.ClientShift_ID;
                        model.AppliedAllDays = confirmed.AppliedAllDays;
                        model.AppliedDaysList = confirmed.AppliedDaysList;
                        model.AppliedShift_ID = confirmed.AppliedShift_ID;
                        model.Remarks = confirmed.Remarks;
                        model.PhoneNumber = user != null ? user.PhoneNumber : "";
                        model.Location = _commonService.GetCityName(user.City_ID) + " ZipCode:" + user.ZipCode;
                        model.Applied = not.Status == 1 ? false : true;
                        model.InstitutionName = institution.InstitutionName;
                        model.ShiftsDates = shift.ShiftsDates;


                    }
                    if(days!=null)
                    {
                        model.inviedDays = new List<Models.Applicant.InviedDaysViewModel>();
                        foreach(var day in days)
                        {
                            model.inviedDays.Add(new Models.Applicant.InviedDaysViewModel
                            {
                                Day = day.Day,
                                EndTime = day.ClockOutTime,
                                StartTime = day.ClockInTime,
                                Invited = true
                            });
                        }
                    }

                    return View(model);
                }
            }
            return RedirectToAction("ConfirmedShifts", "Applicant");
        }
        public IActionResult AcceptInvitation(int id, AppliedShifts model)
        {
            if (model.AppliedDaysList != null)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                Applicants app = _commonService.FindApplicantByUserID(userId);
                ApplicationUser appuser = _userManager.GetUserAsync(HttpContext.User).Result;
                if (app != null)
                {
                    Notifications notify = _commonService.GetUserNotifications(userId).Where(x => x.Special_ID == id).FirstOrDefault();
                    ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShift(id);

                    if (appliedShifts != null)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedShifts.ClientShift_ID);
                        if (shift != null)
                        {
                            var shiftdates = shift.ShiftsDates.Split(",");
                            var appliedates = model.AppliedDaysList.Split(",");
                            if (shiftdates.Length == appliedates.Length)
                            {
                                appliedShifts.AppliedAllDays = true;
                            }
                            else
                            {
                                appliedShifts.AppliedAllDays = false;
                            }
                        }

                        appliedShifts.AppliedDaysList = model.AppliedDaysList;
                        appliedShifts.Accepted = true;

                        if (shift != null)
                        {
                            List<string> Applieddays = appliedShifts.AppliedDaysList.Split(',').ToList();
                            List<ApplicantClockInClockOutTime> worktime = new List<ApplicantClockInClockOutTime>();
                            {
                                foreach (string appliedday in Applieddays)
                                {
                                    DateTime day = Convert.ToDateTime(appliedday);
                                    try
                                    {
                                        worktime.Add(new ApplicantClockInClockOutTime()
                                        {
                                            AppliedShift_ID = appliedShifts.AppliedShift_ID,
                                            WorkEndTime = shift.ClockOutTime,
                                            WorkStartTime = shift.ClockInTime,
                                            WorkingDay = day,

                                        });
                                    }
                                    catch { }
                                }
                            }
                            if (worktime.Count > 0)
                            {
                                _commonService.ConfirmApplicantAppliedShift(appliedShifts, worktime);
                                #region sendingemail
                                var clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                                if (clinical != null)
                                {
                                    var clincialuser = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                                    var branch = _commonService.GetLocations(shift.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                                    Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                                    string appfilecontent = GetHtmlStringFromPath("ConfirmShift");
                                    string AppSubject = appfilecontent.Substring(0, appfilecontent.IndexOf(Environment.NewLine));
                                    string AppBody = appfilecontent.Replace(AppSubject, "");
                                    string clientfilecontent = GetHtmlStringFromPath("ToClientConfirmShift");
                                    string clientSubject = clientfilecontent.Substring(0, clientfilecontent.IndexOf(Environment.NewLine));
                                    string clientBody = clientfilecontent.Replace(clientSubject, "");
                                    emailkeys.Add("{AppName}", app.FirstName + " " + app.LastName);
                                    emailkeys.Add("{Institution}", clinical.InstitutionName);
                                    emailkeys.Add("{Date}", String.Join(",", worktime.Select(x => x.WorkingDay.ToString("MMMM dd"))));
                                    emailkeys.Add("{Time}", shift.ClockInTime.ToShortTimeString() + "-" + shift.ClockOutTime.ToShortTimeString());
                                    emailkeys.Add("{Location}", branch == null ? clinical.InstitutionName : branch.BranchName);
                                    emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);
                                    emailkeys.Add("{ReturnUrl}", Url.Action("ConfirmedShifts", "Applicant", values: null, protocol: _rootpath.Type, host: _rootpath.ReleasedLink));
                                    emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                                    emailkeys.Add("{AdminTitle}", administrator.Title);
                                    emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                                    foreach (var key in emailkeys)
                                    {
                                        AppBody = AppBody.Replace(key.Key, key.Value);
                                        AppSubject = AppSubject.Replace(key.Key, key.Value);
                                        clientBody = clientBody.Replace(key.Key, key.Value);
                                        clientSubject = clientSubject.Replace(key.Key, key.Value);
                                    }

                                    var emailanswer = _emailService.SendEmailAsync(appuser.Email, AppBody, AppSubject, true).Result;
                                    var clientemailanswer = _emailService.SendEmailAsync(clincialuser.Email, clientBody, clientSubject, true).Result;
                                }
                                #endregion sendingemail
                                if (notify != null)
                                {
                                    if (notify.Status == 1)
                                    {
                                        _commonService.RemoveNotifications(notify.Notification_ID);
                                    }
                                }

                            }
                        }


                    }
                    return RedirectToAction("ConfirmedShifts", "Applicant");
                }
            }
            return RedirectToAction("Notifications", "Home");
        }
        public IActionResult RejectInvitation(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Notifications notify = _commonService.GetUserNotifications(userId).Where(x => x.Special_ID == id).FirstOrDefault();
            ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShift(id);
            ClientShifts shift = _commonService.GetClientShiftByID(appliedShifts.ClientShift_ID);
            Applicants app = _commonService.FindApplicantByUserID(userId);
            if (app != null && shift != null)
            {
                if (app.Applicant_ID == appliedShifts.Applicant_ID)
                {
                    appliedShifts.Accepted = false;
                    appliedShifts.Status = -1;
                    _commonService.UpdateApplicantAppliedShift(appliedShifts);
                    if (appliedShifts.Invited.ToLower() == "invited by client")
                    {
                        #region sendingemail
                        List<DateTime> workday = appliedShifts.AppliedDaysList.Split(',').Select(x => DateTime.Parse(x)).ToList();
                        var clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        if (clinical != null)
                        {
                            var clincialuser = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                            var branch = _commonService.GetLocations(shift.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                            Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                            Dictionary<string, string> emailkeys = new Dictionary<string, string>();

                            string clientfilecontent = GetHtmlStringFromPath("ToClientRejectedShift");
                            string clientSubject = clientfilecontent.Substring(0, clientfilecontent.IndexOf(Environment.NewLine));
                            string clientBody = clientfilecontent.Replace(clientSubject, "");
                            emailkeys.Add("{AppName}", app.FirstName + " " + app.LastName);
                            emailkeys.Add("{Institution}", clinical.InstitutionName);
                            emailkeys.Add("{Date}", String.Join(",", workday.Select(x => x.ToString("MMMM dd"))));
                            emailkeys.Add("{Time}", shift.ClockInTime.ToShortTimeString() + "-" + shift.ClockOutTime.ToShortTimeString());
                            emailkeys.Add("{Location}", branch == null ? clinical.InstitutionName : branch.BranchName);
                            emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);

                            emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                            emailkeys.Add("{AdminTitle}", administrator.Title);
                            emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                            foreach (var key in emailkeys)
                            {

                                clientBody = clientBody.Replace(key.Key, key.Value);
                                clientSubject = clientSubject.Replace(key.Key, key.Value);
                            }
                            var clientemailanswer = _emailService.SendEmailAsync(clincialuser.Email, clientBody, clientSubject, true).Result;
                        }
                        #endregion sendingemail
                    }
                    if (notify != null)
                    {
                        if (notify.Status == 1)
                        {
                            _commonService.RemoveNotifications(notify.Notification_ID);
                        }
                    }
                }
            }

            return RedirectToAction("Dashboard", "Applicant");
        }
        public IActionResult PickedShifts()
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                string LocationName = "";
                string Location = "";
                List<AppliedShifts> model = new List<AppliedShifts>();
                List<ApplicantAppliedShifts> appliedshifts = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Accepted == false && x.Status == 0).ToList();
                if (appliedshifts != null)
                {
                    foreach (ApplicantAppliedShifts appliedshift in appliedshifts)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedshift.ClientShift_ID);
                        ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                        if (shift.Branch_ID.GetValueOrDefault() != 0)
                        {
                            ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault());
                            LocationName = branch.BranchName;
                            Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                        }
                        else
                        {
                            LocationName = institution.InstitutionName;
                            Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                        }
                        model.Add(new AppliedShifts()
                        {
                            ClockInTime = shift.ClockInTime,
                            ClockOutTime = shift.ClockOutTime,
                            StartDate = shift.StartDate,
                            EndDate = shift.EndDate,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            DateOfShift = shift.DateOfShift,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            Institution_ID = shift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = LocationName,
                            Location = Location,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                            Occurrences = shift.Occurrences,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ClientShift_ID = shift.ClientShift_ID,
                            AppliedAllDays = appliedshift.AppliedAllDays,
                            AppliedDaysList = appliedshift.AppliedDaysList,
                            AppliedShift_ID = appliedshift.AppliedShift_ID,
                            ShiftsDates = shift.ShiftsDates,


                        });
                    }
                }
                return View(model);
            }
            return View();
        }
        public IActionResult ConfirmShift(int AppliedShift_ID)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            ApplicationUser appuser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (app != null)
            {
                ApplicantAppliedShifts confirmed = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == false && x.Status == 1 && x.AppliedShift_ID == AppliedShift_ID).FirstOrDefault();
                if (confirmed != null)
                {

                    confirmed.Accepted = true;
                    ClientShifts shift = _commonService.GetClientShiftByID(confirmed.ClientShift_ID);
                    if (shift != null)
                    {
                        List<string> Applieddays = confirmed.AppliedDaysList.Split(',').ToList();

                        List<ApplicantClockInClockOutTime> worktime = new List<ApplicantClockInClockOutTime>();
                        {
                            foreach (string appliedday in Applieddays)
                            {
                                DateTime day = Convert.ToDateTime(appliedday);
                                try
                                {
                                    worktime.Add(new ApplicantClockInClockOutTime()
                                    {
                                        AppliedShift_ID = confirmed.AppliedShift_ID,
                                        WorkEndTime = shift.ClockOutTime,
                                        WorkStartTime = shift.ClockInTime,
                                        WorkingDay = day,

                                    });
                                }
                                catch { }
                            }
                        }
                        if (worktime.Count > 0)
                        {
                            _commonService.ConfirmApplicantAppliedShift(confirmed, worktime);
                            #region sendingemail
                            var clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                            if (clinical != null)
                            {
                                var clincialuser = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                                var branch = _commonService.GetLocations(shift.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                                Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                                Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                                string appfilecontent = GetHtmlStringFromPath("ConfirmShift");
                                string AppSubject = appfilecontent.Substring(0, appfilecontent.IndexOf(Environment.NewLine));
                                string AppBody = appfilecontent.Replace(AppSubject, "");
                                string clientfilecontent = GetHtmlStringFromPath("ToClientConfirmShift");
                                string clientSubject = clientfilecontent.Substring(0, clientfilecontent.IndexOf(Environment.NewLine));
                                string clientBody = clientfilecontent.Replace(clientSubject, "");
                                emailkeys.Add("{AppName}", app.FirstName + " " + app.LastName);
                                emailkeys.Add("{Institution}", clinical.InstitutionName);
                                emailkeys.Add("{Date}", String.Join(",", worktime.Select(x => x.WorkingDay.ToString("MMMM dd"))));
                                emailkeys.Add("{Time}", shift.ClockInTime.ToShortTimeString() + "-" + shift.ClockOutTime.ToShortTimeString());
                                emailkeys.Add("{Location}", branch == null ? clinical.InstitutionName : branch.BranchName);
                                emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);
                                emailkeys.Add("{ReturnUrl}", Url.Action("ConfirmedShifts", "Applicant", values: null, protocol: _rootpath.Type, host: _rootpath.ReleasedLink));
                                emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                                emailkeys.Add("{AdminTitle}", administrator.Title);
                                emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                                foreach (var key in emailkeys)
                                {
                                    AppBody = AppBody.Replace(key.Key, key.Value);
                                    AppSubject = AppSubject.Replace(key.Key, key.Value);
                                    clientBody = clientBody.Replace(key.Key, key.Value);
                                    clientSubject = clientSubject.Replace(key.Key, key.Value);
                                }

                                var emailanswer = _emailService.SendEmailAsync(appuser.Email, AppBody, AppSubject, true).Result;
                                var clientemailanswer = _emailService.SendEmailAsync(clincialuser.Email, clientBody, clientSubject, true).Result;
                            }
                            #endregion sendingemail
                            Notifications not = _commonService.GetUserNotifications(userId).Where(x => x.NotificationTemplate_ID == 3 && x.Special_ID == AppliedShift_ID).FirstOrDefault();
                            if (not != null)
                            {
                                _commonService.RemoveNotifications(not.Notification_ID);
                            }
                        }
                    }

                }
            }
            return RedirectToAction("ConfirmedShifts", "Applicant");
        }
        public IActionResult DismissedShifts(int nid)
        {

            List<AppliedShifts> model = new List<AppliedShifts>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (nid != 0)
            {
                Notifications not = _commonService.GetUserNotifications(userId).Where(x => x.Notification_ID == nid && x.NotificationTemplate_ID == 4).FirstOrDefault();
                if (not != null)
                {
                    _commonService.RemoveNotifications(not.Notification_ID);
                    ApplicantAppliedShifts dismissed = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == false && x.Status == -1 && x.AppliedShift_ID == not.Special_ID).FirstOrDefault();
                    if (dismissed != null)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(dismissed.ClientShift_ID);
                        model.Add(new AppliedShifts()
                        {
                            ClockInTime = shift.ClockInTime,
                            ClockOutTime = shift.ClockOutTime,

                            StartDate = shift.StartDate,
                            EndDate = shift.EndDate,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            DateOfShift = shift.DateOfShift,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            Institution_ID = shift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                            Occurrences = shift.Occurrences,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ClientShift_ID = shift.ClientShift_ID,
                            ShiftsDates = shift.ShiftsDates,
                            AppliedDaysList = dismissed.AppliedDaysList,
                            Remarks = dismissed.Remarks

                        });

                    }
                }
            }


            return View(model);
        }

        public IActionResult ConfirmedShifts()
        {

            AcceptedShiftViewModel model = new AcceptedShiftViewModel();
            List<AccpetedShiftClockinViewModel> todaysshift = new List<AccpetedShiftClockinViewModel>();
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            Applicants app = _commonService.FindApplicantByUserID(user.Id);
            if (app.Atwork)
            {
                ApplicantClockInClockOutTime activeShift = _commonService.GetApplicantAcitveShift(user.Id);
                if (activeShift != null)
                {
                    ApplicantAppliedShifts confirme = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Accepted == true && (x.Status == 1) && x.AppliedShift_ID == activeShift.AppliedShift_ID).FirstOrDefault();
                    ClientShifts shift = _commonService.GetClientShiftByID(confirme.ClientShift_ID);
                    model.ActiveShift = new AccpetedShiftClockinViewModel()
                    {
                        AppliedShift_ID = confirme.AppliedShift_ID,
                        ClockInTime = shift.ClockInTime,
                        ClockOutTime = shift.ClockOutTime,
                        StartDate = shift.StartDate,
                        EndDate = shift.EndDate,
                        Responsibility = shift.Responsibility,
                        ShiftDescription = shift.ShiftDescription,
                        DateOfShift = shift.DateOfShift,
                        ShiftExpirationDate = shift.ShiftExpirationDate,
                        Institution_ID = shift.Institution_ID,
                        Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                   Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                   .Select(x => x.SpecialityName).ToList()),
                        Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                        BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                        ShiftLabel_ID = shift.ShiftLabel_ID,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                        HolidayShift = shift.HolidayShift,
                        Occurrences = shift.Occurrences,
                        Consecutive_Occurrences = shift.Consecutive_Occurrences,
                        ClientShift_ID = shift.ClientShift_ID,
                        AppliedAllDays = confirme.AppliedAllDays,
                        AppliedDaysList = confirme.AppliedDaysList,
                        isconfirm = false,
                        hasmanually = false,
                        disable = !app.Atwork,
                        workingshift = true,
                        clockiIn_ID = activeShift.ClockinClockOutTime_ID,
                        WorkingDay = activeShift.WorkingDay.Date,
                        TimeSpanOffset = user.TimeOffset,





                    };
                }
                else
                {
                    app.Atwork = false;
                    _commonService.UpdateApplicant(app);
                }
            }
            else
            {
                model.ActiveShift = null;
            }

            List<ApplicantClockInClockOutTime> todaysacceptedshift_id = _commonService.GetTodaysClockinShifts(user.Id).ToList();
            List<ApplicantAppliedShifts> applicantAppliedShifts = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Accepted == true && (x.Status == 1)).ToList();
            List<string> listofdays = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Accepted = true && x.Status == 1).Select(x => x.AppliedDaysList).ToList();
            if (listofdays.Count > 0)
            {
                model.AcceptedShiftDays = new List<string>();
                model.nearbyshifts = new List<AccpetedShiftClockinViewModel>();
                model.notclockinshifts = new List<AccpetedShiftClockinViewModel>();
                foreach (string days in listofdays)
                {
                    model.AcceptedShiftDays.AddRange(days.Split(','));
                }
            }
            foreach (var todayaceptedshift in todaysacceptedshift_id)
            {
                if (applicantAppliedShifts != null)
                {
                    ApplicantAppliedShifts confirme = applicantAppliedShifts.Where(x => x.AppliedShift_ID == todayaceptedshift.AppliedShift_ID).FirstOrDefault();
                    if (confirme != null)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(confirme.ClientShift_ID);
                        model.nearbyshifts.Add(new AccpetedShiftClockinViewModel()
                        {
                            AppliedShift_ID = confirme.AppliedShift_ID,
                            ClockInTime = todayaceptedshift.WorkingDay.Date.Add(shift.ClockInTime.TimeOfDay),
                            ClockOutTime = todayaceptedshift.WorkingDay.Date.Add(shift.ClockOutTime.TimeOfDay),


                            StartDate = shift.StartDate,
                            EndDate = shift.EndDate,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            DateOfShift = shift.DateOfShift,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            Institution_ID = shift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                            HolidayShift = shift.HolidayShift,
                            Occurrences = shift.Occurrences,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ClientShift_ID = shift.ClientShift_ID,
                            AppliedAllDays = confirme.AppliedAllDays,
                            AppliedDaysList = confirme.AppliedDaysList,
                            isconfirm = todaysacceptedshift_id.Select(x => x.AppliedShift_ID).Contains(confirme.AppliedShift_ID) ? true : false,
                            hasmanually = false,
                            disable = !app.Atwork,
                            workingshift = false,
                            clockiIn_ID = todayaceptedshift.ClockinClockOutTime_ID,
                            WorkingDay = todayaceptedshift.WorkingDay.Date,
                            InstitutionName = _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            MaxClockinDifference = TimeSpan.FromMinutes(double.Parse(_timespanModel.TimeSpanValue)),
                            TimeSpanOffset = user.TimeOffset

                        });
                        string location = "";
                        string CityName = "";
                        int CityID = 0;
                        if (shift.Branch_ID == null)
                        {
                            ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                            ApplicationUser clientuser = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                            location = _commonService.GetCityName(clientuser.City_ID) + ", Address: " + user.Address;
                            CityName = _commonService.GetCityName(clientuser.City_ID);
                            CityID = clientuser.City_ID;

                        }
                        else
                        {
                            ClinicalInstitutionBranches branches = _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault());
                            location = _commonService.GetCityName(branches.CityId) + ", Address: " + branches.Address;
                            CityName = _commonService.GetCityName(branches.CityId);
                            CityID = branches.CityId;
                        }
                        model.nearbyshifts.Last().CityName = CityName;
                        model.nearbyshifts.Last().Address = location;
                        model.nearbyshifts.Last().CityID = CityID;
                    }
                }
            }
            List<ApplicantClockInClockOutTime> notclockinShifts = _commonService.GetNotClockinShifts(user.Id).ToList();

            if (notclockinShifts.Count > 0)
            {

                foreach (var notclockinShift in notclockinShifts)
                {
                    if (applicantAppliedShifts != null)
                    {
                        ApplicantAppliedShifts confirme = applicantAppliedShifts.Where(x => x.AppliedShift_ID == notclockinShift.AppliedShift_ID).FirstOrDefault();
                        if (confirme != null)
                        {
                            ClientShifts shift = _commonService.GetClientShiftByID(confirme.ClientShift_ID);
                            model.notclockinshifts.Add(new AccpetedShiftClockinViewModel()
                            {
                                AppliedShift_ID = confirme.AppliedShift_ID,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,


                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                DateOfShift = shift.DateOfShift,
                                ShiftExpirationDate = shift.ShiftExpirationDate,
                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                                HolidayShift = shift.HolidayShift,
                                Occurrences = shift.Occurrences,
                                Consecutive_Occurrences = shift.Consecutive_Occurrences,
                                ClientShift_ID = shift.ClientShift_ID,
                                AppliedAllDays = confirme.AppliedAllDays,
                                AppliedDaysList = confirme.AppliedDaysList,
                                isconfirm = false,
                                hasmanually = true,
                                disable = !app.Atwork,
                                workingshift = false,
                                clockiIn_ID = notclockinShift.ClockinClockOutTime_ID,
                                WorkingDay = notclockinShift.WorkingDay.Date,
                                TimeSpanOffset = user.TimeOffset
                            });
                        }
                    }
                }

            }
            if (model.nearbyshifts != null)
            {
                model.nearbyshifts = model.nearbyshifts.OrderBy(x => x.WorkingDay).ToList();
                var cityname = model.nearbyshifts.Select(x => new { x.CityID, x.CityName }).Distinct().ToList();
                ViewBag.Location = new SelectList(cityname, "CityID", "CityName");
                ViewBag.Clinical = new SelectList(model.nearbyshifts.Select(x => new { x.Institution_ID, x.InstitutionName }).Distinct().ToList(), "Institution_ID", "InstitutionName");
            }
            else
            {
                model.nearbyshifts = new List<AccpetedShiftClockinViewModel>();
            }
            return View(model);


        }
        public IActionResult CompletedShifts()
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Applicants app = _commonService.FindApplicantByUserID(user.Id);
            if (app != null)
            {
                AppDashboardViewModel model = new AppDashboardViewModel();
                model.Shifts = new List<AppliedShifts>();
                model.PickedShifts = new List<AppliedShifts>();
                model.RejectedShifts = new List<AppliedShifts>();
                model.PayChecks = new List<PayCheckViewModel>();


                List<ApplicantAppliedShifts> appliedshifts = _commonService.GetApplicantAppliedShifts(user.Id).Where(x => x.Status == 2 && x.Accepted == true).ToList();
                if (appliedshifts != null)
                {
                    string LocationName = "";
                    string Location = "";
                    foreach (ApplicantAppliedShifts appliedshift in appliedshifts)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedshift.ClientShift_ID);
                        ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                        if (shift.Branch_ID.GetValueOrDefault() != 0)
                        {
                            ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault());
                            LocationName = branch.BranchName;
                            Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                        }
                        else
                        {
                            LocationName = institution.InstitutionName;
                            Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                        }


                        model.Shifts.Add(new AppliedShifts()
                        {
                            InstitutionName = institution.InstitutionName,
                            ClockInTime = shift.ClockInTime,
                            ClockOutTime = shift.ClockOutTime,
                            StartDate = shift.StartDate,
                            EndDate = shift.EndDate,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            DateOfShift = shift.DateOfShift,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            Institution_ID = shift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = LocationName,
                            Location = Location,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                            Occurrences = shift.Occurrences,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ClientShift_ID = shift.ClientShift_ID,
                            AppliedAllDays = appliedshift.AppliedAllDays,
                            AppliedDaysList = appliedshift.AppliedDaysList,
                            AppliedShift_ID = appliedshift.AppliedShift_ID,
                            Paid = 2,
                            Remarks = appliedshift.Remarks,
                            PhoneNumber = clinical.PhoneNumber,
                            ShiftsDates = shift.ShiftsDates


                        });
                        List<ApplicantClockInClockOutTime> clockintimes = _commonService.GetAppliedShiftClockinClockouttimes(appliedshift.AppliedShift_ID);
                        if (clockintimes != null)
                        {
                            model.Shifts.Last().NumberofShift = clockintimes.Count();
                            model.Shifts.Last().CompletedNumberofShift = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count();
                            if (model.Shifts.Last().CompletedNumberofShift != 0)
                            {
                                TimeSpan time = TimeSpan.Zero;
                                List<ApplicantClockInClockOutTime> completeclockintimes = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                                if (completeclockintimes.Count > 0)
                                {
                                    foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                    {
                                        time += times.ClockOutTime - times.ClockInTime;
                                    }
                                }
                                model.Shifts.Last().Numberofworkedhours = time.TotalHours;
                            }
                        }
                        else
                        {
                            model.Shifts.Last().NumberofShift = 0;
                            model.Shifts.Last().CompletedNumberofShift = 0;
                            model.Shifts.Last().Numberofworkedhours = 0;
                        }


                    }

                }

                return View(model);
            }
            return View();

        }
        public IActionResult GetAccpetedShiftbyDay(Object days)
        {
            return View();
        }

        public IActionResult ClockinShift(int ClientShift_ID)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicantAppliedShifts confirmed = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == true && x.Status == 1 && x.ClientShift_ID == ClientShift_ID).First();
            if (confirmed != null)
            {
                confirmed.Accepted = true;
                _commonService.UpdateApplicantAppliedShift(confirmed);
            }
            return RedirectToAction("ConfirmedShifts", "Applicant");
        }
        public IActionResult ConfirmClockin(int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {

                int timeoffset = user.TimeOffset;
                TimeSpan offset = TimeSpan.FromMinutes(-(timeoffset));
                DateTime ClockinTime = DateTime.UtcNow.Add(offset);
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                Applicants app = _commonService.FindApplicantByUserID(userId);

                ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetTodaysClockinShifts(userId).Where(x => x.ClockinClockOutTime_ID == id).First();
                clockinClockOutTime.ClockInTime = ClockinTime;
                app.Atwork = true;
                bool answer = _commonService.UpdateClockin(clockinClockOutTime, app);
                if (answer)
                {
                    ApplicantAppliedShifts appliedShift = _commonService.GetAppliedShift(clockinClockOutTime.AppliedShift_ID);
                    ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
                    ClinicalInstitutions clinic = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                    var branch = _commonService.GetLocations(clinic.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                    Notifications notify = new Notifications()
                    {
                        User_ID = clinic.User_ID,
                        NotificationTemplate_ID = 5,
                        NotificationTitle = String.Format("{0} {1} clocked in", app.LastName, app.FirstName),
                        Status = 1,
                        NotificationBody = String.Format("{0} {1} have successfully clocked in shift (ID {2}) at {3} on {4} at {5}", app.LastName, app.FirstName, shift.ClientShift_ID, ClockinTime.ToShortTimeString(), clockinClockOutTime.WorkingDay.ToString("MMMM dd"), branch == null ? clinic.InstitutionName : branch.BranchName)
                    };
                    _commonService.AddNotification(notify);
                    #region sendingemail
                    Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                    string filecontent = GetHtmlStringFromPath("ClockIn");
                    string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                    string Body = filecontent.Replace(Subject, "");
                    emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                    emailkeys.Add("{Date}", clockinClockOutTime.WorkingDay.ToString("MMMM dd"));
                    emailkeys.Add("{Time}", ClockinTime.ToShortTimeString());
                    emailkeys.Add("{Location}", branch == null ? clinic.InstitutionName : branch.BranchName);
                    emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);

                    emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                    emailkeys.Add("{AdminTitle}", administrator.Title);
                    emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                    foreach (var key in emailkeys)
                    {
                        Body = Body.Replace(key.Key, key.Value);
                        Subject = Subject.Replace(key.Key, key.Value);
                    }

                    var emailanswer = _emailService.SendEmailAsync(user.Email, Subject, Body, true).Result;
                    #endregion sendingemail


                }
            }
            return RedirectToAction("ConfirmedShifts", "Applicant");

        }
        public IActionResult _clockinpartial(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Clockoutpartialviewmodel model = new Clockoutpartialviewmodel();
            model.Clockin_difference = TimeSpan.FromMinutes(double.Parse(_timespanModel.TimeSpanValue));
            ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetTodaysClockinShifts(userId).Where(x => x.ClockinClockOutTime_ID == id).First();
            model.SelectedDay = clockinClockOutTime.WorkingDay.Date;
            model.clockin_id = clockinClockOutTime.ClockinClockOutTime_ID;
            ApplicantAppliedShifts clicked = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == true && (x.Status == 1) && x.AppliedShift_ID == clockinClockOutTime.AppliedShift_ID).First();
            ClientShifts shift = _commonService.GetClientShiftByID(clicked.ClientShift_ID);
            model.ClockinTime = clockinClockOutTime.WorkingDay.Date.Add(shift.ClockInTime.TimeOfDay);
            model.SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                .Select(x => x.SpecialityName).ToList());
            model.BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;
            return PartialView("_clockinpartial", model);
        }
        public IActionResult _clockoutpartial(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Clockoutpartialviewmodel model = new Clockoutpartialviewmodel();
            ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetApplicantAcitveShift(userId);
            if (clockinClockOutTime.ClockinClockOutTime_ID == id)
            {
                model.SelectedDay = clockinClockOutTime.WorkingDay.Date;
                model.clockin_id = clockinClockOutTime.ClockinClockOutTime_ID;
                ApplicantAppliedShifts clicked = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == true && (x.Status == 1) && (x.AppliedShift_ID == clockinClockOutTime.AppliedShift_ID)).First();
                ClientShifts shift = _commonService.GetClientShiftByID(clicked.ClientShift_ID);
                model.ClockinTime = clockinClockOutTime.ClockInTime;
                model.ClockOutTime = clockinClockOutTime.WorkingDay.Date.Add(shift.ClockOutTime.TimeOfDay);
                model.SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                    Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                    .Select(x => x.SpecialityName).ToList());
                model.BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;
                return PartialView("_clockoutpartial", model);
            }
            return null;
        }
        public JsonResult ConfirmClockout(int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                int timeoffset = user.TimeOffset;
                TimeSpan offset = TimeSpan.FromMinutes(-(timeoffset));
                DateTime Clockouttime = DateTime.UtcNow.Add(offset);
                Guid userId = user.Id;
                Applicants app = _commonService.FindApplicantByUserID(userId);
                ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetApplicantAcitveShift(userId);
                if (clockinClockOutTime.ClockinClockOutTime_ID == id)
                {
                    clockinClockOutTime.ClockOutTime = Clockouttime;
                    app.Atwork = false;
                    bool answer = _commonService.UpdateClockin(clockinClockOutTime, app);
                    if (answer)
                    {
                        ApplicantAppliedShifts appliedShift = _commonService.GetAppliedShift(clockinClockOutTime.AppliedShift_ID);
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
                        ClinicalInstitutions clinic = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        var branch = _commonService.GetLocations(clinic.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                        Notifications notify = new Notifications()
                        {
                            User_ID = clinic.User_ID,
                            NotificationTemplate_ID = 5,
                            NotificationTitle = String.Format("{0} {1} clocked out", app.LastName, app.FirstName),
                            Status = 1,
                            NotificationBody = String.Format("{0} {1} have successfully clocked out shift (ID {2}) at {3} for {4} at {5}", app.LastName, app.FirstName, shift.ClientShift_ID, Clockouttime.ToShortTimeString(), clockinClockOutTime.WorkingDay.ToString("MMMM dd"), branch == null ? clinic.InstitutionName : branch.BranchName),

                        };
                        _commonService.AddNotification(notify);
                        #region sendingemail
                        Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                        Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                        string filecontent = GetHtmlStringFromPath("ClockOut");
                        string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                        string Body = filecontent.Replace(Subject, "");
                        emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                        emailkeys.Add("{Date}", clockinClockOutTime.WorkingDay.ToString("MMMM dd"));
                        emailkeys.Add("{Time}", Clockouttime.ToShortTimeString());
                        emailkeys.Add("{Location}", branch == null ? clinic.InstitutionName : branch.BranchName);
                        emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);

                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                        foreach (var key in emailkeys)
                        {
                            Body = Body.Replace(key.Key, key.Value);
                            Subject = Subject.Replace(key.Key, key.Value);
                        }

                        var emailanswer = _emailService.SendEmailAsync(user.Email, Subject, Body, true).Result;
                        #endregion sendingemail


                    }
                    return Json(true);
                }
            }
            return Json(null);
        }
        public JsonResult ConfirmClockoutManually(DateTime clockouttime, int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                Applicants app = _commonService.FindApplicantByUserID(userId);
                ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetApplicantAcitveShift(userId);
                if (clockinClockOutTime.ClockinClockOutTime_ID == id)
                {
                    clockinClockOutTime.ClockOutTime = clockinClockOutTime.WorkingDay.Date.Add(clockouttime.TimeOfDay);
                    clockinClockOutTime.Manually = true;
                    app.Atwork = false;
                    bool answer = _commonService.UpdateClockin(clockinClockOutTime, app);
                    if (answer)
                    {
                        ApplicantAppliedShifts appliedShift = _commonService.GetAppliedShift(clockinClockOutTime.AppliedShift_ID);
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
                        ClinicalInstitutions clinic = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);

                        var branch = _commonService.GetLocations(clinic.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                        Notifications notify = new Notifications()
                        {
                            User_ID = clinic.User_ID,
                            NotificationTemplate_ID = 5,
                            NotificationTitle = String.Format("{0} {1} clocked out", app.LastName, app.FirstName),
                            Status = 1,
                            NotificationBody = String.Format("{0} {1} have successfully clocked out shift (ID {2}) at {3} for {4} at {5}", app.LastName, app.FirstName, shift.ClientShift_ID, clockouttime.ToShortTimeString(), clockinClockOutTime.WorkingDay.ToString("MMMM dd"), branch == null ? clinic.InstitutionName : branch.BranchName),

                        };
                        _commonService.AddNotification(notify);
                        #region sendingemail
                        Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                        Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                        string filecontent = GetHtmlStringFromPath("ClockOut");
                        string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                        string Body = filecontent.Replace(Subject, "");
                        emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                        emailkeys.Add("{Date}", clockinClockOutTime.WorkingDay.ToString("MMMM dd"));
                        emailkeys.Add("{Time}", clockouttime.ToShortTimeString());
                        emailkeys.Add("{Location}", branch == null ? clinic.InstitutionName : branch.BranchName);
                        emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);

                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                        foreach (var key in emailkeys)
                        {
                            Body = Body.Replace(key.Key, key.Value);
                            Subject = Subject.Replace(key.Key, key.Value);
                        }

                        var emailanswer = _emailService.SendEmailAsync(user.Email, Subject, Body, true).Result;
                        #endregion sendingemail


                    }

                    return Json(true);
                }
            }
            return Json(null);
        }
        public IActionResult _clockinclockoutmanually(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Clockoutpartialviewmodel model = new Clockoutpartialviewmodel();
            ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetTodaysClockinShifts(userId).Where(x => x.ClockinClockOutTime_ID == id).First();
            if (clockinClockOutTime != null)
            {
                model.SelectedDay = clockinClockOutTime.WorkingDay.Date;
                model.clockin_id = clockinClockOutTime.ClockinClockOutTime_ID;
                ApplicantAppliedShifts clicked = _commonService.GetApplicantAppliedShifts(userId).Where(x => x.Accepted == true && (x.Status == 1)).First();
                ClientShifts shift = _commonService.GetClientShiftByID(clicked.ClientShift_ID);
                model.ClockinTime = clockinClockOutTime.WorkingDay.Date.Add(shift.ClockInTime.TimeOfDay);
                model.ClockOutTime = clockinClockOutTime.WorkingDay.Date.Add(shift.ClockOutTime.TimeOfDay);

                model.SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                    Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                    .Select(x => x.SpecialityName).ToList());
                model.BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;

                return PartialView("_clockinclockoutmanually", model);
            }
            return null;
        }
        public JsonResult ConfirmClockinClockoutManually(DateTime clockouttime, DateTime clockintime, int id)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            if (app != null)
            {
                ApplicantClockInClockOutTime clockinClockOutTime = _commonService.GetNotClockinShifts(userId).Where(x => x.ClockinClockOutTime_ID == id).First();
                if (clockinClockOutTime != null)
                {
                    clockinClockOutTime.ClockOutTime = clockinClockOutTime.WorkingDay.Date.Add(clockouttime.TimeOfDay);
                    clockinClockOutTime.ClockInTime = clockinClockOutTime.WorkingDay.Date.Add(clockintime.TimeOfDay);
                    clockinClockOutTime.Manually = true;

                    bool answer = _commonService.UpdateClockin(clockinClockOutTime, app);
                    if (answer)
                    {
                        ApplicantAppliedShifts appliedShift = _commonService.GetAppliedShift(clockinClockOutTime.AppliedShift_ID);
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
                        ClinicalInstitutions clinic = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        var branch = _commonService.GetLocations(clinic.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                        Notifications notify = new Notifications()
                        {
                            User_ID = clinic.User_ID,
                            NotificationTemplate_ID = 5,
                            NotificationTitle = String.Format("{0} {1} Clock In/Clock Out Completed", app.LastName, app.FirstName),
                            Status = 1,
                            NotificationBody = String.Format("{0} {1} have successfully clocked in shift (ID {2}) at {3} and clocked out at {4} on {5} at {6}", app.LastName, app.FirstName, shift.ClientShift_ID, clockintime.ToShortTimeString(), clockouttime.ToShortTimeString(), clockinClockOutTime.WorkingDay.ToString("MMMM dd"), branch == null ? clinic.InstitutionName : branch.BranchName),

                        };
                        _commonService.AddNotification(notify);
                        #region sendingemail
                        Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                        Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                        string filecontent = GetHtmlStringFromPath("ManuallyClockIn");
                        string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                        string Body = filecontent.Replace(Subject, "");
                        emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                        emailkeys.Add("{Date}", clockinClockOutTime.WorkingDay.ToString("MMMM dd"));
                        emailkeys.Add("{ClockInTime}", clockintime.ToShortTimeString());
                        emailkeys.Add("{ClockOutTime}", clockouttime.ToShortTimeString());
                        emailkeys.Add("{Location}", branch == null ? clinic.InstitutionName : branch.BranchName);
                        emailkeys.Add("{ReleasedLink}", _rootpath.Type + "://" + _rootpath.ReleasedLink);

                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                        foreach (var key in emailkeys)
                        {
                            Body = Body.Replace(key.Key, key.Value);
                            Subject = Subject.Replace(key.Key, key.Value);
                        }

                        var emailanswer = _emailService.SendEmailAsync(user.Email, Subject, Body, true).Result;
                        #endregion sendingemail


                    }

                    return Json(true);
                }
            }
            return Json(null);
        }
        [AllowAnonymous]
        public IActionResult _ApplicantWorkHistory(int model)
        {
            var Specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = Specialities;
            return PartialView("_ApplicantWorkHistory", model);
        }
        [AllowAnonymous]
        public IActionResult _ApplicantCeritificates(int model)
        {
            var certificateTypes = new SelectList(_commonService.GetCertificateTypes(), "Certificate_ID", "Certificate_TypeName");
            ViewBag.Certificates = certificateTypes;
            return PartialView("_ApplicantCeritificates", model);
        }
        [AllowAnonymous]
        public IActionResult _ApplicantSpeciality(int model)
        {
            var Specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = Specialities;
            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            ViewBag.States = states;
            return PartialView("_ApplicantSpeciality", model);
        }

        public IActionResult _ApplicantReference(int model)
        {

            return PartialView("_ApplicantReference", model);
        }
        public IActionResult AppRegistrationSuccess()
        {
            return View();
        }
        public IActionResult _shiftdetailpartial(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ClientShifts clientShift = _commonService.GetClientShiftByID(id);

            if (clientShift != null)
            {
                string LocationName = "";
                string InstitutionName = "";
                string Location = "";
                ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                InstitutionName = institution.InstitutionName;
                if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                {
                    ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                    LocationName = branch.BranchName;
                    Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                }
                else
                {

                    Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                }
                AppliedShifts model = new AppliedShifts()
                {
                    Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                    ClientShift_ID = clientShift.ClientShift_ID,
                    ClockInTime = clientShift.ClockInTime,
                    ClockOutTime = clientShift.ClockOutTime,
                    BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                    Consecutive_Occurrences = clientShift.Consecutive_Occurrences,

                    DateOfShift = clientShift.DateOfShift,
                    EndDate = clientShift.EndDate,
                    Institution_ID = clientShift.Institution_ID,
                    Occurrences = clientShift.Occurrences,
                    Responsibility = clientShift.Responsibility,
                    ShiftDescription = clientShift.ShiftDescription,
                    ShiftExpirationDate = clientShift.ShiftExpirationDate,
                    ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                    ShiftLabel_ID = clientShift.ShiftLabel_ID,
                    StartDate = clientShift.StartDate,
                    SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                    Applied = _commonService.GetApplicantAppliedShifts(userId).Select(x => x.ClientShift_ID).Contains(clientShift.ClientShift_ID) ? true : false,
                    Location = Location,
                    InstitutionName = institution.InstitutionName,
                    ContactPerson = institution.ContactPerson,
                    PhoneNumber = clinical.PhoneNumber,
                    ShiftsDates = clientShift.ShiftsDates

                };
                return PartialView("_shiftdetailpartial", model);
            }
            return PartialView("_shiftdetailpartial");
        }
        public IActionResult _pickedshiftdetail(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShiftsbyClientShift_ID(id).Where(x => x.Applicant_ID == app.Applicant_ID).FirstOrDefault();
            if (appliedShifts != null)
            {
                ClientShifts clientShift = _commonService.GetClientShiftByID(appliedShifts.ClientShift_ID);

                if (clientShift != null)
                {
                    string LocationName = "";
                    string InstitutionName = "";
                    string Location = "";
                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                    InstitutionName = institution.InstitutionName;
                    if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                    }
                    else
                    {

                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                    }
                    AppliedShifts model = new AppliedShifts()
                    {
                        Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                        ClientShift_ID = clientShift.ClientShift_ID,
                        ClockInTime = clientShift.ClockInTime,
                        ClockOutTime = clientShift.ClockOutTime,
                        BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                        Consecutive_Occurrences = clientShift.Consecutive_Occurrences,

                        DateOfShift = clientShift.DateOfShift,
                        EndDate = clientShift.EndDate,

                        Institution_ID = clientShift.Institution_ID,
                        Occurrences = clientShift.Occurrences,
                        Responsibility = clientShift.Responsibility,
                        ShiftDescription = clientShift.ShiftDescription,
                        ShiftExpirationDate = clientShift.ShiftExpirationDate,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                        ShiftLabel_ID = clientShift.ShiftLabel_ID,
                        StartDate = clientShift.StartDate,
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                        Applied = _commonService.GetApplicantAppliedShifts(userId).Select(x => x.ClientShift_ID).Contains(clientShift.ClientShift_ID) ? true : false,
                        Location = Location,
                        AppliedAllDays = appliedShifts.AppliedAllDays,
                        AppliedDaysList = appliedShifts.AppliedDaysList,
                        AppliedShift_ID = appliedShifts.AppliedShift_ID,
                        PhoneNumber = clinical.PhoneNumber,
                        ContactPerson = institution.ContactPerson,
                        InstitutionName = institution.InstitutionName,
                        Remarks = appliedShifts.Remarks,
                        ShiftsDates = clientShift.ShiftsDates


                    };
                    return PartialView("_pickedshiftdetail", model);
                }

            }
            return PartialView("_pickedshiftdetail");
        }
        public IActionResult _activeshiftdetail(int id)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShift(id);
            if (appliedShifts != null)
            {
                ClientShifts clientShift = _commonService.GetClientShiftByID(appliedShifts.ClientShift_ID);

                if (clientShift != null)
                {
                    string LocationName = "";
                    string InstitutionName = "";
                    string Location = "";
                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                    InstitutionName = institution.InstitutionName;
                    if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                    }
                    else
                    {

                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                    }
                    AppliedShifts model = new AppliedShifts()
                    {
                        Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                        ClientShift_ID = clientShift.ClientShift_ID,
                        ClockInTime = clientShift.ClockInTime,
                        ClockOutTime = clientShift.ClockOutTime,
                        BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                        Consecutive_Occurrences = clientShift.Consecutive_Occurrences,
                        DateOfShift = clientShift.DateOfShift,
                        EndDate = clientShift.EndDate,
                        Institution_ID = clientShift.Institution_ID,
                        Occurrences = clientShift.Occurrences,
                        Responsibility = clientShift.Responsibility,
                        ShiftDescription = clientShift.ShiftDescription,
                        ShiftExpirationDate = clientShift.ShiftExpirationDate,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientShift.ShiftLabel_ID).First().ShiftLabelName,
                        ShiftLabel_ID = clientShift.ShiftLabel_ID,
                        StartDate = clientShift.StartDate,
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                        Applied = _commonService.GetApplicantAppliedShifts(userId).Select(x => x.ClientShift_ID).Contains(clientShift.ClientShift_ID) ? true : false,
                        Location = Location,
                        AppliedAllDays = appliedShifts.AppliedAllDays,
                        AppliedDaysList = appliedShifts.AppliedDaysList,
                        AppliedShift_ID = appliedShifts.AppliedShift_ID,
                        PhoneNumber = clinical.PhoneNumber,
                        ContactPerson = institution.ContactPerson,
                        InstitutionName = institution.InstitutionName,
                        Remarks = appliedShifts.Remarks,
                        ShiftsDates = clientShift.ShiftsDates


                    };
                    List<ApplicantClockInClockOutTime> clockintimes = _commonService.GetAppliedShiftClockinClockouttimes(appliedShifts.AppliedShift_ID);
                    if (clockintimes != null)
                    {
                        model.NumberofShift = clockintimes.Count();
                        model.CompletedNumberofShift = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count();
                        if (model.CompletedNumberofShift != 0)
                        {
                            TimeSpan time = TimeSpan.Zero;
                            List<ApplicantClockInClockOutTime> completeclockintimes = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                            if (completeclockintimes.Count > 0)
                            {
                                foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                {
                                    time += times.ClockOutTime - times.ClockInTime;
                                }
                            }
                            model.Numberofworkedhours = time.TotalHours;
                        }
                    }
                    else
                    {
                        model.NumberofShift = 0;
                        model.CompletedNumberofShift = 0;
                        model.Numberofworkedhours = 0;
                    }
                    return PartialView("_activeshiftdetail", model);
                }

            }
            return PartialView("_activeshiftdetail");
        }
        public IActionResult _applicantAvailableDays()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            {
                if(app!=null)
                {
                var availables=_commonService.GetApplicantAvailables(app.Applicant_ID).Where(x=>x.AvailableDay>=DateTime.Now).ToList();
                    if(availables!=null)
                    {
                        var ShiftLabels = _commonService.GetShiftLabels();
                        List<ApplicantAvailableViewModel> model = new List<ApplicantAvailableViewModel>();
                        foreach(ApplicantAvailables available in availables)
                        {
                            model.Add(new ApplicantAvailableViewModel() {
                              
                                 Applicant_ID=app.Applicant_ID,
                                  AvailableDay=available.AvailableDay,
                                   ApplicantAvailable_ID=available.ApplicantAvailable_ID,
                                    ApplicantAvailableDays=available.ApplicantAvailableDays,
                                     EndTime=available.EndTime,
                                      StartTime=available.StartTime
                                  
                              
                            });
                        }
                        return PartialView("_applicantAvailableDays",model);
                    }
                }
            }
            return PartialView("_applicantAvailableDays");
        }
        public IActionResult AvailableDays()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants app = _commonService.FindApplicantByUserID(userId);
            {
                if (app != null)
                {
                    var availables = _commonService.GetApplicantAvailables(app.Applicant_ID).Where(x => x.AvailableDay >= DateTime.Now).ToList();
                    if (availables != null)
                    {
                        var ShiftLabels = _commonService.GetShiftLabels();
                        List<ApplicantAvailableViewModel> model = new List<ApplicantAvailableViewModel>();
                        foreach (ApplicantAvailables available in availables)
                        {
                            model.Add(new ApplicantAvailableViewModel()
                            {
                               
                                Applicant_ID = app.Applicant_ID,
                                AvailableDay = available.AvailableDay,
                                ApplicantAvailable_ID = available.ApplicantAvailable_ID,
                                ApplicantAvailableDays = available.ApplicantAvailableDays,
                                EndTime = available.EndTime,
                                StartTime = available.StartTime


                            });
                        }
                        return View( model);
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
                var certificateTypes = new SelectList(_commonService.GetCertificateTypes(), "Certificate_ID", "Certificate_TypeName");
                var visastatus = new SelectList(_commonService.GetVisaStatuses(), "VisaStatus_ID", "VisaStatus");

                var availabilites = new SelectList(_commonService.GetAvailabilities(), "Availability_ID", "Availability");
                ViewBag.CertificateTypes = certificateTypes;
                ViewBag.Speciality = specialities;
                ViewBag.VisaStatus = visastatus;
                ViewBag.Availability = availabilites;


                if ((user.ChangesMakedTime - DateTime.Now).Days != 0 || ((user.ChangesMakedTime - DateTime.Now).Days == 0 && (user.ChangesMakedTime - DateTime.Now).Hours != 0))
                {
                    user.ChangesCount = 0;
                    user.ChangesLocked = false;
                    await _userManager.UpdateAsync(user);
                }

                Applicants app = _commonService.FindApplicantByUserID(user.Id);
                if (app != null)
                {
                    ProfileViewModel model = new ProfileViewModel();
                    model.Password = new Models.Account.ChangePasswordViewModel();
                    model.Imgsrc = "/" + app.ProfileImage.Replace('\\', '/');
                    model.PhoneNumber = user.PhoneNumber;
                    model.OldResume = "Upload"+"\\"+"Resume"+"\\"+app.Resume;
                    model.Profile = new ApplicantProfileViewModel()
                    {
                        Applicant_ID = app.Applicant_ID,
                        Availability_ID = app.Availability_ID,
                        FirstName = app.FirstName,
                        IsEligible = app.IsEligible,
                        LastName = app.LastName,
                        MiddleName = app.MiddleName,
                        Email = user.Email,
                        User_ID = app.User_ID,
                        VisaStatus_ID = app.VisaStatus_ID,
                        Disabled = user.ChangesLocked,
                        Address = user.Address,
                        City_ID = user.City_ID,
                        ZipCode = user.ZipCode
                    };
                    var city = _commonService.GetCitiesByCityid(user.City_ID);
                    model.Profile.State_ID = city != null ? city.state_id : 0;
                    var states = new SelectList(_commonService.GetStates(city.country_id), "id", "state_name");
                    ViewBag.States = states;
                    var cities = new SelectList(_commonService.GetCities(city.state_id), "id", "city_name");
                    ViewBag.Cities = cities;
                    var workhistories = _commonService.GetApplicantWorkHistory(app.Applicant_ID);
                    if (workhistories != null)
                    {
                        model.workHistories = new List<ApplicantWorkHistoryViewModel>();
                        foreach (ApplicantWorkHistories workHistory in workhistories)
                        {
                            model.workHistories.Add(new ApplicantWorkHistoryViewModel
                            {
                                EndDate = workHistory.EndDate,
                                StartDate = workHistory.StartDate,
                                PlaceName = workHistory.PlaceName,
                                UntilNow = workHistory.EndDate == DateTime.MinValue ? true : false,
                                JobTitle = workHistory.JobTitle,
                                WorkHistory_ID = workHistory.WorkHistory_ID
                            });
                        }
                    }
                    var Specialities = _commonService.GetApplicantSpecialities(app.Applicant_ID);
                    if (Specialities != null)
                    {
                        model.specialities = new List<ApplicantSpecialitisViewModel>();
                        foreach (ApplicantSpecialities speciality in Specialities)
                        {
                            model.specialities.Add(new ApplicantSpecialitisViewModel
                            {
                                LegabilityStates = speciality.LegabilityStates.Split("; ").ToList(),
                                License = speciality.License,
                                Speciality_ID = speciality.Speciality_ID,
                                Id = speciality.ApplicantSpeciality_ID,
                                IssueDate = speciality.IssueDate

                            });
                        }
                    }
                    var certificates = _commonService.GetApplicantCertificates(app.Applicant_ID);
                    if (certificates != null)
                    {
                        model.certificates = new List<ApplicantCertificateViewModel>();

                        foreach (ApplicantCertificates certificate in certificates)
                        {
                            model.certificates.Add(new ApplicantCertificateViewModel()
                            {
                                CeritifcationImgsrc = certificate.CeritificationImg,
                                CertificateType = certificate.CertificateTypes,
                                Certification_ID = certificate.Ceritification_ID

                            });
                        }

                    }
                    var availableetypes = _commonService.GetAvailableTypes();
                    model.ApplicantAvailables = new List<ApplicantAvailablesViewModel>();
                  
                    var useravailbles = _commonService.GetApplicantAvailables(app.Applicant_ID);
                    foreach (ApplicantAvailableTypes applicantAvailables in availableetypes)
                    {
                        model.ApplicantAvailables.Add(new ApplicantAvailablesViewModel()
                        {
                            Applicant_ID = app.Applicant_ID,
                            ApplicantAvailableType_ID = applicantAvailables.ApplicantAvailableType_ID,
                            ApplicantAvailableTypeName = applicantAvailables.ApplicantAvailableType_Name,
                            ApplicantAvailableTypeValue = useravailbles!=null ? true : false,
                            ApplicantAvailableDays = useravailbles != null ? String.Join(",", useravailbles.Select(x => x.AvailableDay.ToShortDateString())) : ""
                        });
                    }
                    model.IsAvailable = app.IsAvailable;

                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {

            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                if (user != null)
                {

                    Applicants app = _commonService.FindApplicantByUserID(user.Id);

                    if (model.NewResume != null)
                    {
                        app.Resume = model.NewResume;
                        _commonService.UpdateApplicant(app);
                    }

                    if (app != null)
                    {
                        if (user.City_ID != model.Profile.City_ID || user.Address != model.Profile.Address)
                        {
                            if (model.Profile.Latitude == 0 || model.Profile.Longitude == 0)
                            {
                                if (!String.IsNullOrEmpty(model.Profile.Address))
                                {
                                    string query = model.Profile.Address + "," + _commonService.GetCityName(model.Profile.City_ID);
                                    latlong latlong = _commonService.GetLatLongByAddress(query);
                                    if (latlong.Latitude != 0 && latlong.Longitude != 0)
                                    {
                                        user.Latitude = latlong.Latitude;
                                        user.Longitude = latlong.Longitude;
                                    }
                                }
                            }
                            else
                            {
                                user.Longitude = model.Profile.Longitude;
                                user.Latitude = model.Profile.Latitude;
                                HttpContext.Session.Remove("counter");
                            }
                        }

                        app.Availability_ID = model.Profile.Availability_ID;
                        app.IsEligible = model.Profile.IsEligible;
                        app.VisaStatus_ID = model.Profile.VisaStatus_ID;
                        user.Address = model.Profile.Address;
                        user.City_ID = model.Profile.City_ID;
                        user.ZipCode = model.Profile.ZipCode;
                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 9)
                            {
                                user.ChangesLocked = true;
                            }
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = user.ChangesCount + 1;


                        }
                        else
                        {
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = 1;
                            user.ChangesLocked = false;
                        }

                        _commonService.UpdateApplicant(app);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Profile", "Applicant");
                    }

                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> FileView(string path)
        {

            if (!path.Contains("wwwroot"))
            {
                path = _rootpath.UserRoot + path;
            }
            try
            {
                string filename = path.Substring(path.LastIndexOf('\\') + 1);
                string fielmime = path.Substring(path.LastIndexOf('.') + 1);
                string file = path;

                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    byte[] test = wc.DownloadData(path);
                    var memory = new MemoryStream(test);
                    memory.Position = 0;
                    switch (fielmime)
                    {
                        case "png":
                            Response.ContentType = "image/png";
                            break;
                        case "jpg":
                        case "jpeg":
                            Response.ContentType = "image/jpeg";
                            break;
                        case "pdf":
                            Response.ContentType = "application/pdf";
                            break;
                        case "doc":
                            Response.ContentType = "application/msword";
                            break;
                        case "docx":
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case "rtf":
                            Response.ContentType = "application/rtf";
                            break;

                    };
                    return File(memory, Response.ContentType, Path.GetFileName(path));
                }


            }
            catch { return BadRequest(404); }

            return View();
        }

        public async Task<IActionResult> AddSpeciality(ProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            {

                if (user != null && model.specialities != null)
                {

                    Applicants app = _commonService.FindApplicantByUserID(user.Id);
                    if (ModelState.IsValid)
                    {
                        List<ApplicantSpecialities> applicantSpecialities = new List<ApplicantSpecialities>();


                        foreach (ApplicantSpecialitisViewModel speciality in model.specialities)
                        {
                            applicantSpecialities.Add(new ApplicantSpecialities()
                            {
                                Applicant_ID = app.Applicant_ID,
                                LegabilityStates = String.Join("; ", speciality.LegabilityStates),
                                License = speciality.License,
                                Speciality_ID = speciality.Speciality_ID,
                                IssueDate = speciality.IssueDate

                            });

                        }

                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 9)
                            {
                                user.ChangesLocked = true;
                            }
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = user.ChangesCount + 1;


                        }
                        else
                        {
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = 1;
                            user.ChangesLocked = false;
                        }

                        _commonService.AddApplicantSpecialities(applicantSpecialities);
                        await _userManager.UpdateAsync(user);

                    }
                }

            }
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> AddWorkHistory(ProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            {

                if (user != null && model.workHistories != null)
                {

                    Applicants app = _commonService.FindApplicantByUserID(user.Id);
                    if (ModelState.IsValid)
                    {
                        List<ApplicantWorkHistories> applicantWorkHistories = new List<ApplicantWorkHistories>();


                        foreach (ApplicantWorkHistoryViewModel workhistory in model.workHistories)
                        {
                            if (workhistory.StartDate != null && workhistory.PlaceName != null && workhistory.JobTitle != null && (workhistory.UntilNow ? workhistory.UntilNow : (workhistory.EndDate != null)))
                            {
                                applicantWorkHistories.Add(new ApplicantWorkHistories()
                                {
                                    EndDate = workhistory.EndDate.GetValueOrDefault(),
                                    StartDate = workhistory.StartDate.Value,
                                    PlaceName = workhistory.PlaceName,
                                    JobTitle = workhistory.JobTitle,
                                    Applicant_ID = app.Applicant_ID
                                });
                            }

                        }

                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 9)
                            {
                                user.ChangesLocked = true;
                            }
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = user.ChangesCount + 1;


                        }
                        else
                        {
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = 1;
                            user.ChangesLocked = false;
                        }

                        _commonService.AddApplicantWorkHistory(applicantWorkHistories);
                        await _userManager.UpdateAsync(user);

                    }
                }

            }
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> AddCertificates(ProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            {

                if (user != null && model.certificates != null)
                {

                    Applicants app = _commonService.FindApplicantByUserID(user.Id);
                    if (ModelState.IsValid)
                    {
                        List<ApplicantCertificates> applicantCertificates = new List<ApplicantCertificates>();

                        string subpath = "Certificates" + $@"\{user.Id}";
                        foreach (ApplicantCertificateViewModel certificate in model.certificates)
                        {

                            if (certificate.CeritifcationImg != null && certificate.CertificateType != null)
                            {
                                string filename = Savefile(certificate.CeritifcationImg, subpath, certificate.CertificateType.ToString() + "_" + model.certificates.IndexOf(certificate).ToString());
                                if (filename != "")
                                {

                                    applicantCertificates.Add(new ApplicantCertificates() { CertificateTypes = certificate.CertificateType, CeritificationImg = filename });
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Can't upload Certificates");
                                }
                            }

                        }

                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 9)
                            {
                                user.ChangesLocked = true;
                            }
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = user.ChangesCount + 1;


                        }
                        else
                        {
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = 1;
                            user.ChangesLocked = false;
                        }

                        _commonService.AddApplicantCeritificates(applicantCertificates);
                        await _userManager.UpdateAsync(user);

                    }
                }

            }
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> SaveProfileImage(ProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            {

                if (user != null)
                {

                    Applicants app = _commonService.FindApplicantByUserID(user.Id);
                    if (model.ProfileImage != null)
                    {
                        string path = "ApplicantImg";
                        string profilefilename = SaveProfileFile(model.ProfileImage, path, user.Id.ToString());
                        if (profilefilename != "")
                        {
                            app.ProfileImage = profilefilename;
                        }
                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 9)
                            {
                                user.ChangesLocked = true;
                            }
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = user.ChangesCount + 1;


                        }
                        else
                        {
                            user.ChangesMakedTime = DateTime.Now;
                            user.ChangesCount = 1;
                            user.ChangesLocked = false;
                        }

                        _commonService.UpdateApplicant(app);
                        await _userManager.UpdateAsync(user);
                    }
                }

            }
            return RedirectToAction("Profile");
        }
        public IActionResult MakeAvailable()
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {

                Applicants app = _commonService.FindApplicantByUserID(user.Id);
                if (app != null)
                {
                    if (app.IsAvailable)
                    {
                        app.IsAvailable = false;
                        _commonService.UpdateApplicant(app);
                        _commonService.RemoveAllApplicantAvailables(app.Applicant_ID);

                    }
                }
            }

            return RedirectToAction("Profile", "Applicant");
        }
        public IActionResult Dashboard()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Applicants app = _commonService.FindApplicantByUserID(user.Id);
            if (app != null)
            {
                AppDashboardViewModel model = new AppDashboardViewModel();
                model.Shifts = new List<AppliedShifts>();
                model.PickedShifts = new List<AppliedShifts>();
                model.RejectedShifts = new List<AppliedShifts>();
                model.PayChecks = new List<PayCheckViewModel>();
                model.WorkedShiftTable = new List<AppliedShifts>();


                List<ApplicantAppliedShifts> appliedshifts = _commonService.GetApplicantAppliedShifts(user.Id).ToList();
                if (appliedshifts != null)
                {
                    string LocationName = "";
                    string Location = "";
                    foreach (ApplicantAppliedShifts appliedshift in appliedshifts)
                    {
                        ClientShifts shift = _commonService.GetClientShiftByID(appliedshift.ClientShift_ID);
                        ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                        ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();

                        if (shift.Branch_ID.GetValueOrDefault() != 0)
                        {
                            ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault());
                            LocationName = branch.BranchName;
                            Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                        }
                        else
                        {
                            LocationName = institution.InstitutionName;
                            Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                        }
                        if (appliedshift.Accepted == false && appliedshift.Status == 0)
                        {
                            model.PickedShifts.Add(new AppliedShifts()
                            {
                                InstitutionName = institution.InstitutionName,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,
                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                DateOfShift = shift.DateOfShift,
                                ShiftExpirationDate = shift.ShiftExpirationDate,
                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = LocationName,
                                Location = Location,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                                Occurrences = shift.Occurrences,
                                Consecutive_Occurrences = shift.Consecutive_Occurrences,
                                ClientShift_ID = shift.ClientShift_ID,
                                AppliedAllDays = appliedshift.AppliedAllDays,
                                AppliedDaysList = appliedshift.AppliedDaysList,
                                AppliedShift_ID = appliedshift.AppliedShift_ID,
                                Remarks = appliedshift.Remarks,
                                PhoneNumber = clinical.PhoneNumber,
                                ShiftsDates = shift.ShiftsDates


                            });
                        }
                        if (appliedshift.Accepted == true && appliedshift.Status == 1)
                        {
                            model.WorkedShiftTable.Add(new AppliedShifts()
                            {
                                InstitutionName = institution.InstitutionName,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,
                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                DateOfShift = shift.DateOfShift,
                                ShiftExpirationDate = shift.ShiftExpirationDate,
                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = LocationName,
                                Location = Location,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                                Occurrences = shift.Occurrences,
                                Consecutive_Occurrences = shift.Consecutive_Occurrences,
                                ClientShift_ID = shift.ClientShift_ID,
                                AppliedAllDays = appliedshift.AppliedAllDays,
                                AppliedDaysList = appliedshift.AppliedDaysList,
                                AppliedShift_ID = appliedshift.AppliedShift_ID,

                                Remarks = appliedshift.Remarks,
                                PhoneNumber = clinical.PhoneNumber,
                                ShiftsDates = shift.ShiftsDates


                            });
                            List<ApplicantClockInClockOutTime> clockintimes = _commonService.GetAppliedShiftClockinClockouttimes(appliedshift.AppliedShift_ID);
                            if (clockintimes != null)
                            {
                                model.WorkedShiftTable.Last().NumberofShift = clockintimes.Count();
                                model.WorkedShiftTable.Last().CompletedNumberofShift = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count();
                                if (model.WorkedShiftTable.Last().CompletedNumberofShift != 0)
                                {
                                    TimeSpan time = TimeSpan.Zero;
                                    List<ApplicantClockInClockOutTime> completeclockintimes = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                                    if (completeclockintimes.Count > 0)
                                    {
                                        foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                        {
                                            time += times.ClockOutTime - times.ClockInTime;
                                        }
                                    }
                                    model.WorkedShiftTable.Last().Numberofworkedhours = time.TotalHours;
                                }
                            }
                            else
                            {
                                model.WorkedShiftTable.Last().NumberofShift = 0;
                                model.WorkedShiftTable.Last().CompletedNumberofShift = 0;
                                model.WorkedShiftTable.Last().Numberofworkedhours = 0;
                            }

                        }

                        if (appliedshift.Accepted == true && appliedshift.Status == 2)
                        {
                            model.Shifts.Add(new AppliedShifts()
                            {
                                InstitutionName = institution.InstitutionName,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,

                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                DateOfShift = shift.DateOfShift,
                                ShiftExpirationDate = shift.ShiftExpirationDate,
                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = LocationName,
                                Location = Location,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                                Occurrences = shift.Occurrences,
                                Consecutive_Occurrences = shift.Consecutive_Occurrences,
                                ClientShift_ID = shift.ClientShift_ID,
                                AppliedAllDays = appliedshift.AppliedAllDays,
                                AppliedDaysList = appliedshift.AppliedDaysList,
                                AppliedShift_ID = appliedshift.AppliedShift_ID,

                                Remarks = appliedshift.Remarks,
                                PhoneNumber = clinical.PhoneNumber,
                                ShiftsDates = shift.ShiftsDates


                            });
                            List<ApplicantClockInClockOutTime> clockintimes = _commonService.GetAppliedShiftClockinClockouttimes(appliedshift.AppliedShift_ID);
                            if (clockintimes != null)
                            {
                                model.Shifts.Last().NumberofShift = clockintimes.Count();
                                model.Shifts.Last().CompletedNumberofShift = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count();
                                if (model.Shifts.Last().CompletedNumberofShift != 0)
                                {
                                    TimeSpan time = TimeSpan.Zero;
                                    List<ApplicantClockInClockOutTime> completeclockintimes = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                                    if (completeclockintimes.Count > 0)
                                    {
                                        foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                        {
                                            time += times.ClockOutTime - times.ClockInTime;
                                        }
                                    }
                                    model.Shifts.Last().Numberofworkedhours = time.TotalHours;
                                }
                            }
                            else
                            {
                                model.Shifts.Last().NumberofShift = 0;
                                model.Shifts.Last().CompletedNumberofShift = 0;
                                model.Shifts.Last().Numberofworkedhours = 0;
                            }
                        }
                        if (appliedshift.Accepted == false && appliedshift.Status == -1)
                        {
                            model.RejectedShifts.Add(new AppliedShifts()
                            {
                                InstitutionName = institution.InstitutionName,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,

                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                DateOfShift = shift.DateOfShift,
                                ShiftExpirationDate = shift.ShiftExpirationDate,
                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = LocationName,
                                Location = Location,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                                Occurrences = shift.Occurrences,
                                Consecutive_Occurrences = shift.Consecutive_Occurrences,
                                ClientShift_ID = shift.ClientShift_ID,
                                AppliedAllDays = appliedshift.AppliedAllDays,
                                AppliedDaysList = appliedshift.AppliedDaysList,
                                AppliedShift_ID = appliedshift.AppliedShift_ID,
                                Remarks = appliedshift.Remarks,
                                PhoneNumber = clinical.PhoneNumber,
                                ShiftsDates = shift.ShiftsDates


                            });
                        }
                    }
                    if (model.WorkedShiftTable.Count > 0)
                    {
                        model.Shifts.AddRange(model.WorkedShiftTable);
                    }
                }
                List<PayChecks> payChecks = _commonService.GetApplicantPayChecks(app.Applicant_ID).ToList();
                if (payChecks != null)
                {

                    foreach (PayChecks checks in payChecks)
                    {
                        model.PayChecks.Add(new PayCheckViewModel
                        {
                            Applicant_ID = app.Applicant_ID,

                            DateCreated = checks.DateCreated.GetValueOrDefault(),
                            DateModified = checks.DateModified.GetValueOrDefault(),
                            Net_pay = checks.Net_Pay,
                            PayBeginDate = checks.PayBeginDate,
                            PaycheckFilestring = checks.PayCheckFile,
                            Paycheck_ID = checks.PayCheck_ID,
                            PayEndDate = checks.PayEndDate,
                            PayCheckDate = checks.PayCheckDate
                        });
                    }
                }
                var AvailableDays = _commonService.GetApplicantAvailables(app.Applicant_ID).Count();
                model.AvailableDays = AvailableDays;

                var client = _commonService.GetApplicantbyId(app.Applicant_ID);
                if (client.BoardingProcess >= 3)
                {
                    model.CheckPickedShift = true;
                }

                return View(model);
            }
            return View();
        }

        public string Savefile(IFormFile file, string path, string filename)
        {
            string foldername = "Upload";
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername, path)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername, path));
            }
            string extention = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));
            filename += extention;
            using (FileStream fs = new FileStream(Path.Combine(_environment.WebRootPath, foldername, path, filename), FileMode.Create))
            {
                try
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    return foldername + "\\" + path + "\\" + filename;
                }
                catch
                { return ""; }
            }
        }
        public string SaveProfileFile(IFormFile file, string path, string filename)
        {
            string foldername = "Upload";
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername, path)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername, path));
            }
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername, path, filename)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername, path, filename));
            }
            string extention = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));

            var newfilename = Guid.NewGuid().ToString() + extention;
            using (FileStream fs = new FileStream(Path.Combine(_environment.WebRootPath, foldername, path, filename, newfilename), FileMode.Create))
            {
                try
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    return foldername + "\\" + path + "\\" + filename + "\\" + newfilename;
                }
                catch
                { return ""; }
            }
        }
        public IActionResult AddAvailableDays(ApplicantAvailableDaysViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                Applicants app = _commonService.FindApplicantByUserID(user.Id);
                if (app != null)
                {
                    List<ApplicantAvailables> availables = new List<ApplicantAvailables>();
                    if(!String.IsNullOrEmpty(model.StartDate))
                    {
                        var listofDays = model.StartDate.Split(',');
                        if(listofDays!=null)
                        {
                            foreach(string day in listofDays)
                            {
                                DateTime availableday = new DateTime();
                                DateTime.TryParse(day, out availableday);
                                if (availableday != DateTime.MinValue)
                                {
                                    availables.Add(new ApplicantAvailables
                                    {
                                        ApplicantAvailableDays = day,
                                      
                                        Applicant_ID = app.Applicant_ID,
                                        AvailableDay = availableday,
                                        EndTime = availableday.Add(model.EndTime.TimeOfDay),
                                        StartTime = availableday.Add(model.StartTime.TimeOfDay),
                                          

                                    });
                                }
                            }
                        }
                    }

                   if(availables.Count>0)
                    {
                        _commonService.AddApplicantAvailables(availables);
                    }
                }
            }
            return RedirectToAction("Dashboard");
        }
        public async Task FileView(string FileType, int Id)
        {
            var User_ID = _userManager.GetUserId(HttpContext.User);
            if (User_ID != null)
            {
                Applicants app = _commonService.FindApplicantByUserID(Guid.Parse(User_ID));
                string path = "";
                switch (FileType)
                {
                    case "Resume":
                        path = app.Applicant_ID == Id ? _commonService.GetApplicantbyId(Id).Resume : "";
                        break;
                    case "I-9":
                        path = app.Applicant_ID == Id ? _commonService.GetApplicantbyId(Id).I_9 : "";
                        break;
                    case "Certificate":
                        path = _commonService.GetApplicantCertificates(app.Applicant_ID).Where(x => x.Ceritification_ID == Id).FirstOrDefault() != null ? _commonService.GetApplicantCertificates(app.Applicant_ID).Where(x => x.Ceritification_ID == Id).FirstOrDefault().CeritificationImg : "";
                        break;
                    case "PayCheck":
                        PayChecks check = _commonService.GetApplicantPayChecks(app.Applicant_ID).Where(x => x.PayCheck_ID == Id).FirstOrDefault();
                        path = check != null ? check.PayCheckFile : "";
                        break;
                }
                if (path != null)
                {
                    if (!path.Contains("wwwroot"))
                    {
                        path = _environment.WebRootPath + "/" + path;
                    }

                    byte[] array;

                    try
                    {

                        string filename = path.Substring(path.LastIndexOf('\\') + 1);
                        string fielmime = path.Substring(path.LastIndexOf('.') + 1);
                        string file = path;
                        HttpContext.Response.OnStarting(state =>
                        {
                            using (HttpContext.Response.Body)
                            {

                                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                                {
                                    array = new Byte[stream.Length];
                                    stream.Read(array, 0, array.Length);

                                }
                                HttpContext.Response.Headers.Clear();
                                HttpContext.Response.Headers.Add("Content-Disposition", $"inline; filename=Employment_agreement.pdf");
                                switch (fielmime)
                                {
                                    case "png":
                                        HttpContext.Response.ContentType = "image/png";
                                        break;
                                    case "jpg":
                                    case "jpeg":
                                        HttpContext.Response.ContentType = "image/jpeg";
                                        break;
                                    case "pdf":
                                        HttpContext.Response.ContentType = "application/pdf";
                                        break;
                                    case "doc":
                                        HttpContext.Response.ContentType = "application/msword";
                                        break;
                                    case "docx":
                                        HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                                        break;
                                    case "rtf":
                                        HttpContext.Response.ContentType = "application/rtf";
                                        break;

                                };
                                HttpContext.Response.Headers.ContentLength = array.Length;
                                HttpContext.Response.Body.WriteAsync(array, 0, array.Length);
                                HttpContext.Response.Body.FlushAsync();
                                return Task.CompletedTask;
                            }
                        }, null);


                    }
                    catch { }
                }
            }

        }
        public JsonResult GetShiftCities(int stateid)
        {
            var Cities = _commonService.GetShiftCities();
            return Json(Cities.Where(x => x.state_id == stateid).ToList());
        }
        public JsonResult UpdateAvailableDates(string days,  bool Value)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {

                Applicants app = _commonService.FindApplicantByUserID(user.Id);
                if (app != null)
                {

                    var available = _commonService.GetApplicantAvailables(app.Applicant_ID).FirstOrDefault();
                    if (available == null && days != "")
                    {

                        ApplicantAvailables applicantAvailables = new ApplicantAvailables()
                        {
                
                            ApplicantAvailableDays = days,
                            Applicant_ID = app.Applicant_ID
                        };
                        if (!app.IsAvailable)
                        {
                            app.IsAvailable = true;
                            _commonService.UpdateApplicant(app);
                        }

                        _commonService.AddApplicantAvailable(applicantAvailables);
                    }
                    else
                    {
                        if (Value)
                        {
                            available.ApplicantAvailableDays = days;
                            if (!app.IsAvailable)
                            {
                                app.IsAvailable = true;
                                _commonService.UpdateApplicant(app);
                            }

                            _commonService.UpdateApplicantAvailable(available);
                        }
                        else
                        {
                            _commonService.RemoveApplicantAvailable(available);
                            var availables = _commonService.GetApplicantAvailables(app.Applicant_ID).ToList();

                            if (availables.Count == 0 && app.IsAvailable)
                            {
                                app.IsAvailable = false;
                                _commonService.UpdateApplicant(app);
                            }
                        }
                    }



                    return Json(true);
                }
            }
            return Json(false);
        }
        public JsonResult GetShiftDates(int ClientShift_ID)
        {
            ClientShifts shift = _commonService.GetClientShiftByID(ClientShift_ID);
            if (shift != null)

            {
                return Json(shift.ShiftsDates);
            }
            return Json("");
        }
        private string GetHtmlStringFromPath(string FileName)
        {
            string fileContent = "";
            string foldername = "EmailTemplates";
            if (Directory.Exists(Path.Combine(_environment.WebRootPath, foldername)))
            {
                using (StreamReader fs = new StreamReader(Path.Combine(_environment.WebRootPath, foldername, FileName + ".html")))
                {
                    fileContent = fs.ReadToEnd();
                }
            }
            return fileContent;
        }
    }
}