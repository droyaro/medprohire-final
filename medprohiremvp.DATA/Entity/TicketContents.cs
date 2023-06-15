using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class TicketContents
    {
        [Key]
        public int TicketContent_ID { get; set; }
        public int Ticket_ID { get; set; }
        [ForeignKey("Ticket_ID")]
        public Tickets ticket { get; set; }
        public Guid User_ID { get; set; }
        public string TicketContent { get; set; }
        public int Rate { get; set; }
        public DateTime InsertDate { get; set; }
      
    }
}
