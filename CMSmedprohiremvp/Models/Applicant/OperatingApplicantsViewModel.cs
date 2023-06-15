using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class OperatingApplicantsViewModel
    {
        public int Applicant_ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Imgsrc { get; set; }

        public int ClientShift_Id { get; set; }

        public int Category_ID { get; set; }

        public List<ApplicantAppliedShifts> OperatingApplicant { get; set; }
    }
}
