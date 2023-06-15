using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ApplicantCertificates
    {
        [Key]
        public int Ceritification_ID { get; set; }
        [Required]
        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }
        [Required]
        public string CertificateTypes { get; set; }
        public int CertificateType_ID { get; set; }
        [ForeignKey("CertificateType_ID")]
        public CertificateTypes CertificateType { get; set; }
        [Required]
        public string CeritificationImg { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
