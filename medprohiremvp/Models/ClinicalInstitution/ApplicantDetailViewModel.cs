using medprohiremvp.Models.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ApplicantDetailViewModel
    {

        public int Applicant_ID { get; set; }

       
        public ApplicantProfileViewModel Applicant { get; set; }

        public List<ApplicantCertificateViewModel> certificates { get; set; }

        public List<ApplicantWorkHistoryViewModel> workHistories { get; set; }

        public List<ApplicantSpecialitisViewModel> specialities { get; set; }
    }
}
