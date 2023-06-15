using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class AccpetedShiftClockinViewModel
    {
        public int clockiIn_ID { get; set; }
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }

        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }


        [Display(Name = "Clock Out Time")]
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }

    
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ShiftDescription { get; set; }

        [Required]
        [Display(Name = "Responsibility")]
        public string Responsibility { get; set; }

        [Display(Name = "Insert Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfShift { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ShiftExpirationDate { get; set; }

        [Required]
        [Display(Name = "Provider")]
        public List<int> Specialities { get; set; }
        [Display(Name = "Provider")]
        public string SpecialitiesName { get; set; }

        public int Branch_ID { get; set; }
        [Display(Name = "Location")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Occurrences")]

        public int Occurrences { get; set; }
        [Required]
        [Display(Name = "Shift Label")]
        public int ShiftLabel_ID { get; set; }
        public string ShiftLabelName { get; set; }
        [Display(Name = "Consecutive Occurrences")]
        public bool Consecutive_Occurrences { get; set; }
        [Display(Name = "Holiday shift")]
        public bool HolidayShift { get; set; }

        public int AppliedShift_ID { get; set; }
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }
        public bool isconfirm { get; set; }
        public bool hasmanually { get; set; }
        public bool disable { get; set; }
        public bool workingshift { get; set; }

        [Display(Name = "Date")]
        public DateTime WorkingDay { get; set; }
        public string InstitutionName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int CityID { get; set; }
        public TimeSpan MaxClockinDifference { get; set; }
        public int TimeSpanOffset { get; set; }

    }
}
