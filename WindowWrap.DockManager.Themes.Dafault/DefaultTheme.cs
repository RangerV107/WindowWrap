using OpenControls.Wpf.DockManager.Themes;
using System;

namespace WindowWrap.DockManager.Themes.Dafault
{
    public class DefaultTheme : Theme
    {
        public override Uri Uri
        {
            get
            {
                return new Uri("/WindowWrap.DockManager.Themes.Dafault;component/Dictionary.xaml", UriKind.Relative);
            }
        }
    }
}
