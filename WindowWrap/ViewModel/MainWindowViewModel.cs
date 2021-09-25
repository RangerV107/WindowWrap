using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenControls.Wpf.DockManager;
using WindowWrap.Infrastructure.Commands;
using WindowWrap.ViewModel.Base;
using System.Diagnostics;
using OpenControls.Wpf.Utilities;
using Utilities;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

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

        #region WindowsList
        private ObservableCollection<WinInfo> _windowsList;
        public ObservableCollection<WinInfo> WindowsList
        {
            get => _windowsList;
            set => Set(ref _windowsList, value);
        }
        #endregion

        #region DockedWindowsList
        private Dictionary<IViewModel, WinInfo> _dockedWindowsList;
        public Dictionary<IViewModel, WinInfo> DockedWindowsList
        {
            get => _dockedWindowsList;
            set => Set(ref _dockedWindowsList, value);
        }
        #endregion

        //#region WindowNamesList
        //private ObservableCollection<string> _windowNamesList;
        //public ObservableCollection<string> WindowNamesList
        //{
        //    get => _windowNamesList;
        //    set => Set(ref _windowNamesList, value);
        //}
        //#endregion

        //#region SelectedWindowName
        //private string _selectedWindowName;
        //public string SelectedWindowName
        //{
        //    get => _selectedWindowName;
        //    set
        //    {
        //        Set(ref _selectedWindowName, value);
        //        //OnWindowSelect(value);
        //    }
        //}
        //#endregion

        //#region SelectedWindowPtr
        //private IntPtr _selectedWindowPtr;
        //public IntPtr SelectedWindowPtr
        //{
        //    get => _selectedWindowPtr;
        //    set => Set(ref _selectedWindowPtr, value);
        //}
        //#endregion

        //#region SelectedWindowState
        //private WindowState _selectedWindowState;
        //public WindowState SelectedWindowState
        //{
        //    get => _selectedWindowState;
        //    set => Set(ref _selectedWindowState, value);
        //}
        //#endregion
        #endregion


        #region Fields
        #region Documents
        //public readonly IViewModel Window1 = new WindowViewModel() { URL = Guid.NewGuid().ToString(), Title = "Window" };
        //public readonly IViewModel Window2 = new WindowViewModel() { URL = Guid.NewGuid().ToString(), Title = "Window" };
        #endregion
        #region Tools
        public readonly IViewModel ToolOne = new ToolViewModel { Title = "Tool" };
        //public readonly IViewModel ToolTwo = new OtherToolViewModel { Title = "Other Tool" };
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

        #region Test1Command
        public ICommand Test1Command { get; }
        private bool CanTest1CommandExecute(object p) => true;
        private void OnTest1CommandExecuted(object p)
        {
            //OpenControls.Wpf.Utilities.User32.EnumWindows((wnd, param) =>
            //{
            //    string window = User32Utilities.GetWindowModuleFileName(wnd);

            //    Trace.WriteLine(Path.GetFileName(window));

            //    return true;
            //}, IntPtr.Zero);

            Trace.WriteLine("\n============================");
            #region ddd_old
            //foreach (var w in Win32Utilities.GetOpenWindows().OrderBy(c => c.Key))
            //{
            //    Trace.WriteLine(w.Key + "       Ptr: " + w.Value);
            //} 
            #endregion

            #region ddd
            //foreach(var doc in Documents)
            //{
            //    Trace.WriteLine(doc.Title + " : " + doc.URL);
            //} 
            #endregion

            #region mega_ddd
            //for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(App.Current.MainWindow); i++)
            //{
            //    DependencyObject wch = System.Windows.Media.VisualTreeHelper.GetChild(App.Current.MainWindow, i);
            //    Trace.WriteLine(wch);
            //}
            #endregion

            #region new_ddd
            foreach (var item in DockedWindowsList.Values)
            {
                Trace.WriteLine(item);
            }
            #endregion
            Trace.WriteLine("============================\n");

            //App.Current.MainWindow.
            
            
        }
        #endregion

        #region AddWindowCommand
        public ICommand AddWindowCommand { get; }
        private bool CanAddWindowCommandExecute(object p) => true;
        private void OnAddWindowCommandExecuted(object p)
        {
            WinInfo selected = (from w in WindowsList where w.ToString() == 
                                ((p as RoutedEventArgs).OriginalSource as MenuItem).Header.ToString() select w).First();
            OnWindowSelect(selected);
        }
        #endregion

        #region WindowsUpdateCommand
        public ICommand WindowsUpdateCommand { get; }
        private bool CanWindowsUpdateCommandExecute(object p) => true;
        private void OnWindowsUpdateCommandExecuted(object p)
        {
            UpdateWindows();
        }
        #endregion
        #endregion



        public MainWindowViewModel()
        {
            #region Commands
            AppCloseCommand = new ActionCommand(OnAppCloseCommandExecuted, CanAppCloseCommandExecute);
            Test1Command = new ActionCommand(OnTest1CommandExecuted, CanTest1CommandExecute);
            AddWindowCommand = new ActionCommand(OnAddWindowCommandExecuted, CanAddWindowCommandExecute);
            WindowsUpdateCommand = new ActionCommand(OnWindowsUpdateCommandExecuted, CanWindowsUpdateCommandExecute);
            #endregion

            #region DockManager layout
            LayoutLoaded = false;

            Tools = new ObservableCollection<IViewModel>();
            Tools.Add(ToolOne);

            Documents = new ObservableCollection<IViewModel>();
            Documents.CollectionChanged += Documents_CollectionChanged;
            //Documents.Add(Window1);
            //Documents.Add(Window2);
            //Documents.Add(new WindowViewModel(IntPtr.Zero)
            //{
            //    URL = "None",
            //    Title = "None"
            //});
            DockedWindowsList = new Dictionary<IViewModel, WinInfo>();
            #endregion

            UpdateWindows();
        }       

        private void UpdateWindows()
        {
            WindowsList = new ObservableCollection<WinInfo>
                ((from w in Win32Utilities.GetOpenWindows() orderby w.Title select w).Except(DockedWindowsList.Values));
        }

        private void OnWindowSelect(WinInfo window)
        {
            if (window == null)return;
            Documents.Add(new WindowViewModel(window.Ptr)
            {
                URL = window.Process,
                Title = window.Title
            });
            DockedWindowsList.Add(Documents.Last() ,window);
        }


        private void Documents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    DockedWindowsList.Remove(e.OldItems[0] as IViewModel); break;
            }
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
