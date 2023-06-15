using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class Countries
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MaxLength(3)]
        public string shortname { get; set; }
        [Required]
        [MaxLength(150)]
        public string name { get; set; }

        public virtual List<States> States { get; set; }
        public virtual List<Cities> Cities { get; set; }
    }
}
