using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ShiftsCountModelForDashboard
    {
        public int Created { get; set; }
        public int NotStarted { get; set; }
        public int Completed { get; set; }
        public int Active { get; set; }
        public int Incomplete { get; set; }
        public int Cancelled { get; set; }
    }
}
