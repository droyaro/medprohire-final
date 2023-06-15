using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class SignSent
    {
        [Key]
        public Guid SignSended_ID { get; set; }
        public Guid User_ID { get; set; }
        public string Envelope_ID { get; set; }
        public string Status { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int Emp_XPosition { get; set; }
        public int Emp_YPosition { get; set; }
        public int Emp_PageNumber { get; set; }

    }
}
