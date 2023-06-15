using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class InProcessShiftModel
    {
        public int AppliedShift_ID { get; set; }
        public int Applicant_ID { get; set; }
        public int ClientShift_ID { get; set; }
        public int NumberofShift { get; set; }
        public int CompletedNumberofShift { get; set; }
        public string WorkedHours { get; set; }
       public ApplicantDetailModel Applicant { get; set; }
        public ClientShiftModel Shift { get; set; }
        public List<ApplicantClockIn> ClockinClockOutTimes { get; set; }
    }
}
