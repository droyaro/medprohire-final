using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class PayCheckModel
    {
        public int Paycheck_ID { get; set; }
        [Required]
        public int Applicant_ID { get; set; }


        [Required]
        public DateTime PayCheckDate { get; set; }
        [Required]
        public DateTime PayBeginDate { get; set; }
        [Required]
        public DateTime PayEndDate { get; set; }
        [Required]
        public string Net_pay { get; set; }
        [Required]
        public IFormFile PaycheckFile { get; set; }
        public string PaycheckFilestring { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
