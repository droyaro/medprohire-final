using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantAppliedShiftModel
    {
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }
        public string InstitutionName { get; set; }
     

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


        public int Branch_ID { get; set; }
        [Display(Name = "Location")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Occurrences")]

        public int Occurrences { get; set; }
        public string LocationAddress { get; set; }

        public string AppliedDaysList { get; set; }
        public int NumberofShift { get; set; }
        public int CompletedNumberofShift { get; set; }
        public double Numberofworkedhours { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string ShiftsDates { get; set; }
    }
}
