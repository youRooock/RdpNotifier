﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class UserEvent : EventArgs
    {
        public string Username { get; private set; }
        public string Domain { get; private set; }

        public UserEvent(string user, string domain)
        {
            Username = user;
            Domain = domain;
        }

    }
}