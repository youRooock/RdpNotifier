using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using RdpNotifier.Enums;
using System.Threading;
using System.Configuration;

namespace RdpNotifier
{
    class Program
    {
        private static string _server = ConfigurationManager.AppSettings["server"];

        static void Main(string[] args)
        {
            var bot = new SlackBot(ConfigurationManager.AppSettings["channel"], 
                ConfigurationManager.AppSettings["botName"], ConfigurationManager.AppSettings["slackBotToken"], 
                ConfigurationManager.AppSettings["iconUrl"]);

            INotifier notifier = new SlackNotifier(ConfigurationManager.AppSettings["slackApiUrl"], bot);
            var user = new User();
            user.notificationEvent += notifier.Send;
            while (true)
            {
                user.GetInfo(_server);

                Thread.Sleep(1000);
            }
        }
    }
}