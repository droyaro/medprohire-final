using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApplicantAvailables
    {
        [Key]
        public int ApplicantAvailable_ID { get; set; }
        
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }
     
        public string ApplicantAvailableDays { get; set; }
        public DateTime AvailableDay { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
