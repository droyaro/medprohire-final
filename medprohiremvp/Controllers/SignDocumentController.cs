using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using medprohiremvp.Models;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.DATA.IdentityModels;
using System.Linq;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using medprohiremvp.Service.SignSend;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using medprohiremvp.Service.EmailServices;

namespace medprohiremvp.Controllers
{
 
    [Authorize]
    public class SignDocumentController : Controller
    {
        private readonly ICommonServices _commonService;
        private readonly ISignature _signature;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailService _emailService;
        // Constants need to be set:
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RootPath _rootPath;
        private const string docName = "World_Wide_Corp_lorem.pdf";
        private string approle = "Applicant";

        public SignDocumentController(UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
        ICommonServices commonServices,
        ISignature signature, IHostingEnvironment environment, IOptions<RootPath> rootPath, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _signature = signature;
            _environment = environment;
            _rootPath = rootPath.Value;
            _emailService = emailService;
        }
        [HttpGet]
        public async Task<IActionResult> SignCompleted()
        {
            NotificationTemplates temp = new NotificationTemplates();
            string eventanswer = HttpContext.Request.Query["event"];
            string signsendedId = HttpContext.Request.Query["signId"];
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            Guid signid = Guid.Empty;
            Guid.TryParse(signsendedId, out signid);
            Administrators administrator = _commonService.GetAdministratorbyID(Guid.Empty);
            if (signid != Guid.Empty)
            {
                SignSent sign = new SignSent();
                sign = _commonService.GetSignSent(signid);


                if (eventanswer == "signing_complete" && sign.Status == "" && userId == sign.User_ID)
                {
                    
                    // answer get filepath, id download completed
                    var answer = _signature.Downloadsignfile(sign.Envelope_ID, sign.User_ID.ToString(), sign.FileType);
                    if (answer != String.Empty)
                    {
                        sign.Status = "downloaded";
                        sign.FilePath = answer;
                        _commonService.UpdateSignSended(sign);
                        ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                        var userRoles = _userManager.GetRolesAsync(user).Result;

                        if (userRoles.Any(x => x == approle))
                        {
                            Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                            switch (sign.FileType.Substring(0,sign.FileType.LastIndexOf('_')))
                            {
                                case "contract":
                                    applicant.Contract = answer;
                                    break;
                                case "1":
                                    applicant.Employment_agreement = answer;
                                    applicant.BoardingProcess = 3;
                                    break;

                            }
                            _commonService.UpdateApplicant(applicant);

                        }
                        int temp_id = 0;
                        Int32.TryParse(sign.FileType.Substring(0, sign.FileType.LastIndexOf('_')), out temp_id);
                        if (temp_id != 0)
                        {
                         temp= _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == temp_id).FirstOrDefault();
                           Notifications not = _commonService.GetUserNotifications(userId).Where(x => x.NotificationTemplate_ID == temp_id).FirstOrDefault();
                            if(not.NotificationTemplate_ID==1)
                            {
                                string firstname = "";
                                string lastname = "";
                                string[] name = user.Name.Split(";");
                                if (name.Count() == 3)
                                {
                                    firstname = name[0];
                                    lastname = name[1];
                                }

                                string Subject = "";
                                string Body = "";
                                Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                                string filecontent = GetHtmlStringFromPath("SignEmploymentAgreement");
                                Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                                Body = filecontent.Replace(Subject, "");
                                emailkeys.Add("{Name}", firstname+" "+lastname);
                                emailkeys.Add("{ReturnUrl}", Url.Action( "AvailableShifts", "Applicant", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                                emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);

                                emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                                emailkeys.Add("{AdminTitle}", administrator.Title);
                                emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                               
                                foreach (var key in emailkeys)
                                {
                                    Body = Body.Replace(key.Key, key.Value);
                                    Subject = Subject.Replace(key.Key, key.Value);
                                }

                                await _emailService.SendEmailAsync(user.Email, Subject, Body, true);
                            }
                           
                            if(not!=null)
                            {
                                _commonService.UncheckNotifications(not.Notification_ID);
                            }
                       }
                    }
                }
            }
            return View(temp);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Sign()
        {
            SignModel model = new SignModel();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            model.EmailAddress = user.Email;
            var userRoles = _userManager.GetRolesAsync(user).Result;

            if (userRoles.Any(x => x == approle))
            {
                Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                model.LastName = applicant.LastName + " " + applicant.FirstName;


            }
            else
            {
                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                model.LastName = clinical.ContactPerson;

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Sign(SignModel model)
        {
            if (ModelState.IsValid)
            {
             
                Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
                var callbackUrl = Url.Action("SignCompleted", "SignDocument", new { }, Request.Scheme);
                string url = _signature.Geturlsignature("contract", Path.Combine(_environment.WebRootPath, "Upload") + "\\Contract.pdf", model.EmailAddress, model.LastName, userId, callbackUrl, 90, 260, 5,null,null,null);
                if (url != "")
                {
                    return Redirect(url);
                }

            }
            return View(model);

        }

        [HttpGet]
        [Authorize]
        public IActionResult GeneratePdf()
        {
            SignModel model = new SignModel();
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            model.EmailAddress = user.Email;
            var userRoles = _userManager.GetRolesAsync(user).Result;

            if (userRoles.Any(x => x == approle))
            {
                Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                model.LastName = applicant.LastName + " " + applicant.FirstName;


            }
            else
            {
                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                model.LastName = clinical.ContactPerson;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> GeneratePdf(SignModel model)
        {

            int xposition;
            int yposition;
            int pagenumber;
            Guid userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));
            PdfReader pdfReader = new PdfReader(Path.Combine(_environment.WebRootPath, "Upload") + "\\template.pdf");
            using (FileStream os = new FileStream(Path.Combine(_environment.WebRootPath, "Upload") + "\\Contract"+userId+".pdf", FileMode.Create))
            {
                PdfStamper stamper = new PdfStamper(pdfReader, os);

                AcroFields fields = stamper.AcroFields;
                fields.SetField("name", model.LastName);
                fields.SetField("day", DateTime.Now.ToString("dd"));
                fields.SetField("month", DateTime.Now.ToString("MMMM"));
                fields.SetField("year", DateTime.Now.ToString("yy"));
                float [] signposition = fields.GetFieldPositions("sign_es_:signer:signature");
                Rectangle rectangle= pdfReader.GetPageSize(Convert.ToInt32(signposition[0]));
                yposition = Convert.ToInt32(rectangle.Top - signposition[4] - (signposition[4] - signposition[2]));
                xposition= Convert.ToInt32(signposition[1] + ((signposition[3] - signposition[1])/2));
                pagenumber = Convert.ToInt32(signposition[0]);
                stamper.Close();
                pdfReader.Close();
            }
         
            var callbackUrl = Url.Action("SignCompleted", "SignDocument", new { }, Request.Scheme);
            string url = _signature.Geturlsignature("contract", Path.Combine(_environment.WebRootPath, "Upload") + "\\Contract" + userId + ".pdf", model.EmailAddress, model.LastName, userId, callbackUrl, xposition,yposition, pagenumber,null,null,null);

            return Redirect(url);


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
