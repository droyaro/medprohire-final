using MedProHireAPI.Models.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantProfileDetailModel
    {
  

      

        // [RegularExpression(@"^[\w]*[A-z]+[\s()-]*\w+$", ErrorMessage ="Name must contain at least one letter")]

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string MiddleName { get; set; }



        [Required(ErrorMessage = "Employment Eligibility is required")]
        [Display(Name = "Employment Eligibility")]
        public int VisaStatus_ID { get; set; }
        [RequiredIf("VisaStatus_ID", "3", ErrorMessage = "Field is required if Employment Eligibility is Other")]
        public bool IsEligible { get; set; }

        [Required(ErrorMessage = "Availability ID is required")]
        public int Availability_ID { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int City_ID { get; set; }

        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "ZipCode must be five-digit or five-digit plus four-digit type")]
        [Required(ErrorMessage = "ZipCode is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        public bool Disabled { get; set; }
    }
}
