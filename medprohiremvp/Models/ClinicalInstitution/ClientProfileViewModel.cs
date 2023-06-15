using medprohiremvp.Models.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class ClientProfileViewModel
    {
        public ClinicalInstitutionProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel Password { get; set; }
        public List<SpecialtiesCosts> Specialties { get; set; }

        public IFormFile Logo { get; set; }
        public string LogoSrc { get; set; }

        public string PhoneNumber { get; set; }
    }
}
