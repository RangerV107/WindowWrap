using System.Windows.Controls;

namespace WindowWrap.View
{
    /// <summary>
    /// Логика взаимодействия для WindowView.xaml
    /// </summary>
    public partial class WindowView : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
            //System.Diagnostics.Trace.WriteLine(this + " : " + this.Name);
        }

        public OpenControls.Wpf.DockManager.IViewModel IViewModel { get; set; }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine(this + " : " + this.Name);
        }

    }
}
