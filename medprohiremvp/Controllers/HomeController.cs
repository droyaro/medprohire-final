using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using medprohiremvp.Models.Home;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using medprohiremvp.Repo.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using medprohiremvp.Models;
using medprohiremvp.Models.Applicant;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using medprohiremvp.Service.EmailServices;
using System.Net;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace medprohiremvp.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly RootPath _rootPath;
        private readonly ILogger<HomeController> _logger;
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices,
            IHostingEnvironment environment,
            IEmailService emailService, IOptions<RootPath> rootPath, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _environment = environment;
            _emailService = emailService;
            _rootPath = rootPath.Value;
            _logger = logger;

        }
        public IActionResult Index()
        {
       
            return View();
        }
        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            LogHttpError(statusCode);
            ViewData["StatusCode"] = statusCode;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (user != null)
                {
                    if (_userManager.GetRolesAsync(user).Result.Contains(clrole))
                    {
                        return RedirectToAction("Dashboard", clrole);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", approle);
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        private void LogHttpError(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exception = new Exception($"Http {statusCode} returned when accessing {feature.OriginalPath}/{feature.OriginalQueryString}");


            _logger.LogError(exception, exception.Message);
        }
        public IActionResult FindTalent(int id)
        {
            string model = "pool_talent_services";
            switch (id)
            {
                case 1:
                    model = "pool_talent_services";
                    break;
                case 2:
                    model = "direct_placement_services";
                    break;
                case 3:
                    model = "contract_or_contract_to_hire";
                    break;
                case 4:
                    model = "home_infusion_therapy_nursing";
                    break;

            }



            return View("FindTalent",model);
        }
        [HttpGet]
        public IActionResult Login()
        {
         
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (_userManager.GetRolesAsync(user).Result.Contains(clrole))
                {
                    return RedirectToAction("Dashboard", clrole);
                }
                else
                {
                    return RedirectToAction("Dashboard", approle);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, [FromQuery] string ReturnUrl)
        {

            if (ModelState.IsValid)
            {

            //   HttpContext.Request.Query
          
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(model.UserName);

                }
                if (user != null)

                {
                   string offset= HttpContext.Session.GetString("timeoffset");
                    HttpContext.Session.Remove("timeoffset");
                    int timeoffset = 0;
                    Int32.TryParse(offset, out timeoffset);
                    user.TimeOffset = timeoffset;
                   await _userManager.UpdateAsync(user);
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        // if user is applicant, checking if other registration forms are filled
                        if (userRoles.Any(x => x == approle))
                        {
                            HttpContext.Session.SetString("user_ID", user.Id.ToString());
                            HttpContext.Session.SetString("role", approle);
                            Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                            if (applicant == null)
                            {
                                return RedirectToAction(approle + "Register", "Home");
                            }
                            else
                            {
                                await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
                             //   await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));
                                if (!String.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    switch (applicant.BoardingProcess)
                                    {
                                        case 0:
                                            return RedirectToAction("RegistrationSuccess", "Account");

                                        case 1:
                                            return RedirectToAction("ApplicantBoardingProcess", "Home");
                                        default:
                                            return RedirectToAction("Dashboard", "Applicant");
                                    }
                                }

                            }
                        }
                        else
                        // if user is clinicalInstitution, checking if registration form is filled
                        if (userRoles.Any(x => x == clrole))
                        {
                            ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                            if (clinical == null)
                            {
                                HttpContext.Session.SetString("user_ID", user.Id.ToString());
                                HttpContext.Session.SetString("role", clrole);
                                if (!String.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    
                                    return RedirectToAction(clrole + "Register", "Home");
                                }
                            }

                        }
                        else { return RedirectToAction("Register", "Home"); }
                        await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));
                        if (!String.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                          
                            return RedirectToAction("Dashboard", clrole);
                        }
                     
                    }
                    // if login failed
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password is not valid");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName is not valid");
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Register()
        {
            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            states.FirstOrDefault().Selected = true;
            ViewBag.States = states;
            
            var cities = new SelectList(_commonService.GetCities(3946), "id", "city_name");
            ViewBag.Cities = cities;
            return View();
        }
        public IActionResult Registeration(int id)
        {
            if(id==0)
            {
                TempData["user"] = "Applicant";
            }
            else
            {
                TempData["user"] = "Clinical";
            }
            return RedirectToAction("Register", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                 string  phonenumber = model.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                    phonenumber = "+1" + phonenumber;
                    //PhoneVerify phoneVerify = _commonService.GetVerificationKey(phonenumber);
                    //string keyID = HttpContext.Session.GetString("key").ToString();
                    //bool answer = phoneVerify != null && keyID != null ?
                    //                (phoneVerify.PhoneVerify_ID.ToString() == keyID && phoneVerify.IsVerified ? true : false)
                    //                : false;
                    if (true)
                    {
                        string Name = "";
                        if (model.Ishirer == 1)
                        {
                            Name = model.InstitutionName.Replace(";", ",") + ";" + model.ContactPerson.Replace(";", ",") + ";" + model.ContactTitle.Replace(";", ",");

                        }
                        else
                        {
                            Name = model.FirstName.Replace(";", ",") + ";" + model.LastName.Replace(";", ",") + ";" + model.MiddleName?.Replace(";", ",");
                        }

                        ApplicationUser user = new ApplicationUser
                        {
                            Email = model.Ishirer == 1?model.ContactEmailAddress: model.EmailAddress,
                            UserName = model.UserName,
                            Address = model.Address,
                            City_ID = model.City_ID,
                            PhoneNumber = "+1 " + model.Phone,
                            ZipCode = model.ZipCode,
                            isVerified = true,
                            Name = Name,
                            Address2 = model.Address2
                        };
                        // Adding role

                        string role = approle;
                        if (model.Ishirer == 1)
                        {
                            role = clrole;
                        }
                        if (model.Longitude == 0 || model.Latitude == 0)
                        {
                            if (!String.IsNullOrEmpty(model.Address))
                            {
                                string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                latlong latlong = _commonService.GetLatLongByAddress(query);
                                user.Latitude = latlong.Latitude;
                                user.Longitude = latlong.Longitude;
                            }
                            else
                            {
                                user.Latitude = model.Latitude;
                                user.Longitude = model.Longitude;
                            }
                        }
                        else
                        {
                            user.Latitude = model.Latitude;
                            user.Longitude = model.Longitude;
                            HttpContext.Session.Remove("counter");
                        }



                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {


                            await _userManager.AddToRoleAsync(user, role);
                            HttpContext.Session.Clear();
                            _commonService.RemoveVerificationKey(model.Phone);
                            // passing user_ID and role as session, for filling other registration forms    
                            HttpContext.Session.SetString("user_ID", user.Id.ToString());
                            HttpContext.Session.SetString("role", role);
                            return RedirectToAction(role + "Register", "Home");

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Please verify Phone Number");
                    }

                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Please try again");
                }

            }


            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            states.FirstOrDefault().Selected = true;
            ViewBag.States = states;

            var cities = new SelectList(_commonService.GetCities(3946), "id", "city_name");
            ViewBag.Cities = cities;
            return View(model);
        }


        [HttpGet]
        public IActionResult ApplicantRegister()
        {
            //Checking if user passed first registration form

            if (HttpContext.Session.GetString("user_ID") == null || HttpContext.Session.GetString("role") != approle )
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
            var certificateTypes = new SelectList(_commonService.GetCertificateTypes(), "Certificate_ID", "Certificate_TypeName");
            var visastatus = new SelectList(_commonService.GetVisaStatuses(), "VisaStatus_ID", "VisaStatus");

            var availabilites = new SelectList(_commonService.GetAvailabilities(), "Availability_ID", "Availability");
            ViewBag.Speciality = specialities;
            ViewBag.CertificateTypes = certificateTypes;
            ViewBag.VisaStatus = visastatus;
            ViewBag.Availability = availabilites;
            var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
            ViewBag.States = states;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplicantRegister(ApplicantModel model)
        {
            // Checking if user passed first registration form
            if (HttpContext.Session.GetString("user_ID") == null || HttpContext.Session.GetString("role") != approle)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string profilefilename = "";
                string resumefilename = "";
                List<ApplicantCertificates> applicantCertificates = new List<ApplicantCertificates>();
                var specialities = new SelectList(_commonService.GetSpecialities(), "Speciality_ID", "SpecialityName");
                var certificateTypes = new SelectList(_commonService.GetCertificateTypes(), "Certificate_ID", "Certificate_TypeName");
                var visastatus = new SelectList(_commonService.GetVisaStatuses(), "VisaStatus_ID", "VisaStatus");

                var availabilites = new SelectList(_commonService.GetAvailabilities(), "Availability_ID", "Availability");
                ViewBag.CertificateTypes = certificateTypes;
                ViewBag.Speciality = specialities;
                ViewBag.VisaStatus = visastatus;
                ViewBag.Availability = availabilites;
                var states = new SelectList(_commonService.GetStates(231), "id", "state_name");
                ViewBag.States = states;

                if (ModelState.IsValid)
                {
                    user_ID = HttpContext.Session.GetString("user_ID");


                    try
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(user_ID);

                        if (user != null && user.isVerified)
                        {
                            string firstname = user.Name;
                            string lastname = user.Name;
                            string middlename = user.Name;
                            string[] splitName = user.Name.Split(";");
                            if (splitName.Count() == 3)
                            {
                                firstname = splitName[0];
                                lastname = splitName[1];
                                middlename = splitName[2];
                            }

                            // uploading forms files 
                            if (model.certificates.Count > 0)
                            {
                                string subpath = "Certificates" + $@"\{user_ID}";

                                foreach (ApplicantCertificateViewModel certificate in model.certificates)
                                {
                                    if (certificate.CeritifcationImg != null && certificate.CertificateType != null)
                                    {
                                        string filename = Savefile(certificate.CeritifcationImg, subpath, certificate.CertificateType.ToString() + "_" + model.certificates.IndexOf(certificate).ToString());
                                        if (filename != "")
                                        {

                                            applicantCertificates.Add(new ApplicantCertificates() { CertificateTypes = certificate.CertificateType, CeritificationImg = filename, StartDate = certificate.StartDate, EndDate = certificate.EndDate });
                                        }
                                        else
                                        {
                                            ModelState.AddModelError(string.Empty, "Can't upload Certificates");
                                        }
                                    }

                                }
                            }
                            if (model.ProfileImage != null)
                            {
                                string path = "ApplicantImg";
                                profilefilename = Savefile(model.ProfileImage, path, user_ID);
                                if (profilefilename == "")
                                {
                                    ModelState.AddModelError(string.Empty, "Can't upload Profile picture");
                                }

                            }
                            if (model.Resume != null)
                            {
                                string path = "Resume";
                                resumefilename = Savefile(model.Resume, path, user_ID);
                                if (resumefilename == "")
                                {
                                    ModelState.AddModelError(string.Empty, "Can't upload Resume");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Resume is required");
                                return View(model);
                            }
                            Applicants applicant = new Applicants()
                            {
                                User_ID = Guid.Parse(user_ID),
                                Availability_ID = model.Availability_ID,
                                FirstName = firstname,
                                LastName = lastname,
                                MiddleName = middlename,
                                VisaStatus_ID = model.VisaStatus_ID,
                                //WorkAuth = model.WorkAuth,
                                //CEU = model.CEU,
                                ProfileImage = profilefilename,
                                IsEligible = model.IsEligible,
                                Resume = resumefilename

                            };

                            List<ApplicantCertificates> appcertificates = applicantCertificates;
                            List<ApplicantSpecialities> appspecialities = new List<ApplicantSpecialities>();
                            List<ApplicantWorkHistories> appworkHistories = new List<ApplicantWorkHistories>();

                            if (model.workHistories.Count > 0)
                            {
                                foreach (var workhistory in model.workHistories)
                                {

                                    if (workhistory.StartDate != null && workhistory.PlaceName != null && workhistory.JobTitle != null && (workhistory.UntilNow ? workhistory.UntilNow : (workhistory.EndDate != null)))
                                    {
                                        appworkHistories.Add(new ApplicantWorkHistories()
                                        {
                                            EndDate = workhistory.EndDate.GetValueOrDefault(),
                                            StartDate = workhistory.StartDate.Value,
                                            PlaceName = workhistory.PlaceName,
                                            JobTitle = workhistory.JobTitle
                                        });
                                    }

                                }
                            }

                            if (model.specialities.Count > 0)
                            {
                                foreach (var speciality in model.specialities)
                                {
                                    if (speciality != null)
                                    {
                                        appspecialities.Add(new ApplicantSpecialities()
                                        {
                                            Speciality_ID = speciality.Speciality_ID,
                                            License = speciality.License,
                                            LegabilityStates = String.Join("; ", speciality.LegabilityStates),
                                            IssueDate = speciality.IssueDate
                                        });
                                    }
                                }
                            }
                            bool addchanges = _commonService.SaveApplicant(applicant, appcertificates, appspecialities, appworkHistories.Count == 0 ? null : appworkHistories);
                            if (addchanges)
                            {
                                await _signInManager.SignInAsync(user, false);
                                ViewBag.Errortype = "success";
                                ViewBag.ErrorMessage = "Registration first step successfully finished";
                                return RedirectToAction("RegistrationSuccess", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Registration failed. Please try again");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Registration failed. PhoneNumber is not verified!");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                }
            }
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public IActionResult ApplicantBoardingProcess()
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Applicants applicant = _commonService.FindApplicantByUserID(userId);
            if (applicant != null)
            {
                if (applicant.BoardingProcess == 0)
                {
                    return RedirectToAction("RegistrationSuccess", "Account");
                }
                else if (applicant.BoardingProcess >= 2)
                {
                     
                        return RedirectToAction("Dashboard", "Applicant");
                  
                    //ApplicantAdditionalRegisterModel model = new ApplicantAdditionalRegisterModel()
                    //{
                    //    SSN = applicant.SSN,
                    //    TIN = applicant.TIN,
                    //};
                    //List<ApplicantReferences> references = _commonService.GetApplicantReferences(applicant.Applicant_ID);
                    //if (references.Count > 0)
                    //{
                    //    List<ApplicantReferencesViewModel> referencesViewModels = new List<ApplicantReferencesViewModel>();
                    //    foreach (var reference in references)
                    //    {
                    //        referencesViewModels.Add(new ApplicantReferencesViewModel()
                    //        {
                    //            Applicant_ID = applicant.Applicant_ID,
                    //            Company = reference.Company,
                    //            ContactName = reference.ContactName,
                    //            Email = reference.Email,
                    //            PhoneNumber = reference.PhoneNumber,
                    //            Position = reference.Position,
                    //        });
                    //    }
                    //    model.References = referencesViewModels;
                    //}
                    //return View(model);
                }
               
            }
            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public IActionResult ApplicantBoardingProcess(ApplicantAdditionalRegisterModel model)
        {

            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
         
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            var userRoles = _userManager.GetRolesAsync(user).Result;
            if (!userRoles.Any(x => x == approle))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string I9_file = "";
                    //string W4_file = "";
                 
                    try
                    {
                        Applicants applicant = _commonService.FindApplicantByUserID(userId);
                        if (applicant != null)
                        {
                            if (applicant.BoardingProcess == 1)
                            {
                                //if (model.I_9 != null)
                                //{
                                //    I9_file = Savefile(model.I_9, "I9", user_ID);
                                //    if (I9_file == "")
                                //    {
                                //        ModelState.AddModelError("I9", "Can't upload I9 file");
                                //    }
                                //}
                                //else
                                //{
                                //    ModelState.AddModelError("I9", "I9 is not uploaded");
                                //}
                                //if (model.W_4 != null)
                                //{
                                //    W4_file = Savefile(model.I_9, "W4", user_ID);
                                //    if (W4_file == "")
                                //    {
                                //        ModelState.AddModelError("W4", "Can't upload W4 file");
                                //    }
                                //}
                                //else
                                //{
                                //    ModelState.AddModelError("W4", "W4 is not uploaded");
                                //}
                                //applicant.W_4 = W4_file;
                                applicant.I_9 = I9_file;
                                // applicant.Sub_specialities = model.Sub_specialities;
                                //applicant.TIN = model.TIN != null ? model.TIN : null;
                                //applicant.SSN = model.SSN != null ? model.SSN : null;
                                applicant.BoardingProcess = 2;
                                List<ApplicantReferences> references = new List<ApplicantReferences>();
                                if (model.References != null)
                                {
                                    foreach (ApplicantReferencesViewModel reference in model.References)
                                    {
                                        references.Add(new ApplicantReferences()
                                        {
                                            Company = reference.Company == null ? " " : reference.Company,
                                            ContactName = reference.ContactName == null ? " " : reference.ContactName,
                                            Email = reference.Email == null ? " " : reference.Email,
                                            PhoneNumber = "+1 " + reference.PhoneNumber == null ? " " : "+1 " + reference.PhoneNumber,
                                            Position = reference.Position == null ? " " : reference.Position
                                        });
                                    }
                                }
                                bool addfileds = _commonService.AddBoardingProcessFileds(applicant, references);
                                if (addfileds)
                                {
                                    Notifications not = _commonService.GetUserNotifications(userId).Where(x => x.NotificationTemplate_ID == 2).FirstOrDefault();
                                    if (not != null)
                                    {
                                        _commonService.UncheckNotifications(not.Notification_ID);
                                    }
                                    Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
                                    string Subject = "";
                                    string Body = "";
                                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                                    string email = _userManager.GetEmailAsync(user).Result;
                                    string filecontent = GetHtmlStringFromPath("OnBoardingCompleted");
                                    Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                                    Body = filecontent.Replace(Subject, "");
                                    emailkeys.Add("{Name}", applicant.FirstName + " " + applicant.LastName);
                                   
                                    emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

                                    emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                                    emailkeys.Add("{AdminTitle}", administrator.Title);
                                    emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);

                                    _emailService.SendEmailAsync(email, Subject, Body, false);

                                    return RedirectToAction("Dashboard", "Applicant");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ClinicalInstitutionRegister()
        {
            //// Checking if user passed first registration form
            if (HttpContext.Session.GetString("user_ID") == null || HttpContext.Session.GetString("role") != clrole)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            var institutiontypes = new SelectList(_commonService.GetInstitutionTypes(), "InstitutionType_ID", "InstitutionTypeName");
            ViewBag.InstitutionTypes = institutiontypes;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClinicalInstitutionRegister(ClinicalInstitutionModel model)
        {
            // Checking if user passed first registration form
            if (HttpContext.Session.GetString("user_ID") == null || HttpContext.Session.GetString("role") != clrole)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var institutiontypes = new SelectList(_commonService.GetInstitutionTypes(), "InstitutionType_ID", "InstitutionTypeName");
                ViewBag.InstitutionTypes = institutiontypes;
                if (ModelState.IsValid)
                {
                    string logo = "";
                    user_ID = HttpContext.Session.GetString("user_ID");
                    

                    try
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(user_ID);
                        if (user != null && user.isVerified)
                        {
                            string name = user.Name;
                            string contactperson = user.Name;
                            string contacttitle = user.Name;
                            string[] splitName = user.Name.Split(";");
                            if (splitName.Count() == 3)
                            {
                                name = splitName[0];
                                contactperson = splitName[1];
                                contacttitle = splitName[2];
                            }
                            if (model.Logo != null)
                            {
                                logo = Savefile(model.Logo, "ClinicalLogo", user_ID);
                                if (logo == "")
                                {
                                    ModelState.AddModelError(string.Empty, "Can't upload logo, please try again.");
                                }

                            }
                            ClinicalInstitutions clinicalInstitution = new ClinicalInstitutions()
                            {
                                User_ID = Guid.Parse(user_ID),
                                InstitutionType_ID = model.InstitutionType_ID,
                                InstitutionName = name,
                                ContactPerson = contactperson,
                                ContactTitle = contacttitle,
                                InstitutionTaxId = "",
                                InstitutionDescription = model.InstitutionDescription,
                                Logo = logo,
                                Specialties=model.PreferredSpecialites
                            };
                           
                    
                            if (_commonService.SaveClinicalIntitution(clinicalInstitution))
                            {
                                await _signInManager.SignInAsync(user, false);
                                HttpContext.Session.Clear();
                                return RedirectToAction("RegistrationSuccess", "Account");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Registration failed. Please try again");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Registration failed. PhoneNumber is not verified!");

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Notifications()
        {
            NotificationsViewModel model = new NotificationsViewModel();
            if (HttpContext.User != null)
            {
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                var userRoles = _userManager.GetRolesAsync(user).Result;

                if (userId != null)
                {

                    List<Notifications> newnot = _commonService.GetUserNotifications(userId).ToList();
                    List<Notifications> oldnot = _commonService.GetUserOldNotifications(userId).ToList();
                    List<NotificationTemplateModel> newnotificationTemplates = new List<NotificationTemplateModel>();
                    List<NotificationTemplateModel> oldnotificationTemplates = new List<NotificationTemplateModel>();
                    if (newnot != null)
                    {
                        for (int i = 0; i < newnot.Count; i++)
                        {
                            NotificationTemplates temp = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == newnot[i].NotificationTemplate_ID).FirstOrDefault();
                            newnotificationTemplates.Add(new NotificationTemplateModel
                            {
                                Notification_ID = newnot[i].Notification_ID,
                                NotificationTemplate_ID = temp.NotificationTemplate_ID,
                                Notification_Action = temp.NotificationAction,
                                Notification_controller = temp.NotificationController,
                                Notification_Body = temp.NotificationTemplate_ID == 5 ? newnot[i].NotificationBody : temp.NotificationBody,
                                Notification_Title = temp.NotificationTemplate_ID == 5 ? newnot[i].NotificationTitle : temp.NotificationTitle

                            });
                            if (newnot[i].NotificationTemplate_ID == 5)
                            {
                                _commonService.RemoveNotifications(newnot[i].Notification_ID);
                            }
                        }



                    }
                    if (oldnot != null)
                    {
                        for (int i = 0; i < oldnot.Count; i++)
                        {
                            NotificationTemplates temp = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == oldnot[i].NotificationTemplate_ID).FirstOrDefault();
                            oldnotificationTemplates.Add(new NotificationTemplateModel
                            {
                                Notification_ID = oldnot[i].Notification_ID,
                                NotificationTemplate_ID = temp.NotificationTemplate_ID,
                                Notification_Action = temp.NotificationAction,
                                Notification_controller = temp.NotificationController,
                                Notification_Body = temp.NotificationTemplate_ID == 5 ? oldnot[i].NotificationBody : temp.NotificationBody,
                                Notification_Title = temp.NotificationTemplate_ID == 5 ? oldnot[i].NotificationTitle : temp.NotificationTitle

                            });
                        }

                    }
                    model.newNotifications = newnotificationTemplates;
                    model.oldNotifications = oldnotificationTemplates;
                }
            }
            return View(model);

        }

        #region AJAX
        [HttpPost]
        public JsonResult GetStates(int countryid)
        {
            return Json(_commonService.GetStates(countryid));
        }
        public JsonResult GetCities(int stateid)
        {
            return Json(_commonService.GetCities(stateid));
        }
        public JsonResult GetSpecalities()
        {
            return Json(_commonService.GetSpecialities());
        }
        public JsonResult GetCertificates()
        {
            return Json(_commonService.GetCertificateTypes());
        }
        public JsonResult CheckUserName(string UserName)
        {
            ApplicationUser user = _userManager.Users.Where(x => x.UserName == UserName).FirstOrDefault();
            return Json(user == null);
        }
        public JsonResult CheckEmail(string EmailAddress)
        {

            ApplicationUser user = _userManager.Users.Where(x => x.Email == EmailAddress).FirstOrDefault();
            return Json(user == null);
        }
        public JsonResult _checkEmail(string ContactEmailAddress)
        {

            ApplicationUser user = _userManager.Users.Where(x => x.Email == ContactEmailAddress).FirstOrDefault();
            return Json(user == null);
        }
        public JsonResult CheckZipCode(string ZipCode)
        {
            var zipcode = 0;
            if(Int32.TryParse(ZipCode, out zipcode))
            {
                if(zipcode>55000 && zipcode<56800)
                    return Json(true);
            }
            return Json(false);
        }
        public JsonResult SendSms(string phonenumber)
        {

            
              // phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
             //   bool answer = _commonService.SendPhoneVerificationCode(phonenumber);
                return Json(true);

        }
        public JsonResult VerifySms(string verifykey, string phonenumber)
        {
            //phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
            //phonenumber = "+1" + phonenumber;
            //PhoneVerify phoneVerify = _commonService.GetVerificationKey(phonenumber);

            //if (phoneVerify != null)
            //{
            //    if (phoneVerify.VerificationCode == verifykey && verifykey != null && verifykey != "")
            //    {
            //        phoneVerify.IsVerified = true;
            //        HttpContext.Session.SetString("key", phoneVerify.PhoneVerify_ID.ToString());
            //        if (_commonService.UpdateVerifiactionKey(phoneVerify))
            //        {
            //            return Json(true);
            //        }
            //    }
            //}
            return Json(true);
        }
        public JsonResult IsVerified(string phonenumber)
        {
            phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
            phonenumber = "+1" + phonenumber;
            PhoneVerify phoneVerify = _commonService.GetVerificationKey(phonenumber);
            string keyID = HttpContext.Session.GetString("key").ToString();
            bool answer = phoneVerify != null && keyID != null ?
                            (phoneVerify.PhoneVerify_ID.ToString() == keyID && phoneVerify.IsVerified ? true : false)
                            : false;
            if (answer)
            {

                return Json(true);
            }
            else
            {
                HttpContext.Session.Clear();
                _commonService.RemoveVerificationKey(phonenumber);
                return Json(false);
            }


        }
       public async  Task<JsonResult> GetTime(int time)
        {
            HttpContext.Session.SetString("timeoffset", time.ToString());
                return Json(true);
        }
        public JsonResult GetZipCode (int City_ID, string address)
        {
           string counter= HttpContext.Session.GetString("counter");
            int count= 0;
            if(!String.IsNullOrEmpty(counter))
            {
                Int32.TryParse(counter, out count);

            }
            if (count < 5)
            {
                if (City_ID > 0 && !String.IsNullOrEmpty(address))
                {
                    string query = address + "," + _commonService.GetCityName(City_ID);
                    Dictionary<string, string> Answer = new Dictionary<string, string>();
                    try
                    {
                        latlong latlong = _commonService.GetLatLongByAddress(query);
                        Answer.Add("latitude", latlong.Latitude.ToString());
                        Answer.Add("longitude", latlong.Longitude.ToString());
                        Answer.Add("zipcode", latlong.ZipCode.ToString());
                        HttpContext.Session.SetString("counter", (count + 1).ToString());
                        return Json(Answer);


                    }
                    catch { return Json(null); }
                }
            }
            return Json(null);
        }
        #endregion

        // Saving all type files in the specific path
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
