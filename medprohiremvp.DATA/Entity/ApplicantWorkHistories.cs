using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ApplicantWorkHistories
    {
        [Key]
        public int WorkHistory_ID { get; set; }
        [Required]
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string PlaceName { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }

}
