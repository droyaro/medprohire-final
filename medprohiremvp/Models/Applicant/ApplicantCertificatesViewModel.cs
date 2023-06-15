using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class ApplicantCertificateViewModel
    {
       [Display(Name = "Certification Name")]
        public string CertificateType{ get; set; }
        [Display(Name = "Ceritifcate Image (Optional)")]
        public IFormFile CeritifcationImg { get; set; }
        [Display(Name = "Ceritifcate Image (Optional)")]
        public string CeritifcationImgsrc { get; set; }
        public int Certification_ID { get; set; }
        public int Applicant_ID { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
    }
}