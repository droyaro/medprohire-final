using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ApplicantSearchModel
    {
        public int Applicant_ID { get; set; }
        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string MiddleName { get; set; }

        [Display(Name = "Employment Eligibility")]
        public string VisaStatus { get; set; }

        public bool IsEligible { get; set; }
        public string ImgSrc { get; set; }
        public bool IsAvailable { get; set; }
        public string SpecialitesString { get; set; }
        public string Certificates { get; set; }
        public string Availability { get; set; }
    }
}
