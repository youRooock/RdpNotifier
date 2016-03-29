using RdpNotifier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RdpNotifier
{
  public class User
  {
    public event Action<NotificationEvent> notificationEvent;
    private string _action;

    private IntPtr OpenServer(String Name)
    {
      IntPtr server = Wts.WTSOpenServer(Name);
      return server;
    }

    private void CloseServer(IntPtr ServerHandle)
    {
      Wts.WTSCloseServer(ServerHandle);
    }

    public void GetInfo(String ServerName)
    {
      IntPtr serverHandle = IntPtr.Zero;

      serverHandle = OpenServer(ServerName);
      try
      {
        IntPtr SessionInfoPtr = IntPtr.Zero;
        Int32 sessionCount = 0;
        Int32 retVal = Wts.WTSEnumerateSessions(serverHandle, 0, 1, ref SessionInfoPtr, ref sessionCount);
        Int32 dataSize = Marshal.SizeOf(typeof(SessionInfo));
        Int32 currentSession = (int)SessionInfoPtr;

        uint bytes = 0;

        if (retVal != 0)
        {
          for (int i = 0; i < sessionCount; i++)
          {
            IntPtr userPtr = IntPtr.Zero;
            IntPtr domainPtr = IntPtr.Zero;

            var si = (SessionInfo)Marshal.PtrToStructure((System.IntPtr)currentSession, typeof(SessionInfo));
            currentSession += dataSize;

            Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSUserName, out userPtr, out bytes);
            Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSDomainName, out domainPtr, out bytes);

            if (si.State == WtsConnectionState.WTSActive)
            {
              var username = Marshal.PtrToStringAnsi(userPtr);
              var domain = Marshal.PtrToStringAnsi(domainPtr);
              _action = " has entered";

              notificationEvent(new SlackNotificationEvent(domain, username, _action, SlackStatus.Good));

              while (true)
              {
                IntPtr state = IntPtr.Zero;
                Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSConnectState, out state, out bytes);
                var status = Marshal.ReadInt32(state);

                if ((WtsConnectionState)status != WtsConnectionState.WTSActive)
                {
                  _action = " has left";
                  notificationEvent(new SlackNotificationEvent(domain, username, _action, SlackStatus.Danger));
                  Wts.WTSFreeMemory(state);
                  break;
                }
                Thread.Sleep(500);
                Wts.WTSFreeMemory(state);
              }
            }

            Wts.WTSFreeMemory(userPtr);
            Wts.WTSFreeMemory(domainPtr);
          }
          Wts.WTSFreeMemory(SessionInfoPtr);
        }
      }

      finally
      {
        CloseServer(serverHandle);
      }
    }
  }
}