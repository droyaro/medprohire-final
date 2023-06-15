using MedProHireAPI.Models.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantProfileModel
    {
        public Guid User_ID { get; set; }
        public int Applicant_ID { get; set; }
        public ApplicantProfileDetailModel Profile { get; set; }
        public ChangePasswordModel Password { get; set; }
        public List<ApplicantCertificateModel> certificates { get; set; }

        public List<ApplicantWorkHistoryModel> workHistories { get; set; }

        public List<ApplicantSpecialtyModel> specialities { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Imgsrc { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsAvailable { get; set; }
      
        public List<ApplicantAvailableDatesModel> ApplicantAvailables { get; set; }
       
    }
}
