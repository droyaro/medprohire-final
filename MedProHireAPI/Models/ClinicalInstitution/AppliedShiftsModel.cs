using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class AppliedShiftsModel
    {
        public int Applicant_ID { get; set; }
        public int ClientShift_ID { get; set; }
        public string Remarks { get; set; }
        [Required]
        public List<String> AppliedDays { get; set; }
     }
}
