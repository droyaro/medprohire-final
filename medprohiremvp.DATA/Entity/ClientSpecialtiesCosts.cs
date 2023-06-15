using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class ClientSpecialtiesCosts
    {
        [Key]
        public int Id { get; set; }
        public int ClientSpeciality_ID { get; set; }
        [ForeignKey("ClientSpeciality_ID")]
        public virtual ClientSpecialties ClientSpecialties {get;set;}
        public int ShiftLabel_ID { get; set; }
        [ForeignKey("ShiftLabel_ID")]
        public virtual ShiftLabels ShiftLabels { get; set; }
        public float Cost { get; set; }
    }
}
