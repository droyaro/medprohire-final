using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ClientDashboardModel
    {
        public List<LocationCountModelForDashboard> AllLocation { get; set; }
        public List<LocationCountModelForDashboard> YearLocation { get; set; }
        public List<LocationCountModelForDashboard> Q1Location { get; set; }
        public List<LocationCountModelForDashboard> Q2Location { get; set; }
        public List<LocationCountModelForDashboard> Q3Location { get; set; }
        public List<LocationCountModelForDashboard> Q4Location { get; set; }
        public ShiftsCountModelForDashboard Allshifts { get; set; }
        public ShiftsCountModelForDashboard Yearshifts { get; set; }
        public ShiftsCountModelForDashboard Q1shifts { get; set; }
        public ShiftsCountModelForDashboard Q2shifts { get; set; }
        public ShiftsCountModelForDashboard Q3shifts { get; set; }
        public ShiftsCountModelForDashboard Q4shifts { get; set; }
    }
}
