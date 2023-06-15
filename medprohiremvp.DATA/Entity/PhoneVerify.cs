using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class PhoneVerify
    {
        [Key]
        public Guid PhoneVerify_ID { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
    }
}
