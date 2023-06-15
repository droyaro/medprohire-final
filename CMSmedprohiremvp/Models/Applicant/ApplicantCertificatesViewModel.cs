using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class ApplicantCertificateViewModel
    {
       
        public string AppCertificateType { get; set; }
  
        public string CeritifcationImg { get; set; }
    }
}