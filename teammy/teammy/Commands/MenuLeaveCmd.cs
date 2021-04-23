using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace teammy.Commands
{
    public class MenuLeaveCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it lost focus.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        public void Execute(object parameter)
        {
            Button btnIcon = parameter as Button;
            btnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
