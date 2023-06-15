using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Models.Applicant;
using medprohiremvp.Models.Home;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class SearchModel
    {
       
        
        [Required(ErrorMessage = "Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

         public int Distance { get; set; }

        public string Certificates { get; set; }

        public List<int> Specialities { get; set; }

        public List<int> VisaStatuses { get; set; }
        public string Date { get; set; }
       
        public int Location { get; set; }
        public string ZipCode { get; set; }
    }
    public class SearchViewModel
    {
        public SearchModel Search{ get; set; }
        public List<ApplicantModel> Applicants { get; set; }
    }
}
