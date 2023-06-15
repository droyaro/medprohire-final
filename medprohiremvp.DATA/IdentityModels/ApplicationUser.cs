using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.DATA.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace medprohiremvp.DATA.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public int City_ID { get; set; }
        [ForeignKey("City_ID")]
        public Cities Cities { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "ZipCode is not valid")]
        public string ZipCode { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
      public int TimeOffset { get; set; }
        public bool isVerified { get; set; }
        public string Name { get; set; }
        public int ChangesCount { get; set; }
        public bool ChangesLocked { get; set; }
        public DateTime ChangesMakedTime { get; set; }
        public virtual Applicants Applicant { get; set; }
        public virtual ClinicalInstitutions ClinicalInstitution { get; set; }
    }
}
