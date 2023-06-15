using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Models.Applicant;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ClientShiftViewModel
    {
        [Display(Name ="Req ID")]
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }

        [Required]
        [Display(Name = "Number of providers needed")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Number of providers must contain only number")]
        public int ContractorCount { get; set; }

 

        [Required]
        [Display(Name = "Start In Time")]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }

        [Required]
        [Display(Name = "Start Out Time")]
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
        [Display(Name = "Shift Dates")]
        public string ShiftsDates { get; set; }
        [Required]
        [Display(Name = "Required Provider")]
        public int Specialities { get; set; }
        [Display(Name = "Required Provider")]
        public string SpecialitiesName { get; set; }
        [Display(Name = "Location")]
        public int Branch_ID { get; set; }
        [Display(Name = "Location")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Occurrences")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Occurrences must contain only numbers")]
        public int Occurrences { get; set; }
        [Required]
        [Display(Name = "Shift Label")]
        public int ShiftLabel_ID { get; set; }
        public string ShiftLabelName { get; set; }
        public bool Applied { get; set; } // if user apllied this shift , makes-button apply disable
        [Display(Name = "Status")]
        public int Category_ID { get; set; }
        [Display(Name = "Category")]
        public string Category_Name { get; set; }
        public string Freedays { get; set; }
        public List<ApplicantAvailables> applicantAvailables { get; set; }
        public List<ShiftDetailViewModel> ShiftDetailPicked { get; set; }
    }
}
