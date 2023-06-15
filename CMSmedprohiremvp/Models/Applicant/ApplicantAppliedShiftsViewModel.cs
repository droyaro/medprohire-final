using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class ApplicantAppliedShiftsViewModel
    {
        
        public int AppliedShift_ID { get; set; }
        public int Applicant_ID { get; set; }
        public Guid User_ID { get; set; }
        public bool Accepted { get; set; }
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }

        [Required]
        [Display(Name = "Contractor Count")]

        public int ContractorCount { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]

        public float HourlyRate { get; set; }

        [Required]
        [Display(Name = "Clock In Time")]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }

        [Required]
        [Display(Name = "Clock Out Time")]
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }

        [Required]
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
        public List<int> Specialities { get; set; }
        [Display(Name = "Specialities")]
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
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }
        public string ApplicantName { get; set; }
        public string InstitutionName { get; set; }
        public int NumberofShift { get; set; }
        public int NumberofCompletedShift { get; set; }
        public TimeSpan CountofHours { get; set; }
        public int Status { get; set; }
        public int Paid { get; set; }
       public string Remarks { get; set; }
        public string ShiftsDates { get; set; }
}
}
