using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class NewClientShiftModel
    {
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }

        [Required]
        [Display(Name = "Contractor Count")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Contractor Count must contain only number")]
        public int ContractorCount { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        [RegularExpression("^[0-9]+.?[0-9]*$", ErrorMessage = "HourlyRate must contain only number")]
        public int HourlyRate { get; set; }

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
        [Display(Name = "Shift Dates")]
        public string ShiftsDates { get; set; }

        [Required]
        [Display(Name = "Specialty")]
        public List<int> Specialities { get; set; }

        [Display(Name = "Location")]
        public int Branch_ID { get; set; }

        [Required]
        [Display(Name = "Shift Label")]
        public int ShiftLabel_ID { get; set; }

 
    }
}
