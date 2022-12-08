using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace teammy.Commands
{
    public class WindowMinCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Window active = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            active.WindowState = WindowState.Minimized;
        }
    }
}
