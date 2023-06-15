using MedProHireAPI.Models.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Applicant
{
    public class ApplicantBoardingProcessModel
    {

        public Guid User_ID { get; set; }
        [RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$", ErrorMessage = "SSN is not valid format.")]
        [RequiredIf("TIN", "", ErrorMessage = "SSN or EIN is required")]
        public string SSN { get; set; }
        [Display(Name = "EIN")]
        [RegularExpression(@"^\d{2}-?\d{7}$", ErrorMessage = "EIN is not valid format.")]

        public string TIN { get; set; }
        [Required]
        public List<ApplicantReferenceModel> References { get; set; }
    }
}
