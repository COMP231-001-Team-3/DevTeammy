using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class UsernameLostCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginViewModel loginVModel;

        public UsernameLostCmd(LoginViewModel lvModel)
        {
            loginVModel = lvModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TextBox txtUserName = parameter as TextBox;

            if (loginVModel.txtUserName == null ||  loginVModel.txtUserName.Equals(""))
            {
                loginVModel.txtUserName = "Enter your user name";
                txtUserName.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
