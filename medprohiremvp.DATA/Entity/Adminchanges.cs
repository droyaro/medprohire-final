using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class AdminChanges: Datefields
    {
        [Key]
        public int AdminChanges_ID { get; set; }
        public Guid Admin_ID { get; set; }
        public Guid User_ID { get; set; }
        public string Changes { get; set; }

    }
}
