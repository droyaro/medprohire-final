using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ClinicalInstitutionBranches: Datefields
    {
        [Key]
        public int Branch_ID { get; set; }
        [ForeignKey("Institution_ID")]
        public int Institution_ID { get; set; }
        public ClinicalInstitutions Institution { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int CityId { get; set; }
      
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "ZipCode is not valid")]
        public string ZipCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ContactName { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        [DataType(DataType.EmailAddress,ErrorMessage ="Email Address is not valid")]
        public string Email { get; set; }


    }
}
