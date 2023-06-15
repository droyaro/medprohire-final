using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
  public  class ApplicantClockInClockOutTime
    {
        [Key]
        public int ClockinClockOutTime_ID { get; set; }
        public int AppliedShift_ID { get; set; }
        [ForeignKey("AppliedShift_ID")]
        public ApplicantAppliedShifts AppliedShift { get; set; }
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }

        public bool Manually { get; set; }

        public bool AcceptedByClient { get; set; }

        [DataType(DataType.Date)]
        public DateTime WorkingDay { get; set; }
        [DataType(DataType.Time)]
        public DateTime WorkStartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime WorkEndTime { get; set; }

        public bool EndStatus { get; set; }


    }
}
