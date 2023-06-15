using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class ShiftLabels
    {

        [Key]
        public int ShiftLabel_ID { get; set; }

        [Required]
        public string ShiftLabelName { get; set; }  

    }
}
