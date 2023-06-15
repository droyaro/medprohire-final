using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class ApplicantWorkHistoryViewModel
    {
        
        public string AppSpeciality{ get; set; }
     
        public string PlaceName { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       public bool UntilNow { get; set; }
    }
}
