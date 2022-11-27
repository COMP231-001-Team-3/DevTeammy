using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class UsernameFocusCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private LoginVM loginVModel;

        public UsernameFocusCmd(LoginVM lvModel)
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
            if (txtUserName.Text.Equals("Enter your user name"))
            {
                loginVModel.txtUserName = "";
                txtUserName.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
