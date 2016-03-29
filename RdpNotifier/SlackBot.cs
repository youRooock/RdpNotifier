using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class SlackBot
    {
        public string Channel { get; private set; }
        public string Name { get; private set; }
        public string IconUrl { get; private set; }
        public string Token { get; private set; }

        public SlackBot(string channel, string name, string token, string iconUrl)
        {
            this.Channel = channel;
            this.Name = name;
            this.Token = token;
            this.IconUrl = iconUrl;
        }
    }
}
