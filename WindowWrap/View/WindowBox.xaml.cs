using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        }

        private static void OnWindowPtrChangee(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBox instance = (WindowBox)d;
            instance.WindowPtrChange((IntPtr)e.NewValue, (IntPtr)e.OldValue);
        }

        private void WindowPtrChange(IntPtr new_ptr, IntPtr old_ptr)
        {
            UndockWindow(old_ptr);
            if (new_ptr == IntPtr.Zero)
                return;
            DockWindow(new_ptr);


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
            User32.SetWindowLongPtrA(window, User32.WindowLongFlags.GWL_STYLE,User32.WindowLongFlagsExtend.WS_VISIBLE);

            //User32.SetWindowLongPtr(
            //    window,
            //    User32.WindowLongFlags.GWLP_HWNDPARENT,
            //    new WindowInteropHelper(App.Current.MainWindow).Handle);

            MoveWindow(window);
            //User32.SetActiveWindow(Process.GetCurrentProcess().MainWindowHandle);
        }

        private void UndockWindow(IntPtr window)
        {
            User32.SetParent(window, IntPtr.Zero);
            int style = User32.GetWindowLong(window, User32.WindowLongFlags.GWL_STYLE);
            User32.SetWindowLongPtrA(window, User32.WindowLongFlags.GWL_STYLE, style | User32.WindowLongFlagsExtend.WS_OVERLAPPEDWINDOW);
            //User32.SetActiveWindow(Process.GetCurrentProcess().MainWindowHandle);

            //User32.SetWindowLongPtr(
            //    window,
            //    User32.WindowLongFlags.GWLP_HWNDPARENT,
            //    IntPtr.Zero);
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

                User32.MoveWindow(window, (int)point_local.X, (int)point_local.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
                //User32.MoveWindow(window, (int)point_global.X, (int)point_global.Y, 1000, 1000, true);
                //User32.SetWindowPos(window, User32.HWND.NoTopMost, (int)point_local.X, (int)point_local.Y, (int)this.ActualWidth, (int)this.ActualHeight, (uint)User32.SWP.SHOWWINDOW);
            }
            catch(Exception ex) { }
        }



        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveWindow(WindowPtr);
        }
    }
}
