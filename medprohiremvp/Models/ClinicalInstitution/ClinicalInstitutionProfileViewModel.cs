using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ClinicalInstitutionProfileViewModel
    {
        [Display(Name = "Institution ID")]
        public int Institution_ID { get; set; }
        public Guid User_ID { get; set; }
        [Display(Name = "Institution Name")]
        //   [Required(ErrorMessage = " ClinilacInstitution Name is required")]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = "Choose Institution Type")]
        [Display(Name = "Institution Type")]
        public int InstitutionType_ID { get; set; }

        [Display(Name = "Contact Person")]
        [Required(ErrorMessage = "Contact Person is required")]
        public string ContactPerson { get; set; }

        [Display(Name = "Contact Title")]
        [Required(ErrorMessage = "Contact Title is required")]
        public string ContactTitle { get; set; }

        [Display(Name = "Tax ID")]
        [RegularExpression(@"^(\d+-?\d+)*$", ErrorMessage = "Tax ID is not valid format.")]
        public string InstitutionTaxId { get; set; }

        [Required(ErrorMessage = "Institution Description is required")]
        public string InstitutionDescription { get; set; }
     
        public string Email { get; set; }
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
        public bool Disabled { get; set; }
        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}

