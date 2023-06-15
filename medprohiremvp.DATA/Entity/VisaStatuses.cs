using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class VisaStatuses
    {
        [Key]
        public int VisaStatus_ID { get; set; }
        [Required]
        public string VisaStatus { get; set; }
    }
}
