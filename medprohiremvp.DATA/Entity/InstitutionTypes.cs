using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class InstitutionTypes
    {
        [Key]
        public int InstitutionType_ID { get; set; }
        [Required]
        public string InstitutionTypeName { get; set; }

        public virtual List<ClinicalInstitutions> ClinicalInstitutions { get; set; }
    }
}
