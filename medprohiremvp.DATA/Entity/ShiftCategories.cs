using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ShiftCategory
    {
        [Key]
        public int Category_ID { get; set; }
        public string CategoryName { get; set; }
    }
}
