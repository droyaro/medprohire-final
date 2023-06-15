using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.IServices;
using CMSmedprohiremvp.Models.Ticket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMSmedprohiremvp.Controllers
{
    public class TicketController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;

        public TicketController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
           ICommonServices commonServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
          
        }
        public IActionResult Index()
        {
            List<Tickets> tickets = _commonService.GetAllTickets();
            if(tickets != null)
            {
                List<TicketViewModel> model = new List<TicketViewModel>();
                foreach (Tickets ticket in tickets)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == ticket.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                        model.Add(new TicketViewModel
                        {
                            DateCreated = ticket.DateCreated.GetValueOrDefault(),
                            DateModified = ticket.DateModified.GetValueOrDefault(),
                            Subject = ticket.Subject,
                            TicketCategory_ID = ticket.TicketCategory_ID,
                            TicketType = ticket.TicketType,
                            Ticket_ID = ticket.Ticket_ID,
                            TicketCategoryName = _commonService.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault().TicketCategoryName
                        });
                    }
                }
                return View(model);

            }
            return View();
        }

        public IActionResult TicketContent(int Ticket_ID)
        {               
                Tickets ticket = _commonService.GetAllTickets().Where(x => x.Ticket_ID == Ticket_ID).FirstOrDefault();
                ApplicationUser user = _userManager.Users.Where(x=>x.Id==ticket.User_ID).FirstOrDefault();
            
                TicketViewModel model = new TicketViewModel();
            if (ticket != null && user != null)
            {
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                TicketCategories categories = _commonService.GetTicketCategories(IsClient).Where(x => x.TicketCategory_ID == ticket.TicketCategory_ID).FirstOrDefault();
                model.Answer = "";
                model.DateCreated = ticket.DateCreated.GetValueOrDefault();
                model.DateModified = ticket.DateModified.GetValueOrDefault();
                model.Subject = ticket.Subject;
                model.TicketCategory_ID = ticket.TicketCategory_ID;
                model.TicketCategoryName = categories != null ? categories.TicketCategoryName : "";
                model.TicketType = ticket.TicketType;
                model.Ticket_ID = ticket.Ticket_ID;
                List<TicketContents> contents = _commonService.GetTicketContents(ticket.Ticket_ID);
                model.TicketContents = new List<TicketContentViewModel>();
                if (contents != null)
                {
                    foreach (TicketContents content in contents)
                    {
                        ApplicationUser contentuser = _userManager.Users.Where(x => x.Id == content.User_ID).FirstOrDefault();
                        string UserName = "";
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
                return View(model);
            }
             
            
            return RedirectToAction("Support", "Account");
        }
        public JsonResult RateTicketContent(int id, int rate)
        {
            TicketContents ticketContent = _commonService.GetTicketContentByID(id);
            ticketContent.Rate = rate;
            if (_commonService.UpdateTicketContent(ticketContent))
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
           

            if (user != null )
            {
                bool IsClient = _userManager.GetRolesAsync(user).Result.Contains("Applicant") ? false : true;
                int timeoffset = user.TimeOffset;
                TimeSpan timespanoffset = TimeSpan.FromMinutes(-(timeoffset));
                DateTime timenow = DateTime.UtcNow.Add(timespanoffset);
                TicketContents ticketContent = new TicketContents();
                ticketContent.InsertDate = timenow;
                ticketContent.TicketContent = content;
                ticketContent.Ticket_ID = id;
                ticketContent.User_ID = user.Id;
                Tickets ticket = _commonService.GetTicketByID(id);
                ticket.TicketType = 1;
                if (ticket != null)
                {
                    if (_commonService.AddTicketContent(ticketContent, ticket))
                    {
                        return Json(true);
                    }
                   
                }
               
            }
           
                return Json(null);
            
        }
        public JsonResult CompleteTicket(int ticket_id)
        {
            if (ticket_id > 0)
            {
                Tickets ticket = _commonService.GetTicketByID(ticket_id);
                if (ticket != null)
                {
                    ticket.TicketType = 3;
                }
               bool answer= _commonService.UpdateTicket(ticket);
                if(answer)
                {
                    return Json(true);
                }

            }
            return Json(false);
        }
    }
}