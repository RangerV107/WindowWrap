using OpenControls.Wpf.DockManager;
using WindowWrap.ViewModel.Base;

namespace WindowWrap.ViewModel
{
    class OtherToolViewModel : ViewModelBase, IViewModel
    {
        public OtherToolViewModel()
        {
            Title = "Other Tool View Model";
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
                return true;
            }
        }

        public bool isSelected { get; set; }

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
