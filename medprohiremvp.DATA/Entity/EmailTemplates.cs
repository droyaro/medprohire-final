using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class EmailTemplates
    {
        [Key]
        public int EmailTemplate_ID { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }
    }
}
