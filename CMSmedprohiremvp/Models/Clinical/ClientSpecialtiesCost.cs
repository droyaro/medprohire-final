using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Clinical
{
    public class ClientSpecialtiesCost
    {
        public int Specialty_ID { get; set; }
        public string SpecialtyName { get; set; }
        public List<ShiftLabelsCost> Cost { get; set; }
    }
}
