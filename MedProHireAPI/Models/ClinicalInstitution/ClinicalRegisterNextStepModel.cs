using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ClinicalRegisterNextStepModel
    {
        [Required]
        public Guid User_ID { get; set; }
        [Required]
        public string InstitutionName { get; set; }

        [Required]
        public int InstitutionType_ID { get; set; }


        [Required(ErrorMessage = "Institution Tax ID is required.")]
        [RegularExpression(@"^(\d+-?\d+)*$", ErrorMessage = "Tax ID is not valid format.")]
        public string InstitutionTaxId { get; set; }

        public IFormFile Logo { get; set; }
        [Required(ErrorMessage = "Institution Description is required.")]
        public string InstitutionDescription { get; set; }

    }
}
