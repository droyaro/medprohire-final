using medprohiremvp.Models.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class ApplicantProfileViewModel
    {
        public int Applicant_ID { get; set; }

        public Guid User_ID { get; set; }

        // [RegularExpression(@"^[\w]*[A-z]+[\s()-]*\w+$", ErrorMessage ="Name must contain at least one letter")]
      
       [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }

        public IFormFile Resume { get; set; }

        [Required(ErrorMessage = "Employment Eligibility is required")]
        [Display(Name = "Employment Eligibility")]
        public int VisaStatus_ID { get; set; }
        [RequiredIf("VisaStatus_ID", "3", ErrorMessage = "Field is required if Employment Eligibility is Other")]
        public bool IsEligible { get; set; }

        [Required(ErrorMessage = "Availability ID is required")]
        [Display(Name = "Availability")]
        public int Availability_ID { get; set; }

 

        [Display(Name = "Provider")]
        public string SpecialitiesString { get; set; }

        [Display(Name = "Certificate Types")]
        public string CertificatiesString { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public bool Disabled { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int City_ID { get; set; }

        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "ZipCode must be five-digit or five-digit plus four-digit type")]
        [Required(ErrorMessage = "ZipCode is required")]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Employment Eligibility")]
        public string VisaStatusString { get; set; }
        [Display(Name = "Availability")]
        public string AvailabilityString { get; set; }
        public string ImgSrc { get; set; }
    public string PhoneNumber { get; set; }
        public float Latitude { get; set; }

        public float Longitude { get; set; }

    }
}
