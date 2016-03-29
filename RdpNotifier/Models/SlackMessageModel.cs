using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier.Models
{
    public class SlackMessageModel
    {
        [RequestField("token")]
        public string Token;

        [RequestField("channel")]
        public string Channel;

        [RequestField("text")]
        public string Text;

        [RequestField("username")]
        public string Username;

        [RequestField("icon_url")]
        public string IconUrl;

        [RequestField("attachments", true)]
        public List<AttachmentsModel> Attachments;
    }
}
