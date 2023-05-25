using System;
using System.Runtime.InteropServices;

namespace ForegroundTimeTracker.Utils
{
    public class IdleDetectHelper
    {
        // Import the required Win32 API functions
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            if (!GetLastInputInfo(ref lastInputInfo))
            {
                throw new Exception("Failed to get last input info");
            }

            uint idleTime = (uint)Environment.TickCount - lastInputInfo.dwTime;
            return TimeSpan.FromMilliseconds(idleTime);
        }
    }

}