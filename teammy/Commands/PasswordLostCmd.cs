using System;
using System.Windows;
using System.Windows.Input;
using teammy.Views;

namespace teammy.Commands
{
    public class PasswordLostCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            LoginWindow loginWdw = parameter as LoginWindow;

            if (loginWdw.pwdPassword.Password == null || loginWdw.pwdPassword.Password.Equals(""))
            {
                loginWdw.pwdPassword.Visibility = Visibility.Hidden;
                loginWdw.pwdPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
