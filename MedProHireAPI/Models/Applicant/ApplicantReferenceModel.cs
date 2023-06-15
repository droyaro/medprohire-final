using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantReferenceModel
    {
        public int Applicant_ID { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Phone number is not valid")]
        public string PhoneNumber { get; set; }
    }
}
