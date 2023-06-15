using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class Specialities
    {
        [Key]
        public int Speciality_ID { get; set; }
        [Required]
        public string SpecialityName { get; set; }
    }
}
