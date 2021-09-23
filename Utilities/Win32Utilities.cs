using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities.Win;

namespace Utilities
{
    public class Win32Utilities
    {
        public static string GetWindowModuleFileName(IntPtr hWnd)
        {
            uint processId = 0;
            const int nChars = 1024;
            StringBuilder filename = new StringBuilder(nChars);
            User32.GetWindowThreadProcessId(hWnd, out processId);
            IntPtr hProcess = Kernel32.OpenProcess(1040, 0, processId);
            Psapi.GetModuleFileNameEx(hProcess, IntPtr.Zero, filename, nChars);
            Kernel32.CloseHandle(hProcess);
            return (filename.ToString());
        }

        public static IDictionary<string, IntPtr> GetOpenWindows()
        {
            IntPtr shellWindow = User32.GetShellWindow();
            //IntPtr selfWindow = Process.GetCurrentProcess().MainWindowHandle;
            Dictionary<string, IntPtr> windows = new Dictionary<string, IntPtr>();

            User32.EnumWindows((hWnd, lParam) =>
            {
                if (hWnd == shellWindow) return true;
                //if (hWnd == selfWindow) return true;
                if (!User32.IsWindowVisible(hWnd)) return true;

                int length = User32.GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                User32.GetWindowText(hWnd, builder, length + 1);

                var procName = "";
                try
                {
                    var path = GetWindowModuleFileName(hWnd);
                    procName = Path.GetFileName(path) + "::";
                }
                catch { }

                windows[procName + builder.ToString()] = hWnd;
                return true;

            }, IntPtr.Zero);

            return windows;
        }


    }
}
