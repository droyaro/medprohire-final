using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Clinical
{
    public class InviteShiftViewModel
    {
        public List<ApplicantViewModel> Applicants { get; set; }
        public ClientShiftViewModel Shift { get; set; }
    }
}
