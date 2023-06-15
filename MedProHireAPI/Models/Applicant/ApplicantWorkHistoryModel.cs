using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantWorkHistoryModel
    {

            public int WorkHistory_ID { get; set; }
            public int? Speciality_ID { get; set; }
        public string JobTitle { get; set; }
            public string PlaceName { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool UntilNow { get; set; }
            public string SpecialityName { get; set; }
        }
    }
