using Newtonsoft.Json;
using RdpNotifier.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class SlackNotifier : INotifier
    {
        private readonly string _slackApiUrl;
        private readonly SlackBot _slackBot;
        
        public SlackNotifier(string url, SlackBot bot)
        {
            this._slackApiUrl = url;
            this._slackBot = bot;
        }

        public void Send(object obj)
        {
            var notificationEvent = (SlackNotificationEvent)obj;

            var model = new SlackMessageModel
            {
                Channel = _slackBot.Channel,
                IconUrl = _slackBot.IconUrl,
                Username =  _slackBot.Name,
                Token = _slackBot.Token,
                Attachments = new List<AttachmentsModel>()
                {
                    new AttachmentsModel
                    {
                        Color = notificationEvent.SlackStatus,
                        Text = notificationEvent.Domain + "\\" + notificationEvent.Username + notificationEvent.Action
                    }
                }
            };

            var dictionary = GetValues(model);

            using (var wb = new WebClient())
            {
                var response = wb.UploadValues("https://slack.com/api/chat.postMessage", "POST", dictionary);
            }
        }

        private NameValueCollection GetValues(SlackMessageModel model)
        {
            var dictionary = new NameValueCollection();
            object value;

            foreach (var field in typeof(SlackMessageModel).GetFields())
            {
                foreach (var attr in field.GetCustomAttributes(true))
                {
                    if (attr.GetType().Name == "RequestFieldAttribute")
                    {
                        var request = attr as RequestFieldAttribute;

                        if ((bool)attr.GetType().GetProperty("IsJson").GetValue(attr) == true)
                        {
                            value = field.GetValue(model);
                            if (value != null)
                                dictionary.Add(request.RequestField, JsonConvert.SerializeObject(value));
                            break;
                        }

                        value = field.GetValue(model);
                        if(value!=null)
                            dictionary.Add(request.RequestField, value.ToString());
                    }
                }
            }

            return dictionary;
        }
    }
}
