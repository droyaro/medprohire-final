using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class DashboardtableViewModel
    {
        public int AppliedShift_ID { get; set; }
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int NumberofShift { get; set; }
        public int CompletedNumberofShift { get; set; }
        public double Numberofworkedhours { get; set; }
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

        [Display(Name = "Holiday Shift")]
        public bool HolidayShift { get; set; }
        public string Location { get; set; }


        [Display(Name = "Hourly Rate")]
        [RegularExpression("^[0-9]+.?[0-9]*$", ErrorMessage = "Hourly Rate must contain only number")]
        public int HourlyRate { get; set; }
        public int ClientShift_ID { get; set; }
        public string SpecialitiesString { get; set; }
        


    }
}
