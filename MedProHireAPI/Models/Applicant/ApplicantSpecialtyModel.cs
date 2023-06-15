using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantSpecialtyModel
    {
        public int Applicant_ID { get; set; }

        [Required]
        [Display(Name = "Specialty")]
        public int Speciality_ID { get; set; }

        [Required]
        [Display(Name = "License Number")]
        public string License { get; set; }

        [Required]
        [Display(Name = "Licensure States")]
        public List<string> LegabilityStates { get; set; }

        [Display(Name = "Specialty")]
        public string SpecialityName { get; set; }
    }
}