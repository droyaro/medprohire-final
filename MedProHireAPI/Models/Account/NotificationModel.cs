using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.Account
{
    public class NotificationModel
    {
        public int NotificationTemplate_ID { get; set; }
        public string Notification_Title { get; set; }
        public string Notification_Body { get; set; }
        public string Notification_Action { get; set; }
        public string Notification_Controller { get; set; }
        public int Notification_ID { get; set; }
        public bool  IsNew { get; set; }
    }
}
