using System;
using System.Windows.Input;

namespace Hex_Handler_App.Infrastructure.Services
{
    internal class RellayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Action<object> _execute;

        private readonly Predicate<object> _canExecute;

        public RellayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new Exception("Execute is null!!!");
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }
    }
}
