using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class InProcessShiftDetailViewModel
    {
      
        [Display(Name = "Contractor Count")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Contractor Count must contain only number")]
        public int ContractorCount { get; set; }

      
        [Display(Name = "Hourly Rate")]
        [RegularExpression("^[0-9]+.?[0-9]*$", ErrorMessage = "HourlyRate must contain only number")]
        public int HourlyRate { get; set; }

      
        [Display(Name = "Clock In Time")]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }

      
        [Display(Name = "Clock Out Time")]
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }

      
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

      
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Consecutive Occurrences")]
        public bool Consecutive_Occurrences { get; set; }
        [Display(Name = "Holiday Shift")]
        public bool HolidayShift { get; set; }
        public string Location { get; set; }
        public int AppliedShift_ID { get; set; }
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }

     
        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string MiddleName { get; set; }

        public string SpecialitiesString { get; set; }
        public string PhoneNumber { get; set; }
        public string Img { get; set; }
        public List<ApplicantClockInClockOutTime> ClockinClockOutTimes { get; set; }
    }
}
