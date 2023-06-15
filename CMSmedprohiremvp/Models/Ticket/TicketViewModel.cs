using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;

namespace CMSmedprohiremvp.Models.Ticket
{
    public class TicketViewModel
    {
        public int Ticket_ID { get; set; }
        [Required]
        public int TicketCategory_ID { get; set; }
        public string TicketCategoryName { get; set; }
        [Required]
        public string Subject { get; set; }
        public int TicketType { get; set; }
        [Required]
        public string Answer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<TicketContentViewModel> TicketContents { get; set; }
    }
}
