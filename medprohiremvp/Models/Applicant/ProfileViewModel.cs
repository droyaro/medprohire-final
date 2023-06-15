using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.Models.Account;
using Microsoft.AspNetCore.Http;


namespace medprohiremvp.Models.Applicant
{
    public class ProfileViewModel
    {
        public string OldResume { get; set; }
        [Display(Name = "New Resume")]
        public string NewResume { get; set; }

        public ApplicantProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel Password { get; set; }
        public List<ApplicantCertificateViewModel> certificates { get; set; }

        public List<ApplicantWorkHistoryViewModel> workHistories { get; set; }

        public List<ApplicantSpecialitisViewModel> specialities { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Imgsrc { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string ApplicantAvailableDays { get; set; }
        public List<ApplicantAvailablesViewModel> ApplicantAvailables { get; set; }
        public string ApplicantAvailableList { get; set; }
    }
  
}
