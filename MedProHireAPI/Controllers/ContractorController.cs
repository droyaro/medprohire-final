using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Geolocation;
using MedProHireAPI.Models.Applicant;
using MedProHireAPI.Models.ClinicalInstitution;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Applicant/[Action]")]
    public class ContractorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly RootPath _rootPath;
        private readonly IHostingEnvironment _environment;
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public ContractorController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices, IOptions<RootPath> rootPath, IHostingEnvironment environment
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _rootPath = rootPath.Value;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult ApplicantDashboard([FromHeader]string ApiKey, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                if (app != null)
                {
                    ApplicantDashboardModel model = new ApplicantDashboardModel();
                    model.Shifts = new List<ApplicantAppliedShiftModel>();
                    model.CurrentShifts = new List<ApplicantAppliedShiftModel>();
                    model.PickedShifts = new List<ApplicantAppliedShiftModel>();
                    model.RejectedShifts = new List<ApplicantAppliedShiftModel>();
                    model.PayChecks = new List<PayCheckModel>();


                    List<ApplicantAppliedShifts> AppliedShiftModel = _commonService.GetApplicantAppliedShifts(app.User_ID).ToList();
                    if (AppliedShiftModel != null)
                    {
                        string LocationName = "";
                        string Location = "";
                        foreach (ApplicantAppliedShifts appliedshift in AppliedShiftModel)
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
                                model.PickedShifts.Add(new ApplicantAppliedShiftModel()
                                {
                                    InstitutionName = institution.InstitutionName,
                                    ClockInTime = shift.ClockInTime,
                                    ClockOutTime = shift.ClockOutTime,
                                    HourlyRate = shift.HourlyRate,
                                    StartDate = shift.StartDate,
                                    EndDate = shift.EndDate,
                                    Institution_ID = shift.Institution_ID,
                                    Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                    BranchName = LocationName,
                                    LocationAddress = Location,
                                    Occurrences = shift.Occurrences,
                                    ClientShift_ID = shift.ClientShift_ID,
                                    AppliedDaysList = appliedshift.AppliedDaysList,
                                    PhoneNumber = clinical.PhoneNumber,
                                    ShiftsDates = shift.ShiftsDates


                                });
                            }
                            if (appliedshift.Accepted == true && appliedshift.Status == 1)
                            {
                                model.CurrentShifts.Add(new ApplicantAppliedShiftModel()
                                {
                                    InstitutionName = institution.InstitutionName,
                                    ClockInTime = shift.ClockInTime,
                                    ClockOutTime = shift.ClockOutTime,
                                    HourlyRate = shift.HourlyRate,
                                    StartDate = shift.StartDate,
                                    EndDate = shift.EndDate,
                                    Institution_ID = shift.Institution_ID,
                                    Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                    BranchName = LocationName,
                                    LocationAddress = Location,
                                    Occurrences = shift.Occurrences,
                                    ClientShift_ID = shift.ClientShift_ID,
                                    AppliedDaysList = appliedshift.AppliedDaysList,
                                    PhoneNumber = clinical.PhoneNumber,
                                    ShiftsDates = shift.ShiftsDates


                                });
                                List<ApplicantClockInClockOutTime> clockintimes = _commonService.GetAppliedShiftClockinClockouttimes(appliedshift.AppliedShift_ID);
                                if (clockintimes != null)
                                {
                                    model.CurrentShifts.Last().NumberofShift = clockintimes.Count();
                                    model.CurrentShifts.Last().CompletedNumberofShift = clockintimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count();
                                    if (model.CurrentShifts.Last().CompletedNumberofShift != 0)
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
                                        model.CurrentShifts.Last().Numberofworkedhours = time.TotalHours;
                                    }
                                }
                                else
                                {
                                    model.CurrentShifts.Last().NumberofShift = 0;
                                    model.CurrentShifts.Last().CompletedNumberofShift = 0;
                                    model.CurrentShifts.Last().Numberofworkedhours = 0;
                                }
                            }
                            if (appliedshift.Accepted == true && appliedshift.Status == 2)
                            {
                                model.Shifts.Add(new ApplicantAppliedShiftModel()
                                {
                                    InstitutionName = institution.InstitutionName,
                                    ClockInTime = shift.ClockInTime,
                                    ClockOutTime = shift.ClockOutTime,
                                    HourlyRate = shift.HourlyRate,
                                    StartDate = shift.StartDate,
                                    EndDate = shift.EndDate,
                                    Institution_ID = shift.Institution_ID,
                                    Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                    BranchName = LocationName,
                                    LocationAddress = Location,
                                    Occurrences = shift.Occurrences,
                                    ClientShift_ID = shift.ClientShift_ID,
                                    AppliedDaysList = appliedshift.AppliedDaysList,
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
                                model.RejectedShifts.Add(new ApplicantAppliedShiftModel()
                                {
                                    InstitutionName = institution.InstitutionName,
                                    ClockInTime = shift.ClockInTime,
                                    ClockOutTime = shift.ClockOutTime,
                                    HourlyRate = shift.HourlyRate,
                                    StartDate = shift.StartDate,
                                    EndDate = shift.EndDate,
                                    Institution_ID = shift.Institution_ID,
                                    Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                    BranchName = LocationName,
                                    LocationAddress = Location,
                                    Occurrences = shift.Occurrences,
                                    ClientShift_ID = shift.ClientShift_ID,
                                    AppliedDaysList = appliedshift.AppliedDaysList,
                                    PhoneNumber = clinical.PhoneNumber,
                                    ShiftsDates = shift.ShiftsDates


                                });
                            }
                        }
                        if (model.CurrentShifts.Count > 0)
                        {
                            model.Shifts.AddRange(model.CurrentShifts);
                        }

                    }
                    List<PayChecks> payChecks = _commonService.GetApplicantPayChecks(app.Applicant_ID).ToList();
                    if (payChecks != null)
                    {

                        foreach (PayChecks checks in payChecks)
                        {
                            model.PayChecks.Add(new PayCheckModel
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
                    return Ok(model);
                }

                else
                {
                    ModelState.AddModelError("", "Applicant ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public async Task<IActionResult> ApplicantProfile([FromHeader]string ApiKey, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                if (app != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();

                    if ((user.ChangesMakedTime - DateTime.Now).Days != 0 || ((user.ChangesMakedTime - DateTime.Now).Days == 0 && (user.ChangesMakedTime - DateTime.Now).Hours != 0))
                    {
                        user.ChangesCount = 0;
                        user.ChangesLocked = false;
                        await _userManager.UpdateAsync(user);
                    }

                    ApplicantProfileModel model = new ApplicantProfileModel();
                    model.Password = new Models.Account.ChangePasswordModel();
                    model.Imgsrc = "/" + app.ProfileImage.Replace('\\', '/');
                    model.PhoneNumber = user.PhoneNumber;
                    model.Applicant_ID = app.Applicant_ID;
                    model.User_ID = app.User_ID;
                    model.Profile = new ApplicantProfileDetailModel()
                    {

                        Availability_ID = app.Availability_ID,
                        FirstName = app.FirstName,
                        IsEligible = app.IsEligible,
                        LastName = app.LastName,
                        MiddleName = app.MiddleName,
                        Latitude = user.Latitude,
                        Longitude = user.Longitude,
                        Email = user.Email,
                        VisaStatus_ID = app.VisaStatus_ID,
                        Disabled = user.ChangesLocked,
                        Address = user.Address,
                        City_ID = user.City_ID,
                        ZipCode = user.ZipCode


                    };
                    var city = _commonService.GetCitiesByCityid(user.City_ID);
                    model.Profile.State_ID = city != null ? city.state_id : 0;
                    var Specialities = _commonService.GetSpecialities();
                    var workhistories = _commonService.GetApplicantWorkHistory(app.Applicant_ID);
                    if (workhistories != null)
                    {
                        model.workHistories = new List<ApplicantWorkHistoryModel>();
                        foreach (ApplicantWorkHistories workHistory in workhistories)
                        {
                            model.workHistories.Add(new ApplicantWorkHistoryModel
                            {
                                EndDate = workHistory.EndDate,
                                StartDate = workHistory.StartDate,
                                PlaceName = workHistory.PlaceName,
                                UntilNow = workHistory.EndDate == DateTime.MinValue ? true : false,
                                 JobTitle= workHistory.JobTitle,
                                WorkHistory_ID = workHistory.WorkHistory_ID,
                                SpecialityName = workHistory.JobTitle
                            });
                        }
                    }
                    var AppSpecialities = _commonService.GetApplicantSpecialities(app.Applicant_ID);

                    if (AppSpecialities != null)
                    {
                        model.specialities = new List<ApplicantSpecialtyModel>();
                        foreach (ApplicantSpecialities speciality in AppSpecialities)
                        {
                            model.specialities.Add(new ApplicantSpecialtyModel
                            {
                                Applicant_ID = speciality.Applicant_ID,
                                LegabilityStates = speciality.LegabilityStates.Split("; ").ToList(),
                                License = speciality.License,
                                Speciality_ID = speciality.Speciality_ID,

                                SpecialityName = Specialities.Where(x => x.Speciality_ID == speciality.Speciality_ID).FirstOrDefault().SpecialityName

                            });
                        }
                    }
                    var certificates = _commonService.GetApplicantCertificates(app.Applicant_ID);
                    if (certificates != null)
                    {
                        model.certificates = new List<ApplicantCertificateModel>();

                        foreach (ApplicantCertificates certificate in certificates)
                        {
                            model.certificates.Add(new ApplicantCertificateModel()
                            {
                                CeritifcationImgsrc = certificate.CeritificationImg,
                                CertificateType = certificate.CertificateTypes,
                                Certification_ID = certificate.Ceritification_ID,
                                Applicant_ID = certificate.Applicant_ID

                            });
                        }

                    }
                    var availableetypes = _commonService.GetAvailableTypes();
                    model.ApplicantAvailables = new List<ApplicantAvailableDatesModel>();

                    var useravailbles = _commonService.GetApplicantAvailables(app.Applicant_ID);
                    foreach (ApplicantAvailableTypes applicantAvailables in availableetypes)
                    {
                        model.ApplicantAvailables.Add(new ApplicantAvailableDatesModel()
                        {
                            Applicant_ID = app.Applicant_ID,
                            ApplicantAvailableType_ID = applicantAvailables.ApplicantAvailableType_ID,
                            ApplicantAvailableTypeName = applicantAvailables.ApplicantAvailableType_Name,
                            ApplicantAvailableTypeValue = useravailbles!=null ? true : false,
                            ApplicantAvailableDays = useravailbles != null ? String.Join(",",useravailbles.Select(x=>x.AvailableDay.ToShortDateString())) : ""
                        });
                    }
                    model.IsAvailable = app.IsAvailable;

                    return Ok(model);

                }
                else
                {
                    ModelState.AddModelError("", "Applicant ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> SaveApplicantProfileImage([FromHeader]string ApiKey, [FromBody]IFormFile profileImage, [FromBody] int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);

                {

                    if (app != null)
                    {

                        ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                        if (user != null)
                        {
                            if (profileImage != null)
                            {
                                string path = "ApplicantImg";
                                string profilefilename = SaveProfileFile(profileImage, path, user.Id.ToString());
                                if (profilefilename != "")
                                {
                                    app.ProfileImage = profilefilename;
                                }
                                else
                                {
                                    ModelState.AddModelError(String.Empty, "Can't upload file");
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
                                return Ok();
                            }
                            else
                            {

                                ModelState.AddModelError(String.Empty, "Can't find Profile file");

                            }
                        }
                        else
                        {

                            ModelState.AddModelError(String.Empty, "User is null");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Can't find  user");
                    }

                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> SaveApplicantProfileChanges([FromHeader]string ApiKey, [FromBody]ApplicantProfileDetailModel model, [FromBody] int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {


                if (ModelState.IsValid)
                {
                    Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                    if (app != null)
                    {

                        ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                        if (user != null)
                        {

                            if (user.City_ID != model.City_ID || user.Address != model.Address)
                            {
                                if (model.Latitude != user.Latitude || model.Longitude != user.Longitude)
                                {
                                    if (!String.IsNullOrEmpty(model.Address))
                                    {
                                        string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
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
                                    user.Longitude = model.Longitude;
                                    user.Latitude = model.Latitude;
                                    HttpContext.Session.Remove("counter");
                                }
                            }

                            app.Availability_ID = model.Availability_ID;
                            app.IsEligible = model.IsEligible;
                            app.VisaStatus_ID = model.VisaStatus_ID;
                            user.Address = model.Address;
                            user.City_ID = model.City_ID;
                            user.ZipCode = model.ZipCode;
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
                            return Ok();


                        }
                        else
                        {
                            ModelState.AddModelError("", "User is null");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Applicant ID is not valid");
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> AddSpecialty([FromHeader]string ApiKey, [FromBody] int Applicant_ID, List<ApplicantSpecialtyModel> specialties)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                if (app != null)
                {

                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        if (specialties != null)
                        {

                            if (ModelState.IsValid)
                            {
                                List<ApplicantSpecialities> applicantSpecialities = new List<ApplicantSpecialities>();


                                foreach (ApplicantSpecialtyModel speciality in specialties)
                                {
                                    applicantSpecialities.Add(new ApplicantSpecialities()
                                    {
                                        Applicant_ID = app.Applicant_ID,
                                        LegabilityStates = String.Join("; ", speciality.LegabilityStates),
                                        License = speciality.License,
                                        Speciality_ID = speciality.Speciality_ID,

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
                        else
                        {
                            ModelState.AddModelError(String.Empty, "Specialties is null");
                        }
                    }
                    else
                    {

                        ModelState.AddModelError(String.Empty, "User is null");
                    }
                }
                else
                {

                    ModelState.AddModelError(String.Empty, "Can't find  Applicant");
                }
            }

            else
            {
                ModelState.AddModelError(String.Empty, "Api Key is not valid");
            }


            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> AddWorkHistory([FromHeader]string ApiKey, [FromBody] int Applicant_ID, [FromBody] List<ApplicantWorkHistoryModel> workHistories)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                if (app != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();

                    if (user != null)
                    {
                        if (workHistories != null)
                        {


                            if (ModelState.IsValid)
                            {
                                List<ApplicantWorkHistories> applicantWorkHistories = new List<ApplicantWorkHistories>();


                                foreach (ApplicantWorkHistoryModel workhistory in workHistories)
                                {
                                    if (workhistory.StartDate != null && workhistory.PlaceName != null && workhistory.Speciality_ID != null && (workhistory.UntilNow ? workhistory.UntilNow : (workhistory.EndDate != null)))
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
                                if (applicantWorkHistories.Count > 0)
                                {
                                    _commonService.AddApplicantWorkHistory(applicantWorkHistories);
                                    await _userManager.UpdateAsync(user);
                                    return Ok();
                                }
                                else
                                {
                                    ModelState.AddModelError(String.Empty, "WorkHistory is empty");
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, "WorkHistory is null");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "User is null");
                    }


                }

                else
                {

                    ModelState.AddModelError(String.Empty, "Can't find  Applicant");
                }

            }
            else
            {
                ModelState.AddModelError(String.Empty, "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> AddCertificates([FromHeader]string ApiKey, [FromBody] int Applicant_ID, [FromBody] List<ApplicantCertificateModel> certificates)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                if (app != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                    {
                        if (user != null)
                        {
                            if (certificates != null)
                            {


                                if (ModelState.IsValid)
                                {
                                    List<ApplicantCertificates> applicantCertificates = new List<ApplicantCertificates>();

                                    string subpath = "Certificates" + $@"\{user.Id}";
                                    foreach (ApplicantCertificateModel certificate in certificates)
                                    {

                                        if (certificate.CeritifcationImg != null && certificate.CertificateType != null)
                                        {
                                            string filename = SaveFile(certificate.CeritifcationImg, subpath, certificate.CertificateType.ToString() + "_" + certificates.IndexOf(certificate).ToString());
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
                                    return Ok();
                                }
                            }

                            else
                            {

                                ModelState.AddModelError(String.Empty, "Certifcate is null");
                            }
                        }
                        else
                        {

                            ModelState.AddModelError(String.Empty, "User is null");
                        }



                    }
                }
                else
                {

                    ModelState.AddModelError(String.Empty, "Can't find  Applicant");
                }

            }
            else
            {
                ModelState.AddModelError(String.Empty, "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult AvailableShifts([FromHeader] string ApiKey, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (applicant != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                    List<ClientShifts> clientShifts = _commonService.ApplicantSearchShift(null, null, applicant.User_ID);
                    List<AppliedShiftModel> AvailableShifts = new List<AppliedShiftModel>();
                    List<AppliedShiftModel> PickedShifts = new List<AppliedShiftModel>();
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
                            AvailableShifts.Add(new AppliedShiftModel()
                            {
                                Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                                ClientShift_ID = clientShift.ClientShift_ID,
                                ClockInTime = clientShift.ClockInTime,
                                ClockOutTime = clientShift.ClockOutTime,
                                BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),

                                EndDate = clientShift.EndDate,

                                HourlyRate = clientShift.HourlyRate,
                                Institution_ID = clientShift.Institution_ID,
                                Occurrences = clientShift.Occurrences,

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
                            PickedShifts.Add(new AppliedShiftModel()
                            {
                                ClockInTime = shift.ClockInTime,
                                ClockOutTime = shift.ClockOutTime,
                                HourlyRate = shift.HourlyRate,
                                ContractorCount = shift.ContractorCount,
                                StartDate = shift.StartDate,
                                EndDate = shift.EndDate,

                                Institution_ID = shift.Institution_ID,
                                Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                            Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                            .Select(x => x.SpecialityName).ToList()),
                                Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                BranchName = LocationName,
                                Location = Location,

                                Occurrences = shift.Occurrences,

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


                    return Ok(new { PickedShifts, AvailableShifts });
                }
                else
                {
                    ModelState.AddModelError("", "Can't find Applicant");

                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");

            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult AvailableShifts([FromHeader] string ApiKey, int Applicant_ID, [FromBody]AvailableShiftSearchModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                    if (applicant != null)
                    {
                        List<int> cities_list = model.Cities;
                        var ShiftCitiesList = _commonService.GetShiftCities();
                        if (cities_list == null)
                        {
                            if (model.States != null)
                            {
                                cities_list = new List<int>();
                                foreach (int state_id in model.States)
                                {
                                    cities_list.AddRange(_commonService.GetCities(state_id).Select(x => x.id).ToList());
                                }
                            }
                        }
                        List<ClientShifts> clientShifts = new List<ClientShifts>();
                        List<AppliedShiftModel> AvailableShifts = new List<AppliedShiftModel>();
                        List<AppliedShiftModel> PickedShifts = new List<AppliedShiftModel>();
                        ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                        if (model.Distance > 0)
                        {
                            clientShifts = _commonService.ApplicantSearchShift(cities_list, model.Specialities, applicant.User_ID, model.Distance, user.Longitude, user.Latitude);
                        }
                        else
                        {
                            clientShifts = _commonService.ApplicantSearchShift(cities_list, model.Specialities, applicant.User_ID);
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

                                }
                                else
                                {

                                    Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;

                                }
                                int dist = Convert.ToInt32(GeoCalculator.GetDistance(user.Latitude, user.Longitude, clinical.Latitude, clinical.Longitude, 2, distanceUnit: DistanceUnit.Miles));
                                AvailableShifts.Add(new AppliedShiftModel()
                                {
                                    Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                                    ClientShift_ID = clientShift.ClientShift_ID,
                                    ClockInTime = clientShift.ClockInTime,
                                    ClockOutTime = clientShift.ClockOutTime,
                                    BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),

                                    EndDate = clientShift.EndDate,

                                    HourlyRate = clientShift.HourlyRate,
                                    Institution_ID = clientShift.Institution_ID,
                                    Occurrences = clientShift.Occurrences,

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
                                PickedShifts.Add(new AppliedShiftModel()
                                {
                                    ClockInTime = shift.ClockInTime,
                                    ClockOutTime = shift.ClockOutTime,
                                    HourlyRate = shift.HourlyRate,
                                    ContractorCount = shift.ContractorCount,
                                    StartDate = shift.StartDate,
                                    EndDate = shift.EndDate,

                                    Institution_ID = shift.Institution_ID,
                                    Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID),
                                    SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                                    Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                    BranchName = LocationName,
                                    Location = Location,

                                    Occurrences = shift.Occurrences,

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


                        return Ok(new { model, PickedShifts, AvailableShifts });

                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't find Applicant");

                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");

            }
            return BadRequest(ModelState);

        }
        [HttpGet]
        public IActionResult GetShiftDetail([FromHeader] string ApiKey, int ClientShift_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClientShifts clientShift = _commonService.GetClientShiftByID(ClientShift_ID);
                if (clientShift != null)

                {
                    string LocationName = "";
                    string InstitutionName = "";
                    string Location = "";
                    string ContactPerson = "";
                    string PhoneNumber = "";

                    ClinicalInstitutions institution = _commonService.GetClinicalInstitution_byID(clientShift.Institution_ID);
                    ApplicationUser clinical = _userManager.Users.Where(x => x.Id == institution.User_ID).FirstOrDefault();
                    InstitutionName = institution.InstitutionName;
                    if (clientShift.Branch_ID.GetValueOrDefault() != 0)
                    {
                        ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(clientShift.Branch_ID.GetValueOrDefault());
                        LocationName = branch.BranchName;
                        Location = _commonService.GetCityName(branch.CityId) + " Address:" + branch.Address;
                        ContactPerson = branch.ContactName;
                        PhoneNumber = branch.PhoneNumber;

                    }
                    else
                    {
                        Location = _commonService.GetCityName(clinical.City_ID) + ", " + clinical.ZipCode + ", " + clinical.Address;
                        ContactPerson = institution.ContactPerson;
                        PhoneNumber = clinical.PhoneNumber;

                    }

                    var model = new AppliedShiftModel()
                    {
                        Branch_ID = clientShift.Branch_ID.GetValueOrDefault(),
                        ClientShift_ID = clientShift.ClientShift_ID,
                        ClockInTime = clientShift.ClockInTime,
                        ClockOutTime = clientShift.ClockOutTime,
                        BranchName = InstitutionName + (LocationName == "" ? "" : " (" + LocationName + ")"),
                        EndDate = clientShift.EndDate,
                        HourlyRate = clientShift.HourlyRate,
                        Institution_ID = clientShift.Institution_ID,
                        Occurrences = clientShift.Occurrences,
                        StartDate = clientShift.StartDate,
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientShift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                        Applied = false,
                        Location = Location,
                        AppliedAllDays = false,
                        AppliedDaysList = "",
                        InstitutionName = institution.InstitutionName,
                        ShiftsDates = clientShift.ShiftsDates,
                        PhoneNumber = PhoneNumber,
                        ContactPerson = ContactPerson,
                        ContractorCount = clientShift.ContractorCount,




                    };
                    return Ok(model);

                }
                else
                {
                    ModelState.AddModelError("", "Client Shift ID is not valid");

                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");

            }
            return BadRequest(ModelState);

        }
        [HttpPost]
        public IActionResult PickShift([FromHeader] string ApiKey, [FromBody] PickShiftModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if(ModelState.IsValid)
                {
                    Applicants applicant = _commonService.GetApplicantbyId(model.Applicant_ID);
                    ClientShifts shift = _commonService.GetClientShiftByID(model.ClientShift_ID);
                    if(applicant!=null && shift!=null)
                    {
                        if(model.AppliedDates!=null)
                        {
                            if(model.AppliedDates.Count>0)
                            {
                                ApplicantAppliedShifts appliedShifts = new ApplicantAppliedShifts()
                                {
                                    Applicant_ID = applicant.Applicant_ID,
                                    ClientShift_ID = shift.ClientShift_ID,
                                    AppliedDaysList = String.Join(',', model.AppliedDates.Select(x => x.ToString("MM/dd/yyyy").ToList())),
                                    AppliedAllDays = model.AppliedDates.Count == shift.Occurrences ? true : false,
                                    Status = 0,
                                    Accepted = false,
                                    Invited = "",

                                };
                                var answer = _commonService.AddApplicantAppliedShift(appliedShifts);
                                if(answer)
                                {
                                    return Ok();
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Can't save Picked shift");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "AppliedDates is empty");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "AppliedDates is not valid");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Client Shift ID or Applicant ID is not valid");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");

            }
            return BadRequest(ModelState);

        }
        [HttpGet]
        public IActionResult ConfirmAcceptedShift([FromHeader] string ApiKey, [FromBody] AppliedShiftModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");

            }
            return BadRequest(ModelState);
        }
        private string SaveFile(IFormFile file, string path, string filename)
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
        private string SaveProfileFile(IFormFile file, string path, string filename)
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
    }
}