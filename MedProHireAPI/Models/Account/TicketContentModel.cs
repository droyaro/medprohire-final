using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Account
{
    public class TicketContentModel
    {
        public int TicketContent_ID { get; set; }
        [Required]
        public int Ticket_ID { get; set; }
        [Required]
        public Guid User_ID { get; set; }

        public string UserName { get; set; }
        [Required]
        public string TicketContent { get; set; }
        public DateTime InsertDate { get; set; }
        public int Rate { get; set; }
    }
}
