using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class AcceptedShiftViewModel
    {
      public  List<string> AcceptedShiftDays { get; set; }
      public  AccpetedShiftClockinViewModel ActiveShift { get; set; }
      public  List<AccpetedShiftClockinViewModel> nearbyshifts { get; set; }
        public List <AccpetedShiftClockinViewModel> notclockinshifts { get; set; }
    }
}
