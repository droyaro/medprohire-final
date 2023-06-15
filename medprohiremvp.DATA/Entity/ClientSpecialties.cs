using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ClientSpecialties
    {
        [Key]
        public int ClientSpeciality_ID { get; set; }
        [Required]
        public int Institution_ID { get; set; }
        [ForeignKey("Institution_ID")]
        public ClinicalInstitutions Institution { get; set; }
        [Required]
        public int Speciality_ID { get; set; }
        [ForeignKey("Speciality_ID")]
        public Specialities Speciality { get; set; }
     
    }
}
