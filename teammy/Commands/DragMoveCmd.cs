using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace teammy.Commands
{
    public class DragMoveCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Window active = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            active.DragMove();
        }
    }
}
