using System;
using System.Windows;
using System.Windows.Input;

namespace teammy.Commands
{
    public class WindowClosedCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
