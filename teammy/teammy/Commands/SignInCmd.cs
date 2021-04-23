using System;
using System.Windows;
using System.Windows.Input;
using teammy.ViewModels;
using teammy.Views;

namespace teammy.Commands
{
    public class SignInCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginViewModel loginVModel;

        public SignInCmd(LoginViewModel lvModel)
        {
            loginVModel = lvModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            LoginWindow loginWdw = parameter as LoginWindow;

            bool loggedIn = loginVModel.SignIn(loginWdw.pwdPassword.Password);
            if (loggedIn)
            {
                (parameter as Window).Hide();
            }            
        }
    }
}
