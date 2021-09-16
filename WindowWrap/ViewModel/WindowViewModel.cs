using OpenControls.Wpf.DockManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using Utilities;
using Utilities.Win;
using WindowWrap.Infrastructure.Commands;
using WindowWrap.ViewModel.Base;

namespace WindowWrap.ViewModel
{
    internal class WindowViewModel : ViewModelBase, IViewModel
    {

        #region Properties
        #region Title
        private string _title;
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region URL
        private string _URL;
        public string URL
        {
            get => _URL;
            set => Set(ref _URL, value);
        }
        #endregion

        #region WindowList
        private ObservableCollection<string> _windowList;
        public ObservableCollection<string> WindowList
        {
            get => _windowList;
            set => Set(ref _windowList, value);
        }
        #endregion

        #region SelectedWindow
        private string _selectedWindow;
        public string SelectedWindow
        {
            get => _selectedWindow;
            set 
            {
                Set(ref _selectedWindow, value);
                OnWindowSelected(value);
            }
        }
        #endregion

        #region Windows
        private IDictionary<string, IntPtr> _windows;
        private IDictionary<string, IntPtr> Windows
        {
            get => _windows;
            set => Set(ref _windows, value);
        }
        #endregion

        #region WindowPtr
        private IntPtr _windowPtr;
        public IntPtr WindowPtr
        {
            get => _windowPtr;
            set => Set(ref _windowPtr, value);
        }
        #endregion

        #endregion

        #region Fields


        #region IViewModel
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
        #endregion
        #endregion


        #region Commands
        #region WindowSelectCommand
        public ICommand WindowSelectCommand { get; }
        private bool CanWindowSelectCommandExecute(object p) => true;
        private void OnWindowSelectCommandExecuted(object p)
        {
            ComboBox comboBox = (ComboBox)p;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            Title = selectedItem.Name;
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






        public WindowViewModel()
        {
            #region Commands
            WindowSelectCommand = new ActionCommand(
                OnWindowSelectCommandExecuted, CanWindowSelectCommandExecute);
            WindowsUpdateCommand = new ActionCommand(
                OnWindowsUpdateCommandExecuted, CanWindowsUpdateCommandExecute);
            #endregion

            
        }

        private void UpdateWindows()
        {
            Windows = Win32Utilities.GetOpenWindows();
            WindowList = new ObservableCollection<string>(Windows.Keys.OrderBy(c => c).AsEnumerable());
        }

        private void OnWindowSelected(string window)
        {
            if (window == null)
                return;

            IntPtr window_ptr;
            Windows.TryGetValue(window, out window_ptr);

            string[] words = window.Split("::");
            URL = words[0] + " : " + window_ptr;
            Title = words[1];

            WindowPtr = window_ptr;
            //User32.SetParent(window_ptr, new WindowInteropHelper(App.Current.MainWindow).Handle);
            //User32.MoveWindow(window_ptr, 0, 0, )
        }




        #region IViewModel
        public void Save()
        {
            // Do nowt!
        }

        public void Close()
        {
            // Do nowt!
        } 
        #endregion

    }
}
