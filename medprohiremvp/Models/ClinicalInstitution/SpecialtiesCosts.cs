using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class SpecialtiesCosts
    {
        public int Specialty_ID { get; set; }
        public string SpecialtyName { get; set; }
        public List<ShiftLabelsCost> Cost { get; set; }
   
    }
}
