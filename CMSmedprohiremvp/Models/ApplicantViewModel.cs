using medprohiremvp.DATA.Entity;
using CMSmedprohiremvp.Models.Applicant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSmedprohiremvp.Models
{
    public class ApplicantViewModel
    {
        public int Applicant_ID { get; set; }

        public Guid User_ID { get; set; }

        // [RegularExpression(@"^[\w]*[A-z]+[\s()-]*\w+$", ErrorMessage ="Name must contain at least one letter")]
        [Required(ErrorMessage = "Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool WorkAuth { get; set; }
       
        public string PreferredID { get; set; }

        [RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$", ErrorMessage = "SSN is not valid format.")]
    
        public string SSN { get; set; }

        [RegularExpression(@"^(\d+-?\d+)*$", ErrorMessage = "TIN is not valid format.")]
       
        public string TIN { get; set; }

      
        public bool CEU { get; set; }
        
        public string Imgsrc { get; set; }

        [Required(ErrorMessage = "VisaStatus_ID is required")]
        public int VisaStatus_ID { get; set; }
        public string Visastatus { get; set; }
        public bool IsEligible { get; set; }

        [Required(ErrorMessage = "Availability_ID is required")]
        public int Availability_ID { get; set; }
        public string Availability { get; set; }

        public List<ApplicantCertificateViewModel> certificates { get; set; }

        public List<ApplicantWorkHistoryViewModel> workHistories { get; set; }

        public List<ApplicantSpecialitisViewModel> specialities { get; set; }
        public List<ApplicantReferencesViewModel> references { get; set; }

        [Display(Name = "Specialities")]
        public string SpecialitiesString { get; set; }

        [Display(Name = "Certificate Types")]
        public string CertificatiesString { get; set; }

        public bool BackgroundCheck { get; set; }

        public bool Drugscreen { get; set; }

        public string DrugscreenStatus { get; set; }
        public string Status { get; set; }
        public string I_9 { get; set; }
        public string W_4 { get; set; }
        public string Contract { get; set; }
        public string Resume { get; set; }
        public string Employment_agreement { get; set; }
        public string Sub_specialities { get; set; }
        public bool E_verify { get; set; }
        public int BoardingProcess { get; set; }

        public int City_ID { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"\d{5}-?(\d{4})?$", ErrorMessage = "ZipCode is not valid")]
        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public string Type_new_old { get; set; }

        public List<ApplicantAppliedShiftsViewModel> AppliedShifts { get; set; }
        public int AppliedShiftCount { get; set; }
        public bool Applied { get; set; }
    }
}
