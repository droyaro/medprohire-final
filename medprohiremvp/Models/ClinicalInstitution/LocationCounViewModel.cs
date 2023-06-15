using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class LocationCounViewModel
    {
        [Display(Name = "Location ID")]
        public int Location_ID { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Display(Name = "Active Reqs")]
        public int ActiveShiftsCount { get; set; }
        [Display(Name = "Completed Reqs")]
        public int CompletedShiftsCount { get; set; }
        [Display(Name = "Created Reqs")]
        public int CreatedShiftsCount { get; set; }
    }
}
