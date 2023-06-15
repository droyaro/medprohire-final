using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
  public class Notifications
    {
        [Key]
        public int Notification_ID { get; set; }

        public Guid User_ID { get; set; }

        
        public int NotificationTemplate_ID { get; set; }
        [ForeignKey("NotificationTemplate_ID")]
        public NotificationTemplates notificationtemplate { get; set; }
        public byte Status { get; set; }
        public string NotificationTitle { get; set; }
        public String NotificationBody { get; set; }
        public int Special_ID { get; set; }
    }
}
