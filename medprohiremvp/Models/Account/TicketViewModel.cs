using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
namespace medprohiremvp.Models.Account
{
    public class TicketViewModel
    {
        [Display(Name = "Ticket ID")]
        public int Ticket_ID { get; set; }
        [Required]
        [Display(Name = "Ticket Category")]
        public int TicketCategory_ID { get; set; }
        [Display(Name = "Ticket Category")]
        public string TicketCategoryName { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Display(Name = "Status")]
        public int TicketType { get; set; }
        [Required]
        
        [Display(Name = "Content")]
        public string Answer { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name = "Last Changed")]
        public DateTime DateModified { get; set; }
        public List<TicketContentViewModel> TicketContents { get; set; }
    }
}
