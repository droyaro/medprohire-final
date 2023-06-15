using MedProHireAPI.Models.Applicant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ApplicantDetailModel
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string MiddleName { get; set; }

        [Display(Name = "Employment Eligibility")]
        public string VisaStatus { get; set; }
       
        public bool IsEligible { get; set; }


        public string Email { get; set; }

    
        [Display(Name = "State")]
        public string State { get; set; }

       
        [Display(Name = "City")]
        public string  CityName { get; set; }

        public string ZipCode { get; set; }


        public string Address { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
   
        public string Imgsrc { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string SpecialitesString { get; set; }
        public string Availability { get; set; }
        public List<ApplicantAvailableDatesModel> ApplicantAvailables { get; set; }
        public List<ApplicantCertificateModel> Certificates { get; set; }

        public List<ApplicantWorkHistoryModel> WorkHistories { get; set; }

        public List<ApplicantSpecialtyModel> Specialities { get; set; }
    }
}
