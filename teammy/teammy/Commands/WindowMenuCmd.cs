using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using teammy.Views;
using System.Windows.Controls;

namespace teammy.Commands
{
    public class WindowMenuCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private static ResourceDictionary globalItems = Application.Current.Resources;

        public bool CanExecute(object parameter)
        {
            Window active = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            return active?.GetType() != typeof(LoginWindow);
        }

        public void Execute(object parameter)
        {
            ContextMenu cm = globalItems["cmButton"] as ContextMenu;
            cm.PlacementTarget = parameter as Button;
            cm.IsOpen = true;
        }
    }
}
