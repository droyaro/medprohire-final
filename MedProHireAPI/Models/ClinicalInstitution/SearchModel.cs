using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class SearchModel
    {
        public List<int> Specialities { get; set; }
        public List<int> Availabilities { get; set; }
        public int Location_ID { get; set; }
        public int Distance { get; set; }
    }
}
