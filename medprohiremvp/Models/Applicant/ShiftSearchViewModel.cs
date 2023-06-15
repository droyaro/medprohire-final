using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.Models.ClinicalInstitution;

namespace medprohiremvp.Models.Applicant
{
    public class ShiftSearchViewModel
    {

        [Display(Name = "Provider")]
        public List<int> Specialities { get; set; }
        [Display(Name = "City")]
        public List<int> Cities { get; set; }
        [Display(Name = "State")]
        public List<int> States { get; set; }
        public int Distance { get; set; }
    }
    public class AppShiftSearchViewModel
    {
        public bool picked { get; set; }
        public ShiftSearchViewModel Search { get; set; }
        public List<AppliedShifts> Shifts { get; set; }
        public List<AppliedShifts> PickedShifts { get; set; }
    }
}
