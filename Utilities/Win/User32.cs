using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities.Win
{
    public class User32 //https://www.pinvoke.net/index.aspx
    {

        #region Structs
        /// <summary>
        /// Window handles (HWND) used for hWndInsertAfter
        /// </summary>
        public static class HWND
        {
            public static IntPtr
            NoTopMost = new IntPtr(-2),
            TopMost = new IntPtr(-1),
            Top = new IntPtr(0),
            Bottom = new IntPtr(1);
        }

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;
        }


        public static class WindowLongFlags
        {
            public static int
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4;
        }

        public static class WindowLongFlagsExtend
        {
            //https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles
            public static int
            GWL_STYLE = -16,
            WS_VISIBLE = 0x10000000,        //The window is initially visible.
            WS_EX_NOACTIVATE = 0x08000000,
            WS_CAPTION = 0x00C00000,        //The window has a title bar (includes the WS_BORDER style).
            WS_MAXIMIZE = 0x01000000,
            WS_OVERLAPPED = 0x00C00000,     //The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
            WS_SYSMENU = 0x00080000,        //The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
            WS_THICKFRAME = 0x00040000,
            WS_MINIMIZEBOX = 0x00020000,    //The window has a minimize button.Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CLIPCHILDREN = 0x02000000,   //Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.

            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        }


        public struct RECT
        {
            public int left;       // это нужная X координата 
            public int top;        // это нужная Y координата
            public int right;       // x position of lower-right corner
            public int bottom;      // y position of lower-right corner
        }
        #endregion


        /// <summary>
        ///     Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered
        ///     according to their appearance on the screen. The topmost window receives the highest rank and is the first window
        ///     in the Z order.
        ///     <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545%28v=vs.85%29.aspx for more information.</para>
        /// </summary>
        /// <param name="hWnd">C++ ( hWnd [in]. Type: HWND )<br />A handle to the window.</param>
        /// <param name="hWndInsertAfter">
        ///     C++ ( hWndInsertAfter [in, optional]. Type: HWND )<br />A handle to the window to precede the positioned window in
        ///     the Z order. This parameter must be a window handle or one of the following values.
        ///     <list type="table">
        ///     <itemheader>
        ///         <term>HWND placement</term><description>Window to precede placement</description>
        ///     </itemheader>
        ///     <item>
        ///         <term>HWND_BOTTOM ((HWND)1)</term>
        ///         <description>
        ///         Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost
        ///         window, the window loses its topmost status and is placed at the bottom of all other windows.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_NOTOPMOST ((HWND)-2)</term>
        ///         <description>
        ///         Places the window above all non-topmost windows (that is, behind all topmost windows). This
        ///         flag has no effect if the window is already a non-topmost window.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_TOP ((HWND)0)</term><description>Places the window at the top of the Z order.</description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_TOPMOST ((HWND)-1)</term>
        ///         <description>
        ///         Places the window above all non-topmost windows. The window maintains its topmost position
        ///         even when it is deactivated.
        ///         </description>
        ///     </item>
        ///     </list>
        ///     <para>For more information about how this parameter is used, see the following Remarks section.</para>
        /// </param>
        /// <param name="X">C++ ( X [in]. Type: int )<br />The new position of the left side of the window, in client coordinates.</param>
        /// <param name="Y">C++ ( Y [in]. Type: int )<br />The new position of the top of the window, in client coordinates.</param>
        /// <param name="cx">C++ ( cx [in]. Type: int )<br />The new width of the window, in pixels.</param>
        /// <param name="cy">C++ ( cy [in]. Type: int )<br />The new height of the window, in pixels.</param>
        /// <param name="uFlags">
        ///     C++ ( uFlags [in]. Type: UINT )<br />The window sizing and positioning flags. This parameter can be a combination
        ///     of the following values.
        ///     <list type="table">
        ///     <itemheader>
        ///         <term>HWND sizing and positioning flags</term>
        ///         <description>Where to place and size window. Can be a combination of any</description>
        ///     </itemheader>
        ///     <item>
        ///         <term>SWP_ASYNCWINDOWPOS (0x4000)</term>
        ///         <description>
        ///         If the calling thread and the thread that owns the window are attached to different input
        ///         queues, the system posts the request to the thread that owns the window. This prevents the calling
        ///         thread from blocking its execution while other threads process the request.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_DEFERERASE (0x2000)</term>
        ///         <description>Prevents generation of the WM_SYNCPAINT message. </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_DRAWFRAME (0x0020)</term>
        ///         <description>Draws a frame (defined in the window's class description) around the window.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_FRAMECHANGED (0x0020)</term>
        ///         <description>
        ///         Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message
        ///         to the window, even if the window's size is not being changed. If this flag is not specified,
        ///         WM_NCCALCSIZE is sent only when the window's size is being changed
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_HIDEWINDOW (0x0080)</term><description>Hides the window.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOACTIVATE (0x0010)</term>
        ///         <description>
        ///         Does not activate the window. If this flag is not set, the window is activated and moved to
        ///         the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
        ///         parameter).
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOCOPYBITS (0x0100)</term>
        ///         <description>
        ///         Discards the entire contents of the client area. If this flag is not specified, the valid
        ///         contents of the client area are saved and copied back into the client area after the window is sized or
        ///         repositioned.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOMOVE (0x0002)</term>
        ///         <description>Retains the current position (ignores X and Y parameters).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOOWNERZORDER (0x0200)</term>
        ///         <description>Does not change the owner window's position in the Z order.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOREDRAW (0x0008)</term>
        ///         <description>
        ///         Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies
        ///         to the client area, the nonclient area (including the title bar and scroll bars), and any part of the
        ///         parent window uncovered as a result of the window being moved. When this flag is set, the application
        ///         must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOREPOSITION (0x0200)</term><description>Same as the SWP_NOOWNERZORDER flag.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOSENDCHANGING (0x0400)</term>
        ///         <description>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOSIZE (0x0001)</term>
        ///         <description>Retains the current size (ignores the cx and cy parameters).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOZORDER (0x0004)</term>
        ///         <description>Retains the current Z order (ignores the hWndInsertAfter parameter).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_SHOWWINDOW (0x0040)</term><description>Displays the window.</description>
        ///     </item>
        ///     </list>
        /// </param>
        /// <returns><c>true</c> or nonzero if the function succeeds, <c>false</c> or zero otherwise or if function fails.</returns>
        /// <remarks>
        ///     <para>
        ///     As part of the Vista re-architecture, all services were moved off the interactive desktop into Session 0.
        ///     hwnd and window manager operations are only effective inside a session and cross-session attempts to manipulate
        ///     the hwnd will fail. For more information, see The Windows Vista Developer Story: Application Compatibility
        ///     Cookbook.
        ///     </para>
        ///     <para>
        ///     If you have changed certain window data using SetWindowLong, you must call SetWindowPos for the changes to
        ///     take effect. Use the following combination for uFlags: SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
        ///     SWP_FRAMECHANGED.
        ///     </para>
        ///     <para>
        ///     A window can be made a topmost window either by setting the hWndInsertAfter parameter to HWND_TOPMOST and
        ///     ensuring that the SWP_NOZORDER flag is not set, or by setting a window's position in the Z order so that it is
        ///     above any existing topmost windows. When a non-topmost window is made topmost, its owned windows are also made
        ///     topmost. Its owners, however, are not changed.
        ///     </para>
        ///     <para>
        ///     If neither the SWP_NOACTIVATE nor SWP_NOZORDER flag is specified (that is, when the application requests that
        ///     a window be simultaneously activated and its position in the Z order changed), the value specified in
        ///     hWndInsertAfter is used only in the following circumstances.
        ///     </para>
        ///     <list type="bullet">
        ///     <item>Neither the HWND_TOPMOST nor HWND_NOTOPMOST flag is specified in hWndInsertAfter. </item>
        ///     <item>The window identified by hWnd is not the active window. </item>
        ///     </list>
        ///     <para>
        ///     An application cannot activate an inactive window without also bringing it to the top of the Z order.
        ///     Applications can change an activated window's position in the Z order without restrictions, or it can activate
        ///     a window and then move it to the top of the topmost or non-topmost windows.
        ///     </para>
        ///     <para>
        ///     If a topmost window is repositioned to the bottom (HWND_BOTTOM) of the Z order or after any non-topmost
        ///     window, it is no longer topmost. When a topmost window is made non-topmost, its owners and its owned windows
        ///     are also made non-topmost windows.
        ///     </para>
        ///     <para>
        ///     A non-topmost window can own a topmost window, but the reverse cannot occur. Any window (for example, a
        ///     dialog box) owned by a topmost window is itself made a topmost window, to ensure that all owned windows stay
        ///     above their owner.
        ///     </para>
        ///     <para>
        ///     If an application is not in the foreground, and should be in the foreground, it must call the
        ///     SetForegroundWindow function.
        ///     </para>
        ///     <para>
        ///     To use SetWindowPos to bring a window to the top, the process that owns the window must have
        ///     SetForegroundWindow permission.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.DLL")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.DLL")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.DLL")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.DLL")]
        public static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.DLL")]
        public static extern int SetWindowLongPtrA(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.DLL")]
        public static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetActiveWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.DLL")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        

    }
}
