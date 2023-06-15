using medprohiremvp.Models.ClinicalInstitution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ClientAdditionRegisterModel
    {
       public int Institution_ID { get; set; }
        public List<SpecialtiesCosts> Costs { get; set; }
    }
}
