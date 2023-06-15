using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMSmedprohiremvp.Models.Login;
using CMSmedprohiremvp.Models;
using CMSmedprohiremvp.Models.Applicant;
using Microsoft.AspNetCore.Authorization;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.Service.IServices;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using medprohiremvp.Service.EmailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMSmedprohiremvp.Controllers
{

    public class ApplicantController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly string savefiletypeprefix = "adminsign";
        private readonly RootPath _rootPath;
        private string user_ID;
        // role names
        private string role = "admin";
      
        public ApplicantController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
           ICommonServices commonServices, IHostingEnvironment environment, IEmailService emailService, IOptions<RootPath> rootPath)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _environment = environment;
            _emailService = emailService;
            _rootPath = rootPath.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ApplicantDetails(int Applicant_ID)
        {
            if (Applicant_ID != 0)
            {
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);

                if (app != null)
                {


                    ApplicantViewModel model = GetApplicantViewModel(app);
                    model.specialities = new List<ApplicantSpecialitisViewModel>();
                    var specialities = _commonService.GetApplicantSpecialities(app.Applicant_ID);
                    if (specialities != null)
                    {
                        foreach (var speciality in specialities)
                        {
                            model.specialities.Add(new ApplicantSpecialitisViewModel()
                            {
                                LegabilityStates = String.Join("; ", _commonService.GetStates(231)
                                .Where(x => speciality.LegabilityStates.Split("; ").Contains(x.id.ToString()))
                                .Select(x => x.state_name).ToList()),
                                License = speciality.License,
                                AppSpeciality = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).First().SpecialityName,

                            });
                        }
                    }
                    model.workHistories = new List<ApplicantWorkHistoryViewModel>();
                    var workHistories = _commonService.GetApplicantWorkHistory(app.Applicant_ID);
                    if (workHistories != null)
                    {
                        foreach (var workHistory in workHistories)
                        {
                            model.workHistories.Add(new ApplicantWorkHistoryViewModel()
                            {
                                EndDate = workHistory.EndDate,
                                StartDate = workHistory.StartDate,
                                PlaceName = workHistory.PlaceName,
                                UntilNow = workHistory.EndDate == null ? true : false,
                                AppSpeciality = workHistory.JobTitle,

                            });
                        }
                    }
                    model.certificates = new List<ApplicantCertificateViewModel>();
                    var certificates = _commonService.GetApplicantCertificates(app.Applicant_ID);
                    if (certificates != null)
                    {
                        foreach (var certificate in certificates)
                        {
                            model.certificates.Add(new ApplicantCertificateViewModel()
                            {
                                CeritifcationImg = certificate.CeritificationImg,
                                AppCertificateType = certificate.CertificateTypes


                            });
                        }
                    }
                    if (app.BoardingProcess > 1)
                    {
                        model.references = new List<ApplicantReferencesViewModel>();
                        var references = _commonService.GetApplicantReferences(app.Applicant_ID);
                        if (references != null)
                        {
                            foreach (var reference in references)
                            {
                                model.references.Add(new ApplicantReferencesViewModel()
                                {
                                    Company = reference.Company,
                                    ContactName = reference.ContactName,
                                    Email = reference.Email,
                                    PhoneNumber = reference.PhoneNumber,
                                    Position = reference.Position

                                });
                            }
                        }
                    }
                    return View(model);
                }
            }
            return View();
        }


        public async Task<ActionResult> FileView(string path)
        {
            if (!path.Contains("wwwroot"))
            {
                path = _rootPath.UserRoot + path;
            }
            try
            {
                byte[] array;
                string filename = path.Substring(path.LastIndexOf('\\') + 1);
                string fielmime = path.Substring(path.LastIndexOf('.') + 1);
                string file = path;

                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    array = wc.DownloadData(path);
                    var memory = new MemoryStream(array);
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
            catch(Exception ex)
            {
                return View(ex.Message);
            }
            //catch
            //{
            //    throw new Exception("This Field is not found");
            //}
            return View();
        }

        //public async Task<IActionResult> FileView(string path)
        //{
        //    if (path != null)
        //    {
        //        if (!path.Contains("wwwroot"))
        //        {
        //            path = _rootPath.UserRoot+ path;
        //        }
        //        try
        //        {
        //            byte[] array;
        //            string filename = path.Substring(path.LastIndexOf('\\') + 1);
        //            string fielmime = path.Substring(path.LastIndexOf('.') + 1);
        //            string file = path;
        //            using (Response.Body)
        //            {
        //                Response.Headers.Clear();
        //                Response.Headers.Add("Content-Disposition", $"inline; filename=" + filename);
        //                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
        //                {
        //                    array = new Byte[stream.Length];
        //                    stream.Read(array, 0, array.Length);

        //                }
        //                Response.Headers.ContentLength = array.Length;
        //                switch (fielmime)
        //                {
        //                    case "png":
        //                        Response.ContentType = "image/png";
        //                        break;
        //                    case "jpg":
        //                    case "jpeg":
        //                        Response.ContentType = "image/jpeg";
        //                        break;
        //                    case "pdf":
        //                        Response.ContentType = "application/pdf";
        //                        break;
        //                    case "doc":
        //                        Response.ContentType = "application/msword";
        //                        break;
        //                    case "docx":
        //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //                        break;
        //                    case "rtf":
        //                        Response.ContentType = "application/rtf";
        //                        break;

        //                };


        //                await Response.Body.WriteAsync(array, 0, array.Length);
        //                await Response.Body.FlushAsync();

        //            }
        //        }
        //        catch { return View(); }
        //    }
        //    return View();
        //}
        public IActionResult ViewEmpAgreement(Guid user_id)
        {
            Applicants applicant = _commonService.FindApplicantByUserID(user_id);
            if (applicant != null)
            {
                return RedirectToAction("FileView", new { path = applicant.Employment_agreement });
            }
            return RedirectToAction("FileView");
        }
        public IActionResult AcceptShift(int AppliedShift_ID, string Remarks)
        {
            try
            {

                ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShift(AppliedShift_ID);
                if (appliedShifts != null)
                {

                    Applicants app = _commonService.GetApplicantbyId(appliedShifts.Applicant_ID);
                    ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();

                    Notifications notifications = new Notifications()
                    {
                        NotificationTemplate_ID = 3,
                        User_ID = app.User_ID,
                        Special_ID = AppliedShift_ID,
                        
                    };

                    int notification_ID = _commonService.AddNotification(notifications);
                    appliedShifts.Status = 1;
                    appliedShifts.Remarks = Remarks;

                    _commonService.UpdateApplicantAppliedShift(appliedShifts);
                    #region sendingemail
                    ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                    Guid adminid = Guid.Empty;
                    if (user != null)
                    {
                        adminid = user.Id;
                    }
                    Administrators administrator = _commonService.GetAdministratorbyID(adminid);
                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                    string filecontent = GetHtmlStringFromPath("AcceptShift");
                    string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                    string Body = filecontent.Replace(Subject, "");
                    string url = "";
                    emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                    NotificationTemplates template = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == 3).FirstOrDefault();
                    if (template != null)
                       url = Url.Action(template.NotificationAction, template.NotificationController, new { nid = notifications.Notification_ID }, protocol: _rootPath.Type, host: _rootPath.ReleasedLink);
                    emailkeys.Add("{ReturnUrl}",url);
                    emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

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
                        Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                        Changes = "Shift Accepted",
                        User_ID = app.User_ID
                    };
                    _commonService.AddAdminChanges(changes);

                }

            }
            catch { }
            return RedirectToAction("UsersList", "Home");
        }
        public IActionResult DismissShift(int AppliedShift_ID, string Remarks)
        {
            try
            {

                ApplicantAppliedShifts appliedShifts = _commonService.GetAppliedShift(AppliedShift_ID);
                if (appliedShifts != null)
                {

                    Applicants app = _commonService.GetApplicantbyId(appliedShifts.Applicant_ID);
                    ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();

                    Notifications notifications = new Notifications()
                    {
                        NotificationTemplate_ID = 4,
                        User_ID = app.User_ID,
                        Special_ID = AppliedShift_ID
                    };
                    #region sendingemail
                    ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                    Guid adminid = Guid.Empty;
                    if (user != null)
                    {
                        adminid = user.Id;
                    }
                    Administrators administrator = _commonService.GetAdministratorbyID(adminid);
                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                    string filecontent = GetHtmlStringFromPath("DismissShift");
                    string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                    string Body = filecontent.Replace(Subject, "");
                   
                    emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);

                    emailkeys.Add("{ReturnUrl}", Url.Action("AvailableShifts", "Applicant", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                    emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

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
                    int notification_ID = _commonService.AddNotification(notifications);
                    appliedShifts.Status = -1;
                    appliedShifts.Remarks = Remarks;

                    _commonService.UpdateApplicantAppliedShift(appliedShifts);
                    AdminChanges changes = new AdminChanges()
                    {
                        Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                        Changes = "Shift Dissmissed",
                        User_ID = app.User_ID
                    };
                    _commonService.AddAdminChanges(changes);

                }

            }
            catch { }
            return RedirectToAction("UsersList", "Home");
        }
  
        public JsonResult ActivateBoarding(int Applicant_ID)
        {
            try
            {

                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();
                app.BoardingProcess = app.BoardingProcess + 1;
                _commonService.UpdateApplicant(app);
                Notifications notifications = new Notifications()
                {
                    NotificationTemplate_ID = 2,
                    User_ID = app.User_ID,
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
                emailkeys.Add("{Name}", app.FirstName+" "+ app.LastName);
                emailkeys.Add("{ReturnUrl}", Url.Action("Profile", "Applicant", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

                emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                emailkeys.Add("{AdminTitle}", administrator.Title);
                emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                foreach (var key in emailkeys)
                {
                    Body = Body.Replace(key.Key, key.Value);
                    Subject = Subject.Replace(key.Key, key.Value);
                }

            var answer= _emailService.SendEmailAsync(appuser.Email, Subject, Body, true).Result;
                #endregion sendingemail
                AdminChanges changes = new AdminChanges()
                {
                    Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                    Changes = "Boarding Proccess activation",
                    User_ID = app.User_ID
                };
                _commonService.AddAdminChanges(changes);
                return Json(true);
            }
            catch { }
            return Json(false);
        }
        public JsonResult CompleteBoarding(int Applicant_ID)
        {
            try
            {

                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();
                app.BoardingProcess = app.BoardingProcess + 1;
                _commonService.UpdateApplicant(app);
                //Notifications notifications = new Notifications()
                //{
                //    NotificationTemplate_ID = 5,
                //    User_ID = app.User_ID,
                //    NotificationBody = "MedProHire LLC administration make completed your onboarding proccess.",
                //    NotificationTitle = "Onboarding Process Completed"
                //};
                //#region sendingemail
                //Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                //Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                //string filecontent = GetHtmlStringFromPath("CompleteOnBoarding");
                //string Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                //string Body = filecontent.Replace(Subject, "");
                //emailkeys.Add("{Name}", app.FirstName + " " + app.LastName);
                ////emailkeys.Add("{ReturnUrl}", Url.Action("Profile", "Applicant", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                //emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

                //emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                //emailkeys.Add("{AdminTitle}", administrator.Title);
                //emailkeys.Add("{AdminPhoneNumber}", administrator.PhoneNumber);
                //foreach (var key in emailkeys)
                //{
                //    Body = Body.Replace(key.Key, key.Value);
                //    Subject = Subject.Replace(key.Key, key.Value);
                //}

                //var answer = _emailService.SendEmailAsync(appuser.Email, Subject, Body, true).Result;
                //#endregion sendingemail
               // _commonService.AddNotification(notifications);
                AdminChanges changes = new AdminChanges()
                {
                    Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                    Changes = "Boarding Proccess completed",
                    User_ID = app.User_ID
                };
                _commonService.AddAdminChanges(changes);
                return Json(true);
            }
            catch { }
            return Json(false);
        }
        public JsonResult CompleteShift(int AppliedShift_ID)
        {
            try
            {

                ApplicantAppliedShifts AppliedShift = _commonService.GetAppliedShift(AppliedShift_ID);
                if (AppliedShift != null)
                {
                    AppliedShift.Status = 2;
                    _commonService.UpdateApplicantAppliedShift(AppliedShift);
                    Applicants app = _commonService.GetApplicantbyId(AppliedShift.Applicant_ID);
                    Notifications notifications = new Notifications()
                    {
                        NotificationTemplate_ID = 5,  
                        User_ID = app.User_ID,
                        NotificationBody = "MedProHire LLC administration make completed your Applied Shift, AppliedShift_ID "+AppliedShift_ID,
                        NotificationTitle = "Applied Shift Completed"
                    };
                    _commonService.AddNotification(notifications);
                  List<ApplicantAppliedShifts> appliedclientShifts=  _commonService.GetAppliedShiftsbyClientShift_ID(AppliedShift.ClientShift_ID);
                    ClientShifts clientshift = _commonService.GetClientShiftByID(AppliedShift.ClientShift_ID);
                    if(appliedclientShifts != null && clientshift!=null)
                    {
                      if( clientshift.EndDate<DateTime.Now)
                        {
                            int activecontractorscount = appliedclientShifts.Where(x => x.Accepted == true && x.Status == 1).Count();
                            if(activecontractorscount == 0)
                            {
                                int appliedcontractorscount = appliedclientShifts.Where(x => x.Accepted == true && x.Status == 2).Select(x => x.Applicant_ID).Distinct().Count();
                                if (appliedcontractorscount == clientshift.ContractorCount)
                                {
                                    clientshift.Category_ID = 4;
                                }
                                else
                                {
                                    clientshift.Category_ID = 5;
                                }
                                _commonService.UpdateClientShift(clientshift);
                            }
                        }
                    }
                
                    AdminChanges changes = new AdminChanges()
                    {
                        Admin_ID = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                        Changes = "AppliedShift completed, AppliedShift_ID=" + AppliedShift_ID,
                        User_ID = app.User_ID
                    };
                    _commonService.AddAdminChanges(changes);
                    return Json(true);
                }
            }
            catch { }
            return Json(false);
        }
        public ApplicantViewModel GetApplicantViewModel(Applicants app)
        {
            ApplicationUser appuser = _userManager.Users.Where(x => x.Id == app.User_ID).First();
            ApplicantViewModel model = new ApplicantViewModel()
            {
                Applicant_ID = app.Applicant_ID,
                Address = appuser.Address + ", " + _commonService.GetCityName(appuser.City_ID) + ", " + appuser.ZipCode,
                Availability_ID = app.Availability_ID,
                Availability = app.Availability_ID != 0 ? _commonService.GetAvailabilities().Where(x => x.Availability_ID == app.Availability_ID).Select(x => x.Availability).First() : null,
                BackgroundCheck = app.BackgroundCheck,
                BoardingProcess = app.BoardingProcess,
                LastName = app.LastName,
                FirstName = app.FirstName,
                MiddleName = app.MiddleName,
                IsEligible = app.IsEligible,
                CEU = app.CEU,
                City_ID = appuser.City_ID,
                CityName = _commonService.GetCitiesByCityid(appuser.City_ID).city_name,
                StateName = _commonService.GetCityName(appuser.City_ID),
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
                Resume = app.Resume,
                WorkAuth = app.WorkAuth,
                W_4 = app.W_4,
                ZipCode = appuser.ZipCode,
                CertificatiesString = String.Join(',',  _commonService.GetApplicantCertificates(app.Applicant_ID).Select(c => c.CertificateTypes).ToList()),
                
                    
                SpecialitiesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                DateCreated = app.DateCreated,
                DateModified = app.DateModified,
                AppliedShiftCount = _commonService.GetApplicantAppliedShifts(appuser.Id).Where(x => x.Status == 0 && x.Accepted != true).ToList().Count
            };
            return model;
        }
        public IActionResult PayCheck(ContractorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PayCheck != null)
                {
                    PayChecks checks = new PayChecks();
                    checks.Applicant_ID = model.PayCheck.Applicant_ID;
                    
                    checks.Net_Pay = model.PayCheck.Net_pay;
                    checks.PayBeginDate = model.PayCheck.PayBeginDate;
                    checks.PayEndDate = model.PayCheck.PayEndDate;
                    checks.PayCheckDate = model.PayCheck.PayCheckDate;
                    IFormFile file = model.PayCheck.PaycheckFile;
                    string foldername = "Upload\\PayCheck" + $@"\{model.PayCheck.Applicant_ID}";
                    string filename = "PayCheck" +"_"+ DateTime.Now.ToShortDateString().Replace("/", "_").Replace(".", "_").Replace(@"\", "_");
                    if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername)))
                    {
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername));
                    }
                    string extention = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));
                    filename += extention;
                    using (FileStream fs = new FileStream(Path.Combine(_environment.WebRootPath, foldername, filename), FileMode.Create))
                    {
                        try
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                            checks.PayCheckFile = Path.Combine(_environment.WebRootPath, foldername, filename);
                        }
                        catch
                        {
                            TempData["paycheckadded"] = "fileerror";
                            return RedirectToAction("ContractorsList", "Home");
                        }

                    }
                    _commonService.Add_PayCheck(checks);
                    TempData["paycheckadded"] = "ok";
                }
                else
                {
                    TempData["paycheckadded"] = "modelerror";
                }
            }
            return RedirectToAction("ContractorsList", "Home");
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
        #region AJAX
        public IActionResult _ApplicantPersonalInfo(Guid User_ID)
        {

            try
            {

                Applicants app = _commonService.FindApplicantByUserID(User_ID);

                if (app != null)
                {


                    ApplicantViewModel model = GetApplicantViewModel(app);
                    return PartialView("_ApplicantPersonalInfo", model);
                }
            }
            catch { }
            return PartialView("_ApplicantPersonalInfo");
        }
        public IActionResult _ApplicantBoardingProcess(Guid User_ID)
        {

            try
            {

                Applicants app = _commonService.FindApplicantByUserID(User_ID);

                if (app != null)
                {


                    ApplicantViewModel model = GetApplicantViewModel(app);
                    List<ApplicantReferences> references = _commonService.GetApplicantReferences(app.Applicant_ID);
                    if (references.Count > 0)
                    {
                        List<ApplicantReferencesViewModel> appref = new List<ApplicantReferencesViewModel>();
                        foreach (ApplicantReferences reference in references)
                        {
                            appref.Add(new ApplicantReferencesViewModel
                            {
                                Company = reference.Company,
                                ContactName = reference.ContactName,
                                Email = reference.Email,
                                PhoneNumber = reference.PhoneNumber,
                                Position = reference.Position,
                                 Applicant_ID=reference.Applicant_ID
                            });
                        }
                        model.references = appref;
                    }
                    return PartialView("_ApplicantBoardingProcess", model);
                }
            }
            catch { }
            return PartialView("_ApplicantBoardingProcess");
        }
        public IActionResult _ApplicantSpecialities(Guid User_ID)
        {
            try
            {
                Applicants app = _commonService.FindApplicantByUserID(User_ID);

                if (app != null)
                {
                    List<ApplicantSpecialitisViewModel> model = new List<ApplicantSpecialitisViewModel>();

                    var specialities = _commonService.GetApplicantSpecialities(app.Applicant_ID);
                    if (specialities != null)
                    {
                        foreach (var speciality in specialities)
                        {
                            model.Add(new ApplicantSpecialitisViewModel()
                            {
                                LegabilityStates = String.Join("; ", _commonService.GetStates(231)
                                .Where(x => speciality.LegabilityStates.Split("; ").Contains(x.id.ToString()))
                                .Select(x => x.state_name).ToList()),
                                License = speciality.License,
                                AppSpeciality = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).First().SpecialityName,

                            });
                        }
                    }
                    return PartialView("_ApplicantSpecialities", model);
                }
            }
            catch { }
            return PartialView("_ApplicantSpecialities");
        }

        public IActionResult _ApplicantAplliedShifts(Guid User_ID)
        {
            try
            {
                Applicants app = _commonService.FindApplicantByUserID(User_ID);

                if (app != null)
                {
                    List<ApplicantAppliedShiftsViewModel> model = new List<ApplicantAppliedShiftsViewModel>();

                    var appliedShifts = _commonService.GetApplicantAppliedShifts(User_ID).Where(x => x.Accepted == false && x.Status == 0).ToList();
                    if (appliedShifts != null)
                    {
                        foreach (var appliedShift in appliedShifts)
                        {
                            model.Add(new ApplicantAppliedShiftsViewModel()
                            {
                                Accepted = appliedShift.Accepted,
                                Applicant_ID = appliedShift.Applicant_ID,
                                AppliedShift_ID = appliedShift.AppliedShift_ID,
                                ClientShift_ID = appliedShift.ClientShift_ID,
                                AppliedAllDays = appliedShift.AppliedAllDays,
                                Status=appliedShift.Status


                            });
                            List<string> applieddays = appliedShift.AppliedDaysList.Split(',').ToList();

                            for (int i = 0; i < applieddays.Count; i++)
                            {
                                applieddays[i] = Convert.ToDateTime(applieddays[i].Trim()).ToString("MMMM dd");
                            }
                            model.Last().AppliedDaysList = appliedShift.AppliedDaysList;// String.Join("; ", applieddays);
                            ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
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
                                model.Last().ShiftsDates = shift.ShiftsDates;

                            }
                        }
                    }
                    return PartialView("_ApplicantAplliedShifts", model);
                }
            }
            catch { }
            return PartialView("_ApplicantAplliedShifts");
        }
 
        public IActionResult _ApplicantAppliedShiftDetail(int AppliedShift_ID)
        {
            try
            {
                
                    ApplicantAppliedShiftsViewModel model = new ApplicantAppliedShiftsViewModel();

                    var appliedShift = _commonService.GetAppliedShift(AppliedShift_ID);
                    if (appliedShift != null)
                    {



                    model.Accepted = appliedShift.Accepted;
                    model.Applicant_ID = appliedShift.Applicant_ID;
                    model.AppliedShift_ID = appliedShift.AppliedShift_ID;
                    model.ClientShift_ID = appliedShift.ClientShift_ID;
                    model.AppliedAllDays = appliedShift.AppliedAllDays;
                    model.Status = appliedShift.Status;
                    model.AppliedDaysList = appliedShift.AppliedDaysList;// String.Join("; ", applieddays);
                            ClientShifts shift = _commonService.GetClientShiftByID(appliedShift.ClientShift_ID);
                            if (shift != null)
                            {

                                model.ClockInTime = shift.ClockInTime;
                                model.ClockOutTime = shift.ClockOutTime;
                                model.HourlyRate = shift.HourlyRate;
                                model.ContractorCount = shift.ContractorCount;
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
                                model.HolidayShift = shift.HolidayShift;
                                model.Occurrences = shift.Occurrences;
                                model.Consecutive_Occurrences = shift.Consecutive_Occurrences;
                        model.ShiftsDates = shift.ShiftsDates;

                            }
                        
                    }
                    return PartialView("_ApplicantAppliedShiftDetail", model);
                
            }
            catch { }
            return PartialView("_ApplicantAppliedShiftDetail");
        }
        #endregion
    }
}