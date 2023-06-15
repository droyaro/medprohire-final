using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models
{
    public class ApplicantRegisterFirstStepModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }



        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int City_ID { get; set; }

        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "ZipCode must be five-digit or five-digit plus four-digit type")]
        [Required(ErrorMessage = "ZipCode is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Phone Number is not valid")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(.{4,15})$", ErrorMessage = "Password must contain at least one upper case, one lower case letter, a digit and a symbol.")]
        public string Password { get; set; }
      
        [Required]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required( ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required( ErrorMessage = "Middle Name is required")]
        public string MiddleName { get; set; }

  
    }
}
