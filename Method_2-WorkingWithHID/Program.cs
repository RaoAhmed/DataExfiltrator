using System;
using System.Runtime.InteropServices;


namespace Method_2_WorkingWithHID
{
    internal class Program
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern short GetKeyState(int keyCode);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int extraInfo);

        const int VK_CAPITAL = 0x14;
        const uint KEYEVENTF_EXTENDEDKEY = 0x1;
        const uint KEYEVENTF_KEYUP = 0x2;
        static void Main(string[] args)
        {
            //keybd_event(VK_CAPITAL, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
            //// Simulate key release
            //keybd_event(VK_CAPITAL, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

            Console.WriteLine(IsScrollLock());


        }
        public static bool IsScrollLock()
        {
            return (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
        }
    }
}
