using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WindowWrap.ViewModel.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(property);
            return true;
        }

    }
}
