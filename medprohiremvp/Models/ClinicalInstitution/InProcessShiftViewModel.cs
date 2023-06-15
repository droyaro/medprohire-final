using medprohiremvp.DATA.Entity;
using medprohiremvp.Models.Applicant;
using medprohiremvp.Models.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class InProcessShiftViewModel
    {
        public int AppliedShift_ID { get; set; }
        [Display(Name= "Applicant ID")]
        public int Applicant_ID { get; set; }
        [Display(Name = "Req ID")]
        public int ClientShift_ID { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }
        [Display(Name = "Provider")]
        public string SpecialitiesString { get; set; }
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        public string Img { get; set; }
        [Display(Name = "Number of Shift")]
        public int NumberofShift { get; set; }
        [Display(Name = "Completed Shift")]
        public int CompletedNumberofShift { get; set; }
        [Display(Name = "Worked Hours")]
        public string WorkedHours { get; set; }
        public List<ApplicantClockInClockOutTime> ClockinClockOutTimes { get; set; }
        public ApplicantProfileViewModel Applicant { get; set; }
        public ClientShiftViewModel Shift { get; set; }

        public List<ApplicantCertificateViewModel> certificates { get; set; }

        public List<ApplicantWorkHistoryViewModel> workHistories { get; set; }

        public List<ApplicantSpecialitisViewModel> specialities { get; set; }
    }
}
