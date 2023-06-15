using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class PayChecks:Datefields
    {
        [Key]
        public int PayCheck_ID { get; set; }
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }
      
        [Required]
        public DateTime PayCheckDate { get; set; }
        [Required]
        public DateTime PayBeginDate { get; set; }
      
        [Required]
        public DateTime PayEndDate { get; set; }
        [Required]
        public string Net_Pay { get; set; }
        public string PayCheckFile { get; set; }
    }
}
