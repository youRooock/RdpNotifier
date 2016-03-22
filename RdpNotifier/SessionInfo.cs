using RdpNotifier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    [StructLayout(LayoutKind.Sequential)]
    public class SessionInfo
    {
        public Int32 SessionID;

        [MarshalAs(UnmanagedType.LPStr)]
        public String pWinStationName;

        public WtsConnectionState State;
    }
}
