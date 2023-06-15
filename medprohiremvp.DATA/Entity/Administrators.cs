using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
   public class Administrators
    {
        [Key]
        public Guid Admin_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImg { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
