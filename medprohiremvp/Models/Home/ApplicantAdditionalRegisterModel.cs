using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.Models.Applicant;
using Microsoft.AspNetCore.Http;
using medprohiremvp.Models.ValidationAttributes;
namespace medprohiremvp.Models.Home
{
    public class ApplicantAdditionalRegisterModel
    {
        //[Display(Name = "I 9")]
        //[Required (ErrorMessage ="I9 file is required")]
        //public IFormFile I_9 { get; set; }

        //[Display(Name = "W 4")]
        //[Required(ErrorMessage = "W4 file is required")]
        //public IFormFile W_4 { get; set; }

        //[Required (ErrorMessage ="Sub Specialities is required")]
        //public string Sub_specialities{ get; set; }

        //[RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$", ErrorMessage = "SSN is not valid format.")]
        //[RequiredIf("TIN","", ErrorMessage = "SSN or Tax Identification Number is required")]
        //public string SSN { get; set; }
        //[Display(Name ="EIN")]
        //[RegularExpression(@"^\d{2}-?\d{7}$", ErrorMessage = "EIN is not valid format.")]

        //public string TIN { get; set; }
        [Required]
        public List<ApplicantReferencesViewModel> References { get; set; }

    }
}
