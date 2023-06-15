using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class AppDashboardViewModel
    {
        public List<AppliedShifts> Shifts { get; set; }
        public List<AppliedShifts> PickedShifts { get; set; }
        public List<AppliedShifts> RejectedShifts { get; set; }
        public List <AppliedShifts> WorkedShiftTable { get; set; }
        public List <PayCheckViewModel> PayChecks { get; set; }
        public int AvailableDays { get; set; }
        public bool CheckPickedShift { get; set; }
    }
}
