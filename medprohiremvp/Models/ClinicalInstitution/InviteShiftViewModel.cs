using medprohiremvp.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class InviteShiftViewModel
    {
        public ApplicantModel Applicant { get; set; }
        public List<ClientShiftViewModel> ClientShift { get; set; }
        public string ActiveDays { get; set; }
     public string InvitedDays { get; set; }
    }
}
