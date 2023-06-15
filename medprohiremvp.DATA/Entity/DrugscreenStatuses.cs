using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class DrugscreenStatuses
    {
        [Key]
        public int DrugscreenStatus_ID { get; set; }
        [Required]
        public string DrugscreenStatus { get; set; }
    }
}
