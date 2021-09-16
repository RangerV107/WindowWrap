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

        private void WindowPtrChange(IntPtr new_ptr, IntPtr old_ptr)
        {
            //window_name.Content = "Ptr: " + WindowPtr;

            User32.SetParent(old_ptr, IntPtr.Zero);
            User32.SetParent(new_ptr, new WindowInteropHelper(App.Current.MainWindow).Handle);

            User32.MoveWindow(new_ptr, 20, 20, (int)this.ActualWidth, (int)this.ActualHeight, true);
            //Trace.WriteLine("WindowPtr: " + WindowPtr);
        }

        private static void OnWindowPtrChangee(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowBox instance = (WindowBox)d;
            instance.WindowPtrChange((IntPtr)e.NewValue, (IntPtr)e.OldValue);
        }


        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            User32.MoveWindow(WindowPtr, 20, 20, (int)this.ActualWidth, (int)this.ActualHeight, true);
        }
    }
}
