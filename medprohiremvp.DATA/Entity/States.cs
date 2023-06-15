using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class States
    {
        [Key]
        public int id { get; set; }

        public int country_id { get; set; }
        [ForeignKey("country_id")]
        public Countries country { get; set; }
        [Required]
        [MaxLength(150)]
        public string state_name {get; set;}

        public virtual List<Cities> Cities { get; set; }
    }
}
