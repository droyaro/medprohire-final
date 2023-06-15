using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApplicantAppliedShiftsDays
    {
        [Key]
        public int AppliedShiftDay_ID { get; set; }
        public int AppliedShift_ID { get; set; }
        [ForeignKey("AppliedShift_ID")]
        public ApplicantAppliedShifts appliedShifts { get; set; }

        [DataType(DataType.Date)]
        public DateTime Day { get; set; }
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }
    }
}
