using MedProHireAPI.Models.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantRegistrationNextModel
    {
   

        public Guid User_ID { get; set; }



        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }
   
        [Required(ErrorMessage = "Resume is required")]
        public IFormFile Resume { get; set; }

        [Required(ErrorMessage = "VisaStatus field is required")]
        [Display(Name = "Legal Status")]
        public int VisaStatus_ID { get; set; }
        [RequiredIf("VisaStatus_ID", "3", ErrorMessage = "Field is required if Legal Status is Other")]
        public bool IsEligible { get; set; }

        [Required(ErrorMessage = "Availability field is required")]
        public int Availability_ID { get; set; }

        public List<ApplicantCertificateModel> certificates { get; set; }

        public List<ApplicantWorkHistoryModel> workHistories { get; set; }
        [Required]
        public List<ApplicantSpecialtyModel> specialities { get; set; }
    }
}
