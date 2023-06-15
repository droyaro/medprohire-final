using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using medprohiremvp.DATA.IdentityModels;

namespace medprohiremvp.DATA.Entity
{
    public class Cities
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MaxLength(150)]
        public string city_name { get; set; }
        
        public int state_id { get; set; }
        [ForeignKey("state_id")]
        public States state { get; set; }

        public int country_id { get; set; }
        [ForeignKey("country_id")]
        public Countries country { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        public string MaxZipCode { get; set; }
        public string MinZipCode { get; set; }

        //public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
