using System;

namespace OpenControls.Wpf.DockManager.Themes
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
