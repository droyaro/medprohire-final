using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Account
{
    public class NewTicketModel
    {
        [Required]
        public Guid User_ID { get; set; }
        [Required]
        public int TicketCategory_ID { get; set; }
        [Required]
        public string Subject { get; set; }
       
        [Required]
        public string Answer { get; set; }
    }
}
