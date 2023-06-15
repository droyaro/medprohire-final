using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApplicantReferences
    {
        [Key]
        public int Reference_ID { get; set; }
        //[Required]
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public virtual Applicants applicant{get; set;}
        //[Required]
        public string Company { get; set; }
        //[Required]
        public string Position { get; set; }
        //[Required]
        public string ContactName { get; set; }
        //[Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        [Phone]
        [RegularExpression(@"[\(]?[+]?\d+[\)]? ([\s -]?\d+)+", ErrorMessage = "Phone number is not valid")]
        public string PhoneNumber { get; set; }
       
    }
}
