using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedProHireAPI.Models.Account;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account/[Action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly RootPath _rootPath;
        private readonly IEmailService _emailService;
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices, IOptions<RootPath> rootPath, IEmailService emailService
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _emailService = emailService;
            _rootPath = rootPath.Value;
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromHeader] string ApiKey, [FromBody]ChangePasswordModel model, [FromBody] string User_ID)
        {

            if (ModelState.IsValid)
            {
                Guid Api = Guid.Empty;
                if (!String.IsNullOrEmpty(ApiKey))
                {
                    Guid.TryParse(ApiKey, out Api);
                }
                var apiAnswer = _commonService.CheckFullApiKey(Api);
                if (apiAnswer)
                {
                    Guid user_ID = Guid.Empty;
                    if (!String.IsNullOrEmpty(User_ID))
                    {
                        Guid.TryParse(User_ID, out user_ID);
                    }
                    ApplicationUser user =  _userManager.Users.Where(x=>x.Id==user_ID).FirstOrDefault();
                    var role = _userManager.GetRolesAsync(user).Result;
                    string rolename = role[0];
                    if (user != null)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
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
                            return Ok();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty,string.Join("; ", result.Errors.Select(x => x.Description).ToList()));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User is null");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Api Key is not valid");
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult SendVerificationCode([FromHeader] string ApiKey, [FromBody]string phonenumber, [FromBody] string User_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                Guid user_ID = Guid.Empty;
                if (!String.IsNullOrEmpty(User_ID))
                {
                    Guid.TryParse(User_ID, out user_ID);
                }
                ApplicationUser user = _userManager.Users.Where(x=>x.Id== user_ID).FirstOrDefault();
                if (user != null)
                {
                    if (!user.PhoneNumber.Trim().Equals(phonenumber.Trim()))
                    {

                        bool answer = _commonService.SendPhoneVerificationCode(phonenumber);
                        if (answer)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest("Can't send sms");
                        }
                    }
                    else
                    {
                         ModelState.AddModelError("","Phone Number is the same");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User is null");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult VerifyAndSavePhoneNumber([FromHeader] string ApiKey, [FromBody]string verifykey, [FromBody]string phonenumber, [FromBody] string User_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                Guid user_ID = Guid.Empty;
                if (!String.IsNullOrEmpty(User_ID))
                {
                    Guid.TryParse(User_ID, out user_ID);
                }
                ApplicationUser user = _userManager.Users.Where(x => x.Id == user_ID).FirstOrDefault();
                if (user != null)
                {
                    phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                    phonenumber = "+1" + phonenumber;
                    PhoneVerify phoneVerify = _commonService.GetVerificationKey(phonenumber);

                    if (phoneVerify != null)
                    {
                        if (phoneVerify.VerificationCode == verifykey && verifykey != null && verifykey != "")
                        {
                            phoneVerify.IsVerified = true;
                                user.PhoneNumber = phonenumber;
                                user.isVerified = true;
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
                            var answer = _userManager.UpdateAsync(user).Result;
                            if (answer.Succeeded)
                            {
                                return Ok();
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, string.Join("; ", answer.Errors.Select(x => x.Description).ToList()));
                            }
                            


                        }
                        else
                        {
                            ModelState.AddModelError("", "Verification Key is not valid");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is no verification");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User is null");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult Support([FromHeader] string ApiKey, string User_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Guid user_ID = Guid.Empty;
                if (!String.IsNullOrEmpty(User_ID))
                {
                    Guid.TryParse(User_ID, out user_ID);
                }
                ApplicationUser user = _userManager.Users.Where(x => x.Id == user_ID).FirstOrDefault();
                if (user != null)
                {
                    
                    bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                    List<Tickets> tickets = _commonService.GetUserTickets(user.Id);
                    List<TicketModel> model = new List<TicketModel>();
                    if (tickets != null)
                    {
                        foreach (Tickets ticket in tickets)
                        {
                            TicketCategories categories = _commonService.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault();
                            model.Add(new TicketModel
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
                           List <TicketContents> ticketContents = _commonService.GetTicketContents(ticket.Ticket_ID);
                          if(ticketContents!=null)
                            {
                                foreach(TicketContents ticketContent in ticketContents)
                                {
                                    ApplicationUser contentuser = _userManager.Users.Where(x => x.Id == ticketContent.User_ID).FirstOrDefault();
                                    string UserName = "";
                                    if (contentuser != null)
                                    {
                                        var roles = _userManager.GetRolesAsync(contentuser).Result;
                                        if (roles.Contains("Applicant"))
                                        {
                                            Applicants app = _commonService.FindApplicantByUserID(contentuser.Id);
                                            UserName = app.LastName + " " + app.FirstName;
                                        }
                                        else
                                        {
                                            if (roles.Contains("ClinicalInstituition"))
                                            {
                                                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(contentuser.Id);
                                                UserName = clinical.ContactPerson;
                                            }
                                            else
                                            {
                                                Administrators admin = _commonService.GetAdministratorbyID(contentuser.Id);
                                                if (admin != null)
                                                {
                                                    UserName = admin.LastName + " " + admin.FirstName;
                                                }
                                            }
                                        }
                                    }
                                    model.Last().TicketContents = new List<TicketContentModel>();
                                    model.Last().TicketContents.Add(new TicketContentModel {
                                         InsertDate=ticketContent.InsertDate,
                                          Rate=ticketContent.Rate,
                                           TicketContent=ticketContent.TicketContent,
                                            TicketContent_ID=ticketContent.TicketContent_ID,
                                             Ticket_ID=ticketContent.Ticket_ID,
                                              UserName=UserName,
                                               User_ID=ticketContent.User_ID
                                    });
                                }
                            }
                        }
                    }
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Can't find User");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult TicketContent([FromHeader] string ApiKey, int Ticket_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
              
                  
                    Tickets ticket = _commonService.GetTicketByID(Ticket_ID);
           

                    if (ticket != null)
                    {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == ticket.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                        TicketCategories categories = _commonService.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault();
                      TicketModel  model=new TicketModel()
                        {
                            Answer = "",
                            DateCreated = ticket.DateCreated.GetValueOrDefault(),
                            DateModified = ticket.DateModified.GetValueOrDefault(),
                            Subject = ticket.Subject,
                            TicketCategory_ID = ticket.TicketCategory_ID,
                            TicketCategoryName = categories != null ? categories.TicketCategoryName : "",
                            TicketType = ticket.TicketType,
                            Ticket_ID = ticket.Ticket_ID

                        };
                        List<TicketContents> ticketContents = _commonService.GetTicketContents(ticket.Ticket_ID);
                        if (ticketContents != null)
                        {
                            foreach (TicketContents ticketContent in ticketContents)
                            {
                                ApplicationUser contentuser = _userManager.Users.Where(x => x.Id == ticketContent.User_ID).FirstOrDefault();
                                string UserName = "";
                                if (contentuser != null)
                                {
                                    var roles = _userManager.GetRolesAsync(contentuser).Result;
                                    if (roles.Contains("Applicant"))
                                    {
                                        Applicants app = _commonService.FindApplicantByUserID(contentuser.Id);
                                        UserName = app.LastName + " " + app.FirstName;
                                    }
                                    else
                                    {
                                        if (roles.Contains("ClinicalInstituition"))
                                        {
                                            ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(contentuser.Id);
                                            UserName = clinical.ContactPerson;
                                        }
                                        else
                                        {
                                            Administrators admin = _commonService.GetAdministratorbyID(contentuser.Id);
                                            if (admin != null)
                                            {
                                                UserName = admin.LastName + " " + admin.FirstName;
                                            }
                                        }
                                    }
                                }
                                model.TicketContents = new List<TicketContentModel>();
                                model.TicketContents.Add(new TicketContentModel
                                {
                                    InsertDate = ticketContent.InsertDate,
                                    Rate = ticketContent.Rate,
                                    TicketContent = ticketContent.TicketContent,
                                    TicketContent_ID = ticketContent.TicketContent_ID,
                                    Ticket_ID = ticketContent.Ticket_ID,
                                    UserName = UserName,
                                    User_ID = ticketContent.User_ID
                                });
                            }
                        }
                        return Ok(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't find User");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Can't find Ticket");
                }

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult AddTicketContent([FromHeader] string ApiKey, [FromBody]TicketContentModel ticketContent)
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
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == ticketContent.User_ID).FirstOrDefault();
                    if(user!=null)
                    {
                        Tickets ticket = _commonService.GetTicketByID(ticketContent.Ticket_ID);
                        if(ticket!=null)
                        {
                            int offset = user.TimeOffset;
                            TimeSpan timespanoffset = TimeSpan.FromMinutes(-(offset));
                            DateTime timenow = DateTime.UtcNow.Add(timespanoffset);
                            ticketContent.InsertDate = timenow;
                            ticket.TicketType = 2;
                            TicketContents content = new TicketContents(){
                                 TicketContent=ticketContent.TicketContent,
                                  InsertDate=timenow,
                                   Ticket_ID=ticket.Ticket_ID,
                                    User_ID=ticketContent.User_ID,
                                     Rate=ticketContent.Rate,
                                     
                            };
                           
                            var answer = _commonService.AddTicketContent(content, ticket);
                            if(answer)
                            {
                                return Ok();
                            }
                            else
                            {
                                ModelState.AddModelError("", "Can't save Ticket Content");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't find Ticket");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't find User");
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
        public IActionResult NewTicket ([FromHeader] string ApiKey, [FromBody]NewTicketModel model)
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
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == model.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        TimeSpan timespanoffset = TimeSpan.FromMinutes(-(user.TimeOffset));
                        DateTime timenow = DateTime.UtcNow.Add(timespanoffset);
                        Tickets ticket = new Tickets()
                        {
                            TicketCategory_ID = model.TicketCategory_ID,
                            Subject = model.Subject,
                            DateCreated = timenow,
                            TicketType = 0,
                            User_ID = user.Id,
                            DateModified = timenow,


                        };
                        TicketContents contents = new TicketContents();
                        contents.User_ID = user.Id;
                        contents.TicketContent = model.Answer;
                        contents.InsertDate = timenow;
                        var answer = _commonService.AddTicket(ticket, contents);
                        if (answer)
                        {
                            return Ok();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't Add Ticket");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User ID is not valid");
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
       public IActionResult Notifications([FromHeader] string ApiKey, string User_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                Guid user_ID = Guid.Empty;
                if (!String.IsNullOrEmpty(User_ID))
                {
                    Guid.TryParse(User_ID, out user_ID);
                }
                ApplicationUser user = _userManager.Users.Where(x => x.Id == user_ID).FirstOrDefault();
                if (user != null)
                {
                    List<NotificationModel> notifications = new List<NotificationModel>();
                    List<Notifications> newnotifications = _commonService.GetUserNotifications(user.Id);
                    if(newnotifications!=null)
                    {
                        foreach (Notifications newnotification in newnotifications)
                        {
                            NotificationTemplates temp = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == newnotification.NotificationTemplate_ID).FirstOrDefault();
                            notifications.Add(new NotificationModel()
                            {
                                IsNew = true,
                                Notification_ID = newnotification.Notification_ID,
                                NotificationTemplate_ID = newnotification.NotificationTemplate_ID,
                                Notification_Action = temp.NotificationAction,
                                Notification_Controller = temp.NotificationController,
                                Notification_Body = temp.NotificationTemplate_ID == 5 ? newnotification.NotificationBody : temp.NotificationBody,
                                Notification_Title = temp.NotificationTemplate_ID == 5 ? newnotification.NotificationTitle : temp.NotificationTitle,
                                  
                            });
                        }
                    }
                    List<Notifications> oldnotifications = _commonService.GetUserOldNotifications(user.Id);
                    if (oldnotifications != null)
                    {
                        foreach (Notifications oldnotification in oldnotifications)
                        {
                            NotificationTemplates temp = _commonService.GetNotification_Templates().Where(x => x.NotificationTemplate_ID == oldnotification.NotificationTemplate_ID).FirstOrDefault();
                            notifications.Add(new NotificationModel()
                            {
                                IsNew = false,
                                Notification_ID = oldnotification.Notification_ID,
                                NotificationTemplate_ID = oldnotification.NotificationTemplate_ID,
                                Notification_Action = temp.NotificationAction,
                                Notification_Controller = temp.NotificationController,
                                Notification_Body = temp.NotificationTemplate_ID == 5 ? oldnotification.NotificationBody : temp.NotificationBody,
                                Notification_Title = temp.NotificationTemplate_ID == 5 ? oldnotification.NotificationTitle : temp.NotificationTitle,

                            });
                        }
                    }
                    return Ok(notifications);

                }
                else
                {
                    ModelState.AddModelError("", "Can't find User");
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