using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medprohiremvp.DATA.Entity
{
    public class ApplicantSpecialities
    {
        [Key]
        public int ApplicantSpeciality_ID { get; set; }
        [Required]
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }
        [Required]
        public int Speciality_ID { get; set; }
        [ForeignKey("Speciality_ID")]
        public Specialities Speciality { get; set; }
        [Required]
        public string License { get; set; }
        public int Status { get; set; }
        [Required]
        public string LegabilityStates { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }

    }
}
