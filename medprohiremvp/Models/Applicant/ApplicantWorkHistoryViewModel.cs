using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace medprohiremvp.Models.Applicant
{
    public class ApplicantWorkHistoryViewModel
    {
        public int WorkHistory_ID { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
     
        [Display(Name= "Employer")]
        public string PlaceName { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "To Date")]
        public bool UntilNow { get; set; }
        [Display(Name = "Job Title")]
        public string SpecialityName { get; set; }
    }
}
