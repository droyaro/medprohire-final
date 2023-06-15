using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class AppliedShiftModel
    {
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }
        public string InstitutionName { get; set; }
        [Required]
        [Display(Name = "Contractor Count")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Contractor Count must contain only number")]
        public int ContractorCount { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        [RegularExpression("^[0-9]+.?[0-9]*$", ErrorMessage = "HourlyRate must contain only number")]
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
        public List<int> Specialities { get; set; }
        [Display(Name = "Specialty")]
        public string SpecialitiesName { get; set; }

        public int Branch_ID { get; set; }
        [Display(Name = "Location")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Occurrences")]

        public int Occurrences { get; set; }


        public string Location { get; set; }

        public bool Applied { get; set; } // if user apllied this shift , makes-button apply disable
        public bool AppliedAllDays { get; set; }
        public string AppliedDaysList { get; set; }
        public int AppliedShift_ID { get; set; }
        public int Paid { get; set; }
        public string Remarks { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public int Distance { get; set; }
        public string ShiftsDates { get; set; }
    }
}
