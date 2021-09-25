using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Utilities.Win;

namespace Utilities
{
    //public class WinInfo : INotifyPropertyChanged
    //{
    //    #region Title
    //    private string _title;
    //    public string Title
    //    {
    //        get => _title;
    //        set => Set(ref _title, value);
    //    }
    //    #endregion

    //    #region Process
    //    private string _process;
    //    public string Process
    //    {
    //        get => _process;
    //        set => Set(ref _process, value);
    //    }
    //    #endregion

    //    #region Ptr
    //    private IntPtr _ptr;
    //    public IntPtr Ptr
    //    {
    //        get => _ptr;
    //        set => Set(ref _ptr, value);
    //    }
    //    #endregion

    //    public override string ToString()
    //    {
    //        return $"Title: {Title}, Process: {Process}, Ptr: {Ptr}";
    //    }


    //    #region INotifyPropertyChanged
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    //    }
    //    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string property = null)
    //    {
    //        if (Equals(field, value)) return false;
    //        field = value;
    //        OnPropertyChanged(property);
    //        return true;
    //    }
    //    #endregion
    //}
    public class WinInfo
    {
        public string Title { get; set; }
        public string Process { get; set; }
        public IntPtr Ptr { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, Process: {Process}, Ptr: {Ptr}";
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            return (this.Ptr == (obj as WinInfo).Ptr);
        }
        public override int GetHashCode()
        {
            return this.Ptr.GetHashCode();
        }
    }

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

        //public static IDictionary<string, IntPtr> GetOpenWindows()
        //{
        //    IntPtr shellWindow = User32.GetShellWindow();
        //    //IntPtr selfWindow = Process.GetCurrentProcess().MainWindowHandle;
        //    Dictionary<string, IntPtr> windows = new Dictionary<string, IntPtr>();

        //    User32.EnumWindows((hWnd, lParam) =>
        //    {
        //        if (hWnd == shellWindow) return true;
        //        //if (hWnd == selfWindow) return true;
        //        if (!User32.IsWindowVisible(hWnd)) return true;

        //        int length = User32.GetWindowTextLength(hWnd);
        //        if (length == 0) return true;

        //        StringBuilder builder = new StringBuilder(length);
        //        User32.GetWindowText(hWnd, builder, length + 1);

        //        var procName = "";
        //        try
        //        {
        //            var path = GetWindowModuleFileName(hWnd);
        //            procName = Path.GetFileName(path) + "::";
        //        }
        //        catch { }

        //        windows[procName + builder.ToString()] = hWnd;
        //        return true;

        //    }, IntPtr.Zero);

        //    return windows;
        //}
        public static List<WinInfo> GetOpenWindows()
        {
            IntPtr shellWindow = User32.GetShellWindow();
            //IntPtr selfWindow = Process.GetCurrentProcess().MainWindowHandle;
            List<WinInfo> windows = new List<WinInfo>();

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
                    procName = Path.GetFileName(path);
                }
                catch { }

                //windows[builder.ToString() + "::" + procName + "::" + hWnd] = hWnd;
                windows.Add(new WinInfo
                {
                    Title = builder.ToString(),
                    Process = procName,
                    Ptr = hWnd
                });
                return true;

            }, IntPtr.Zero);

            return windows;
        }

        public static List<WinInfo> GetChildWindows(IntPtr parent)
        {
            List<WinInfo> windows = new List<WinInfo>();

            User32.EnumChildWindows(parent, (hWnd, lParam) =>
            {
                //if (!User32.IsWindowVisible(hWnd)) return true;

                int length = User32.GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                User32.GetWindowText(hWnd, builder, length + 1);

                var procName = "";
                try
                {
                    var path = GetWindowModuleFileName(hWnd);
                    procName = Path.GetFileName(path);
                }
                catch { }

                //windows[builder.ToString() + "::" + procName + "::" + hWnd] = hWnd;
                windows.Add(new WinInfo
                {
                    Title = builder.ToString(),
                    Process = procName,
                    Ptr = hWnd
                });
                return true;

            }, IntPtr.Zero);

            return windows;
        }


    }
}
