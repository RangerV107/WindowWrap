using System.Windows.Controls;

namespace WindowWrap.View
{
    /// <summary>
    /// Логика взаимодействия для ToolView.xaml
    /// </summary>
    public partial class ToolView : UserControl
    {
        public ToolView()
        {
            InitializeComponent();
        }

        public OpenControls.Wpf.DockManager.IViewModel IViewModel { get; set; }
    }
}
