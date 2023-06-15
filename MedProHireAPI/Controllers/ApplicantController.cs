using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geolocation;
using MedProHireAPI.Models.Applicant;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.IServices;
using medprohiremvp.Service.SignSend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Applicant/[Action]")]
    public class ApplicantController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly RootPath _rootPath;
        private readonly ISignature _signature;
        private readonly IEmailService _emailService;
        private readonly string savefiletypeprefix = "usersign";
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public ApplicantController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices, IOptions<RootPath> rootPath, IEmailService emailService, ISignature signature
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _emailService = emailService;
            _rootPath = rootPath.Value;
            _signature = signature;
        }
        [HttpPost]
        public IActionResult ApplicantBoardingProcess([FromHeader] string ApiKey, [FromBody]ApplicantBoardingProcessModel model)
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


                    try
                    {
                        Applicants applicant = _commonService.FindApplicantByUserID(model.User_ID);
                        if (applicant != null)
                        {
                            if (applicant.BoardingProcess == 1)
                            {


                                applicant.TIN = model.TIN != null ? model.TIN : null;
                                applicant.SSN = model.SSN != null ? model.SSN : null;
                                applicant.BoardingProcess = 2;
                                List<ApplicantReferences> references = new List<ApplicantReferences>();
                                if (model.References != null)
                                {
                                    foreach (ApplicantReferenceModel reference in model.References)
                                    {
                                        references.Add(new ApplicantReferences()
                                        {
                                            Company = reference.Company,
                                            ContactName = reference.ContactName,
                                            Email = reference.Email,
                                            PhoneNumber = "+1 " + reference.PhoneNumber,
                                            Position = reference.Position,
                                            Applicant_ID = applicant.Applicant_ID,

                                        });
                                    }
                                }
                                bool addfileds = _commonService.AddBoardingProcessFileds(applicant, references);
                                if (addfileds)
                                {
                                    Notifications not = _commonService.GetUserNotifications(model.User_ID).Where(x => x.NotificationTemplate_ID == 2).FirstOrDefault();
                                    if (not != null)
                                    {
                                        _commonService.UncheckNotifications(not.Notification_ID);
                                    }
                                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                                    string email = _userManager.GetEmailAsync(user).Result;
                                    string subject = "Boarding Process";
                                    string message = "Thank you for registration";
                                    _emailService.SendEmailAsync(email, subject, message, false);

                                    return Ok("AppRegistrationSuccess");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Applicant onboading proccess is not activated. Status "+applicant.BoardingProcess.ToString());
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Applicant is not valid");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
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
        public IActionResult EmploymentAgreement([FromHeader] string ApiKey,  int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                string FilePath = "";
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (applicant != null)
                {
                    if (applicant.Employment_agreement == null)
                    {
                        SignSent signsend = _commonService.GetEmploymentAgreementFile(applicant.User_ID);
                        if (signsend.FilePath != null)

                        {
                            return Ok(new string[] { "Signed by Admin", _rootPath.AdminRoot+ signsend.FilePath });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Didn't Signed");
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return Ok(new string [] { "Signed by User", _rootPath.UserRoot+ applicant.Employment_agreement });
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Can't find Applicant");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
                return BadRequest(ModelState);
            }
           
        }
        [HttpPost]

        public IActionResult SignEmploymentAgreement([FromHeader] string ApiKey, int Applicant_ID)

        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                string FilePath = "";
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                if (applicant != null)
                {

                    if (applicant.Employment_agreement == null)
                    {
                        ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                        if (user != null)
                        {
                            SignSent signsend = _commonService.GetEmploymentAgreementFile(applicant.User_ID);

                            if (signsend.FilePath != null)
                            {
                                string file = _rootPath.AdminRoot + signsend.FilePath;
                                var callbackUrl = Url.Action("SignCompleted", "SignDocument", new { }, Request.Scheme);
                                string url = _signature.Geturlsignature("1_" + savefiletypeprefix, file, user.Email, applicant.LastName + " " + applicant.FirstName, applicant.User_ID, callbackUrl, signsend.Emp_XPosition, signsend.Emp_YPosition, signsend.Emp_PageNumber, signsend.Emp_XPosition, signsend.Emp_YPosition, signsend.Emp_PageNumber);
                                return Ok(url);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Can't find file signed by admin");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't find User");
                          
                        }
                    }
                    else

                    {

                        string filepath = _rootPath.UserRoot + applicant.Employment_agreement;
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                        return Ok(File(fileBytes, "application/x-msdownload", "Employment_agreement.pdf"));
                    }

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
     
          
    }
}