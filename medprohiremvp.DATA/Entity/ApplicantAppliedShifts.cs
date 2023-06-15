using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApplicantAppliedShifts
    {
        [Key]
        public int AppliedShift_ID { get; set; }
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants applicant { get; set; }
        public int ClientShift_ID { get; set; }
        [ForeignKey("ClientShift_ID")]
        public ClientShifts shift { get; set; }
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }
        public bool Accepted { get; set; }
        public int Status { get; set; }
        public int Paid { get; set; }
        public string Remarks { get; set; }
        public string Invited { get; set; }
        public virtual List<ApplicantAppliedShiftsDays> AppliedShiftsDays { get; set; }

        public int Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public ShiftCategory Category { get; set; }
    }
}
