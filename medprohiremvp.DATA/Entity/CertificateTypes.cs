using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class CertificateTypes
    {
        [Key]
        public int Certificate_ID { get; set; }
        [Required]
        public string CertificateTypeName { get; set; }
      
    }
}
