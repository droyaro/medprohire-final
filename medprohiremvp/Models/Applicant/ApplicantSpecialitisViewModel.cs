using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class ApplicantSpecialitisViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "License")]
        public int Speciality_ID { get; set; }
      
        [Required]
        [Display(Name = "License Number")]
        public string License { get; set; }

        [Required]
        [Display(Name = "Licensure States")]
        public List<string> LegabilityStates { get; set; }

        [Display(Name = "License")]
        public string SpecialityName { get; set; }
        [Display(Name = "Issued Date")]
        public DateTime IssueDate { get; set; }
    }
}
