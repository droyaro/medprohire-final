using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using medprohiremvp.DATA.Entity;

namespace medprohiremvp.DATA.Entity
{
    public class ClientShifts: Datefields
    {
        [Key]
        public int ClientShift_ID { get; set; }

        [Required]
        public int Institution_ID { get; set; }
        [ForeignKey("Institution_ID")]
        public ClinicalInstitutions institution { get; set; }

        [Required]
        public int ContractorCount { get; set; }

        [Required]
        public float HourlyRate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime ClockInTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime ClockOutTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string ShiftsDates { get; set; }

        [Required]
        public string ShiftDescription { get; set; }


        [Required]
        public string Responsibility { get; set; }


        [Required]
        public DateTime DateOfShift { get; set; }

        [Required]
        public DateTime ShiftExpirationDate { get; set; }

        public int? Branch_ID { get; set; }
        [ForeignKey("Branch_ID")]
        public ClinicalInstitutionBranches Branches { get; set; }
        [Required]
        public int Occurrences { get; set; }

        public int ShiftLabel_ID { get; set;}
        [ForeignKey("ShiftLabel_ID")]
        public ShiftLabels ShiftLabels { get; set; }

        public bool Consecutive_Occurrences { get; set; }

        public bool HolidayShift { get; set; }
        [DefaultValue(1)]
        public int Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public ShiftCategory category { get; set; }

        public bool Available { get; set; }

        public virtual List<ShiftSpecialities> ShiftSpecialities { get; set; }

  




    }
}
