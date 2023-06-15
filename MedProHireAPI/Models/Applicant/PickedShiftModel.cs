using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class PickShiftModel
    {
        [Required]
        public int Applicant_ID { get; set; }
        [Required]
        public int ClientShift_ID { get; set; }
        [Required]
        public List<DateTime> AppliedDates { get; set; }
    }
}
