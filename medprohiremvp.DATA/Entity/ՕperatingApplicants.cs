using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ՕperatingApplicants
    {
        [Key]
        public int Operating_ID { get; set; }

        public int Applicant_ID { get; set; }
        [ForeignKey("Applicant_ID")]
        public Applicants Applicant { get; set; }

        public int ClientShift_ID { get; set; }
        [ForeignKey("ClientShift_ID")]
        public ClientShifts ClientShift { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DataChoose { get; set; }
    }
}
