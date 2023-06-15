using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ShiftLabelsCost
    {
        public int ShiftLabel_ID { get; set; }
        public string ShiftLabelName { get; set; }
        [DefaultValue(0)]
        public float? Cost { get; set; }
    }
}
