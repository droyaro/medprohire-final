using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class LocationCountModelForDashboard
    {
        public int Location_ID { get; set; }
        public string LocationName { get; set; }
        public int ActiveShiftsCount { get; set; }
        public int CompletedShiftsCount { get; set; }
        public int CreatedShiftsCount { get; set; }
    }
}
