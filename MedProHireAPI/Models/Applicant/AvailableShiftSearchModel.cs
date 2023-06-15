using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class AvailableShiftSearchModel
    {
        public List<int> Specialities { get; set; }
        public List<int> Cities { get; set; }
        public List<int> States { get; set; }
        public int Distance { get; set; }
    }
}
