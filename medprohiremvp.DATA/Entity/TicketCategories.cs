using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class TicketCategories
    {
        [Key]
        public int TicketCategory_ID { get; set; }
        public string TicketCategoryName { get; set; }
        public bool IsClient { get; set; }
    }
}
