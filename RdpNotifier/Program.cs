using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using RdpNotifier.Enums;
using System.Threading;

namespace RdpNotifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User();
            user.userEvent += SendRequest;
            while (true)
            {
                user.GetInfo("webintegration.plarium.local");

                Thread.Sleep(1000);
            }
        }

        static void SendRequest(UserEvent e)
        {
            Console.WriteLine(e.Username + " " + e.Domain);
        }
    }
}