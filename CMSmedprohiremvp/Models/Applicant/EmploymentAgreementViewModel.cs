using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;

namespace CMSmedprohiremvp.Models.Applicant
{
    public class EmploymentAgreementViewModel
    {
        public Guid Employee_ID { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_address { get; set; }
        public string Employer_Name { get; set; }
        public string Employer_address { get; set; }
        public string Hourly_rate { get; set; }
        public List<int> specialities { get; set; }

        public DateTime InsertDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
