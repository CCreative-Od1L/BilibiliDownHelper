using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BvDownkr.src.Implement {
    class ReplyCommand<T>(Action<T?> action, bool canExecute = false) : ICommand {
        public event EventHandler? CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
            }
            remove {
                CommandManager.RequerySuggested -= value; 
            }
        }
        readonly bool _canExecute = canExecute;
        readonly Action<T?> _execute = action;

        public bool CanExecute(object? parameter) {
            return _canExecute;
        }

        public void Execute(object? parameter) {
            if (_canExecute) {
                _execute?.Invoke((T?)parameter);
            }
        }
    }
}
