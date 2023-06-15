using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Models.Applicant;
using medprohiremvp.Models.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace medprohiremvp.Models.Home

{
    public class ApplicantModel
    {
        public int Applicant_ID { get; set; }

        public Guid User_ID { get; set; }

        // [RegularExpression(@"^[\w]*[A-z]+[\s()-]*\w+$", ErrorMessage ="Name must contain at least one letter")]
        //[Required(ErrorMessage = "First Name is required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        // [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Middle Name is required")]
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Display(Name = "Profile Image (Optional)")]
        public IFormFile ProfileImage { get; set; }
        public string Imgsrc { get; set; }
        [Required(ErrorMessage = "Resume is required")]
        public IFormFile Resume { get; set; }

        [Required(ErrorMessage = "Legal Status field is required")]
        [Display(Name = "Legal Status")]
        public int VisaStatus_ID { get; set; }
        [RequiredIf("VisaStatus_ID", "3", ErrorMessage = "Field is required if Legal Status is Other")]
        public bool IsEligible { get; set; }

        [Required(ErrorMessage = "Availability field is required")]
        [Display(Name = "Availability")]
        public int Availability_ID { get; set; }

        public List<ApplicantCertificateViewModel> certificates { get; set; }

        public List<ApplicantWorkHistoryViewModel> workHistories { get; set; }

        public List<ApplicantSpecialitisViewModel> specialities { get; set; }
        public List<ApplicantAvailables> availability { get; set; }
        [Display(Name = "Occupation")]
        public string SpecialitiesString { get; set; }

        [Display(Name = "Certification")]
        public string CertificatiesString { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Availability")]
        public string Availability { get; set; }
        [Display(Name = "Legal Status")]
        public string VisaStatus { get; set; }
        public bool IsAvailable { get; set; }
        [Display(Name = "Days Available")]
        public string ApplicantAvailableDays { get; set; }
    }
}
