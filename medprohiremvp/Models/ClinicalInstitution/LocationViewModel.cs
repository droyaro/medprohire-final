using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class LocationViewModel
    {

        public int Branch_ID { get; set; }

        public int Institution_ID { get; set; }
        [Required]
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "State")]
        public int State_ID { get; set; }
        [Required]
        [Display(Name = "City")]
        public int City_ID { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "ZipCode must be five-digit or five-digit plus four-digit type")]
        [Required]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Contact Person Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Contact Person Name")]
        public string ContactName {get; set;}
        public string cityname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}
