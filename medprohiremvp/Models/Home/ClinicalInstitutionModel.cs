using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;

namespace medprohiremvp.Models.Home
{
    public class ClinicalInstitutionModel
    {
        public int Institution_ID { get; set; }
        public Guid User_ID { get; set; }
     //   [Required(ErrorMessage = " ClinilacInstitution Name is required")]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = "Choose Institution Type")]
        public int InstitutionType_ID { get; set; }

        //[Display(Name = "Contact Person")]
        //[Required(ErrorMessage = "Contact Person is required")]
        //public string ContactPerson { get; set; }

        //[Display(Name = "Contact Title")]
        //[Required(ErrorMessage = "Contact Title is required")]
        //public string ContactTitle { get; set; }

       
        [RegularExpression(@"^(\d+-?\d+)*$", ErrorMessage = "Tax ID is not valid format.")]
        public string InstitutionTaxId { get; set; }
    
        public IFormFile Logo { get; set; }
        [Required(ErrorMessage = "Institution Description is required.")]
        public string InstitutionDescription { get; set; }

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Select providers is required")]
        public string PreferredSpecialites { get; set; }
       
        public List<ClinicalInstitutionBranches> Branches { get; set; }
    }
}
