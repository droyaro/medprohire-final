using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.Models.ValidationAttributes;

namespace medprohiremvp.Models.Home
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        [Remote("CheckUserName", "Home", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string UserName { get; set; }

        
        [RequiredIf("applicant", "true", ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage ="Email Address is not valid")]
        [Display(Name = "Email")]
        [Remote("CheckEmail", "Home", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different Email.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect address")]  
        public string EmailAddress { get; set; }


        [RequiredIf("clinical", "true", ErrorMessage = "Contact Person Email Address is required")]
        [EmailAddress(ErrorMessage = "Contact Person Email Address is not valid")]
        [Display(Name = "Email")]
        [Remote("_checkEmail", "Home", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different Email.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect address")]
        public string ContactEmailAddress { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address Line 1")]
        public string Address { get; set; }

   

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int State_ID { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int City_ID { get; set; }
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "ZipCode must be five-digit or five-digit plus four-digit type")]  
        [Required(ErrorMessage = "ZipCode is required")]
        //[Remote("CheckZipCode", "Home", HttpMethod = "POST", ErrorMessage = "ZipCode must be from 55001-56763")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]  
        [Phone(ErrorMessage ="Phone Number is not valid")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(.{4,15})$", ErrorMessage = "Password must contain at least one upper case, one lower case letter, a digit and a symbol.")]
        public string Password { get; set; }
        public int Ishirer { get; set; }
        [Required]
        [DataType(DataType.Password)]
      
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        [Display(Name = "First Name")]
        [RequiredIf("applicant", "true", ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RequiredIf("applicant", "true", ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name or Initial")]
        //[RequiredIf("applicant", "true", ErrorMessage = "Middle Name is required")]
        public string MiddleName { get; set; }

        [RequiredIf("clinical", "true",ErrorMessage = "Institution or Corporate Name is required")]
        public string InstitutionName { get; set; }

        [Display(Name = "Contact Person")]
        [RequiredIf("clinical", "true", ErrorMessage = "Contact Person is required")]
        public string ContactPerson { get; set; }

        [Display(Name = "Contact Title")]
        [RequiredIf("clinical", "true", ErrorMessage = "Contact Title is required")]
        public string ContactTitle { get; set; }
        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }
    }
}
