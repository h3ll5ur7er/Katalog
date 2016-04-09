using System;
using System.Windows.Input;

namespace Katalog
{
    public class CommandRelay : ICommand
    {
        private readonly Action a;

        public CommandRelay(Action a)
        {
            this.a = a;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            a();
        }

        public event EventHandler CanExecuteChanged;
    }
}