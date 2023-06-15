using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class AppliedShifts
    {
        [Display(Name = "Shift N")]
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }
        [Display(Name = "Client Name")]
        public string InstitutionName{ get; set; }
       
        [Required]
        [Display(Name = "Time")]
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
        [Display(Name = "Shift Label")]
        public string ShiftLabelName { get; set; }
        [Display(Name = "Consecutive Occurrences")]
        public bool Consecutive_Occurrences { get; set; }
      
        [Display(Name = "Location Address")]
        public string Location { get; set; }

        public bool Applied { get; set; } // if user apllied this shift , makes-button apply disable
        [Display(Name = "Applied All Days")]
        public bool AppliedAllDays { get; set; }
        [Display(Name = "Applied Days")]
        public string AppliedDaysList { get; set; }
        public int AppliedShift_ID { get; set; }

        [Display(Name = "Shift Count")]
        public int NumberofShift { get; set; }
        [Display(Name = "Completed Shift Count")]
        public int CompletedNumberofShift { get; set; }
        [Display(Name = "Hours Worked")]
        public double Numberofworkedhours { get; set; }
        public int Paid { get; set; }
        [Display(Name = "Remarkss")]
        public string Remarks { get; set; }
        [Display(Name = "Contact Person Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contact Person Name")]
        public string ContactPerson { get; set; }
        public int Distance { get; set; }
        [Display(Name = "Date")]
        public string ShiftsDates { get; set; }
        public List<InviedDaysViewModel> inviedDays { get; set; }

    }
}
