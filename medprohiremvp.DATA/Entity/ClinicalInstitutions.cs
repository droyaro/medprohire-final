using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using medprohiremvp.DATA.IdentityModels;

namespace medprohiremvp.DATA.Entity
{
    public class ClinicalInstitutions: Datefields
    {
        [Key]
        public int Institution_ID { get; set; }
        [Required]
        
        public Guid User_ID { get; set; }
        [ForeignKey("User_ID")]
        public ApplicationUser User { get; set; }
        [Required]
        public int InstitutionType_ID { get; set; }
        public InstitutionTypes InstitutionType { get; set; }
        [Required]
        public string InstitutionName { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public string ContactTitle { get; set; }
        [Required]
        [Encrypted]
        public string InstitutionTaxId { get; set; }
        
        public string InstitutionDescription { get; set; }

        public int Status { get; set; }

        public string Logo { get; set; }

        public string Specialties { get; set; }

        public virtual List<ClinicalInstitutionBranches> Branches { get; set; }
        public virtual List<ClientShifts> ClientShifts { get; set; }
    }
}
