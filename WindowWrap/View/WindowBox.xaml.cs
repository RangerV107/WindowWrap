using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Utilities.Win;
using WindowWrap.Infrastructure.Properties;

namespace WindowWrap.View
{
    /// <summary>
    /// Логика взаимодействия для WindowBox.xaml
    /// </summary>
    public partial class WindowBox : UserControl
    { 
        #region WindowPtr
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
        #endregion

        #region WindowState
        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(
                "WindowState",
                typeof(WindowState),
                typeof(WindowBox),
                new FrameworkPropertyMetadata(
                    System.Windows.WindowState.Normal,
                    new PropertyChangedCallback(OnWindowStateChangee)));
        public WindowState WindowState
        {
            get { return (WindowState)GetValue(WindowStateProperty); }
            set { SetValue(WindowStateProperty, value); }
        }
        #endregion


        private Window _parentWindow;
        public Window ParentWindow
        {
            get { return _parentWindow; }
            set { _parentWindow = value; }
        }



        public WindowBox()
        {
            InitializeComponent();

            WindowInteropHelper helper = new WindowInteropHelper(App.Current.MainWindow);
            HwndSource.FromHwnd(helper.Handle).AddHook(HwndMessageHook);
        }


        const int WM_SIZING = 0x0214;
        const int WM_MOVING = 0x0216;
        //https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-move
        //https://stackoverflow.com/questions/12376141/intercept-a-move-event-in-a-wpf-window-before-the-move-happens-on-the-screen
        private IntPtr HwndMessageHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool bHandled)
        {
            switch (msg)
            {
                //case WM_SIZING:
                case WM_MOVING:
                    {
                        MoveWindow(WindowPtr);
                    }
                    break;

            }
            return IntPtr.Zero;
        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(WindowPtr);
        }

        


        private static void OnWindowPtrChangee(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((WindowBox)d).WindowPtrChange((IntPtr)e.NewValue, (IntPtr)e.OldValue);
        private void WindowPtrChange(IntPtr new_ptr, IntPtr old_ptr)
        {
            //ParentWindow = FindParentWindow(this);
            UndockWindow(old_ptr);
            if (new_ptr == IntPtr.Zero)
                return;
            DockWindow(new_ptr);
        }

        private static void OnWindowStateChangee(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((WindowBox)d).WindowStateChangee((WindowState)e.NewValue, (WindowState)e.OldValue);
        private void WindowStateChangee(WindowState new_state, WindowState old_state)
        {
            switch (new_state)
            {
                case WindowState.Normal:
                    User32.ShowWindow(WindowPtr, User32.WindowShowFlags.SW_NORMAL); break;
                case WindowState.Maximized:
                    User32.ShowWindow(WindowPtr, User32.WindowShowFlags.SW_MAXIMIZE); break;
                case WindowState.Minimized:
                    User32.ShowWindow(WindowPtr, User32.WindowShowFlags.SW_MINIMIZE); break;
            }
        }



        private void DockWindow(IntPtr window)
        {
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, User32.WindowLongFlagsExtend.WS_VISIBLE);
            User32.SetWindowLongPtr(
                window,
                User32.WindowLongFlags.GWLP_HWNDPARENT,
                new WindowInteropHelper(App.Current.MainWindow).Handle);

            User32.SetActiveWindow(window);
            User32.ShowWindow(window, User32.WindowShowFlags.SW_MINIMIZE);
            User32.ShowWindow(window, User32.WindowShowFlags.SW_NORMAL);
            MoveWindow(window);
        }

        private void UndockWindow(IntPtr window)
        {
            int style = User32.GetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE);
            User32.SetWindowLongPtr(window, User32.WindowLongFlags.GWL_STYLE, style | User32.WindowLongFlagsExtend.WS_OVERLAPPEDWINDOW);
            User32.SetWindowLongPtr(
                window,
                User32.WindowLongFlags.GWLP_HWNDPARENT,
                IntPtr.Zero);
        }

        private void MoveWindow(IntPtr window)
        {
            try
            {
                GeneralTransform generalTransform1 = this.TransformToAncestor(App.Current.MainWindow);
                Point point_local = generalTransform1.Transform(new Point(0, 0));
                Point point_global = App.Current.MainWindow.PointToScreen(new Point(0, 0));
                point_global.Offset(point_local.X, point_local.Y);
                User32.MoveWindow(window, (int)point_global.X, (int)point_global.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
            }
            catch (Exception ex) { /*Trace.WriteLine(ex.Source + " : " + ex.Message);*/ }

            //if (ParentWindow != null)
            //{
            //    GeneralTransform generalTransform1 = this.TransformToAncestor(App.Current.MainWindow);
            //    Point point_local = generalTransform1.Transform(new Point(0, 0));
            //    Point point_global = App.Current.MainWindow.PointToScreen(new Point(0, 0));
            //    point_global.Offset(point_local.X, point_local.Y);
            //    User32.MoveWindow(window, (int)point_global.X, (int)point_global.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
            //}
        }




        public static Window FindParentWindow(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            //CHeck if this is the end of the tree
            if (parent == null) return null;

            Window parentWindow = parent as Window;
            if (parentWindow != null)
            {
                return parentWindow;
            }
            else
            {
                //use recursion until it reaches a Window
                return FindParentWindow(parent);
            }
        }



        #region INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string property = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        //}

        //protected virtual bool Set<T>(ref T field, T value, string property = null)
        //{
        //    if (Equals(field, value)) return false;
        //    field = value;
        //    OnPropertyChanged(property);
        //    return true;
        //} 
        #endregion


    }
}
