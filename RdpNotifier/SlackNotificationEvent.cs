using RdpNotifier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class SlackNotificationEvent : NotificationEvent
    {
        public string SlackStatus { get; private set; }

        public SlackNotificationEvent(string username, string domain, string action, SlackStatus status) 
            : base(username, domain, action)
        {
            SlackStatus = status.ToString().ToLower();
        }
    }
}
