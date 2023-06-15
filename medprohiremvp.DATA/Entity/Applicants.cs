using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using medprohiremvp.DATA.IdentityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace medprohiremvp.DATA.Entity
{
    public class Applicants: Datefields
    {
        [Key]
        public int Applicant_ID { get; set; }
        [Required]
        public Guid User_ID { get; set; }
        [ForeignKey("User_ID")]
        public ApplicationUser User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public int Availability_ID { get; set; }
        [ForeignKey("Availability_ID")]
        public Availabilities Availability { get; set; }

        public bool BackgroundCheck { get; set; }

        public bool Drugscreen { get; set; }

        public int? DrugscreenStatus_ID { get; set; }
        [ForeignKey("DrugscreenStatus_ID")]
        public DrugscreenStatuses DrugscreenStatus { get; set; }

        public bool WorkAuth { get; set; }
        [Required]
        public int VisaStatus_ID { get; set; }
        [ForeignKey("VisaStatus_ID")]
        public VisaStatuses VisaStatus { get; set; }
        public bool IsEligible { get; set; }

        [Encrypted]
        public string PreferredID { get; set; }
        [Encrypted]
        public string SSN { get; set; }
        [Encrypted]
        public string TIN { get; set; }
        public bool CEU { get; set; }
        public string ProfileImage { get; set; }
        public int Status_ID { get; set; }
        public string I_9 { get; set; }
        public string W_4 { get; set; }
        public string Contract { get; set; }
        public string Employment_agreement { get; set; }
        public string Sub_specialities { get; set; }
        public bool E_verify { get; set; }
        public int BoardingProcess { get; set; }

        public bool Atwork { get; set; }
        public string Resume { get; set; }
        public bool IsAvailable { get; set; }
        public bool Picked { get; set; }


        public virtual List<ApplicantCertificates> Certificates { get; set; }
        public virtual List<ApplicantSpecialities> Specialities { get; set; }
        public virtual List<ApplicantWorkHistories> WorkHistories { get; set; }
        public virtual List<ApplicantAvailables> ApplicantAvailables { get; set; }



    }
}
