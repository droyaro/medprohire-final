using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using medprohiremvp.Models.Account;
using medprohiremvp.DATA.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.Service;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Models.Applicant;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace medprohiremvp.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ICommonServices _commonServices;
        private readonly IHostingEnvironment _environment;
        private readonly RootPath _rootPath;
        public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IEmailService emailService, ICommonServices commonServices, IHostingEnvironment environment, IOptions<RootPath> rootPath)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _commonServices = commonServices;
            _environment = environment;
            _rootPath = rootPath.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, code = code }, protocol: Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking this: <a href='{callbackUrl}'>link</a>", true);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrationSuccess()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    Administrators administrator = _commonServices.GetAdministratorbyID(Guid.Empty);
                    string Subject = "";
                    string Body ="";
                    Dictionary<string, string> emailkeys = new Dictionary<string, string>();
                     // If User is Applicant, sending email with welcome text to Applicant
                    if (roles.Contains("Applicant"))
                    {
                        string firstname = "";
                        string lastname = "";
                        string[] name = user.Name.Split(";");
                        if (name.Count() == 3)
                        {
                            firstname = name[0];
                            lastname = name[1];
                        }
                        string filecontent = GetHtmlStringFromPath("ApplicantRegister");
                         Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                         Body = filecontent.Replace(Subject, "");
                        emailkeys.Add("{Name}", firstname+" "+lastname);
                        emailkeys.Add("{ReturnUrl}", Url.Action("Profile", "Applicant", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                        emailkeys.Add("{ReleasedLink}",_rootPath.Type+"://" +_rootPath.ReleasedLink);
                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress); 
                    }
                    // Else If User is Client, sending email with welcome text to Client
                    else
                    {
                        string name = "";
                        string[] names = user.Name.Split(";");
                        if (names.Count() == 3)
                        {
                            name = names[0];

                        }
                        string filecontent = GetHtmlStringFromPath("ClinicalInstitutionRegister");
                        Subject = filecontent.Substring(0, filecontent.IndexOf(Environment.NewLine));
                        Body = filecontent.Replace(Subject, "");
                        emailkeys.Add("{Name}", name);
                        emailkeys.Add("{ReturnUrl}", Url.Action("Profile", "ClinicalInstitution", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                        emailkeys.Add("{ShiftUrl}", Url.Action("NewClientShift", "ClinicalInstitution", values: null, protocol: _rootPath.Type, host: _rootPath.ReleasedLink));
                        emailkeys.Add("{ReleasedLink}", _rootPath.Type + "://" + _rootPath.ReleasedLink);
                        emailkeys.Add("{AdminName}", administrator.LastName + " " + administrator.FirstName);
                        emailkeys.Add("{AdminTitle}", administrator.Title);
                        emailkeys.Add("{AdminEmailAddress}", administrator.EmailAddress);
                    }
                    foreach (var key in emailkeys)
                    {
                        Body = Body.Replace(key.Key, key.Value);
                        Subject = Subject.Replace(key.Key, key.Value);
                    }
                    await _emailService.SendEmailAsync(user.Email, Subject, Body, true);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            TempData["changeanswer"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                var role = _userManager.GetRolesAsync(user).Result;
                string rolename = role[0];
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.Password.OldPassword, model.Password.NewPassword);
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
                   await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        //This tempdata using for redirecting view page to Change Password Tab, and if(answer is not "ok" -> showing massage with errors)
                        TempData["changeanswer"] = "ok";
                    }
                    else
                    {
                        TempData["changeanswer"] = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                    }
                }
                return RedirectToAction("Profile", rolename);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Support()
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                List<Tickets> tickets = _commonServices.GetUserTickets(user.Id);
                List<TicketViewModel> model = new List<TicketViewModel>();
                if (tickets != null)
                {
                    foreach (Tickets ticket in tickets)
                    {
                        TicketCategories categories = _commonServices.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault();
                        model.Add(new TicketViewModel
                        {
                            Answer = "",
                            DateCreated = ticket.DateCreated.GetValueOrDefault(),
                            DateModified = ticket.DateModified.GetValueOrDefault(),
                            Subject = ticket.Subject,
                            TicketCategory_ID = ticket.TicketCategory_ID,
                            TicketCategoryName = categories != null ? categories.TicketCategoryName : "",
                            TicketType = ticket.TicketType,
                            Ticket_ID = ticket.Ticket_ID
                        });
                    }
                }
                return View(model);
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult NewTicket()
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                var ticketCategories = new SelectList(_commonServices.GetTicketCategories(IsClient), "TicketCategory_ID", "TicketCategoryName");
                ViewBag.TicketCategories = ticketCategories;
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult NewTicket(TicketViewModel model)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null )
            {
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                var ticketCategories = new SelectList(_commonServices.GetTicketCategories(IsClient), "TicketCategory_ID", "TicketCategoryName");
                ViewBag.TicketCategories = ticketCategories;
                if (ModelState.IsValid)
                {
                    int offset = user.TimeOffset;
                    TimeSpan timespanoffset = TimeSpan.FromMinutes(-(offset));
                    DateTime timenow = DateTime.UtcNow.Add(timespanoffset);
                    Tickets tickets = new Tickets();
                    TicketContents contents = new TicketContents();
                    tickets.Subject = model.Subject;
                    tickets.TicketCategory_ID = model.TicketCategory_ID;
                    tickets.TicketType = 0;
                    tickets.User_ID = user.Id;
                    contents.InsertDate = timenow;
                    contents.TicketContent = model.Answer;
                    contents.User_ID = user.Id;
                    _commonServices.AddTicket(tickets, contents);
                    return RedirectToAction("Support", "Account");
                }
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult TicketContent(int Ticket_ID)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                // There is difference ticket content for client and applicant, tha't why we are checking is authorized user client or not
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                Tickets ticket = _commonServices.GetUserTickets(user.Id).Where(x => x.Ticket_ID == Ticket_ID).FirstOrDefault();
                TicketViewModel model = new TicketViewModel();
                if (ticket != null)
                {
                    // reading ticket information
                    TicketCategories categories = _commonServices.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault();
                    model.Answer = "";
                    model.DateCreated = ticket.DateCreated.GetValueOrDefault();
                    model.DateModified = ticket.DateModified.GetValueOrDefault();
                    model.Subject = ticket.Subject;
                    model.TicketCategory_ID = ticket.TicketCategory_ID;
                    model.TicketCategoryName = categories != null ? categories.TicketCategoryName : "";
                    model.TicketType = ticket.TicketType;
                    model.Ticket_ID = ticket.Ticket_ID;
                    List<TicketContents> contents = _commonServices.GetTicketContents(ticket.Ticket_ID);
                    model.TicketContents = new List<TicketContentViewModel>();
                    if (contents != null)
                    {
                        //reading ticket content
                        foreach (TicketContents content in contents)
                        {
                            ApplicationUser contentuser = _userManager.Users.Where(x => x.Id == content.User_ID).FirstOrDefault();
                            string UserName = "";
                            if (contentuser != null)
                            {
                                var roles = _userManager.GetRolesAsync(contentuser).Result;
                                if (roles.Contains("Applicant"))
                                {
                                    Applicants app = _commonServices.FindApplicantByUserID(contentuser.Id);
                                    UserName = app.LastName + " " + app.FirstName;
                                }
                                else
                                {
                                    if (roles.Contains("ClinicalInstituition"))
                                    {
                                        ClinicalInstitutions clinical = _commonServices.FindClinicaByUserID(contentuser.Id);
                                        UserName = clinical.ContactPerson;
                                    }
                                    else
                                    {
                                        Administrators admin = _commonServices.GetAdministratorbyID(contentuser.Id);
                                        if (admin != null)
                                        {
                                            UserName = admin.LastName + " " + admin.FirstName;
                                        }
                                    }
                                }
                            }
                            model.TicketContents.Add(new TicketContentViewModel
                            {
                                InsertDate = content.InsertDate,
                                TicketContent = content.TicketContent,
                                TicketContent_ID = content.TicketContent_ID,
                                Ticket_ID = content.Ticket_ID,
                                User_ID = content.User_ID,
                                UserName = UserName,
                                Rate = content.Rate
                            });
                        }
                        model.TicketContents = model.TicketContents.OrderByDescending(x => x.InsertDate).ToList();
                    }
                }
                return View(model);
            }
            return RedirectToAction("Support", "Account");
        }
        public JsonResult RateTicketContent(int id, int rate)
        {
            TicketContents ticketContent = _commonServices.GetTicketContentByID(id);
            ticketContent.Rate = rate;
            if (_commonServices.UpdateTicketContent(ticketContent))
            {
                return Json(true);
            }
            else
            {
                return Json(null);
            }
        }
        public JsonResult AddContent(int id, string content)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                
                int offset = user.TimeOffset;
                TimeSpan timespanoffset = TimeSpan.FromMinutes(-(offset));
                DateTime timenow = DateTime.UtcNow.Add(timespanoffset);
                TicketContents ticketContent = new TicketContents();
                ticketContent.InsertDate = timenow;
                ticketContent.TicketContent = content;
                ticketContent.Ticket_ID = id;
                ticketContent.User_ID = user.Id;
                Tickets ticket = _commonServices.GetTicketByID(id);
                ticket.TicketType = 2;
                if (ticket != null)
                {
                    if (_commonServices.AddTicketContent(ticketContent,ticket))
                    {
                        return Json(true);
                    }
                } 
            }      
                return Json(null);
        }
        public JsonResult SendSms(string phonenumber)
        {
            ////clearing phone number from symbols
            //phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
            //ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            //if (user != null)
            //{
            //    if (!user.PhoneNumber.Trim().Equals(phonenumber.Trim()))
            //    {
            //        bool answer = _commonServices.SendPhoneVerificationCode(phonenumber);
            //        return Json(answer);
            //    }
            //}
            return Json(true);
        }
        public JsonResult VerifySms(string verifykey, string phonenumber)
        {
            ////clearing phone number from symbols
            //phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
            ////adding usa phone code
            //phonenumber = "+1" + phonenumber;
            ////reading verification code from database
            //PhoneVerify phoneVerify = _commonServices.GetVerificationKey(phonenumber);
            ////checking if answer of database reading is not empty
            //if (phoneVerify != null)
            //{
            //    // checking if verification code is equal
            //    if (phoneVerify.VerificationCode == verifykey && verifykey != null && verifykey != "")
            //    {
            //        phoneVerify.IsVerified = true;
            //        ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            //        if (user != null)
            //        {
            //            //updating user, and writing new phone number
            //            user.PhoneNumber = phonenumber;
            //            user.isVerified = true;
            //            if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
            //            {
            //                if (user.ChangesCount >= 9)
            //                {
            //                    user.ChangesLocked = true;
            //                }
            //                user.ChangesMakedTime = DateTime.Now;
            //                user.ChangesCount = user.ChangesCount + 1;
            //            }
            //            else
            //            {
            //                user.ChangesMakedTime = DateTime.Now;
            //                user.ChangesCount = 1;
            //                user.ChangesLocked = false;
            //            }
            //            //updateing user
            //            var answer = _userManager.UpdateAsync(user).Result;
            //            if (answer.Succeeded)
            //            {
            //                return Json(true);
            //            }
            //        }   
            //    }
            //}
            return Json(true);
        }
        public JsonResult CompleteTicket(int ticket_id)
        {
            if (ticket_id > 0)
            {
                Tickets ticket = _commonServices.GetTicketByID(ticket_id);
                if (ticket != null)
                {
                    ticket.TicketType = 3;
                }
                bool answer = _commonServices.UpdateTicket(ticket);
                if (answer)
                {
                    return Json(true);
                }
            }
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