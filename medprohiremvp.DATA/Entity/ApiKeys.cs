using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ApiKeys
    {
        [Key]
        public Guid ApiKey { get; set; }
        public string Role { get; set; }
    }
}
