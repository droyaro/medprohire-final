using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMSmedprohiremvp.Models.Login;
using CMSmedprohiremvp.Models.Clinical;
using CMSmedprohiremvp.Models;
using CMSmedprohiremvp.Models.Applicant;
using Microsoft.AspNetCore.Authorization;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.Service.IServices;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using medprohiremvp.Service.SignSend;
using medprohiremvp.Service.EmailServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace CMSmedprohiremvp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly ISignature _signature;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly RootPath _rootPath;
        private readonly string savefiletypeprefix = "adminsign";
        private string user_ID;
        // role names
        private string role = "admin";



        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             ICommonServices commonServices, ISignature signature, IHostingEnvironment environment, IEmailService emailService, IOptions<RootPath> rootPath)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _signature = signature;
            _environment = environment;
            _emailService = emailService;
            _rootPath = rootPath.Value;
        }
        public IActionResult Index()
        {
            _commonService.UpdateAllExpiredShifts();
            return RedirectToAction("UsersList");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public IActionResult OperatingApplicants(OperatingApplicantsViewModel model)
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));

            if (ModelState.IsValid)
            {
                ՕperatingApplicants operateApplicant = new ՕperatingApplicants()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City,
                    State = model.State,
                    DataChoose = DateTime.Now,
                    Applicant_ID = model.Applicant_ID,
                    ClientShift_ID = model.ClientShift_Id

                };
                _commonService.SaveOperatingApplicants(operateApplicant);
                ApplicantAppliedShifts appApliedShift = _commonService.GetAppApliedShift(model.Applicant_ID,model.ClientShift_Id);
                Applicants app = _commonService.GetApplicantbyId(model.Applicant_ID);
                appApliedShift.Category_ID = 2;
                _commonService.UpdateApplicantAppliedShift(appApliedShift);
                Notifications notifications = new Notifications()
                {
                    NotificationTemplate_ID = 8,
                    User_ID = app.User_ID,
                };
                _commonService.AddNotification(notifications);
            }

            return RedirectToAction("ShiftsList", "Home");
        }

        [AllowAnonymous]
        public IActionResult UsersList()
        {

            List<ApplicantViewModel> model = new List<ApplicantViewModel>();

            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
            if (appusers.Count > 0)
            {

                foreach (ApplicationUser appuser in appusers)
                {
                    Applicants app = _commonService.FindApplicantByUserID(appuser.Id);
                    if (app != null && String.IsNullOrEmpty(app.Employment_agreement))
                    {
                        model.Add(new ApplicantViewModel
                        {
                            Applicant_ID = app.Applicant_ID,
                            Address = appuser.Address + ", " + _commonService.GetCityName(appuser.City_ID) + ", " + appuser.ZipCode,
                            Availability_ID = app.Availability_ID,
                            Availability = app.Availability_ID != 0 ? _commonService.GetAvailabilities().Where(x => x.Availability_ID == app.Availability_ID).Select(x => x.Availability).First() : null,
                            BackgroundCheck = app.BackgroundCheck,
                            BoardingProcess = app.BoardingProcess,

                            LastName = app.LastName,
                            FirstName = app.FirstName,
                            CEU = app.CEU,
                            City_ID = appuser.City_ID,
                            CityName = _commonService.GetCitiesByCityid(appuser.City_ID).city_name,

                            Contract = app.Contract,
                            Drugscreen = app.Drugscreen,
                            DrugscreenStatus = app.DrugscreenStatus_ID != null ? _commonService.GetDrugscreenStatuses().Where(x => x.DrugscreenStatus_ID == app.DrugscreenStatus_ID).Select(x => x.DrugscreenStatus).FirstOrDefault() : null,
                            Email = appuser.Email,
                            Employment_agreement = app.Employment_agreement,
                            E_verify = app.E_verify,
                            Imgsrc = app.ProfileImage != null ? app.ProfileImage.Replace('\\', '/') : null,
                            I_9 = app.I_9,
                            PhoneNumber = appuser.PhoneNumber,
                            PreferredID = app.PreferredID != null ? app.PreferredID : "",
                            SSN = app.SSN,
                            Status = ((StatusEnum)app.Status_ID).ToString(),
                            Sub_specialities = app.Sub_specialities,
                            TIN = app.TIN,
                            User_ID = app.User_ID,
                            VisaStatus_ID = app.VisaStatus_ID,
                            Visastatus = app.VisaStatus_ID != 0 ? _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).Select(x => x.VisaStatus).First() : null,
                            WorkAuth = app.WorkAuth,
                            W_4 = app.W_4,
                            ZipCode = appuser.ZipCode,
                            CertificatiesString = String.Join(',', _commonService.GetApplicantCertificates(app.Applicant_ID).Select(x=>x.CertificateTypes).ToList()),

                            SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                            DateCreated = app.DateCreated,
                            DateModified = app.DateModified,
                            AppliedShiftCount = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.Status == 0 && x.Accepted != true).ToList().Count

                        });
                    };


                }
            }
            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = specialities;
            model = model.OrderByDescending(x => x.DateCreated).ToList();
            UsersListModel usersmodel = new UsersListModel();
            usersmodel.ApplicantViewModels = model;
            //usersmodel.employment = new EmploymentAgreement();

            return View(usersmodel);
        }
        public IActionResult ContractorsList()
        {
            ContractorViewModel model = new ContractorViewModel();
             model.applicantViewModels = new List<ApplicantViewModel>();

            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
            if (appusers.Count > 0)
            {

                foreach (ApplicationUser appuser in appusers)
                {
                    Applicants app = _commonService.FindApplicantByUserID(appuser.Id);
                    if (app != null && !String.IsNullOrEmpty(app.Employment_agreement))
                    {
                        model.applicantViewModels.Add(new ApplicantViewModel
                        {
                            Applicant_ID = app.Applicant_ID,
                            Address = appuser.Address + ", " + _commonService.GetCityName(appuser.City_ID) + ", " + appuser.ZipCode,
                            Availability_ID = app.Availability_ID,
                            Availability = app.Availability_ID != 0 ? _commonService.GetAvailabilities().Where(x => x.Availability_ID == app.Availability_ID).Select(x => x.Availability).First() : null,
                            BackgroundCheck = app.BackgroundCheck,
                            BoardingProcess = app.BoardingProcess,

                            LastName = app.LastName,
                            FirstName = app.FirstName,
                            CEU = app.CEU,
                            City_ID = appuser.City_ID,
                            CityName = _commonService.GetCitiesByCityid(appuser.City_ID).city_name,

                            Contract = app.Contract,
                            Drugscreen = app.Drugscreen,
                            DrugscreenStatus = app.DrugscreenStatus_ID != null ? _commonService.GetDrugscreenStatuses().Where(x => x.DrugscreenStatus_ID == app.DrugscreenStatus_ID).Select(x => x.DrugscreenStatus).FirstOrDefault() : null,
                            Email = appuser.Email,
                            Employment_agreement = app.Employment_agreement,
                            E_verify = app.E_verify,
                            Imgsrc = app.ProfileImage != null ? app.ProfileImage.Replace('\\', '/') : null,
                            I_9 = app.I_9,
                            PhoneNumber = appuser.PhoneNumber,
                            PreferredID = app.PreferredID != null ? app.PreferredID : "",
                            SSN = app.SSN,
                            Status = ((StatusEnum)app.Status_ID).ToString(),
                            Sub_specialities = app.Sub_specialities,
                            TIN = app.TIN,
                            User_ID = app.User_ID,
                            VisaStatus_ID = app.VisaStatus_ID,
                            Visastatus = app.VisaStatus_ID != 0 ? _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).Select(x => x.VisaStatus).First() : null,
                            WorkAuth = app.WorkAuth,
                            W_4 = app.W_4,
                            ZipCode = appuser.ZipCode,
                            CertificatiesString = String.Join(',', _commonService.GetApplicantCertificates(app.Applicant_ID).Select(x => x.CertificateTypes).ToList()),

                            SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                            DateCreated = app.DateCreated,
                            DateModified = app.DateModified,
                            AppliedShiftCount = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.Status == 0 && x.Accepted != true).ToList().Count

                        });
                    };


                }
            }
            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = specialities;
            model.applicantViewModels = model.applicantViewModels.OrderByDescending(x => x.DateModified).ToList();


            return View(model);
        }
        public IActionResult OnShiftContractors()
        {

            List<ApplicantViewModel> model = new List<ApplicantViewModel>();

            List<ApplicationUser> appusers = _userManager.GetUsersInRoleAsync("Applicant").Result.ToList();
            if (appusers.Count > 0)
            {

                foreach (ApplicationUser appuser in appusers)
                {
                    Applicants app = _commonService.FindApplicantByUserID(appuser.Id);

                    if (app != null && !String.IsNullOrEmpty(app.Employment_agreement))
                    {
                        List<ApplicantAppliedShifts> applied = _commonService.GetApplicantAppliedShifts(appuser.Id);
                        if (applied != null)
                        {
                            if (applied.Where(x => x.Accepted == true && x.Status == 1).FirstOrDefault() != null)
                            {
                                model.Add(new ApplicantViewModel
                                {
                                    Applicant_ID = app.Applicant_ID,
                                    Address = appuser.Address + ", " + _commonService.GetCityName(appuser.City_ID) + ", " + appuser.ZipCode,
                                    Availability_ID = app.Availability_ID,
                                    Availability = app.Availability_ID != 0 ? _commonService.GetAvailabilities().Where(x => x.Availability_ID == app.Availability_ID).Select(x => x.Availability).First() : null,
                                    BackgroundCheck = app.BackgroundCheck,
                                    BoardingProcess = app.BoardingProcess,

                                    LastName = app.LastName,
                                    FirstName = app.FirstName,
                                    CEU = app.CEU,
                                    City_ID = appuser.City_ID,
                                    CityName = _commonService.GetCitiesByCityid(appuser.City_ID).city_name,

                                    Contract = app.Contract,
                                    Drugscreen = app.Drugscreen,
                                    DrugscreenStatus = app.DrugscreenStatus_ID != null ? _commonService.GetDrugscreenStatuses().Where(x => x.DrugscreenStatus_ID == app.DrugscreenStatus_ID).Select(x => x.DrugscreenStatus).FirstOrDefault() : null,
                                    Email = appuser.Email,
                                    Employment_agreement = app.Employment_agreement,
                                    E_verify = app.E_verify,
                                    Imgsrc = app.ProfileImage != null ? app.ProfileImage.Replace('\\', '/') : null,
                                    I_9 = app.I_9,
                                    PhoneNumber = appuser.PhoneNumber,
                                    PreferredID = app.PreferredID != null ? app.PreferredID : "",
                                    SSN = app.SSN,
                                    Status = ((StatusEnum)app.Status_ID).ToString(),
                                    Sub_specialities = app.Sub_specialities,
                                    TIN = app.TIN,
                                    User_ID = app.User_ID,
                                    VisaStatus_ID = app.VisaStatus_ID,
                                    Visastatus = app.VisaStatus_ID != 0 ? _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).Select(x => x.VisaStatus).First() : null,
                                    WorkAuth = app.WorkAuth,
                                    W_4 = app.W_4,
                                    ZipCode = appuser.ZipCode,
                                    CertificatiesString = String.Join(',', _commonService.GetApplicantCertificates(app.Applicant_ID).Select(x => x.CertificateTypes).ToList()),

                                    SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                                    DateCreated = app.DateCreated,
                                    DateModified = app.DateModified,
                                    AppliedShiftCount = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.Status == 0 && x.Accepted != true).ToList().Count

                                });
                            };
                        };
                    };


                }
            }
            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = specialities;
            model = model.OrderByDescending(x => x.DateModified).ToList();
            UsersListModel usersmodel = new UsersListModel();
            usersmodel.ApplicantViewModels = model;
            //usersmodel.employment = new EmploymentAgreement();

            return View(usersmodel);
        }
        public IActionResult ActiveShiftContractors()
        {
            List<ApplicantAppliedShiftsViewModel> model = new List<ApplicantAppliedShiftsViewModel>();
           

            List<ApplicantAppliedShifts> activeshifts= _commonService.GetAllActiveShifts();
            if (activeshifts.Count > 0)
            {

                foreach (ApplicantAppliedShifts activeShift in activeshifts)
                {
                    Applicants app = _commonService.GetApplicantbyId(activeShift.Applicant_ID);

                    model.Add(new ApplicantAppliedShiftsViewModel()
                    {
                        Accepted = activeShift.Accepted,
                        Applicant_ID = activeShift.Applicant_ID,
                        AppliedShift_ID = activeShift.AppliedShift_ID,
                        ClientShift_ID = activeShift.ClientShift_ID,
                        AppliedAllDays = activeShift.AppliedAllDays,
                        ApplicantName = app.LastName + " " + app.MiddleName + " " + app.FirstName,
                        User_ID=app.User_ID


                    });
                    List<string> applieddays = activeShift.AppliedDaysList.Split(',').ToList();

                    for (int i = 0; i < applieddays.Count; i++)
                    {
                        applieddays[i] = Convert.ToDateTime(applieddays[i].Trim()).ToString("MMMM dd");
                    }
                    model.Last().AppliedDaysList = activeShift.AppliedDaysList;// String.Join("; ", applieddays);
                    ClientShifts shift = _commonService.GetClientShiftByID(activeShift.ClientShift_ID);
                    if (shift != null)
                    {

                        model.Last().ClockInTime = shift.ClockInTime;
                        model.Last().ClockOutTime = shift.ClockOutTime;
                        model.Last().HourlyRate = shift.HourlyRate;
                        model.Last().ContractorCount = shift.ContractorCount;
                        model.Last().StartDate = shift.StartDate;
                        model.Last().EndDate = shift.EndDate;
                        model.Last().Responsibility = shift.Responsibility;
                        model.Last().ShiftDescription = shift.ShiftDescription;
                        model.Last().DateOfShift = shift.DateOfShift;
                        model.Last().ShiftExpirationDate = shift.ShiftExpirationDate;
                        model.Last().Institution_ID = shift.Institution_ID;
                        model.Last().Specialities = _commonService.GetShiftSpecialities(shift.ClientShift_ID);
                        model.Last().SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList());
                        model.Last().Branch_ID = shift.Branch_ID.GetValueOrDefault();
                        model.Last().BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;
                        model.Last().ShiftLabel_ID = shift.ShiftLabel_ID;
                        model.Last().ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault();
                        model.Last().HolidayShift = shift.HolidayShift;
                        model.Last().Occurrences = shift.Occurrences;
                        model.Last().Consecutive_Occurrences = shift.Consecutive_Occurrences;
                        model.Last().InstitutionName = _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName;
                        model.Last().CountofHours = _commonService.CountofWorkHours(activeShift.AppliedShift_ID);
                        model.Last().NumberofCompletedShift = _commonService.CountofCompletedShift(activeShift.AppliedShift_ID);
                        model.Last().NumberofShift = _commonService.GetAppliedShiftClockinClockouttimes(activeShift.AppliedShift_ID).Count;
                    }


                }
            }
            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            ViewBag.Speciality = specialities;
            model = model.OrderByDescending(x => x.DateOfShift).ToList();
            return View(model);
        }
        public IActionResult ClientsList()
        {

            List<ClinicalInstitutionViewModel> model = new List<ClinicalInstitutionViewModel>();
            List<ApplicationUser> clusers = _userManager.GetUsersInRoleAsync("ClinicalInstitution").Result.ToList();
            if (clusers.Count > 0)
            {
                foreach (ApplicationUser cluser in clusers)
                {
                    ClinicalInstitutions client = _commonService.FindClinicaByUserID(cluser.Id);
                    if (client != null)
                    {
                        var city = _commonService.GetCitiesByCityid(cluser.City_ID);
                        var state = _commonService.GetStates(231).Where(x => x.id == city.state_id).FirstOrDefault();
                        var specialties = _commonService.GetClientSpecialties(client.Institution_ID);
                        model.Add(new ClinicalInstitutionViewModel
                        {
                            Address = cluser.Address,
                            CityName = city.city_name,
                            StateName=state!=null?state.state_name:"",
                            City_ID = cluser.City_ID,
                            ContactPerson = client.ContactPerson,
                            ContactTitle = client.ContactTitle,
                            Email = cluser.Email,
                            InstitutionDescription = client.InstitutionDescription,
                            InstitutionName = client.InstitutionName,
                            InstitutionTaxId = client.InstitutionTaxId,
                            InstitutionType_ID = client.InstitutionType_ID,
                            InstitutionType = _commonService.GetInstitutionTypes().Where(x => x.InstitutionType_ID == client.InstitutionType_ID).Select(x => x.InstitutionTypeName).FirstOrDefault(),
                            Institution_ID = client.Institution_ID,
                            Logo = "/" + client.Logo.Replace('\\', '/'),
                            PhoneNumber = cluser.PhoneNumber,
                            Status = ((BordingProcessEnum)Convert.ToInt32(client.Status)).ToString(),
                            BoardingProcess = client.Status,
                            User_ID = cluser.Id,
                            ZipCode = cluser.ZipCode,
                            Locations = _commonService.GetLocations(client.Institution_ID),
                            DateCreated = client.DateCreated,
                            DateModified = client.DateModified,
                             PreferredSpecialties=specialties!=null?String.Join("; ",specialties.Select(x=>x.SpecialityName).ToList()):""

                        });
                    }
                }

            }

            model = model.OrderByDescending(x => x.DateModified).ToList();
            return View(model);
        }
        public IActionResult ShiftsList()
        {

            List<ClientShiftViewModel> model = new List<ClientShiftViewModel>();
            List<ClientShifts> shifts = _commonService.GetAllShifts();
           
            {
                foreach (ClientShifts shift in shifts)
                {

                    if (shift.Available)
                    {
                        model.Add(new ClientShiftViewModel
                        {
                            Applied = false,
                            Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                            BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : _commonService.GetClinicalInstitution_byID(shift.Institution_ID).InstitutionName,
                            ClientShift_ID = shift.ClientShift_ID,
                            Institution_ID = shift.Institution_ID,
                            ClockInTime = shift.ClockInTime,
                            ClockOutTime = shift.ClockOutTime,
                            Consecutive_Occurrences = shift.Consecutive_Occurrences,
                            ContractorCount = shift.ContractorCount,
                            DateOfShift = shift.DateOfShift,
                            EndDate = shift.EndDate,
                            HolidayShift = shift.HolidayShift,
                            HourlyRate = shift.HourlyRate,
                            Occurrences = shift.Occurrences,
                            Responsibility = shift.Responsibility,
                            ShiftDescription = shift.ShiftDescription,
                            ShiftExpirationDate = shift.ShiftExpirationDate,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == shift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                            ShiftLabel_ID = shift.ShiftLabel_ID,
                            StartDate = shift.StartDate,
                            SpecialitiesName = String.Join(", ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                             Category=_commonService.GetShiftCategories().Where(x=>x.Category_ID==shift.Category_ID).FirstOrDefault().CategoryName,
                             Category_ID=shift.Category_ID

                        });
                    }
                }

            }

            model = model.OrderByDescending(x => x.DateOfShift).ToList();
            return View(model);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _userManager.RemoveClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public IActionResult GenerateEmployment([Bind(Prefix = "employment")] EmploymentAgreementViewModel model)
        {
            NotificationTemplates temp = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == 1).First();
            string SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                               Where(x => model.specialities.Contains(x.Speciality_ID))
                                                                               .Select(x => x.SpecialityName).ToList());
            var Applicant = _commonService.FindApplicantByUserID(model.Employee_ID);
            if(Applicant!=null)
            {
                _commonService.UpdateApplicantSpecialtiesStatus(model.specialities, Applicant.Applicant_ID);
            }
            int xposition;
            int yposition;
            int pagenumber;
            int emp_xposition;
            int emp_yposition;
            int emp_pagenumber;
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            PdfReader pdfReader = new PdfReader(Path.Combine(_environment.WebRootPath, "Upload") + "\\" + temp.NotificationTitle + ".pdf");
            using (FileStream os = new FileStream(Path.Combine(_environment.WebRootPath, "Upload") + "\\" + temp.NotificationTitle + "_" + model.Employee_ID + ".pdf", FileMode.Create))
            {
                PdfStamper stamper = new PdfStamper(pdfReader, os);

                AcroFields fields = stamper.AcroFields;
                fields.SetField("employee_name", model.Employee_Name);
                fields.SetField("employee_address", model.Employee_address);
                fields.SetField("employer_name", model.Employer_Name);
                fields.SetField("employer_address", model.Employer_address);
                fields.SetField("hourly_rate", model.Hourly_rate);
                fields.SetField("specialities", SpecialitiesName);
                fields.SetField("day", DateTime.Now.ToString("dd"));
                fields.SetField("month", DateTime.Now.ToString("MMMM"));
                fields.SetField("year", DateTime.Now.ToString("yyyy"));
                float[] signposition = fields.GetFieldPositions("employer_sign");
                Rectangle rectangle = pdfReader.GetPageSize(Convert.ToInt32(signposition[0]));

                yposition = Convert.ToInt32(rectangle.Top - signposition[4] - (signposition[4] - signposition[2]));
                xposition = Convert.ToInt32(signposition[1] + ((signposition[3] - signposition[1]) / 2));
                pagenumber = Convert.ToInt32(signposition[0]);
                float[] emp_signposition = fields.GetFieldPositions("employee_sign");
                Rectangle emp_rectangle = pdfReader.GetPageSize(Convert.ToInt32(signposition[0]));
                emp_yposition = Convert.ToInt32(emp_rectangle.Top - emp_signposition[4] - (emp_signposition[4] - emp_signposition[2]));
                emp_xposition = Convert.ToInt32(emp_signposition[1] + ((emp_signposition[3] - emp_signposition[1]) / 2));
                emp_pagenumber = Convert.ToInt32(emp_signposition[0]);
                stamper.Close();
                pdfReader.Close();
            }

            var callbackUrl = Url.Action("SignCompleted", "Home", new { }, Request.Scheme);

            string url = _signature.Geturlsignature(temp.NotificationTemplate_ID + "_" + savefiletypeprefix, Path.Combine(_environment.WebRootPath, "Upload") + "\\" + temp.NotificationTitle + "_" + model.Employee_ID + ".pdf", "info@dixonwalther.com", model.Employer_Name, model.Employee_ID, callbackUrl, xposition, yposition, pagenumber, emp_xposition, emp_yposition, emp_pagenumber);

            EmploymentAgreements employment = new EmploymentAgreements()
            {
                EmployeeAddress = model.Employee_address,
                EmployeeName = model.Employee_Name,
                HourlyRate = model.Hourly_rate,
                StartDate = model.StartDate,
                Position = SpecialitiesName,
                User_ID = model.Employee_ID
            };
            _commonService.AddEmploymentAgreement(employment);
            return Redirect(url);



        }
        [HttpGet]
        public IActionResult SignCompleted()
        {
            string eventanswer = HttpContext.Request.Query["event"];
            string signsendedId = HttpContext.Request.Query["signId"];
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Guid signid = Guid.Empty;
            Guid.TryParse(signsendedId, out signid);
            if (signid != Guid.Empty)
            {
                SignSent sign = new SignSent();
                sign = _commonService.GetSignSent(signid);


                if (eventanswer == "signing_complete" && sign.Status == "" && savefiletypeprefix == sign.FileType.Substring(sign.FileType.LastIndexOf('_') + 1))
                {

                    // answer get filepath, id download completed
                    var answer = _signature.Downloadsignfile(sign.Envelope_ID, sign.User_ID.ToString(), sign.FileType);
                    if (answer != String.Empty)
                    {
                        sign.Status = "downloaded";
                        sign.FilePath = answer;
                        _commonService.UpdateSignSended(sign);
                        Notifications notifications = new Notifications();
                        int notificationtemp_id = 0;
                        Int32.TryParse(sign.FileType.Substring(0, sign.FileType.LastIndexOf('_')), out notificationtemp_id);

                        if (notificationtemp_id != 0)
                        {
                            notifications.NotificationTemplate_ID = notificationtemp_id;
                            notifications.User_ID = sign.User_ID;
                            _commonService.AddNotification(notifications);

                        }
                        if (notificationtemp_id == 1)
                        {
                            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                            Guid adminid = Guid.Empty;
                            if (user != null)
                            {
                                adminid = user.Id;
                            }
                            Administrators administrator = _commonService.GetAdministratorbyID(adminid);
                            EmploymentAgreements employmentAgreements = _commonService.GetEmploymentAgreements(sign.User_ID);
                            string filecontent = GetHtmlStringFromPath("EmploymentAgreement");
                            string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                            string Body = filecontent.Replace(Subject, "");
                            Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                            emailkeys.Add("{Name}", employmentAgreements.EmployeeName);
                            emailkeys.Add("{Position}", employmentAgreements.Position);
                            emailkeys.Add("{HourlyRate}", employmentAgreements.HourlyRate);
                            emailkeys.Add("{StartDate}", employmentAgreements.StartDate.ToString("MMMM dd yyyy"));
                            emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                            emailkeys.Add("{AdminTitle}", administrator.Title);
                            emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                            string url = "#";
                          NotificationTemplates template=  _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == 1).FirstOrDefault();
                            if (template != null)
                                url = Url.Action(template.NotificationAction, template.NotificationController, new { nid = notifications.Notification_ID }, protocol: _rootPath.Type, host: _rootPath.ReleasedLink);
                            emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);
                            emailkeys.Add("{Click}", url);
                           
                            foreach (var key in emailkeys)
                            {
                               Body= Body.Replace(key.Key, key.Value);
                                Subject=Subject.Replace(key.Key,key.Value);
                            }
                            ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == employmentAgreements.User_ID).FirstOrDefault();
                            if(applicationUser!=null)
                            {
                               _emailService.SendEmailAsync(applicationUser.Email, Subject, Body, true);
                            }
                        }
                        
                        AdminChanges changes = new AdminChanges()
                        {
                            Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                            Changes = "Emp Agreement signed by admin",
                            User_ID = sign.User_ID
                        };
                        _commonService.AddAdminChanges(changes);

                    }

                }
            }

            return View();
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                    user.TimeOffset = model.TimeOffset;
                   await _userManager.UpdateAsync(user);
                   // await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));

                    RedirectToActionResult ressult = RedirectToAction("UsersList");
                    return ressult;
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult _ComposeEmail(Guid User_ID)
        {

            try
            {
                string name = "";
                ApplicationUser user = _userManager.Users.Where(x=>x.Id==User_ID).FirstOrDefault();
                if(_userManager.GetRolesAsync(user).Result.Contains("Applicant"))
                {
                    Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                    name = applicant.FirstName + " " + applicant.MiddleName + " " + applicant.LastName;
                }
                if(_userManager.GetRolesAsync(user).Result.Contains("ClinicalInstitution"))
                {
                    ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                    name = clinical.ContactPerson;
                }

                if (user != null)
                {
                    EmailSendModel model = new EmailSendModel()
                    {
                        To = user.Email,
                        Subject="",
                        Body="Dear "+name+",",

                    };
                    
                    return PartialView("_ComposeEmail", model );
                }
            }
            catch { }
            return PartialView("_ComposeEmail");
        }
        public async Task<JsonResult> SendEmail(EmailSendModel model)
        {
            string answer = "";
            if(model.To!=""&& model.Subject!=""&&model.Body!="")
            {
                try
                {
                 answer=   await  _emailService.SendEmailAsync(model.To, model.Subject, model.Body.Trim(), true);
                    
                }
                catch(Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return Json(answer);

        }

        public string GetHtmlStringFromPath(string FileName)
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
        public JsonResult GetShiftDates(int ClientShift_ID)
        {
            ClientShifts shift = _commonService.GetClientShiftByID(ClientShift_ID);
            if (shift != null)

            {
                return Json(shift.ShiftsDates);
            }
            return Json("");
        }
        public JsonResult GetEmpAgreementSpecialties(Guid user_id)
        {
            var Applicant = _commonService.FindApplicantByUserID(user_id);
            if (Applicant != null)
            {
                var specialties = _commonService.GetApplicantSpecialities(Applicant.Applicant_ID).Select(x => x.Speciality_ID).ToList();
                var specialtiesnames = _commonService.GetSpecialities();
                if (specialties != null)
                {
                    return Json(specialtiesnames.Where(x => specialties.Contains(x.Speciality_ID)).ToList());
                }
            }
            return Json(null);
        }

    }
}
