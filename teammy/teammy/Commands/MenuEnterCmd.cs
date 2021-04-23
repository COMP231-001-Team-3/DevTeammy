using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace teammy.Commands
{
    public class MenuEnterCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it were hovered 
        ///     upon.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        public void Execute(object parameter)
        {
            //The Button whose background is to be set
            Button btnIcon = parameter as Button;
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }
    }
}
