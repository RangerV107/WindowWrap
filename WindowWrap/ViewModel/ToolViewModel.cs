using OpenControls.Wpf.DockManager;
using WindowWrap.ViewModel.Base;

namespace WindowWrap.ViewModel
{
    class ToolViewModel : ViewModelBase, IViewModel
    {
        public ToolViewModel()
        {
            Title = "Tool View Model";
        }

        public string URL { get; set; }
        public string Title { get; set; }
        public string Tooltip
        {
            get
            {
                return URL;
            }
        }

        public bool CanClose
        {
            get
            {
                return true;
            }
        }

        public bool HasChanged
        {
            get
            {
                return false;
            }
        }

        public bool isSelected { get; set; }
        public bool isActive { get; set; }

        public void Save()
        {
            // Do nowt!
        }

        public void Close()
        {
            // Do nowt!
        }
    }
}
