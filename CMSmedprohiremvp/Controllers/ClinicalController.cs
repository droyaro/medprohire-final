using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CMSmedprohiremvp.Models.Clinical;
using medprohiremvp.DATA.Entity;
using Microsoft.Extensions.Options;
using System.IO;
using CMSmedprohiremvp.Models;
using iTextSharp.text.pdf;
using System.Drawing;
using iTextSharp.text;
using System.Data;
using CMSmedprohiremvp.Models.Applicant;

namespace CMSmedprohiremvp.Controllers
{
    public class ClinicalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly RootPath _rootPath;
        private readonly string savefiletypeprefix = "adminsign";
        private string user_ID;
        // role names
        private string role = "admin";


        public ClinicalController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
           ICommonServices commonServices, IHostingEnvironment environment, IEmailService emailService, IOptions<RootPath> rootpath)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _environment = environment;
            _emailService = emailService;
            _rootPath = rootpath.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShiftView(int Clientshift_ID)
        {
            ApplicantAppliedShifts applicantAppShift = new ApplicantAppliedShifts();
            ClientShifts shift = _commonService.GetClientShiftByID(Clientshift_ID);
            var applicants = _commonService.GetApplicantsById().Where(a => a.ClientShift_ID == shift.ClientShift_ID).ToList();
            var operatingApplicants = _commonService.GetOperatingApplicants().ToList();
            var appliedShifts = _commonService.GetAppApliedShifts().Where(a => a.ClientShift_ID == shift.ClientShift_ID).ToList();
           
             


            var operatingApplicantsViewModel = (from applicant in applicants
                                                join aperatingApplicant in operatingApplicants on applicant.Applicant_ID equals aperatingApplicant.Applicant_ID
                                                join appliedShift in appliedShifts on applicant.Applicant_ID equals appliedShift.Applicant_ID
                                                where applicant.ClientShift_ID == aperatingApplicant.ClientShift_ID && applicant.ClientShift_ID == appliedShift.ClientShift_ID
                                                select new OperatingApplicantsViewModel
                                                {
                                                    FirstName = aperatingApplicant.FirstName,
                                                    LastName = aperatingApplicant.LastName,
                                                    City = aperatingApplicant.City,
                                                    Imgsrc = applicant.applicant.ProfileImage != null ? applicant.applicant.ProfileImage.Replace('\\', '/') : null,
                                                    State = "Texas",
                                                    ClientShift_Id = shift.ClientShift_ID,
                                                    Applicant_ID = aperatingApplicant.Applicant_ID,
                                                    Category_ID = appliedShift.Category_ID

                                                }).ToList();

            if (shift != null)
            {
                ClientShiftViewModel model = new ClientShiftViewModel
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
                    Category = _commonService.GetShiftCategories().Where(x => x.Category_ID == shift.Category_ID).FirstOrDefault().CategoryName,
                    ShiftsDates = shift.ShiftsDates,
                    OperatingApplicants = operatingApplicantsViewModel

                };
                return View(model);
            }
            return View();
        }
        public IActionResult InviteShift(int Clientshift_ID)
        {
            ClientShifts shift = _commonService.GetClientShiftByID(Clientshift_ID);
            InviteShiftViewModel model = new InviteShiftViewModel();
            model.Shift = new ClientShiftViewModel();
            model.Applicants = new List<Models.ApplicantViewModel>();
            if (shift != null)
            {
                model.Shift = new ClientShiftViewModel
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
                    Category = _commonService.GetShiftCategories().Where(x => x.Category_ID == shift.Category_ID).FirstOrDefault().CategoryName,
                    ShiftsDates = shift.ShiftsDates

                };

            }
            List<Applicants> applicants = _commonService.GetContractors();
            if (applicants != null)
            {
                foreach (Applicants app in applicants)
                {
                    ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                    ApplicantAppliedShifts hasshifts = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.ClientShift_ID == Clientshift_ID).FirstOrDefault();
                    model.Applicants.Add(new Models.ApplicantViewModel()
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
                        AppliedShiftCount = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.Status == 0 && x.Accepted != true).ToList().Count,
                        Applied = hasshifts != null ? true : false
                    });
                }
            }
            return View(model);
        }
        public JsonResult AcceptInvitation(ApplicantAppliedShifts model)
        {
            ApplicationUser admin = _userManager.GetUserAsync(HttpContext.User).Result;
            ClientShifts shift = _commonService.GetClientShiftByID(model.ClientShift_ID);

            Applicants app = _commonService.GetApplicantbyId(model.Applicant_ID);
            ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();
            if (shift != null && app != null)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(shift.Institution_ID);
                List<string> freedays = new List<string>();

                freedays = shift.ShiftsDates.Split(',').ToList();
                List<string> ApplicantDays = model.AppliedDaysList.Split(',').ToList();
                if (ApplicantDays.Count == freedays.Count)
                {
                    model.AppliedAllDays = true;
                }
                else
                {
                    model.AppliedAllDays = false;
                }
                model.Invited = "Invited by Admin";
                if (_commonService.AddApplicantAppliedShift(model))
                {
                    Notifications notification = new Notifications()
                    {
                        NotificationTemplate_ID = 6,
                        Status = 1,
                        User_ID = app.User_ID,
                        Special_ID = model.AppliedShift_ID

                    };
                    _commonService.AddNotification(notification);
                    #region sendingemail
                    ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                    Guid adminid = Guid.Empty;
                    if (user != null)
                    {
                        adminid = user.Id;
                    }
                    Administrators administrator = _commonService.GetAdministratorbyID(adminid);
                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                    string filecontent = GetHtmlStringFromPath("InviteToAcceptShift");
                    string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                    string Body = filecontent.Replace(Subject, "");
                    string url = "";
                    var branch = _commonService.GetLocations(clinical.Institution_ID).Where(x => x.Branch_ID == shift.Branch_ID).FirstOrDefault();
                    emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                    NotificationTemplates template = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == 6).FirstOrDefault();
                    if (template != null)
                    {
                        url = Url.Action(template.NotificationAction, template.NotificationController, new { nid = notification.Notification_ID }, protocol: _rootPath.Type, host: _rootPath.ReleasedLink);
                    }

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
                    AdminChanges changes = new AdminChanges()
                    {
                        Changes = "Invite to work with shift" + model.AppliedShift_ID,
                        User_ID = app.User_ID,
                        Admin_ID = admin != null ? admin.Id : Guid.Empty


                    };
                }
                return Json(true);
            }
            return Json(null);

        }
        public JsonResult InviteApplicant(int App_ID, int ClientShift_ID)
        {
            ClientShifts shift = _commonService.GetClientShiftByID(ClientShift_ID);
            if (shift != null)
            {
                List<string> freedays = new List<string>();
                for (DateTime date = shift.StartDate; date <= shift.EndDate; date = date.AddDays(1))
                {
                    freedays.Add(date.ToString("MM/dd/yyyy"));
                }

                List<string> days = new List<string>();
                List<ApplicantAppliedShifts> appliedDays = _commonService.GetAppliedShiftsbyClientShift_ID(ClientShift_ID).Where(x => x.Status == 1 && x.Accepted && x.Applicant_ID != App_ID).ToList();
                if (appliedDays != null)
                {

                    foreach (ApplicantAppliedShifts appliedshift in appliedDays)
                    {
                        days.AddRange(appliedshift.AppliedDaysList.Split(','));
                    }
                    foreach (string day in days)
                    {
                        if (days.FindAll(x => x == day).Count() == shift.ContractorCount)
                        {
                            freedays.Remove(day);
                        }
                    }
                }
                return Json(freedays);
            }
            return Json(null);
        }
        public JsonResult GetAppliedApplicantDays(int ClientShift_ID)
        {
            List<ApplicantAppliedDays> model = new List<ApplicantAppliedDays>();
            List<ApplicantAppliedShifts> shifts = _commonService.GetAppliedShiftsbyClientShift_ID(ClientShift_ID);
            if (shifts != null)
            {
                foreach (ApplicantAppliedShifts shift in shifts)
                {
                    Applicants app = _commonService.GetApplicantbyId(shift.Applicant_ID);
                    if (shift.Status == 0 || shift.Status == 1)
                    {
                        string[] shiftdays = shift.AppliedDaysList.Split(',');
                        if (shiftdays.Count() > 0)
                        {
                            foreach (string shiftday in shiftdays)
                            {
                                model.Add(new ApplicantAppliedDays
                                {
                                    AppliedDay = shiftday.Substring(3, 2),
                                    Type = shift.Status == 0 ? "Picked" : "Accepted",
                                    ApplicantName = app.LastName + " " + app.FirstName

                                });
                            }
                        }
                    }
                }

            }
            return Json(model);
        }
        public IActionResult _clientPersonalInfo(Guid User_ID)
        {
            ClinicalInstitutions client = _commonService.FindClinicaByUserID(User_ID);
            ApplicationUser cluser = _userManager.Users.Where(x => x.Id == User_ID).FirstOrDefault();
            if (client != null && cluser != null)
            {
                var city = _commonService.GetCitiesByCityid(cluser.City_ID);
                List<Specialities> specialties = new List<Specialities>();
                if (client.Status < 2)
                {
                    var SpecIds = client.Specialties.Split(";");
                    specialties = _commonService.GetSpecialities().Where(x => SpecIds.Contains(x.Speciality_ID.ToString())).ToList();
                }
                else
                {
                    var SpecIds = _commonService.GetClientSpecialties(client.Institution_ID).Select(x => x.Speciality_ID).ToList();
                    specialties = _commonService.GetSpecialities().Where(x => SpecIds.Contains(x.Speciality_ID)).ToList();
                }
                var state = _commonService.GetStates(231).Where(x => x.id == city.state_id).FirstOrDefault();

                ClinicalInstitutionViewModel model = new ClinicalInstitutionViewModel
                {
                    Address = cluser.Address,
                    CityName = city.city_name,
                    StateName = state != null ? state.state_name : "",
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
                    Status = ((ClientStatusEnum)Convert.ToInt32(client.Status)).ToString(),
                    BoardingProcess = client.Status,
                    User_ID = cluser.Id,
                    ZipCode = cluser.ZipCode,
                    Locations = _commonService.GetLocations(client.Institution_ID),
                    DateCreated = client.DateCreated,
                    DateModified = client.DateModified,
                    PreferredSpecialties = specialties != null ? String.Join("; ", specialties.Select(x => x.SpecialityName).ToList()) : ""

                };
                return PartialView("_clientPersonalInfo", model);
            }
            return PartialView();
        }
        public IActionResult _clientSpecialtiesCost(Guid User_ID)


        {
            ClinicalInstitutions client = _commonService.FindClinicaByUserID(User_ID);
            ApplicationUser cluser = _userManager.Users.Where(x => x.Id == User_ID).FirstOrDefault();
            if (client != null && cluser != null)
            {
                List<ClientSpecialtiesCost> ClientCosts = new List<ClientSpecialtiesCost>();
                List<Specialities> speciaties = _commonService.GetSpecialities();
                List<ShiftLabels> labels = _commonService.GetShiftLabels();
                // checking if client made cost changes
                if (client.Status == 4)
                {
                    List<ClientCostChanges> changes = _commonService.GetClientCostChanges(client.Institution_ID);

                    List<int> SpecialtiesIds = changes.Select(x => x.Speciality_ID).Distinct().ToList();
                    foreach (var id in SpecialtiesIds)
                    {
                        ClientSpecialtiesCost cost = new ClientSpecialtiesCost();
                        cost.Specialty_ID = id;
                        cost.Cost = new List<ShiftLabelsCost>();
                        cost.SpecialtyName = speciaties.Where(x => x.Speciality_ID == id).Select(x => x.SpecialityName).FirstOrDefault();
                        var labelCosts = changes.Where(x => x.Speciality_ID == id).ToList();
                        foreach (ClientCostChanges labelCost in labelCosts)
                        {
                            cost.Cost.Add(new ShiftLabelsCost()
                            {
                                ShiftLabel_ID = labelCost.ShiftLabel_ID,
                                Cost = labelCost.Cost,
                                ShiftLabelName = labels.Where(x => x.ShiftLabel_ID == labelCost.ShiftLabel_ID).Select(x => x.ShiftLabelName).FirstOrDefault()
                            });
                        }
                        ClientCosts.Add(cost);

                    }
                    if (ClientCosts.Count > 0)
                    {
                        return PartialView("_clientSpecialtiesCost", ClientCosts);
                    }
                }
                else
                {
                    // checking if request come from boarding proccess
                    if (client.Status == 2)
                    {
                        List<ClientSpecialties> specialties = _commonService.GetClientSpecialtiesList(client.Institution_ID);
                        if (specialties != null)
                        {
                            foreach (ClientSpecialties specialty in specialties)
                            {
                                List<ClientSpecialtiesCosts> clientCosts = _commonService.GetClientSpecialtiesCostbyId(specialty.ClientSpeciality_ID);
                                List<ShiftLabelsCost> costs = new List<ShiftLabelsCost>();
                                if (clientCosts != null)
                                {

                                    foreach (var clientCost in clientCosts)
                                    {
                                        costs.Add(new ShiftLabelsCost
                                        {
                                            Cost = clientCost.Cost,
                                            ShiftLabel_ID = clientCost.ShiftLabel_ID,
                                            ShiftLabelName = labels.Where(x => x.ShiftLabel_ID == clientCost.ShiftLabel_ID).Select(x => x.ShiftLabelName).FirstOrDefault()
                                        });
                                    }

                                }


                                ClientCosts.Add(new ClientSpecialtiesCost()
                                {
                                    Specialty_ID = specialty.Speciality_ID,
                                    Cost = costs,
                                    SpecialtyName = speciaties.Where(x => x.Speciality_ID == specialty.Speciality_ID).Select(x => x.SpecialityName).FirstOrDefault()
                                });
                            }
                            if (ClientCosts.Count > 0)
                            {
                                return PartialView("_clientSpecialtiesCost", ClientCosts);
                            }
                        }

                    }
                }
            }
            return null;

        }
        //public IActionResult GenerateSpecialityCosts(List<ClientSpecialtiesCost> model)
        //{
        //    String pathin = Path.Combine(_environment.WebRootPath, "Upload") + "\\Employment Agreement.pdf";
        //    String pathout = Path.Combine(_environment.WebRootPath, "Upload") + "\\test101.pdf";
        //    PdfReader reader = new PdfReader(pathin);
        //    using (FileStream stream = new FileStream(pathout, FileMode.Create))
        //    {
        //        PdfStamper stamper = new PdfStamper(reader, stream);
        //        PdfPCell cell = null;
        //        PdfPTable table = null;
        //        DataTable dt = GetDataTable(model);
        //        if (dt != null)
        //        {
        //            List<ShiftLabels> labels = _commonService.GetShiftLabels();
        //            iTextSharp.text.Font font8 = FontFactory.GetFont("ARIAL", 12);
        //            table = new PdfPTable(dt.Columns.Count);
        //            cell = new PdfPCell(new Phrase(new Chunk("Provider", font8)));
        //            table.AddCell(cell);
                    
        //            for (int i = 0; i < labels.Count(); i++)
        //            {
        //                cell = new PdfPCell(new Phrase(new Chunk(labels[i].ShiftLabelName+" Bill Rate", font8)));
        //                table.AddCell(cell);
        //            }
                
        //            for (int rows = 0; rows < dt.Rows.Count; rows++)
        //            {
        //                for (int column = 0; column < dt.Columns.Count; column++)
        //                {
        //                    cell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
        //                    table.AddCell(cell);
        //                }
        //            }
        //        }
        //        ColumnText ct = new ColumnText(stamper.GetOverContent(5));
        //        ct.AddElement(table);
        //        iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(46, 190, 530, 36);
        //        ct.SetSimpleColumn(30, 30, PageSize.A4.Width, PageSize.A4.Height - 575);
        //        ct.Go();
        //        stamper.Close();
        //        reader.Close();
        //    }
        //    return RedirectToAction("ClientsList", "Home");

        //}
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.White;
            cell.VerticalAlignment = PdfCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
        private DataTable GetDataTable(List<ClientSpecialtiesCost> model)
        {
            List<ShiftLabels> labels = _commonService.GetShiftLabels();
            List<Specialities> specialities = _commonService.GetSpecialities();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Provider", typeof(string)));
            for(int i=0; i<labels.Count();i++)
            {
                dt.Columns.Add(new DataColumn(labels[i].ShiftLabelName + " Bill Rate", typeof(string)));
            }
            foreach(var cost in model)
            {
                object[] row = new object[labels.Count() + 1];
                row[0]=specialities.Where(x=>x.Speciality_ID==cost.Specialty_ID).Select(x=>x.SpecialityName).FirstOrDefault();
                for (int i = 0; i < labels.Count(); i++)
                {
                    var row_cost = cost.Cost.Where(x => x.ShiftLabel_ID == labels[i].ShiftLabel_ID).FirstOrDefault();
                    row[i+1]=row_cost != null ? row_cost.Cost : 0;
                }
                dt.Rows.Add(row);
                
            }
 
            return dt;
        }
        public JsonResult ActivateBoarding(int ClinicalIntitution_ID)
        {
            try
            {

                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(ClinicalIntitution_ID);
                ApplicationUser cluser = _userManager.Users.Where(x => x.Id == clinical.User_ID).First();
                clinical.Status = 1;
                _commonService.UpdateClinical(clinical);
                Notifications notifications = new Notifications()
                {
                    NotificationTemplate_ID = 7,
                    User_ID = clinical.User_ID,
                };
                _commonService.AddNotification(notifications);
                #region sendingemail
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                Guid adminid = Guid.Empty;
                if (user != null)
                {
                    adminid = user.Id;
                }
                Administrators administrator = _commonService.GetAdministratorbyID(adminid);
                Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                string filecontent = GetHtmlStringFromPath("ActivateOnBoarding");
                string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                string Body = filecontent.Replace(Subject, "");
                emailkeys.Add("{Name}", clinical.ContactPerson);
                emailkeys.Add("{ReturnUrl}", Url.Action("Profile", "Clinical", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

                emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                emailkeys.Add("{AdminTitle}", administrator.Title);
                emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                foreach (var key in emailkeys)
                {
                    Body = Body.Replace(key.Key, key.Value);
                    Subject = Subject.Replace(key.Key, key.Value);
                }

                var answer = _emailService.SendEmailAsync(cluser.Email, Subject, Body, true).Result;
                #endregion sendingemail
                AdminChanges changes = new AdminChanges()
                {
                    Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                    Changes = "Client Boarding Proccess activation",
                    User_ID = clinical.User_ID
                };
                _commonService.AddAdminChanges(changes);
                return Json(true);
            }
            catch { }
            return Json(false);
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