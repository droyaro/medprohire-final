using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Ticket
{
    public class TicketContentViewModel
    {
        public int TicketContent_ID { get; set; }
        public int Ticket_ID { get; set; }
      
        public Guid User_ID { get; set; }
        public string UserName { get; set; }
        public string TicketContent { get; set; }
        public DateTime InsertDate { get; set; }
        public int Rate { get; set; }
    }
}
