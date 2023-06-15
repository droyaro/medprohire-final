using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.Home
{
    public class NotificationTemplateModel
    {
        public int NotificationTemplate_ID { get; set; }

        public string Notification_Title { get; set; }

        public string Notification_sm_Body { get; set; }

        public string Notification_Body { get; set; }
        public string Notification_Action { get; set; }
        public string Notification_controller { get; set; }
        public int Notification_ID { get; set; }
    }
}
