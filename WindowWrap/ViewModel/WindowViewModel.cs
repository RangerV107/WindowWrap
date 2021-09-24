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

        #region isActive
        private bool _isActive;
        public bool isActive
        {
            get => _isActive;
            set
            {
                if (value)
                    OnActive();
                else
                    OnDeactive();
                Set(ref _isActive, value);
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






        public WindowViewModel(IntPtr window)
        {
            SelectedWindowPtr = window;
            SelectedWindowState = WindowState.Normal;
        }

        private void OnSelect()
        {
            //Trace.WriteLine(URL + " selected");
            SelectedWindowState = WindowState.Normal;
        }

        private void OnDeselect()
        {
            //Trace.WriteLine(URL + " deselected");
            SelectedWindowState = WindowState.Minimized;
        }

        private void OnActive()
        {
            //Trace.WriteLine(URL + " active");
        }

        private void OnDeactive()
        {
            //Trace.WriteLine(URL + " deactive");
        }



        #region IViewModel
        public void Save()
        {
            // Do nowt!
        }

        public void Close()
        {
            SelectedWindowState = WindowState.Normal;
            SelectedWindowPtr = IntPtr.Zero;
        }
        #endregion

    }
}
