using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class ContractorViewModel
    {
       public List<ApplicantViewModel> applicantViewModels { get; set; }
    public PayCheckViewModel PayCheck { get; set; }
    }
}
