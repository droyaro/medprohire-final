using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    public class NotificationTemplates
    {
        [Key]
       public int NotificationTemplate_ID { get; set; }

       public string NotificationTitle { get; set; }

        public string NotificationSmBody { get; set; }

        public string NotificationBody { get; set; }
        public string NotificationAction { get; set; }
        public string NotificationController { get; set; }

    }
}
