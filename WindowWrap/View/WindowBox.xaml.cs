using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Utilities.Win;

namespace WindowWrap.View
{
    /// <summary>
    /// Логика взаимодействия для WindowBox.xaml
    /// </summary>
    public partial class WindowBox : UserControl
    {

        public static readonly DependencyProperty WindowPtrProperty = DependencyProperty.Register(
                "WindowPtr", 
                typeof(IntPtr), 
                typeof(WindowBox), 
                new FrameworkPropertyMetadata(
                    IntPtr.Zero,
                    new PropertyChangedCallback(OnWindowPtrChangee)));
        public IntPtr WindowPtr
        {
            get { return (IntPtr)GetValue(WindowPtrProperty); }
            set { SetValue(WindowPtrProperty, value); }
        }


        public WindowBox()
        {
            InitializeComponent();

            WindowInteropHelper helper = new WindowInteropHelper(App.Current.MainWindow);
            HwndSource.FromHwnd(helper.Handle).AddHook(HwndMessageHook);

            //InitialWindowLocation = new Point(this.Left, this.Top);

        }

        //private Point InitialWindowLocation;
        //[StructLayout(LayoutKind.Sequential)]
        //public struct WIN32Rectangle
        //{
        //    public int Left;
        //    public int Top;
        //    public int Right;
        //    public int Bottom;
        //}
        const int WM_SIZING = 0x0214;
        const int WM_MOVING = 0x0216;
        //https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-move
        //https://stackoverflow.com/questions/12376141/intercept-a-move-event-in-a-wpf-window-before-the-move-happens-on-the-screen
        private IntPtr HwndMessageHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool bHandled)
        {
            switch (msg)
            {
                case WM_SIZING:
                case WM_MOVING:
                    {
                        //Trace.WriteLine("MOVING");
                        MoveWindow(WindowPtr);
                    }
                    break;

            }
            return IntPtr.Zero;
        }

        


        private static void OnWindowPtrChangee(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBox instance = (WindowBox)d;
            instance.WindowPtrChange((IntPtr)e.NewValue, (IntPtr)e.OldValue);
        }

        private void WindowPtrChange(IntPtr new_ptr, IntPtr old_ptr)
        {
            //UndockWindow(old_ptr);
            //if (new_ptr == IntPtr.Zero)
            //    return;
            //DockWindow(new_ptr);

            UndockWindow2(old_ptr);
            if (new_ptr == IntPtr.Zero)
                return;
            DockWindow2(new_ptr);



            #region ddd
            //IntPtr mainW = new WindowInteropHelper(App.Current.MainWindow).Handle;

            //User32.RECT rect = new User32.RECT();
            //User32.GetWindowRect(mainW, out rect);
            //Trace.WriteLine(
            //    "left: " + rect.left +
            //    " top: " + rect.top +
            //    " right: " + rect.right +
            //    " bottom: " + rect.bottom);

            //Point p = App.Current.MainWindow.PointToScreen(new Point(0, 0));
            //Trace.WriteLine(
            //    "X: " + p.X +
            //    "Y: " + p.Y); 
            #endregion

            #region aaa

            //TestWindow testWindow = new TestWindow();
            //testWindow.Show();
            //testWindow.Owner = App.Current.MainWindow;
            //Window.
            //new WindowInteropHelper(testWindow).Owner = new_ptr;

            //User32.SetWindowLongPtr(
            //    new_ptr,
            //    User32.WindowLongFlags.GWLP_HWNDPARENT,
            //    new WindowInteropHelper(App.Current.MainWindow).Handle);
            #endregion
        }



        private void DockWindow(IntPtr window)
        {
            User32.SetParent(window, new WindowInteropHelper(App.Current.MainWindow).Handle);
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE,User32.WindowLongFlagsExtend.WS_VISIBLE);

            //User32.SetWindowLongPtr(
            //    window,
            //    User32.WindowLongFlags.GWLP_HWNDPARENT,
            //    new WindowInteropHelper(App.Current.MainWindow).Handle);

            MoveWindow(window);
            //User32.SetActiveWindow(Process.GetCurrentProcess().MainWindowHandle);
        }
        private void DockWindow2(IntPtr window)
        {
            //User32.SetParent(window, new WindowInteropHelper(App.Current.MainWindow).Handle);
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, User32.WindowLongFlagsExtend.WS_VISIBLE);

            //int lStyle = User32.GetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE);
            ////lStyle |= User32.WindowLongFlagsExtend.WS_THICKFRAME | User32.WindowLongFlagsExtend.WS_VISIBLE;
            //lStyle = lStyle & 
            //    ~(User32.WindowLongFlagsExtend.WS_CAPTION |
            //    User32.WindowLongFlagsExtend.WS_THICKFRAME |
            //    User32.WindowLongFlagsExtend.WS_MINIMIZEBOX |
            //    User32.WindowLongFlagsExtend.WS_MAXIMIZEBOX |
            //    User32.WindowLongFlagsExtend.WS_SYSMENU);
            //User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, lStyle);
            //User32.SetWindowPos(window, IntPtr.Zero, 0, 0, 0, 0,
                //(uint)(User32.SWP.FRAMECHANGED | User32.SWP.NOMOVE | User32.SWP.NOSIZE | User32.SWP.NOZORDER | User32.SWP.NOOWNERZORDER | User32.SWP.SHOWWINDOW));
                //(uint)User32.SWP.SHOWWINDOW);


            User32.SetWindowLongPtr(
                window,
                User32.WindowLongFlags.GWLP_HWNDPARENT,
                new WindowInteropHelper(App.Current.MainWindow).Handle);

            User32.SetActiveWindow(window);
            User32.ShowWindow(window, User32.WindowShowFlags.SW_MINIMIZE);
            User32.ShowWindow(window, User32.WindowShowFlags.SW_NORMAL);
            //User32.UpdateWindow(window);

            MoveWindow(window);   
        }

        private void UndockWindow(IntPtr window)
        {
            User32.SetParent(window, IntPtr.Zero);
            int style = User32.GetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE);
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, style | User32.WindowLongFlagsExtend.WS_OVERLAPPEDWINDOW);
            //User32.SetActiveWindow(Process.GetCurrentProcess().MainWindowHandle);

            //User32.SetWindowLongPtr(
            //    window,
            //    User32.WindowLongFlags.GWLP_HWNDPARENT,
            //    IntPtr.Zero);
        }
        private void UndockWindow2(IntPtr window)
        {
            //User32.SetParent(window, IntPtr.Zero);
            int style = User32.GetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE);
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, style | User32.WindowLongFlagsExtend.WS_OVERLAPPEDWINDOW);

            User32.SetWindowLongPtr(
                window,
                User32.WindowLongFlags.GWLP_HWNDPARENT,
                IntPtr.Zero);

            //User32.SetActiveWindow(Process.GetCurrentProcess().MainWindowHandle);
        }

        private void MoveWindow(IntPtr window)
        {
            try
            {
                GeneralTransform generalTransform1 = this.TransformToAncestor(App.Current.MainWindow);
                Point point_local = generalTransform1.Transform(new Point(0, 0));
                Point point_global = App.Current.MainWindow.PointToScreen(new Point(0, 0));
                point_global.Offset(point_local.X, point_local.Y);
                //Vector vector = VisualTreeHelper.GetOffset(this);

                User32.MoveWindow(window, (int)point_global.X, (int)point_global.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
                //User32.MoveWindow(window, (int)point_global.X, (int)point_global.Y, 1000, 1000, true);
                //User32.SetWindowPos(window, User32.HWND.NoTopMost, (int)point_local.X, (int)point_local.Y, (int)this.ActualWidth, (int)this.ActualHeight, (uint)User32.SWP.SHOWWINDOW);
            }
            catch(Exception ex) { Trace.WriteLine(ex.Message); }
        }



        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(WindowPtr);
        }
    }
}
