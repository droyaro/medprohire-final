using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Applicant
{
    public class PayCheckViewModel
    {
        public int Paycheck_ID { get; set; }
        [Required]
        public int Applicant_ID { get; set; }
     
    
        [Required]
        [Display(Name = "Check Date")]
        public DateTime PayCheckDate { get; set; }
        [Required]
        [Display(Name = "Check Begin Date")]
        public DateTime PayBeginDate { get; set; }
        [Required]
        [Display(Name = "Check End Date")]
        public DateTime PayEndDate { get; set; }
        [Required]
        [Display(Name = "Net Pay")]
        public string Net_pay { get; set; }
        [Required]
        public IFormFile PaycheckFile { get; set; }
        public string PaycheckFilestring { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
