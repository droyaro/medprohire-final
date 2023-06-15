using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSmedprohiremvp.Models.Applicant;

namespace CMSmedprohiremvp.Models
{
    public class UsersListModel
    {
        public List<ApplicantViewModel> ApplicantViewModels { get; set; }
        public EmploymentAgreementViewModel Employment{ get; set; }
    }
}
