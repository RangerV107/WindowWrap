using System;
using System.Collections.Generic;
using System.Text;
using WindowWrap.Infrastructure.Commands.Base;

namespace WindowWrap.Infrastructure.Commands
{
    internal class ActionCommand : CommandBase
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public ActionCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _CanExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
        public override void Execute(object parameter) => _Execute(parameter);
    }
}
