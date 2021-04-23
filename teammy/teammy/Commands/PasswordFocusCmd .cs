using System;
using System.Windows;
using System.Windows.Input;
using teammy.Views;

namespace teammy.Commands
{
    public class PasswordFocusCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            LoginWindow loginWdw = parameter as LoginWindow;

            loginWdw.pwdPlaceholder.Visibility = Visibility.Hidden;
            loginWdw.pwdPassword.Visibility = Visibility.Visible;
            loginWdw.pwdPassword.Focus();
        }
    }
}
