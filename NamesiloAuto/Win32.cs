using System;
using System.Runtime.InteropServices;

namespace NamesiloAuto
{
    public class Win32
    {
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
                                                     string lpszClass, string lpszWindow);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
    }
}
