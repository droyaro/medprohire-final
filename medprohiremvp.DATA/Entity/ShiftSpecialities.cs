using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ShiftSpecialities
    {
        [Key]
        public int ShiftSpeciality_ID { get; set; }

        [Required]
        public int ClientShift_ID { get; set; }
        [ForeignKey("ClientShift_ID")]
        public ClientShifts ClientShift { get; set; }
        [Required]
        public int Speciality_ID { get; set; }
        [ForeignKey("Speciality_ID")]
        public Specialities Speciality { get; set; }
    }
}
