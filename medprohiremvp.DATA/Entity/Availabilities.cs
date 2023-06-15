using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class Availabilities
    {
        [Key]
        public int Availability_ID { get; set; }
        [Required]
        public string Availability { get; set; }
    }
}
