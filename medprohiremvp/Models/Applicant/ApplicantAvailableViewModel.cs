using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class ApplicantAvailableViewModel
    {

        public int ApplicantAvailable_ID { get; set; }

        public int Applicant_ID { get; set; }
        public int ApplicantAvailableType_ID { get; set; }
        public string ApplicantAvailableTypeText { get; set; }
        public string ApplicantAvailableDays { get; set; }
        public DateTime AvailableDay { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
