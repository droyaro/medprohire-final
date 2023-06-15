using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ApplicantClockIn
    {

         public bool Manually { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkingDay { get; set; }
        [DataType(DataType.Time)]
        public DateTime WorkStartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime WorkEndTime { get; set; }

       

    }
}
