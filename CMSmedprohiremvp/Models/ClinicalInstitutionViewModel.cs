using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models
{
    public class ClinicalInstitutionViewModel
    {
        public int Institution_ID { get; set; }
        public Guid User_ID { get; set; }
        [Required(ErrorMessage = " ClinilacInstitution Name is required")]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = "Choose InstitutionType")]
        public int InstitutionType_ID { get; set; }
        public string InstitutionType { get; set; }

        [Display(Name = "Contact Person")]
        [Required(ErrorMessage = "Contact Person is required")]
        public string ContactPerson { get; set; }

        [Display(Name = "Contact Title")]
        [Required(ErrorMessage = "Contact Title is required")]
        public string ContactTitle { get; set; }

        [Required(ErrorMessage = "InstitutionTaxId is required")]
        [RegularExpression(@"^(\d+-?\d+)*$", ErrorMessage = "TIN is not valid format.")]
        public string InstitutionTaxId { get; set; }

        public string Logo { get; set; }
        [Required(ErrorMessage = "InstitutionDescription is required")]
        public string InstitutionDescription { get; set; }
        public string Status { get; set; }
        public int BoardingProcess { get; set; }

        public List<ClinicalInstitutionBranches> Locations { get; set; }
        public string StateName { get; set; }
        public int City_ID { get; set; }
        public string CityName { get; set; }

        public string Address { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "ZipCode is not valid")]
        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
        public string Type_new_old { get; set; }
        public string PreferredSpecialties { get; set; }
    }
}
