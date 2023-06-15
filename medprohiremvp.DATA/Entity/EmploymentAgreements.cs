using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class EmploymentAgreements
    {
        [Key]
        public int EmploymentAgreement_ID { get; set; }
        public Guid User_ID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeAddress { get; set; }
        public string HourlyRate { get; set; }
        public string Position { get; set; }
       
        public DateTime StartDate { get; set; }

    }
}
