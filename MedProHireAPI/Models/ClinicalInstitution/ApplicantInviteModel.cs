using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ClinicalInstitution
{
    public class ApplicantInviteModel
    {
        public ApplicantSearchModel Applicant {get;set;}
        public List<InviteShiftModel> Shifts { get; set; }
 
    }
}
