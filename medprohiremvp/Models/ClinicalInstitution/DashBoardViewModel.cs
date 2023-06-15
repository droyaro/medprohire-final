using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.Models.ClinicalInstitution;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class DashBoardViewModel
    {
        public List<LocationCounViewModel> AllLocation { get; set; }
        public List<LocationCounViewModel> YearLocation { get; set; }
        public List<LocationCounViewModel> Q1Location { get; set; }
        public List<LocationCounViewModel> Q2Location { get; set; }
        public List<LocationCounViewModel> Q3Location { get; set; }
        public List<LocationCounViewModel> Q4Location { get; set; }
        public ShiftsCountViewModel Allshifts { get; set; }
        public ShiftsCountViewModel Yearshifts { get; set; }
        public ShiftsCountViewModel Q1shifts { get; set; }
        public ShiftsCountViewModel Q2shifts { get; set; }
        public ShiftsCountViewModel Q3shifts { get; set; }
        public ShiftsCountViewModel Q4shifts { get; set; }



    }
}
