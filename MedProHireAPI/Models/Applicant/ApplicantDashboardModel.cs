using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantDashboardModel
    {
        public List<ApplicantAppliedShiftModel> Shifts { get; set; }
        public List<ApplicantAppliedShiftModel> PickedShifts { get; set; }
        public List<ApplicantAppliedShiftModel> RejectedShifts { get; set; }
        public List<ApplicantAppliedShiftModel> CurrentShifts { get; set; }
        public List<PayCheckModel> PayChecks { get; set; }
    }
}
