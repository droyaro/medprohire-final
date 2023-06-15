using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ShiftsCountViewModel
    {
        [Display(Name = "Created")]
       public int  Created { get; set; }
        [Display(Name = "Not Started")]
        public int NotStarted { get; set; }
        [Display(Name = "Completed")]
        public int Completed { get; set; }
        [Display(Name = "Active")]
        public int Active { get; set; }
        [Display(Name = "Expired")]
        public int Incomplete { get; set; }
        [Display(Name = "Cancelled")]
        public int Cancelled { get; set; }


    }
}
