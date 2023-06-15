using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using medprohiremvp.DATA.IdentityModels;

namespace medprohiremvp.DATA.Entity
{
    public class Tickets: Datefields
    {
        [Key]
        [Required]
        public int Ticket_ID { get; set; }
       
        public Guid User_ID { get; set; }


        public int TicketCategory_ID { get; set; }
        [ForeignKey("TicketCategory_ID")]
        public TicketCategories ticketCategories { get; set; }
        public string Subject { get; set; }
        public int TicketType { get; set; }

    }
}
