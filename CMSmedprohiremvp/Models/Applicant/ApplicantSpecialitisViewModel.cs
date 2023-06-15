using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class ApplicantSpecialitisViewModel
    {

        [Required]
        public string AppSpeciality{ get; set; }
      
        [Required]
        [Display(Name = "License number")]
        public string License { get; set; }

        [Required]
        public string LegabilityStates { get; set; }
    }
}
