using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class InviteShiftModel
    {
        public int ClientShift_ID { get; set; }

        public int Institution_ID { get; set; }

  
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
        [Display(Name = "Active Dates")]
       
        public string ActiveDates { get; set; }
        public string Remarks { get; set; }
    }
}
