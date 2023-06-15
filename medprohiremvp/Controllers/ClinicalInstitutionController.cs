using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.IServices;
using medprohiremvp.Models.ClinicalInstitution;
using medprohiremvp.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using medprohiremvp.Models.Applicant;
using Microsoft.Extensions.Options;
using medprohiremvp.Repo.Context;

namespace medprohiremvp.Controllers
{

    [Authorize(Roles = "ClinicalInstitution")]
    public class ClinicalInstitutionController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly RootPath _rootPath;
        private readonly string clrole = "ClinicalInstitution";
        protected readonly MvpDBContext _dbcontext;

        public ClinicalInstitutionController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices,
            IHostingEnvironment environment,
            IEmailService emailService,
             IOptions<RootPath> rootPath,
             MvpDBContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _environment = environment;
            _emailService = emailService;
            _rootPath = rootPath.Value;
            _dbcontext = dbcontext;
        }
        [HttpGet]
        [Authorize(Roles = "ClinicalInstitution")]
        public IActionResult ClientBoardingProcess(int nid)
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(userId);
            if (clinical != null)
            {
                if (clinical.Status == 0)
                {
                    return RedirectToAction("RegistrationSuccess", "Account");
                }
                else if (clinical.Status >= 2)
                {

                    return RedirectToAction("Dashboard", "ClinicalInstitution");


                }

            }
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "ClinicalInstitution")]
        public IActionResult ClientBoardingProcess(List<SpecialtiesCosts> model, [FromQuery] int nid)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));

                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                var userRoles = _userManager.GetRolesAsync(user).Result;
                if (userRoles.Any(x => x == clrole))
                {
                    List<ClientSpecialtiesCosts> cost = new List<ClientSpecialtiesCosts>();
                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                    if (clinical != null)
                    {
                        var labelsCount = _commonService.GetShiftLabels().Count;
                        var addedSpecialties = model.Where(x => x.Cost.Where(c => c.Cost == 0 || c.Cost == null).Count() != labelsCount).Select(x => x.Specialty_ID).ToList();
                        if (addedSpecialties != null)
                        {
                            List<ClientSpecialties> clientSpecialties = new List<ClientSpecialties>();
                            foreach (int SpecialtyID in addedSpecialties)
                            {
                                clientSpecialties.Add(new ClientSpecialties()
                                {
                                    Institution_ID = clinical.Institution_ID,
                                    Speciality_ID = SpecialtyID,
                                });
                            }
                            _commonService.AddClientSpecialites(clientSpecialties);

                            foreach (ClientSpecialties spec in clientSpecialties)
                            {
                                var labelcosts = model.Where(x => x.Specialty_ID == spec.Speciality_ID).FirstOrDefault().Cost;
                                var zeroCostCount = labelcosts.Where(x => x.Cost == 0 || x.Cost == null).Count();
                                if (zeroCostCount != labelsCount)
                                {
                                    foreach (ShiftLabelsCost labelsCost in labelcosts)
                                    {

                                        cost.Add(new ClientSpecialtiesCosts()
                                        {
                                            ClientSpeciality_ID = spec.ClientSpeciality_ID
,
                                            ShiftLabel_ID = labelsCost.ShiftLabel_ID,
                                            Cost = labelsCost.Cost.GetValueOrDefault(),

                                        });
                                    }
                                }
                            }

                            if (cost.Count == 0)
                            {
                                ModelState.AddModelError("", "Please Insert Costs");
                            }
                            else
                            {
                                var answer = _commonService.AddClientSpecialtiesCost(cost);
                                if (answer)
                                {
                                    Notifications not = _commonService.GetUserNotifications(userId).Where(x => x.NotificationTemplate_ID == 7).FirstOrDefault();
                                    if (not != null)
                                    {
                                        _commonService.UncheckNotifications(not.Notification_ID);
                                    }
                                    return RedirectToAction("Dashboard", clrole);

                                }
                                else
                                {
                                    ModelState.AddModelError("", "Please try again");
                                }
                            }
                        }
                    }
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult ClientRegistrationSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NewLocation()
        {

            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            ViewBag.States = states;
            return View();
        }

        [HttpPost]
        public IActionResult NewLocation(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {

                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        float latitude = 0;
                        float longitude = 0;
                        if (model.Latitude == 0 || model.Longitude == 0)
                        {
                            string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                            if (!String.IsNullOrEmpty(model.Address))
                            {
                                latlong latlong = _commonService.GetLatLongByAddress(query);
                                if (latlong.Latitude != 0 && latlong.Longitude != 0)
                                {
                                    latitude = latlong.Latitude;
                                    longitude = latlong.Longitude;
                                }
                            }
                        }
                        else
                        {
                            latitude = model.Latitude;
                            longitude = model.Longitude;
                            HttpContext.Session.Remove("counter");
                        }
                        ClinicalInstitutionBranches branch = new ClinicalInstitutionBranches()
                        {
                            Institution_ID = clinicalInstitution.Institution_ID,
                            ContactName = model.ContactName,
                            ZipCode = model.ZipCode,
                            CityId = model.City_ID,
                            PhoneNumber = model.PhoneNumber,
                            BranchName = model.Name,
                            Address = model.Address,
                            Latitude = latitude,
                            Longitude = longitude,
                            Email = model.Email
                        };
                        var answer = _commonService.AddLocation(branch);
                        if (!answer)
                        {
                            ModelState.AddModelError(String.Empty, "Can't add location");
                        }
                        else
                        {
                            return RedirectToAction("LocationListView", "ClinicalInstitution");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Please Login as Clinical Institution and try again");
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Please Login and try again");
                }
            }
            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            ViewBag.States = states;
            return View();
        }

        [HttpGet]
        public IActionResult EditLocation(int Branch_ID)
        {
            try
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
                ViewBag.States = states;
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (Branch_ID == 0)
                {
                    LocationViewModel generalbranch = new LocationViewModel()
                    {
                        Branch_ID = 0,
                        Institution_ID = clinicalInstitution.Institution_ID,
                        ContactName = clinicalInstitution.ContactPerson,
                        Name = clinicalInstitution.InstitutionName,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        ZipCode = user.ZipCode,
                        City_ID = user.City_ID,
                        State_ID = _commonService.GetCitiesByCityid(user.City_ID).state_id,
                        Email = user.Email

                    };
                    var cities = new SelectList(_commonService.GetCities(generalbranch.State_ID), "id", "city_name");
                    ViewBag.Cities = cities;
                    return View(generalbranch);
                }
                else
                {
                    ClinicalInstitutionBranches branches = _commonService.GetlocationbyId(Branch_ID);
                    if (branches != null && branches.Institution_ID == clinicalInstitution.Institution_ID)
                    {
                        LocationViewModel location = new LocationViewModel()
                        {
                            Branch_ID = branches.Branch_ID,
                            Institution_ID = branches.Institution_ID,
                            ContactName = branches.ContactName,
                            Name = branches.BranchName,
                            Address = branches.Address,
                            PhoneNumber = branches.PhoneNumber,
                            ZipCode = branches.ZipCode,
                            City_ID = branches.CityId,
                            State_ID = _commonService.GetCitiesByCityid(branches.CityId).state_id,
                            Email = branches.Email

                        };
                        var cities = new SelectList(_commonService.GetCities(location.State_ID), "id", "city_name");
                        ViewBag.Cities = cities;
                        return View(location);
                    }

                }

            }
            catch
            {
                return RedirectToAction("LocationListView", "ClinicalInstitution");
            }
            return RedirectToAction("LocationListView", "ClinicalInstitution");
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationViewModel model, int Branch_ID)
        {
            if (ModelState.IsValid)
            {

                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                if (userId != null)
                {
                    ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {

                        if (Branch_ID == 0)
                        {
                            if (model.City_ID != user.City_ID || model.Address != user.Address)
                            {
                                if (model.Latitude == 0 || model.Longitude == 0)
                                {
                                    if (!String.IsNullOrEmpty(model.Address))
                                    {
                                        string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                        latlong latlong = _commonService.GetLatLongByAddress(query);
                                        if (latlong.Latitude != 0 && latlong.Longitude != 0)
                                        {
                                            user.Longitude = latlong.Longitude;
                                            user.Latitude = latlong.Latitude;
                                        }

                                    }
                                }
                                else
                                {
                                    user.Longitude = model.Longitude;
                                    user.Latitude = model.Latitude;
                                    HttpContext.Session.Remove("counter");
                                }

                            }
                            clinicalInstitution.ContactPerson = model.ContactName;
                            user.ZipCode = model.ZipCode;
                            user.City_ID = model.City_ID;
                            user.PhoneNumber = model.PhoneNumber;
                            clinicalInstitution.InstitutionName = model.Name;
                            user.Address = model.Address;

                            _commonService.UpdateClinical(clinicalInstitution);

                            IdentityResult result = await _userManager.UpdateAsync(user);
                            return RedirectToAction("LocationListView", "ClinicalInstitution");
                        }
                        else
                        {
                            ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(Branch_ID);
                            if (model.City_ID != branch.CityId || model.Address != branch.Address)
                            {
                                if (model.Longitude == 0 || model.Latitude == 0)
                                {
                                    if (!String.IsNullOrEmpty(model.Address))
                                    {
                                        string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                        latlong latlong = _commonService.GetLatLongByAddress(query);
                                        if (latlong.Latitude != 0 || latlong.Longitude != 0)
                                        {
                                            branch.Longitude = latlong.Longitude;
                                            branch.Latitude = latlong.Latitude;
                                        }

                                    }
                                }
                                else
                                {
                                    branch.Longitude = model.Longitude;
                                    branch.Latitude = model.Latitude;
                                    HttpContext.Session.Remove("counter");
                                }

                            }

                            branch.ContactName = model.ContactName;
                            branch.ZipCode = model.ZipCode;
                            branch.CityId = model.City_ID;
                            branch.PhoneNumber = model.PhoneNumber;
                            branch.BranchName = model.Name;
                            branch.Address = model.Address;
                            branch.Email = model.Email;

                            var answer = _commonService.UpdateLocation(branch);
                            if (!answer)
                            {
                                ModelState.AddModelError(String.Empty, "Can't Edit location");
                            }
                            else
                            {
                                return RedirectToAction("LocationListView", "ClinicalInstitution");
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Please Login as Clinical Institution and try again");
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Please Login and try again");
                }
            }
            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            ViewBag.States = states;
            return View();
        }

        public IActionResult ClientShiftList()
        {
            var Shift_categories = new SelectList(_commonService.GetShiftCategories(), "Category_ID", "CategoryName");


            Shift_categories.Append(new SelectListItem() { Text = "All", Value = "0", Selected = true });
            ViewBag.ShiftCategory = Shift_categories;
            List<ClientShiftViewModel> shiftlist = new List<ClientShiftViewModel>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (clinicalInstitution != null)
                {

                    List<ClientShifts> clientShifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID);
                    if (clientShifts.Count > 0)
                    {
                        foreach (var clientshift in clientShifts)
                        {
                            shiftlist.Add(new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate,
                                EndDate = clientshift.EndDate,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                Category_ID = clientshift.Category_ID,
                                ShiftsDates = clientshift.ShiftsDates,
                                Category_Name = _commonService.GetShiftCategories().Where(x => x.Category_ID == clientshift.Category_ID).FirstOrDefault().CategoryName


                            });



                        }

                    }
                }
            }
            shiftlist = shiftlist.OrderByDescending(x => x.ClientShift_ID).ToList();
            return View(shiftlist);
        }
        public IActionResult InProcessShiftList()
        {

            List<InProcessShiftViewModel> model = new List<InProcessShiftViewModel>();
            List<ClientShiftViewModel> shiftlist = new List<ClientShiftViewModel>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (clinicalInstitution != null)
                {

                    List<ClientShifts> clientShifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID).Where(x => x.Category_ID == 2).ToList();
                    if (clientShifts.Count > 0)
                    {
                        foreach (var clientshift in clientShifts)
                        {
                            string Location = "";
                            if (clientshift.Branch_ID == null || clientshift.Branch_ID == 0)
                            {
                                Location = clinicalInstitution.InstitutionName;
                            }
                            else
                            {
                                Location = _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName;
                            }
                            List<ApplicantAppliedShifts> appliedShifts = _commonService.GetAppliedShiftsbyClientShift_ID(clientshift.ClientShift_ID).Where(x => x.Accepted == true).OrderBy(x => x.Applicant_ID).ToList();

                            if (appliedShifts != null)
                            {
                                List<int> Applicant_IDs = appliedShifts.Select(x => x.Applicant_ID).Distinct().ToList();

                                foreach (int Applicant_ID in Applicant_IDs)
                                {
                                    List<ApplicantAppliedShifts> applicantAppliedShifts = appliedShifts.Where(x => x.Applicant_ID == Applicant_ID).ToList();
                                    List<ApplicantClockInClockOutTime> clockInClockOutTimes = new List<ApplicantClockInClockOutTime>();
                                    foreach (ApplicantAppliedShifts shift in applicantAppliedShifts)
                                    {
                                        clockInClockOutTimes.AddRange(_commonService.GetAppliedShiftClockinClockouttimes(shift.AppliedShift_ID).OrderBy(x => x.WorkingDay).ToList());
                                    }
                                    TimeSpan time = TimeSpan.Zero;
                                    List<ApplicantClockInClockOutTime> completeclockintimes = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                                    if (completeclockintimes.Count > 0)
                                    {
                                        foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                        {
                                            time += times.ClockOutTime - times.ClockInTime;
                                        }
                                    }
                                    Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                                    List<int> Speciality_IDs = _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList();
                                    model.Add(new InProcessShiftViewModel
                                    {

                                        Applicant = new Models.Applicant.ApplicantProfileViewModel()
                                        {
                                            Applicant_ID = app.Applicant_ID,
                                            Availability_ID = app.Availability_ID,
                                            Address = _commonService.GetCityName(user.City_ID) + ", " + user.Address,
                                            User_ID = app.User_ID,
                                            City_ID = user.City_ID,
                                            CertificatiesString = String.Join("; ", _commonService.GetApplicantCertificates(app.Applicant_ID).Select(x => x.CertificateType)),
                                            Email = user.Email,
                                            FirstName = app.FirstName,
                                            IsEligible = app.IsEligible,
                                            LastName = app.LastName,
                                            MiddleName = app.MiddleName,
                                            SpecialitiesString = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetApplicantSpecialities(app.Applicant_ID).Where(c => c.Status == 1).Select(c => c.Speciality_ID).Contains(x.Speciality_ID))),
                                            ZipCode = user.ZipCode,
                                            VisaStatus_ID = app.VisaStatus_ID,
                                            AvailabilityString = _commonService.GetAvailabilities().Where(x => x.Availability_ID == app.Availability_ID).FirstOrDefault().Availability,
                                            VisaStatusString = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).FirstOrDefault().VisaStatus
                                        },
                                        Shift = new ClientShiftViewModel()
                                        {
                                            Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                            Institution_ID = clientshift.Institution_ID,
                                            BranchName = Location,
                                            Category_ID = clientshift.Category_ID,
                                            ClientShift_ID = clientshift.ClientShift_ID,
                                            ClockInTime = clientshift.ClockInTime,
                                            ClockOutTime = clientshift.ClockOutTime,
                                            ContractorCount = clientshift.ContractorCount,
                                            DateOfShift = clientshift.DateOfShift,
                                            EndDate = clientshift.EndDate,
                                            Occurrences = clientshift.Occurrences,
                                            Responsibility = clientshift.Responsibility,
                                            ShiftDescription = clientshift.ShiftDescription,
                                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                                            ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                                            StartDate = clientshift.StartDate,
                                            ShiftsDates = clientshift.ShiftsDates


                                        },
                                        WorkedHours = time.TotalHours.ToString("0.00"),
                                        Applicant_ID = app.Applicant_ID,
                                        ClientShift_ID = clientshift.ClientShift_ID,
                                        FirstName = app.FirstName,
                                        LastName = app.LastName,
                                        MiddleName = app.MiddleName,
                                        Img = app.ProfileImage == null || app.ProfileImage == "" ? "/Upload/User.png" : "/" + app.ProfileImage.Replace('\\', '/'),
                                        LocationName = Location,
                                        PhoneNumber = user.PhoneNumber,
                                        SpecialitiesString = Speciality_IDs != null ? String.Join("; ", _commonService.GetSpecialities().Where(x => Speciality_IDs.Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()) : "",
                                        ClockinClockOutTimes = clockInClockOutTimes,
                                        NumberofShift = clockInClockOutTimes.Count,
                                        CompletedNumberofShift = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count()

                                    });
                                }
                            }
                        }

                    }
                }
            }
            return View(model);

        }
        public IActionResult _ClinetShiftpartial(int Category_ID)
        {
            List<ClientShiftViewModel> shiftlist = new List<ClientShiftViewModel>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                List<ClientShifts> clientShifts = new List<ClientShifts>();
                if (clinicalInstitution != null)
                {
                    if (Category_ID > 0)
                    {
                        clientShifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID).Where(x => x.Category_ID == Category_ID).ToList();
                    }
                    else
                    {
                        clientShifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID).ToList();
                    }
                    if (clientShifts.Count > 0)
                    {
                        foreach (var clientshift in clientShifts)
                        {
                            shiftlist.Add(new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate,
                                EndDate = clientshift.EndDate,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                ShiftsDates = clientshift.ShiftsDates,

                            });



                        }

                    }
                }
            }
            return PartialView("_ShiftListPartial", shiftlist);
        }
        public IActionResult LocationListView()
        {
            List<LocationViewModel> locationlist = new List<LocationViewModel>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (clinicalInstitution != null)
                {
                    locationlist.Add(new LocationViewModel()
                    {
                        Institution_ID = clinicalInstitution.Institution_ID,
                        City_ID = user.City_ID,
                        ContactName = clinicalInstitution.ContactPerson,
                        Name = clinicalInstitution.InstitutionName,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        ZipCode = user.ZipCode,
                        cityname = _commonService.GetCityName(user.City_ID),
                        Email = user.Email
                    });

                    List<ClinicalInstitutionBranches> locations = _commonService.GetLocations(clinicalInstitution.Institution_ID);
                    if (locations.Count > 0)
                    {
                        foreach (var location in locations)
                        {
                            locationlist.Add(new LocationViewModel()
                            {
                                Institution_ID = location.Institution_ID,
                                City_ID = location.CityId,
                                ContactName = location.ContactName,
                                Branch_ID = location.Branch_ID,
                                Name = location.BranchName,
                                Address = location.Address,
                                PhoneNumber = location.PhoneNumber,
                                ZipCode = location.ZipCode,
                                cityname = _commonService.GetCityName(location.CityId),
                                Email = location.Email
                            });
                        }
                    }
                }
            }
            return View(locationlist);
        }

        [HttpGet]
        public IActionResult ShiftDetails(int ClientShift_ID)
        {

            if (ClientShift_ID > 0)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                        if (clientshift.Institution_ID == clinicalInstitution.Institution_ID)
                        {
                            List<Applicants> applicants = _commonService.GetApplicantsPicked();
                            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
                            List<ShiftDetailViewModel> ShiftDetailList = new List<ShiftDetailViewModel>();

                            var specialities = _commonService.GetApplicantSpeciality();
                            var cityNames = _commonService.GetApplicantCityName();
                            var zipCode = _commonService.GetApplicantZipCode();

                            for (int i = 0; i < applicants.Count; i++)
                            {
                                for (int j = i; j < zipCode.Count; j++)
                                {
                                    for (int g = i; g < cityNames.Count; g++)
                                    {
                                        for (int h = i; h < specialities.Count; h++)
                                        {
                                            ShiftDetailList.Add(new ShiftDetailViewModel()
                                            {
                                                FirstName = applicants[i].FirstName,
                                                LastName = applicants[i].LastName,
                                                Speciality = specialities[h].SpecialityName,
                                                ZipCode = zipCode[j].ZipCode,
                                                City = cityNames[g].city_name,
                                                State = "Texas",
                                                Imgsrc = applicants[i].ProfileImage != null ? applicants[i].ProfileImage.Replace('\\', '/') : null
                                            });
                                            break;
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }

                            ClientShiftViewModel ClientShiftViewModel = new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate,
                                EndDate = clientshift.EndDate,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                                Occurrences = clientshift.Occurrences,
                                ShiftsDates = clientshift.ShiftsDates,
                                ShiftDetailPicked = ShiftDetailList
                            };

                            return View(ClientShiftViewModel);
                        }
                        else
                        {
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("ClientShiftList", "ClinicalInstitution");
            }
        }

        [HttpGet]
        public IActionResult NewClientShift()
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (clinicalInstitution != null)
                {
                    var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                    ViewBag.Speciality = specialities;
                    var branches = new List<SelectListItem>();
                    branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                    branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "BranchName").ToList());
                    ViewBag.Locations = branches;
                    var shiftlables = new SelectList(_commonService.GetShiftLabels(), "ShiftLabel_ID", "ShiftLabelName");
                    ViewBag.ShiftLabels = shiftlables;
                }
            }
            return View();

        }
        [HttpPost]
        public IActionResult NewClientShift(ClientShiftViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        if (model.Specialities > 0)
                        {
                            List<ShiftSpecialities> shiftSpecialities = new List<ShiftSpecialities>();
                            shiftSpecialities.Add(new ShiftSpecialities() { Speciality_ID = model.Specialities });



                            if (model.ShiftsDates != "")
                            {
                                var dates = model.ShiftsDates.Split(',').ToList();
                                dates = dates.OrderBy(x => DateTime.Parse(x)).ToList();
                                ClientSpecialties clientSpecialties = _commonService.GetClientSpecialtiesList(clinicalInstitution.Institution_ID).Where(x => x.Speciality_ID == model.Specialities).FirstOrDefault();
                                if (clientSpecialties != null)
                                {
                                    ClientSpecialtiesCosts Costs = _commonService.GetClientSpecialtiesCostbyId(clientSpecialties.ClientSpeciality_ID).Where(x => x.ShiftLabel_ID == model.ShiftLabel_ID).FirstOrDefault();
                                    if (Costs != null)
                                    {
                                        ClientShifts clientShift = new ClientShifts()
                                        {
                                            ClockInTime = model.ClockInTime,
                                            ClockOutTime = model.ClockOutTime,
                                            HourlyRate = Costs != null ? Costs.Cost : 0,
                                            ContractorCount = model.ContractorCount,
                                            StartDate = DateTime.Parse(dates[0]),
                                            EndDate = DateTime.Parse(dates[dates.Count - 1]),
                                            Responsibility = model.Responsibility,
                                            ShiftDescription = model.ShiftDescription,
                                            DateOfShift = DateTime.Now.Date,
                                            ShiftExpirationDate = DateTime.Parse(dates[dates.Count - 1]),
                                            Institution_ID = clinicalInstitution.Institution_ID,
                                            institution = clinicalInstitution,
                                            Occurrences = dates.Count,
                                            ShiftLabel_ID = model.ShiftLabel_ID,
                                            ShiftsDates = String.Join(',', dates),


                                        };
                                        if (model.Branch_ID != 0)
                                        {
                                            clientShift.Branch_ID = model.Branch_ID;
                                        }
                                        try
                                        {
                                            _commonService.AddClientShift(clientShift, shiftSpecialities);

                                        }
                                        catch
                                        {
                                            ModelState.AddModelError(string.Empty, "Please try again!");
                                        }
                                    }
                                }

                            }
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Specialities can not be empty!");
                        }
                        var branches = new List<SelectListItem>();
                        branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                        branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "Name").ToList());
                        ViewBag.Locations = branches;
                        var shiftlables = new SelectList(_commonService.GetShiftLabels(), "ShiftLabel_ID", "ShiftLabelName");
                        ViewBag.ShiftLabels = shiftlables;
                        var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = specialities;

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditShift(int ClientShift_ID)
        {
            if (ClientShift_ID > 0)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (userId != null)
                {

                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = specialities;
                        var branches = new List<SelectListItem>();
                        branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                        branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "BranchName").ToList());
                        ViewBag.Locations = branches;
                        var shiftlables = new SelectList(_commonService.GetShiftLabels(), "ShiftLabel_ID", "ShiftLabelName");
                        ViewBag.ShiftLabels = shiftlables;
                    }
                    if (clinicalInstitution != null)
                    {
                        ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                        if (clientshift.Institution_ID == clinicalInstitution.Institution_ID)
                        {
                            ClientShiftViewModel ClientShiftViewModel = new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate.Date,
                                EndDate = clientshift.EndDate.Date,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift.Date,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                                Occurrences = clientshift.Occurrences,

                                ShiftsDates = clientshift.ShiftsDates,


                            };

                            return View(ClientShiftViewModel);
                        }
                        else
                        {
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("ClientShiftList", "ClinicalInstitution");
            }

        }
        [HttpPost]
        public IActionResult EditShift(ClientShiftViewModel model, int ClientShift_ID)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        if (model.Specialities > 0)
                        {
                            List<ShiftSpecialities> shiftSpecialities = new List<ShiftSpecialities>();

                            shiftSpecialities.Add(new ShiftSpecialities() { Speciality_ID = model.Specialities });

                            if (model.ShiftsDates != "")
                            {
                                var dates = model.ShiftsDates.Split(',').ToList();
                                dates = dates.OrderBy(x => DateTime.Parse(x)).ToList();
                                ClientSpecialties specialties = _commonService.GetClientSpecialtiesList(clinicalInstitution.Institution_ID).Where(x => x.Speciality_ID == model.Specialities).FirstOrDefault();
                                if (specialties != null)
                                {
                                    List<ClientSpecialtiesCosts> costs = _commonService.GetClientSpecialtiesCostbyId(specialties.ClientSpeciality_ID).ToList();
                                    if (costs != null)
                                    {
                                        var HourlyRate = costs.Where(x => x.ShiftLabel_ID == model.ShiftLabel_ID).FirstOrDefault();
                                        ClientShifts clientShift = _commonService.GetClientShiftByID(ClientShift_ID);
                                        {

                                            clientShift.ClockInTime = model.ClockInTime;
                                            clientShift.ClockOutTime = model.ClockOutTime;
                                            clientShift.HourlyRate = 0;
                                            clientShift.ContractorCount = model.ContractorCount;
                                            clientShift.StartDate = DateTime.Parse(dates[0]);
                                            clientShift.EndDate = DateTime.Parse(dates[dates.Count - 1]);
                                            clientShift.Responsibility = model.Responsibility;
                                            clientShift.ShiftDescription = model.ShiftDescription;
                                            clientShift.ShiftExpirationDate = DateTime.Parse(dates[dates.Count - 1]);
                                            clientShift.Institution_ID = clinicalInstitution.Institution_ID;
                                            clientShift.institution = clinicalInstitution;
                                            clientShift.Occurrences = dates.Count;
                                            clientShift.ShiftLabel_ID = model.ShiftLabel_ID;
                                            clientShift.ShiftsDates = String.Join(',', dates);
                                            clientShift.HourlyRate = HourlyRate != null ? HourlyRate.Cost : 0;

                                        };
                                        if (model.Branch_ID != 0)
                                        {
                                            clientShift.Branch_ID = model.Branch_ID;
                                        }
                                        try
                                        {
                                            _commonService.UpdateShift(clientShift, shiftSpecialities);

                                        }
                                        catch
                                        {
                                            ModelState.AddModelError(string.Empty, "Please try again!");
                                        }
                                    }
                                }
                            }
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Specialities can not be empty!");
                        }
                        var branches = new List<SelectListItem>();
                        branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                        branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "Name").ToList());
                        ViewBag.Locations = branches;
                        var shiftlables = new SelectList(_commonService.GetShiftLabels(), "ShiftLabel_ID", "ShiftLabelName");
                        ViewBag.ShiftLabels = shiftlables;
                        var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = specialities;

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            return View(model);
        }
        [HttpGet]
        public IActionResult EditShiftContractorCount(int ClientShift_ID)
        {
            if (ClientShift_ID > 0)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (userId != null)
                {

                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = specialities;
                        var branches = new List<SelectListItem>();
                        branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                        branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "BranchName").ToList());
                        ViewBag.Locations = branches;
                        var shiftlables = new SelectList(_commonService.GetShiftLabels(), "ShiftLabel_ID", "ShiftLabelName");
                        ViewBag.ShiftLabels = shiftlables;
                    }
                    if (clinicalInstitution != null)
                    {
                        ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                        if (clientshift.Institution_ID == clinicalInstitution.Institution_ID)
                        {
                            ClientShiftViewModel ClientShiftViewModel = new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate.Date,
                                EndDate = clientshift.EndDate.Date,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift.Date,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),

                                Occurrences = clientshift.Occurrences,

                                ShiftsDates = clientshift.ShiftsDates,


                            };

                            return View(ClientShiftViewModel);
                        }
                        else
                        {
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("ClientShiftList", "ClinicalInstitution");
            }

        }
        [HttpPost]
        public IActionResult EditShiftContractorCount(int ContractorCount, int ClientShift_ID)
        {
            if (ContractorCount > 0 && ClientShift_ID > 0)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        ClientShifts clientShift = _commonService.GetClientShiftByID(ClientShift_ID);
                        if (clientShift != null)
                        {
                            if (clientShift.ContractorCount != ContractorCount)
                            {
                                clientShift.ContractorCount = ContractorCount;
                                _commonService.UpdateClientShift(clientShift);

                            }

                        }
                    }
                }
            }

            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
        }

        public IActionResult RemoveShift(int ClientShift_ID)
        {

            if (ClientShift_ID > 0)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (userId != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                    if (clinicalInstitution != null)
                    {
                        ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                        if (clientshift.Institution_ID == clinicalInstitution.Institution_ID)
                        {
                            ClientShiftViewModel ClientShiftViewModel = new ClientShiftViewModel()
                            {
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,

                                ContractorCount = clientshift.ContractorCount,
                                StartDate = clientshift.StartDate,
                                EndDate = clientshift.EndDate,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                DateOfShift = clientshift.DateOfShift,
                                Institution_ID = clientshift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).FirstOrDefault(),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                                ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                                Occurrences = clientshift.Occurrences,
                                ShiftsDates = clientshift.ShiftsDates,


                            };

                            return View(ClientShiftViewModel);
                        }
                        else
                        {
                            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("ClientShiftList", "ClinicalInstitution");
            }
        }
        [HttpPost, ActionName("RemoveShift")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveShiftConfirmed(int ClientShift_ID)
        {
            if (ClientShift_ID > 0)
            {
                _commonService.RemoveShift(ClientShift_ID);
            }
            return RedirectToAction("ClientShiftList", "ClinicalInstitution");
        }
        [HttpGet]
        public IActionResult Search()
        {
            SearchViewModel model = new SearchViewModel();
            model.Search = new SearchModel();
            model.Search.Distance = 50;
            model.Applicants = new List<Models.Home.ApplicantModel>();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (userId != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);

                if (clinicalInstitution != null)
                {


                    List<Applicants> applicants = _commonService.Search(Specialities: null, StartDate: DateTime.MinValue, EndDate: DateTime.MinValue, distance: 50, latitude: user.Latitude, longitude: user.Longitude);
                    if (applicants.Count > 0)
                    {
                        foreach (Applicants applicant in applicants)
                        {
                            var clientshifts = _commonService.GetAvailableShiftsforApplicant(applicant.Applicant_ID, clinicalInstitution.Institution_ID);
                            if (clientshifts != null)
                            {
                                List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();

                                model.Applicants.Add(new Models.Home.ApplicantModel()
                                {
                                    IsAvailable = applicant.IsAvailable,

                                    Applicant_ID = applicant.Applicant_ID,
                                    FirstName = applicant.FirstName,
                                    LastName = applicant.LastName,
                                    Availability_ID = applicant.Availability_ID,
                                    VisaStatus_ID = applicant.VisaStatus_ID,
                                    VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).Select(x => x.VisaStatus).FirstOrDefault(),

                                    CertificatiesString = String.Join(", ", certificates),
                                    SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Where(x => x.Status == 1).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                                });
                            }

                        }
                    }
                    var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                    var availabilites = new SelectList(_commonService.GetAvailableTypes(), "ApplicantAvailableType_ID", "ApplicantAvailableType_Name");
                    var branches = new List<SelectListItem>();
                    branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                    branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "BranchName").ToList());
                    branches.Where(x => x.Value == "0").FirstOrDefault().Selected = true;
                    ViewBag.Locations = branches;
                    ViewBag.Speciality = specialities;
                    ViewBag.Availability = availabilites;
                    ViewBag.LocationName = clinicalInstitution.InstitutionName;

                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {


            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (userId != null)
            {
                float longitude = user.Longitude;
                float latitude = user.Latitude;
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                if (model.Search.Location != 0)
                {
                    var location = _commonService.GetlocationbyId(model.Search.Location);
                    if (location != null)
                    {
                        longitude = location.Longitude;
                        latitude = location.Latitude;
                    }
                }
                if (clinicalInstitution != null)
                {
                    DateTime StartDate = DateTime.MinValue;
                    DateTime EndDate = DateTime.MinValue;
                    if (model.Search.Date != null)
                    {
                        var dates = model.Search.Date.Split(" - ");
                        if (dates.Count() > 0)
                        {
                            StartDate = DateTime.Parse(dates[0]);
                            EndDate = DateTime.Parse(dates[1]);
                        }

                    }
                    List<Applicants> applicants = new List<Applicants>();
                    model.Applicants = new List<Models.Home.ApplicantModel>();
                    if (model.Search == null)
                    {
                        applicants = _commonService.Search(Specialities: null, StartDate: StartDate, EndDate: EndDate, distance: 50, latitude: latitude, longitude: longitude);
                    }
                    else
                    {


                        if (model.Search.Distance > 0)
                        {

                            applicants = _commonService.Search(Specialities: model.Search.Specialities, StartDate: StartDate, EndDate: EndDate, distance: model.Search.Distance, latitude: latitude, longitude: longitude);
                        }
                        else
                        {
                            applicants = _commonService.Search(Specialities: model.Search.Specialities, StartDate: StartDate, EndDate: EndDate, distance: 50, latitude: latitude, longitude: longitude);
                        }

                    }
                    if (applicants.Count > 0)
                    {
                        foreach (Applicants applicant in applicants)
                        {

                            List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();
                            List<ApplicantAvailables> availables = _commonService.GetApplicantAvailables(applicant.Applicant_ID).Where(x => x.AvailableDay >= StartDate && x.AvailableDay <= EndDate).OrderBy(X => X.AvailableDay).Take(5).ToList();
                            model.Applicants.Add(new Models.Home.ApplicantModel()
                            {
                                IsAvailable = applicant.IsAvailable,

                                Applicant_ID = applicant.Applicant_ID,
                                FirstName = applicant.FirstName,
                                LastName = applicant.LastName,
                                Availability_ID = applicant.Availability_ID,
                                VisaStatus_ID = applicant.VisaStatus_ID,
                                availability = availables,
                                CertificatiesString = String.Join(", ", certificates),
                                SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Where(x => x.Status == 1).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                            });

                        }
                    }
                    var specialities = new SelectList(_commonService.GetClientSpecialties(clinicalInstitution.Institution_ID), "Speciality_ID", "SpecialityName");
                    var availabilites = new SelectList(_commonService.GetAvailableTypes(), "ApplicantAvailableType_ID", "ApplicantAvailableType_Name");
                    ViewBag.Speciality = specialities;

                    ViewBag.Availability = availabilites;
                    var branches = new List<SelectListItem>();
                    branches.Add(new SelectListItem() { Text = clinicalInstitution.InstitutionName, Value = "0" });
                    branches.AddRange(new SelectList(_commonService.GetLocations(clinicalInstitution.Institution_ID), "Branch_ID", "BranchName").ToList());
                    branches.Where(x => x.Value == "0").FirstOrDefault().Selected = true;
                    ViewBag.Locations = branches;
                    if (model.Search.Location == 0)
                    {
                        ViewBag.LocationName = clinicalInstitution.InstitutionName;
                    }
                    else
                    {
                        ViewBag.LocationName = branches.Where(x => x.Value == model.Search.Location.ToString()).FirstOrDefault().Text;
                    }

                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ApplicantDetails(int Applicant_ID)
        {
            if (Applicant_ID != 0)
            {
                ApplicantDetailViewModel model = null;


                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (applicant != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();

                    List<int> spec = _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Where(x => x.Status == 1).Select(x => x.Speciality_ID).ToList();

                    model = new ApplicantDetailViewModel()
                    {

                        Applicant = new Models.Applicant.ApplicantProfileViewModel()
                        {
                            Applicant_ID = applicant.Applicant_ID,
                            Availability_ID = applicant.Availability_ID,
                            Address = _commonService.GetCityName(user.City_ID) + ", " + user.Address,
                            PhoneNumber = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault().PhoneNumber,
                            ImgSrc = applicant.ProfileImage == null || applicant.ProfileImage == "" ? "/Upload/User.png" : "/" + applicant.ProfileImage.Replace('\\', '/'),
                            User_ID = applicant.User_ID,
                            City_ID = user.City_ID,
                            CertificatiesString = String.Join("; ", _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateType)),
                            Email = user.Email,
                            FirstName = applicant.FirstName,
                            IsEligible = applicant.IsEligible,
                            LastName = applicant.LastName,
                            MiddleName = applicant.MiddleName,
                            SpecialitiesString = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(c => c.Speciality_ID).Contains(x.Speciality_ID))),
                            ZipCode = user.ZipCode,
                            VisaStatus_ID = applicant.VisaStatus_ID,


                            VisaStatusString = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).FirstOrDefault().VisaStatus
                        },
                    };
                    var workhistories = _commonService.GetApplicantWorkHistory(Applicant_ID);
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
                                WorkHistory_ID = workHistory.WorkHistory_ID,
                                SpecialityName = workHistory.JobTitle

                            });
                        }
                    }
                    var Specialities = _commonService.GetApplicantSpecialities(Applicant_ID).Where(x => x.Status == 1);
                    if (Specialities != null)
                    {
                        model.specialities = new List<ApplicantSpecialitisViewModel>();
                        foreach (ApplicantSpecialities speciality in Specialities)
                        {
                            model.specialities.Add(new ApplicantSpecialitisViewModel
                            {
                                LegabilityStates = _commonService.GetStates(231).Where(x => speciality.LegabilityStates.Split("; ").ToList().Contains(x.id.ToString())).Select(x => x.state_name).ToList(),
                                License = speciality.License,
                                Speciality_ID = speciality.Speciality_ID,
                                Id = speciality.ApplicantSpeciality_ID,
                                SpecialityName = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).FirstOrDefault().SpecialityName


                            });
                        }
                    }
                    var certificates = _commonService.GetApplicantCertificates(Applicant_ID);
                    if (certificates != null)
                    {
                        model.certificates = new List<ApplicantCertificateViewModel>();

                        foreach (ApplicantCertificates certificate in certificates)
                        {
                            model.certificates.Add(new ApplicantCertificateViewModel()
                            {
                                CeritifcationImgsrc = certificate.CeritificationImg,
                                CertificateType = certificate.CertificateTypes,
                                Certification_ID = certificate.Ceritification_ID,
                                Applicant_ID = certificate.Applicant_ID


                            });
                        }

                    }

                }

                return View(model);
            }

            return RedirectToAction("Contractors", "ClinicalInstitution");
        }
        public IActionResult InProccessShiftDetail(int ClientShift_ID, int Applicant_ID)
        {

            if (ClientShift_ID != 0 && Applicant_ID != 0)
            {
                InProcessShiftViewModel model = null;

                ClientShifts shift = _commonService.GetClientShiftByID(ClientShift_ID);
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (shift != null && applicant != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                    List<ApplicantAppliedShifts> appliedShifts = _commonService.GetAppliedShiftsbyClientShift_ID(ClientShift_ID).Where(x => x.Applicant_ID == Applicant_ID).ToList();
                    List<ApplicantClockInClockOutTime> clockinClockOutTimes = new List<ApplicantClockInClockOutTime>();
                    if (appliedShifts.Count > 0)
                    {
                        foreach (ApplicantAppliedShifts app in appliedShifts)
                        {
                            clockinClockOutTimes.AddRange(_commonService.GetAppliedShiftClockinClockouttimes(app.AppliedShift_ID).ToList());
                        }
                        TimeSpan time = TimeSpan.Zero;
                        List<ApplicantClockInClockOutTime> completeclockintimes = clockinClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                        if (completeclockintimes.Count > 0)
                        {
                            foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                            {
                                time += times.ClockOutTime - times.ClockInTime;
                            }
                        }
                        List<int> spec = _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Where(x => x.Status == 1).Select(x => x.Speciality_ID).ToList();

                        model = new InProcessShiftViewModel()
                        {
                            Img = applicant.ProfileImage == null || applicant.ProfileImage == "" ? "/Upload/User.png" : "/" + applicant.ProfileImage.Replace('\\', '/'),
                            PhoneNumber = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault().PhoneNumber,
                            SpecialitiesString = String.Join("; ", _commonService.GetSpecialities().Where(x => spec.Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                            ClockinClockOutTimes = clockinClockOutTimes,
                            FirstName = applicant.FirstName,
                            LastName = applicant.LastName,
                            MiddleName = applicant.MiddleName,
                            WorkedHours = time.TotalHours.ToString("0.00"),
                            Applicant_ID = Applicant_ID,
                            ClientShift_ID = ClientShift_ID,

                            CompletedNumberofShift = clockinClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count(),
                            NumberofShift = clockinClockOutTimes.Count,
                            LocationName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            Applicant = new Models.Applicant.ApplicantProfileViewModel()
                            {
                                Applicant_ID = applicant.Applicant_ID,
                                Availability_ID = applicant.Availability_ID,
                                Address = _commonService.GetCityName(user.City_ID) + ", " + user.Address,
                                User_ID = applicant.User_ID,
                                City_ID = user.City_ID,
                                CertificatiesString = String.Join("; ", _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateType)),
                                Email = user.Email,
                                FirstName = applicant.FirstName,
                                IsEligible = applicant.IsEligible,
                                LastName = applicant.LastName,
                                MiddleName = applicant.MiddleName,
                                SpecialitiesString = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(c => c.Speciality_ID).Contains(x.Speciality_ID))),
                                ZipCode = user.ZipCode,
                                VisaStatus_ID = applicant.VisaStatus_ID,

                                VisaStatusString = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).FirstOrDefault().VisaStatus
                            },
                            Shift = new ClientShiftViewModel()
                            {
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                Institution_ID = shift.Institution_ID,
                                BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                                Category_ID = shift.Category_ID,
                                ClientShift_ID = shift.ClientShift_ID,
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,
                                ContractorCount = shift.ContractorCount,
                                DateOfShift = shift.DateOfShift,
                                EndDate = shift.EndDate,
                                Occurrences = shift.Occurrences,
                                Responsibility = shift.Responsibility,
                                ShiftDescription = shift.ShiftDescription,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                                ShiftLabel_ID = shift.ShiftLabel_ID,
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),

                                StartDate = shift.StartDate,
                                ShiftsDates = shift.ShiftsDates

                            },

                        };
                        var workhistories = _commonService.GetApplicantWorkHistory(Applicant_ID);
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
                                    WorkHistory_ID = workHistory.WorkHistory_ID,
                                    SpecialityName = workHistory.JobTitle

                                });
                            }
                        }
                        var Specialities = _commonService.GetApplicantSpecialities(Applicant_ID).Where(x => x.Status == 1);
                        if (Specialities != null)
                        {
                            model.specialities = new List<ApplicantSpecialitisViewModel>();
                            foreach (ApplicantSpecialities speciality in Specialities)
                            {
                                model.specialities.Add(new ApplicantSpecialitisViewModel
                                {
                                    LegabilityStates = _commonService.GetStates(231).Where(x => speciality.LegabilityStates.Split("; ").ToList().Contains(x.id.ToString())).Select(x => x.state_name).ToList(),
                                    License = speciality.License,
                                    Speciality_ID = speciality.Speciality_ID,
                                    Id = speciality.ApplicantSpeciality_ID,
                                    SpecialityName = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).FirstOrDefault().SpecialityName


                                });
                            }
                        }
                        var certificates = _commonService.GetApplicantCertificates(Applicant_ID);
                        if (certificates != null)
                        {
                            model.certificates = new List<ApplicantCertificateViewModel>();

                            foreach (ApplicantCertificates certificate in certificates)
                            {
                                model.certificates.Add(new ApplicantCertificateViewModel()
                                {
                                    CeritifcationImgsrc = certificate.CeritificationImg,
                                    CertificateType = certificate.CertificateTypes,
                                    Certification_ID = certificate.Ceritification_ID,
                                    Applicant_ID = certificate.Applicant_ID


                                });
                            }

                        }

                    }

                    return View(model);
                }
            }
            return RedirectToAction("InProcessShiftList", "ClinicalInstitution");
        }
        public async Task<IActionResult> Profile()
        {
            var institutiontypes = new SelectList(_commonService.GetInstitutionTypes(), "InstitutionType_ID", "InstitutionTypeName");
            ViewBag.InstitutionTypes = institutiontypes;
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                if ((user.ChangesMakedTime - DateTime.Now).Days != 0 || ((user.ChangesMakedTime - DateTime.Now).Days == 0 && (user.ChangesMakedTime - DateTime.Now).Hours != 0))
                {
                    user.ChangesCount = 0;
                    user.ChangesLocked = false;
                    await _userManager.UpdateAsync(user);
                }
                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);

                if (clinical != null)
                {
                    ClientProfileViewModel model = new ClientProfileViewModel();

                    model.Profile = new ClinicalInstitutionProfileViewModel()
                    {
                        ContactPerson = clinical.ContactPerson,
                        ContactTitle = clinical.ContactTitle,
                        InstitutionName = clinical.InstitutionName,
                        InstitutionTaxId = clinical.InstitutionTaxId,
                        InstitutionType_ID = clinical.InstitutionType_ID,
                        City_ID = user.City_ID,
                        Address = user.Address,
                        ZipCode = user.ZipCode,
                        Institution_ID = clinical.Institution_ID,
                        InstitutionDescription = clinical.InstitutionDescription,
                        User_ID = clinical.User_ID,
                        Email = user.Email,
                        Disabled = user.ChangesLocked

                    };


                    var city = _commonService.GetCitiesByCityid(user.City_ID);
                    model.Profile.State_ID = city != null ? city.state_id : 0;
                    var states = new SelectList(_commonService.GetStates(city.country_id), "id", "state_name");
                    ViewBag.States = states;
                    var cities = new SelectList(_commonService.GetCities(city.state_id), "id", "city_name");
                    ViewBag.Cities = cities;
                    model.LogoSrc = "/" + clinical.Logo.Replace('\\', '/');
                    model.PhoneNumber = user.PhoneNumber;
                    model.Specialties = new List<SpecialtiesCosts>();
                    var specialities = _commonService.GetClientSpecialtiesList(clinical.Institution_ID);
                    var allSpecialties = _commonService.GetSpecialities();
                    var shiftlables = _commonService.GetShiftLabels();
                    if (specialities != null)
                    {
                        foreach (var specialty in specialities)
                        {
                            SpecialtiesCosts costs = new SpecialtiesCosts();
                            costs.Specialty_ID = specialty.Speciality_ID;
                            costs.Cost = new List<ShiftLabelsCost>();
                            costs.SpecialtyName = allSpecialties.Where(x => x.Speciality_ID == specialty.Speciality_ID).Select(x => x.SpecialityName).FirstOrDefault();
                            var clientSpecialtiesCosts = _commonService.GetClientSpecialtiesCostbyId(specialty.ClientSpeciality_ID);
                            foreach (ShiftLabels label in shiftlables)
                            {
                                var labelCost = clientSpecialtiesCosts.Where(x => x.ShiftLabel_ID == label.ShiftLabel_ID).FirstOrDefault();

                                costs.Cost.Add(new ShiftLabelsCost()
                                {
                                    ShiftLabel_ID = label.ShiftLabel_ID,
                                    Cost = labelCost != null ? labelCost.Cost : 0,
                                    ShiftLabelName = label.ShiftLabelName,
                                });
                            }
                            model.Specialties.Add(costs);
                        }
                    }
                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ClientProfileViewModel model)
        {
            var institutiontypes = new SelectList(_commonService.GetInstitutionTypes(), "InstitutionType_ID", "InstitutionTypeName");
            ViewBag.InstitutionTypes = institutiontypes;
            string logostr = "";
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                if (user != null)
                {

                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                    if (clinical != null)
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
                        clinical.ContactPerson = model.Profile.ContactPerson;
                        clinical.ContactTitle = model.Profile.ContactTitle;
                        clinical.InstitutionType_ID = model.Profile.InstitutionType_ID;
                        clinical.InstitutionDescription = model.Profile.InstitutionDescription;
                        clinical.InstitutionName = model.Profile.InstitutionName;
                        user.City_ID = model.Profile.City_ID;
                        user.Address = model.Profile.Address;
                        user.ZipCode = model.Profile.ZipCode;


                        if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                        {
                            if (user.ChangesCount >= 4)
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

                        _commonService.UpdateClinical(clinical);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Profile", "ClinicalInstitution");

                    }
                };
            }
            return View(model);
        }
        public async Task<IActionResult> SaveProfileImage(ClientProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            {

                if (user != null)
                {

                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                    if (model.Logo != null)
                    {
                        string path = "ClinicalLogo";
                        string profilefilename = SaveProfileFile(model.Logo, path, user.Id.ToString());
                        if (profilefilename != "")
                        {
                            clinical.Logo = profilefilename;
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

                        _commonService.UpdateClinical(clinical);
                        await _userManager.UpdateAsync(user);
                    }
                }

            }
            return RedirectToAction("Profile");
        }
        public IActionResult AddSpecialityCosts(ClientProfileViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (user != null)
            {
                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                var specialities = _commonService.GetClientSpecialtiesList(clinical.Institution_ID);

                if (clinical != null)
                {
                    if (model.Specialties != null)
                    {
                        List<ClientCostChanges> changes = new List<ClientCostChanges>();
                        foreach (var speciaties in model.Specialties)
                        {
                            if (speciaties.Specialty_ID != 0 && speciaties.Cost != null)
                            {

                                foreach (var Cost in speciaties.Cost)
                                {
                                    changes.Add(new ClientCostChanges()
                                    {
                                        Speciality_ID = speciaties.Specialty_ID,
                                        Institution_ID = clinical.Institution_ID,
                                        ShiftLabel_ID = Cost.ShiftLabel_ID,
                                        Cost = Cost.Cost.GetValueOrDefault(),
                                        status = true

                                    });

                                }
                            }
                        }

                        List<SpecialtiesCosts> specialitiesCost = new List<SpecialtiesCosts>();
                        if (model.Specialties.Count > specialities.Count)
                        {
                            List<ClientSpecialtiesCosts> cost = new List<ClientSpecialtiesCosts>();    
                            if (clinical != null)
                            {
                                for (int i = 0; i < model.Specialties.Count; i++)
                                {
                                    if (i >= specialities.Count)
                                    {
                                        specialitiesCost.Add(model.Specialties[i]);
                                    }
                                }
                                var labelsCount = _commonService.GetShiftLabels().Count;
                                var addedSpecialties = specialitiesCost.Where(z => z.Cost.Where(c => c.Cost == 0 || c.Cost == null).Count() != labelsCount).Select(z => z.Specialty_ID).ToList();
                                if (addedSpecialties != null)
                                {
                                    List<ClientSpecialties> clientSpecialties = new List<ClientSpecialties>();
                                    foreach (int SpecialtyID in addedSpecialties)
                                    {
                                        clientSpecialties.Add(new ClientSpecialties()
                                        {
                                            Institution_ID = clinical.Institution_ID,
                                            Speciality_ID = SpecialtyID,
                                        });
                                    }
                                    _commonService.AddClientSpecialites(clientSpecialties);

                                    foreach (ClientSpecialties spec in clientSpecialties)
                                    {
                                        var labelcosts = model.Specialties.Where(z => z.Specialty_ID == spec.Speciality_ID).FirstOrDefault().Cost;
                                        var zeroCostCount = labelcosts.Where(z => z.Cost == 0 || z.Cost == null).Count();
                                        if (zeroCostCount != labelsCount)
                                        {
                                            foreach (ShiftLabelsCost labelsCost in labelcosts)
                                            {

                                                cost.Add(new ClientSpecialtiesCosts()
                                                {
                                                    ClientSpeciality_ID = spec.ClientSpeciality_ID,
                                                    ShiftLabel_ID = labelsCost.ShiftLabel_ID,
                                                    Cost = labelsCost.Cost.GetValueOrDefault(),

                                                });
                                            }
                                        }
                                    }

                                    if (cost.Count == 0)
                                    {
                                        ModelState.AddModelError("", "Please Insert Costs");
                                    }
                                    else
                                    {
                                        var answer = _commonService.AddClientSpecialtiesCost(cost); 
                                        if (answer)
                                        {
                                            return RedirectToAction("Profile", clrole); 
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Please try again");
                                        }
                                    }
                                }
                            }

                        }

                        if (changes.Count > 0)
                        {
                            var answer = _commonService.AddClientCostChanges(changes);
                            
                            if (answer)
                            {
                                int count = 0;
                                for (int i = 0; i < specialities.Count; i++)
                                {
                                    var costs = _commonService.GetClientSpecialityCost(specialities[i].ClientSpeciality_ID).ToList();
                                    for (int j = 0; j < costs.Count; j++)
                                    {
                                        costs[j].Cost = changes[count].Cost;
                                        _dbcontext.SaveChanges();
                                        count++;
                                    }

                                }
                                //clinical.Status = 3;
                                _commonService.UpdateClinical(clinical);
                            }

                        }
                    }
                }
            }

            return RedirectToAction("Profile", clrole);
        }
        
        public async Task<IActionResult> Dashboard()
        {
            DashBoardViewModel model = new DashBoardViewModel();
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(user.Id);
                if (clinicalInstitution != null)
                {
                    //Updateing all expired Shifts
                    _commonService.UpdateClientExpiredShifts(clinicalInstitution.Institution_ID);
                    List<ClientShifts> shifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID);
                    model.Yearshifts = new ShiftsCountViewModel();
                    model.Yearshifts.Created = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Yearshifts.NotStarted = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 1).Count();
                    model.Yearshifts.Active = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 2).Count();
                    model.Yearshifts.Completed = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 4).Count();
                    model.Yearshifts.Incomplete = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 5).Count();
                    model.Yearshifts.Cancelled = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 4).Count();
                    model.Allshifts = new ShiftsCountViewModel();
                    model.Allshifts.Created = shifts.Count();
                    model.Allshifts.NotStarted = shifts.Where(x => x.Category_ID == 1).Count();
                    model.Allshifts.Active = shifts.Where(x => x.Category_ID == 2).Count();
                    model.Allshifts.Completed = shifts.Where(x => x.Category_ID == 4).Count();
                    model.Allshifts.Incomplete = shifts.Where(x => x.Category_ID == 5).Count();
                    model.Allshifts.Cancelled = shifts.Where(x => x.Category_ID == 4).Count();
                    model.Q1shifts = new ShiftsCountViewModel();
                    model.Q1shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts = new ShiftsCountViewModel();
                    model.Q2shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts = new ShiftsCountViewModel();
                    model.Q3shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts = new ShiftsCountViewModel();
                    model.Q4shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();

                    List<int> locations_id = shifts.GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.AllLocation = new List<LocationCounViewModel>();
                    foreach (int id in locations_id)
                    {
                        if (id == 0)
                        {

                            model.AllLocation.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.AllLocation.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> years_lid = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.YearLocation = new List<LocationCounViewModel>();
                    foreach (int id in years_lid)
                    {
                        if (id == 0)
                        {

                            model.YearLocation.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.YearLocation.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> Q1_lid = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q1Location = new List<LocationCounViewModel>();
                    foreach (int id in Q1_lid)
                    {
                        if (id == 0)
                        {

                            model.Q1Location.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q1Location.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> Q2_lid = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q2Location = new List<LocationCounViewModel>();
                    foreach (int id in Q2_lid)
                    {
                        if (id == 0)
                        {

                            model.Q2Location.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q2Location.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                    }
                    List<int> Q3_lid = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q3Location = new List<LocationCounViewModel>();
                    foreach (int id in Q3_lid)
                    {
                        if (id == 0)
                        {

                            model.Q3Location.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q3Location.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                    }
                    List<int> Q4_lid = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q4Location = new List<LocationCounViewModel>();
                    foreach (int id in Q4_lid)
                    {
                        if (id == 0)
                        {

                            model.Q4Location.Add(new LocationCounViewModel
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q4Location.Add(new LocationCounViewModel
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }


                    }
                }

            }
            return View(model);
        }
        public JsonResult InviteApplicant(int ClientShift_ID, int Applicant_ID, string Remarks, List<medprohiremvp.Models.ClinicalInstitution.InviedDaysViewModel> invitedday)
        {
            if (ClientShift_ID != 0 && Applicant_ID != 0)
            {
                ClientShifts shift = _commonService.GetClientShiftByID(ClientShift_ID);
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                ApplicationUser appuser = _userManager.Users.Where(x => x.Id == applicant.User_ID).First();
                if (shift != null && applicant != null)
                {
                    ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                    ApplicantAppliedShifts appliedShifts = new ApplicantAppliedShifts();

                    appliedShifts.Applicant_ID = Applicant_ID;
                    appliedShifts.AppliedDaysList = String.Join(',', invitedday.Where(x => x.Invited).Select(x => x.Day));
                    appliedShifts.ClientShift_ID = ClientShift_ID;
                    appliedShifts.Status = 1;
                    appliedShifts.Remarks = Remarks;
                    appliedShifts.Invited = "Invited by Client";
                    appliedShifts.AppliedShiftsDays = new List<ApplicantAppliedShiftsDays>();
                    foreach (var day in invitedday.Where(x => x.Invited))
                    {
                        appliedShifts.AppliedShiftsDays.Add(new ApplicantAppliedShiftsDays()
                        {
                            ClockInTime = day.Day.Date.Add(day.StartTime.TimeOfDay),
                            ClockOutTime = day.Day.Date.Add(day.EndTime.TimeOfDay),
                            Day = day.Day

                        });
                    }
                    List<string> freedays = new List<string>();


                    if (invitedday.Count == shift.Occurrences)
                    {
                        appliedShifts.AppliedAllDays = true;
                    }
                    if (_commonService.AddApplicantAppliedShift(appliedShifts))
                    {
                        Notifications notification = new Notifications()
                        {
                            NotificationTemplate_ID = 6,
                            Status = 1,
                            User_ID = applicant.User_ID,
                            Special_ID = appliedShifts.AppliedShift_ID

                        };

                        _commonService.AddNotification(notification);
                        #region sendingemail

                        Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                        Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                        string filecontent = GetHtmlStringFromPath("InviteToAcceptShift");
                        string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                        string Body = filecontent.Replace(Subject, "");
                        string url = "";
                        var branch = _commonService.GetLocations(clinical.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                        emailkeys.Add("{Name}", applicant.FirstName + " " + applicant.LastName);
                        NotificationTemplates template = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == 6).FirstOrDefault();
                        if (template != null)
                            url = Url.Action(template.NotificationAction, template.NotificationController, new { nid = notification.Notification_ID }, protocol: _rootPath.Type, host: _rootPath.ReleasedLink);
                        emailkeys.Add("{ReturnUrl}", url);
                        emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);
                        emailkeys.Add("{Location}", branch == null ? clinical.InstitutionName : branch.BranchName);
                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                        foreach (var key in emailkeys)
                        {
                            Body = Body.Replace(key.Key, key.Value);
                            Subject = Subject.Replace(key.Key, key.Value);
                        }

                        var answer = _emailService.SendEmailAsync(appuser.Email, Subject, Body, true).Result;
                        #endregion sendingemail
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }
        public async Task FileView(string FileType, int Id, int Applicant_ID)
        {
            Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
            if (app != null)
            {

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
        public IActionResult _inviteshiftpartial(int Applicant_ID)

        {
            InviteShiftViewModel model = new InviteShiftViewModel();
            var user_id = _userManager.GetUserId(HttpContext.User);
            if (user_id != null)
            {
                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(Guid.Parse(user_id));
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (applicant != null && clinical != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                    List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();

                    model.Applicant = new ApplicantModel()
                    {

                        Applicant_ID = applicant.Applicant_ID,
                        Imgsrc = applicant.ProfileImage == null || applicant.ProfileImage == "" ? "/Upload/User.png" : "/" + applicant.ProfileImage.Replace('\\', '/'),
                        FirstName = applicant.FirstName,
                        LastName = applicant.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Availability_ID = applicant.Availability_ID,
                        VisaStatus_ID = applicant.VisaStatus_ID,
                        VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).Select(x => x.VisaStatus).FirstOrDefault(),

                        CertificatiesString = String.Join(", ", certificates),
                        SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Where(x => x.Status == 1).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                    };
                    List<ApplicantAppliedShifts> appliedShifts = _commonService.GetApplicantAppliedShifts(applicant.User_ID);
                    List<int> AppliedClientShift_IDs = appliedShifts.Select(x => x.ClientShift_ID).ToList();
                    List<string> applieddays = new List<string>();
                    foreach (ApplicantAppliedShifts appliedShift in appliedShifts)
                    {
                        if (appliedShift.Status == 1 && appliedShift.Accepted == true)
                        {
                            applieddays.AddRange(appliedShift.AppliedDaysList.Split(","));
                        }
                    }
                    var shifts = _commonService.GetAvailableShiftsforApplicant(applicant.Applicant_ID, clinical.Institution_ID);
                    if (shifts != null)
                    {
                        List<ClientShifts> clientShifts = shifts;

                        List<ApplicantAvailables> applicantAvailables = _commonService.GetApplicantAvailables(applicant.Applicant_ID);
                        List<ClientShifts> RemovableShifts = new List<ClientShifts>();

                        model.ClientShift = new List<ClientShiftViewModel>();
                        foreach (ClientShifts shift in clientShifts)
                        {
                            if (shift != null)
                            {


                                var freedays = applicantAvailables;


                                List<string> appliedshiftdays = new List<string>();
                                List<ApplicantAppliedShifts> appliedDays = _commonService.GetAppliedShiftsbyClientShift_ID(shift.ClientShift_ID).Where(x => x.Status == 1 && x.Accepted && x.Applicant_ID != applicant.Applicant_ID).ToList();
                                if (appliedDays != null)
                                {

                                    foreach (ApplicantAppliedShifts appliedshift in appliedDays)
                                    {
                                        appliedshiftdays.AddRange(appliedshift.AppliedDaysList.Split(','));
                                    }
                                    foreach (string day in appliedshiftdays)
                                    {
                                        if (appliedshiftdays.FindAll(x => x == day).Count() == shift.ContractorCount)
                                            freedays.Remove(freedays.Where(x => x.AvailableDay.ToString("MM/dd/yyyy") == day).FirstOrDefault());
                                    }
                                }
                                var removabledays = new List<ApplicantAvailables>();
                                var shiftdays = shift.ShiftsDates.Split(",").ToList();
                                foreach (var day in freedays)
                                {

                                    if (applieddays.Contains(day.AvailableDay.ToString("MM/dd/yyyy")))
                                        removabledays.Add(day);
                                    if (!shiftdays.Contains(day.AvailableDay.ToString("MM/dd/yyyy")))
                                        removabledays.Add(day);
                                }


                                if (removabledays.Count > 0)
                                {
                                    foreach (var removableday in removabledays)
                                    {
                                        freedays.Remove(removableday);
                                    }
                                }

                                List<string> Specialites = _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList();
                                if (freedays.Count > 0)
                                {
                                    model.ClientShift.Add(new ClientShiftViewModel()
                                    {

                                        ClientShift_ID = shift.ClientShift_ID,
                                        ClockInTime = shift.ClockInTime,
                                        ClockOutTime = shift.ClockOutTime,
                                        ContractorCount = shift.ContractorCount,
                                        StartDate = shift.StartDate,
                                        EndDate = shift.EndDate,
                                        Responsibility = shift.Responsibility,
                                        ShiftDescription = shift.ShiftDescription,
                                        DateOfShift = shift.DateOfShift,
                                        Institution_ID = shift.Institution_ID,
                                        Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID).FirstOrDefault(),
                                        SpecialitiesName = String.Join("; ", Specialites),
                                        Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                        BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : clinical.InstitutionName,
                                        Category_ID = shift.Category_ID,
                                        ShiftLabel_ID = shift.ShiftLabel_ID,
                                        Applied = false,
                                        Freedays = String.Join(",", freedays.Select(x => x.AvailableDay.ToString("MM/dd/yyyy"))),
                                        Occurrences = shift.Occurrences,
                                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(x => x.ShiftLabelName).FirstOrDefault(),
                                        ShiftsDates = shift.ShiftsDates,
                                        applicantAvailables = freedays.OrderBy(x => x.AvailableDay).ToList()


                                    });
                                }
                            }


                        }

                    }
                }
            }
            return PartialView(model);
        }
        public IActionResult inviteshift(string Remarks, List<int> invitedday)

        {

            return View();
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
        #region Ajax
        public IActionResult _dashboardshiftView(string Querter)
        {
            ShiftsCountViewModel model = new ShiftsCountViewModel();
            return PartialView("_dashboardshiftView", model);
        }
        public IActionResult _Client_specialties(int model, List<int> specIds)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(Guid.Parse(userId));
                    if (clinical != null)
                    {

                        var clinical_specId = clinical.Specialties.Split(";");
                        var Specialities = new SelectList(_commonService.GetSpecialities().Where(x => !clinical_specId.Contains(x.Speciality_ID.ToString()) && !specIds.Contains(x.Speciality_ID)).ToList(), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = Specialities;


                        return PartialView("_Client_specialties", model);
                    }
                }
                catch
                {
                    return PartialView();
                }
            }

            return PartialView();
        }
        public IActionResult _client_profile_speciaties(int model, List<int> specIds)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(Guid.Parse(userId));
                    if (clinical != null)
                    {


                        var clinical_specIds = _commonService.GetClientSpecialties(clinical.Institution_ID).Select(x => x.Speciality_ID).ToList();
                        var Specialities = new SelectList(_commonService.GetSpecialities().Where(x => !clinical_specIds.Contains(x.Speciality_ID) && !specIds.Contains(x.Speciality_ID)).ToList(), "Speciality_ID", "SpecialityName");
                        ViewBag.Speciality = Specialities;

                        return PartialView("_client_profile_speciaties", model);
                    }
                }
                catch
                {
                    return PartialView();
                }
            }

            return PartialView();
        }
        public JsonResult RemoveLocaton(int Branch_ID)
        {
            try
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ClinicalInstitutions clinicalInstitution = _commonService.FindClinicaByUserID(userId);
                ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(Branch_ID);
                if (branch != null && branch.Institution_ID == clinicalInstitution.Institution_ID)
                {
                    var answer = _commonService.RemoveLocation(Branch_ID);
                    if (answer)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(null);
                }
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult GetApplicantCities(int stateid)
        {
            var Cities = _commonService.GetApplicantCities();
            return Json(Cities.Where(x => x.state_id == stateid).ToList());
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
        #endregion
    }
}


