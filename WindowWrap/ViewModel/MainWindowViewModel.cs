using System.Windows;
using System.Windows.Input;
using WindowWrap.Infrastructure.Commands;
using WindowWrap.ViewModel.Base;

namespace WindowWrap.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Title
        private string _Title = "ddd";
        /// <summary>Window title</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
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
        #endregion


        public MainWindowViewModel()
        {
            #region Commands
            AppCloseCommand = new ActionCommand(
                OnAppCloseCommandExecuted, CanAppCloseCommandExecute);
            #endregion
        }








    }
}
