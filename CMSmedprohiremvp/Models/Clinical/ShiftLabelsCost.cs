using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Clinical
{
    public class ShiftLabelsCost
    {
        public int ShiftLabel_ID { get; set; }
        public string ShiftLabelName { get; set; }
       
        public float? Cost { get; set; }
    }
}
