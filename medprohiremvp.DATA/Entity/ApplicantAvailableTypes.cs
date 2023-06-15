using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApplicantAvailableTypes
    {
        [Key]
        public int ApplicantAvailableType_ID { get; set; }
        public string ApplicantAvailableType_Name { get; set; }
    }
}
