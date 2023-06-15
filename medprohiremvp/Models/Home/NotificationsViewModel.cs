using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;

namespace medprohiremvp.Models.Home
{
    public class NotificationsViewModel
    {
        public List<NotificationTemplateModel> newNotifications { get; set; }
        public List<NotificationTemplateModel> oldNotifications { get; set; }
    }
}
