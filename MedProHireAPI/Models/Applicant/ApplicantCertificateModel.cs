using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantCertificateModel
    {
        public string CertificateType { get; set; }

        public IFormFile CeritifcationImg { get; set; }
        public string CeritifcationImgsrc { get; set; }
        public int Certification_ID { get; set; }
        public int Applicant_ID { get; set; }
    }
}
