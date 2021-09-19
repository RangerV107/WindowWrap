using OpenControls.Wpf.DockManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using Utilities;
using Utilities.Win;
using WindowWrap.Infrastructure.Commands;
using WindowWrap.Infrastructure.Properties;
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

        #region isSelected
        private bool _isSelected;
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                if (value)
                    OnSelect();
                else
                    OnDeselect();
                Set(ref _isSelected, value);
            }
        }
        #endregion

        #region WindowsList
        private IDictionary<string, IntPtr> _windowsList;
        private IDictionary<string, IntPtr> WindowsList
        {
            get => _windowsList;
            set => Set(ref _windowsList, value);
        }
        #endregion

        #region WindowNamesList
        private ObservableCollection<string> _windowNamesList;
        public ObservableCollection<string> WindowNamesList
        {
            get => _windowNamesList;
            set => Set(ref _windowNamesList, value);
        }
        #endregion

        #region SelectedWindowName
        private string _selectedWindowName;
        public string SelectedWindowName
        {
            get => _selectedWindowName;
            set 
            {
                Set(ref _selectedWindowName, value);
                OnWindowSelect(value);
            }
        }
        #endregion

        #region SelectedWindowPtr
        private IntPtr _selectedWindowPtr;
        public IntPtr SelectedWindowPtr
        {
            get => _selectedWindowPtr;
            set => Set(ref _selectedWindowPtr, value);
        }
        #endregion

        #region SelectedWindowState
        private WindowState _selectedWindowState;
        public WindowState SelectedWindowState
        {
            get => _selectedWindowState;
            set => Set(ref _selectedWindowState, value);
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
                return false;
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

        #region WindowUndockCommand
        public ICommand WindowUndockCommand { get; }
        private bool CanWindowUndockCommandExecute(object p) => true;
        private void OnWindowUndockCommandExecuted(object p)
        {
            SelectedWindowPtr = IntPtr.Zero;
            SelectedWindowName = "None::None";
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
            WindowUndockCommand = new ActionCommand(
                OnWindowUndockCommandExecuted, CanWindowUndockCommandExecute);
            WindowsUpdateCommand = new ActionCommand(
               OnWindowsUpdateCommandExecuted, CanWindowsUpdateCommandExecute);
            #endregion

            
        }

        private void UpdateWindows()
        {
            WindowsList = Win32Utilities.GetOpenWindows();          
            WindowNamesList = new ObservableCollection<string>(WindowsList.Keys.OrderBy(c => c).AsEnumerable());
            WindowsList.Add("None::None", IntPtr.Zero);
            WindowNamesList.Insert(0, "None::None");
        }

        private void OnWindowSelect(string window)
        {
            if (window == null)
                return;

            IntPtr window_ptr;
            WindowsList.TryGetValue(window, out window_ptr);

            string[] words = window.Split("::");
            URL = words[0] + " : " + window_ptr;
            Title = words[1];

            SelectedWindowPtr = window_ptr;
            SelectedWindowState = WindowState.Normal;
        }
    

        private void OnSelect()
        {
            //Trace.WriteLine(Title + " selected");
            SelectedWindowState = WindowState.Normal;
        }

        private void OnDeselect()
        {
            //Trace.WriteLine(Title + " deselected");
            SelectedWindowState = WindowState.Minimized;
        }



        #region IViewModel
        public void Save()
        {
            // Do nowt!
        }

        public void Close()
        {
            SelectedWindowPtr = IntPtr.Zero;
        }
        #endregion

    }
}
