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
        public SessionInfo Info { get; private set; }
        public Action<UserEvent> userEvent;

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
            IntPtr SessionInfoPtr = IntPtr.Zero;
            IntPtr userPtr = IntPtr.Zero;
            IntPtr state = IntPtr.Zero;
            IntPtr domainPtr = IntPtr.Zero;
            Int32 sessionCount = 0;
            Int32 retVal = Wts.WTSEnumerateSessions(serverHandle, 0, 1, ref SessionInfoPtr, ref sessionCount);
            Int32 dataSize = Marshal.SizeOf(typeof(SessionInfo));
            Int32 currentSession = (int)SessionInfoPtr;

            serverHandle = OpenServer(ServerName);
            try
            {
                uint bytes = 0;

                if (retVal != 0)
                {
                    for (int i = 0; i < sessionCount; i++)
                    {
                        var si = (SessionInfo)Marshal.PtrToStructure((System.IntPtr)currentSession, typeof(SessionInfo));
                        currentSession += dataSize;

                        if (si.State == WtsConnectionState.WTSActive)
                        {
                            Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSUserName, out userPtr, out bytes);
                            Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSDomainName, out domainPtr, out bytes);

                            var username = Marshal.PtrToStringAnsi(userPtr);
                            var domain = Marshal.PtrToStringAnsi(domainPtr);

                            userEvent(new UserEvent(domain, username, "has entered"));

                            while (true)
                            {
                                Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSConnectState, out state, out bytes);
                                var status = Marshal.ReadInt32(state);

                                if ((WtsConnectionState)status != WtsConnectionState.WTSActive)
                                {
                                    userEvent(new UserEvent(domain, username, " has left"));
                                    break;
                                }
                                Thread.Sleep(500);
                            }
                        }

                        Wts.WTSFreeMemory(userPtr);
                        Wts.WTSFreeMemory(domainPtr);
                    }
                }
            }
            finally
            {
                CloseServer(serverHandle);
            }
        }
    }
}