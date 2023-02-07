using System;
using System.Windows.Input;

namespace LikeNtfsWalker.UI
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> execute;
        private Func<object, bool> canExecute;
        private Func<Command> refresh;

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Command(Func<Command> refresh)
        {
            this.refresh = refresh;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
