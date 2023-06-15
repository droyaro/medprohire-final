using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class Clockoutpartialviewmodel
    {
        [Display(Name ="Location")]
        public string BranchName { get; set; }
        [Display(Name = "Provider")]
        public string SpecialitiesName { get; set; }
        [Display(Name = "Work Days")]
        public List<DateTime> WorkingDays { get; set; }
        [Display(Name = "Work Day")]
        public DateTime SelectedDay { get; set; }
        [Display(Name = "Clock In Time")]
        public DateTime ClockinTime { get; set; }
        [Display(Name = "Clock Out Time")]
        public DateTime ClockOutTime { get; set; }
        public int clockin_id { get; set; }
        public TimeSpan Clockin_difference { get; set; }
    }
}
