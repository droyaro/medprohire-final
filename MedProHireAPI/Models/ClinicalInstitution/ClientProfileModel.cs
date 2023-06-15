using MedProHireAPI.Models.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ClientProfileModel
    {
        public int Institution_ID { get; set; }
        public Guid User_ID { get; set; }
        public ClientProfileDetailModel Profile { get; set; }
        public ChangePasswordModel Password { get; set; }

        public IFormFile Logo { get; set; }
        public string LogoSrc { get; set; }

        public string PhoneNumber { get; set; }
    }
}
