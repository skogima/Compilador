using System;
using System.Windows.Input;

namespace Compiler
{
    public class RelayCommand : ICommand
    {
        private Action _action;
        public RelayCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// O evento que é ativado quando o valor de <see cref="CanExecute(object)"/> é alterado 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action();
    }
}
