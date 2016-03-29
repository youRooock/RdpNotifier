using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class NotificationEvent : EventArgs
    {
        public string Username { get; private set; }
        public string Domain { get; private set; }
        public string Action { get; private set; }

        public NotificationEvent(string user, string domain, string action)
        {
            Username = user;
            Domain = domain;
            Action = action;
        }

    }
}
