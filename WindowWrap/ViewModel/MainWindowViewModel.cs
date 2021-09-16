using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenControls.Wpf.DockManager;
using WindowWrap.Infrastructure.Commands;
using WindowWrap.ViewModel.Base;

namespace WindowWrap.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {

        #region Properties
        #region Title
        private string _Title = "Window wrap";
        /// <summary>Window title</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Documents
        private ObservableCollection<IViewModel> _documents;
        /// <summary>
        /// Documents sources
        /// </summary>
        public ObservableCollection<IViewModel> Documents
        {
            get => _documents;
            set => Set(ref _documents, value);
        }
        #endregion

        #region Tools
        private ObservableCollection<IViewModel> _tools;
        /// <summary>
        /// Tools sources
        /// </summary>
        public ObservableCollection<IViewModel> Tools
        {
            get => _tools;
            set => Set(ref _tools, value);
        }
        #endregion

        #region LayoutLoaded
        private bool _layoutLoaded;
        /// <summary>
        /// Is DockManager layout loaded
        /// </summary>
        public bool LayoutLoaded
        {
            get => _layoutLoaded;
            set => Set(ref _layoutLoaded, value);
        }
        #endregion
        #endregion

        #region Fields
        #region _keyPath
        //private string _keyPath = System.Environment.Is64BitOperatingSystem ?
        //    @"SOFTWARE\Wow6432Node\OpenControls\WpfDockManagerDemo" : @"SOFTWARE\OpenControls\WpfDockManagerDemo"; 
        #endregion

        #region Tools
        public readonly IViewModel ToolOne = new ToolViewModel { Title = "Tool 1" };
        #endregion

        #region Documents
        public readonly IViewModel Window1 = new WindowViewModel() { URL = "Window1", Title = "Window 1" };
        #endregion
        #endregion

        #region Commands
        #region CloseCommand
        public ICommand AppCloseCommand { get; }
        private bool CanAppCloseCommandExecute(object p) => true;
        private void OnAppCloseCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region WindowLoaded
        public ICommand WindowLoaded { get; }
        private bool CanWindowLoadedExecute(object p) => true;
        private void OnWindowLoadedExecuted(object p)
        {
            #region Save something in rregestry
            //RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyPath);
            //if (key == null)
            //{
            //    key = Registry.CurrentUser.CreateSubKey(_keyPath);
            //}
            //else
            //{
            //    Object obj = key.GetValue("Height");
            //    if (obj != null)
            //    {
            //        Height = Convert.ToDouble(obj);
            //    }
            //    obj = key.GetValue("Width");
            //    if (obj != null)
            //    {
            //        Width = Convert.ToDouble(obj);
            //    }
            //    obj = key.GetValue("Top");
            //    if (obj != null)
            //    {
            //        Top = Convert.ToDouble(obj);
            //    }
            //    obj = key.GetValue("Left");
            //    if (obj != null)
            //    {
            //        Left = Convert.ToDouble(obj);
            //    }
            //} 
            #endregion

            //_layoutManager.Initialise();

        }
        #endregion

        #endregion



        public MainWindowViewModel()
        {
            #region Commands
            AppCloseCommand = new ActionCommand(
                OnAppCloseCommandExecuted, CanAppCloseCommandExecute);
            #endregion

            #region DockManager layout
            LayoutLoaded = false;

            Tools = new ObservableCollection<IViewModel>();
            Tools.Add(ToolOne);

            Documents = new ObservableCollection<IViewModel>();
            Documents.Add(Window1); 
            #endregion



        }

        

        



        



        //public bool IsToolVisible(Type type)
        //{
        //    return (Tools.Where(n => n.GetType() == type).Count() > 0);
        //}

        //public void ShowTool(bool show, Type type)
        //{
        //    bool isVisible = IsToolVisible(type);
        //    if (isVisible == show)
        //    {
        //        return;
        //    }

        //    if (show == false)
        //    {
        //        var enumerator = Tools.Where(n => n.GetType() == type);
        //        Tools.Remove(enumerator.First());
        //    }
        //    else
        //    {
        //        IViewModel iViewModel = (IViewModel)Activator.CreateInstance(type);
        //        System.Diagnostics.Trace.Assert(iViewModel != null);
        //        Tools.Add(iViewModel);
        //    }
        //}

        //private bool _toolOneVisible;
        //public bool ToolOneVisible
        //{
        //    get
        //    {
        //        return (Tools.Where(n => n.GetType() == typeof(ExampleDockManagerViews.ViewModel.ToolOneViewModel)).Count() > 0);
        //    }
        //    set
        //    {
        //        ShowTool(value, typeof(ExampleDockManagerViews.ViewModel.ToolOneViewModel));
        //        //NotifyPropertyChanged("ToolOneVisible");
        //        Set(ref _toolOneVisible, value);
        //    }
        //}

        ////public bool ToolTwoVisible
        ////{
        ////    get
        ////    {
        ////        return (Tools.Where(n => n.GetType() == typeof(ExampleDockManagerViews.ViewModel.ToolTwoViewModel)).Count() > 0);
        ////    }
        ////    set
        ////    {
        ////        ShowTool(value, typeof(ExampleDockManagerViews.ViewModel.ToolTwoViewModel));
        ////        NotifyPropertyChanged("ToolTwoVisible");
        ////    }
        ////}

        ////public bool ToolThreeVisible
        ////{
        ////    get
        ////    {
        ////        return (Tools.Where(n => n.GetType() == typeof(ExampleDockManagerViews.ViewModel.ToolThreeViewModel)).Count() > 0);
        ////    }
        ////    set
        ////    {
        ////        ShowTool(value, typeof(ExampleDockManagerViews.ViewModel.ToolThreeViewModel));
        ////        NotifyPropertyChanged("ToolThreeVisible");
        ////    }
        ////}

        ////public bool ToolFourVisible
        ////{
        ////    get
        ////    {
        ////        return (Tools.Where(n => n.GetType() == typeof(ExampleDockManagerViews.ViewModel.ToolFourViewModel)).Count() > 0);
        ////    }
        ////    set
        ////    {
        ////        ShowTool(value, typeof(ExampleDockManagerViews.ViewModel.ToolFourViewModel));
        ////        NotifyPropertyChanged("ToolFourVisible");
        ////    }
        ////}

        ////public bool ToolFiveVisible
        ////{
        ////    get
        ////    {
        ////        return (Tools.Where(n => n.GetType() == typeof(ExampleDockManagerViews.ViewModel.ToolFiveViewModel)).Count() > 0);
        ////    }
        ////    set
        ////    {
        ////        ShowTool(value, typeof(ExampleDockManagerViews.ViewModel.ToolFiveViewModel));
        ////        NotifyPropertyChanged("ToolFiveVisible");
        ////    }
        ////}

        //public bool IsDocumentVisible(IViewModel iViewModel)
        //{
        //    return (Documents.Contains(iViewModel));
        //}

        //public void ShowDocument(bool show, IViewModel iViewModel)
        //{
        //    bool isVisible = IsDocumentVisible(iViewModel);
        //    if (isVisible == show)
        //    {
        //        return;
        //    }

        //    if (show == false)
        //    {
        //        Documents.Remove(iViewModel);
        //    }
        //    else
        //    {
        //        Documents.Add(iViewModel);
        //    }
        //}

        //private bool _documentOneVisible;
        //public bool DocumentOneVisible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(DocumentOne);
        //    }
        //    set
        //    {
        //        ShowDocument(value, DocumentOne);
        //        //NotifyPropertyChanged("DocumentOneVisible");
        //        Set(ref _documentOneVisible, value);
        //    }
        //}

        //private bool _documentTwoVisible;
        //public bool DocumentTwoVisible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(DocumentTwo);
        //    }
        //    set
        //    {
        //        ShowDocument(value, DocumentTwo);
        //        //NotifyPropertyChanged("DocumentTwoVisible");
        //        Set(ref _documentTwoVisible, value);
        //    }
        //}

        //private bool _window1Visible;
        //public bool Window1Visible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(Window1);
        //    }
        //    set
        //    {
        //        ShowDocument(value, Window1);
        //        Set(ref _window1Visible, value);
        //    }
        //}

        //public bool DocumentThreeVisible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(DocumentThree);
        //    }
        //    set
        //    {
        //        ShowDocument(value, DocumentThree);
        //        NotifyPropertyChanged("DocumentThreeVisible");
        //    }
        //}

        //public bool DocumentFourVisible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(DocumentFour);
        //    }
        //    set
        //    {
        //        ShowDocument(value, DocumentFour);
        //        NotifyPropertyChanged("DocumentFourVisible");
        //    }
        //}

        //public bool DocumentFiveVisible
        //{
        //    get
        //    {
        //        return IsDocumentVisible(DocumentFive);
        //    }
        //    set
        //    {
        //        ShowDocument(value, DocumentFive);
        //        NotifyPropertyChanged("DocumentFiveVisible");
        //    }
        //}




    }
}
