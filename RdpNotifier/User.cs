using RdpNotifier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class User
    {
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
            List<String> resultList = new List<string>();
            serverHandle = OpenServer(ServerName);

            try
            {
                IntPtr SessionInfoPtr = IntPtr.Zero;
                IntPtr userPtr = IntPtr.Zero;
                IntPtr domainPtr = IntPtr.Zero;
                Int32 sessionCount = 0;
                Int32 retVal = Wts.WTSEnumerateSessions(serverHandle, 0, 1, ref SessionInfoPtr, ref sessionCount);
                Int32 dataSize = Marshal.SizeOf(typeof(SessionInfo));
                Int32 currentSession = (int)SessionInfoPtr;

                uint bytes = 0;

                if (retVal != 0)
                {
                    for (int i = 0; i < sessionCount; i++)
                    {
                        var si = (SessionInfo)Marshal.PtrToStructure((System.IntPtr)currentSession, typeof(SessionInfo));
                        currentSession += dataSize;

                        Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSUserName, out userPtr, out bytes);
                        Wts.WTSQuerySessionInformation(serverHandle, si.SessionID, WtsInfo.WTSDomainName, out domainPtr, out bytes);

                        if (si.State == WtsConnectionState.WTSActive)
                        {
                            userEvent(new UserEvent(Marshal.PtrToStringAnsi(userPtr), Marshal.PtrToStringAnsi(domainPtr)));
                            Console.WriteLine("User: " + Marshal.PtrToStringAnsi(domainPtr) + "\\" + Marshal.PtrToStringAnsi(userPtr));
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

            //Console.ReadLine(); 
        }
    }
}
