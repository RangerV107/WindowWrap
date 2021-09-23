using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities.Win
{
    class Psapi
    {
        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

    }
}
