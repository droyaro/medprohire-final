using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantAvailableDatesModel
    {
        public int Applicant_ID { get; set; }
        public int ApplicantAvailableType_ID { get; set; }
        public string ApplicantAvailableTypeName { get; set; }
        public bool ApplicantAvailableTypeValue { get; set; }
        public string ApplicantAvailableDays { get; set; }
    }
}
