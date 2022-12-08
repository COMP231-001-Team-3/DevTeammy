using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using teammy.Commands;
using teammy.Models;

namespace teammy.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        private List<User> users;
        private IMongoDatabase dbContext = DBConnector.Connect();

        private string _txtUserName = "Enter your user name";
        public string txtUserName
        {
            get => _txtUserName;
            set
            {
                SetProperty(ref _txtUserName, value);
            }
        }

        private string _pwdPlaceholder = "Enter your password";
        public string pwdPlaceholder
        {
            get => _pwdPlaceholder;
            set
            {
                SetProperty(ref _pwdPlaceholder, value);
            }
        }

        public ICommand signInCommand { get; set; } 
        public ICommand usernameFocusCmd { get; set; }
        public ICommand passwordFocusCmd { get; set; }
        public ICommand userLostCmd { get; set; }
        public ICommand passwordLostCmd { get; set; }

        public LoginVM()
        {
            Application.Current.Resources["loginInstance"] = this;
            Parallel.Invoke(() =>
            {
                users = 
                (from user in dbContext.GetCollection<User>("users").AsQueryable()
                select user).ToList();
            });

            signInCommand = new SignInCmd(this);
            usernameFocusCmd = new UsernameFocusCmd(this);
            userLostCmd = new UsernameLostCmd(this);
            passwordFocusCmd = new PasswordFocusCmd();
            passwordLostCmd = new PasswordLostCmd();
        }

        public bool SignIn(string pwdPassword)
        {
            User userEntered = users.Find((user) => user.Username.Equals(txtUserName));
            bool? validPassword = userEntered?.Password.Equals(pwdPassword);

            if (userEntered == null || !(bool)validPassword)
            {
                MessageBox.Show("The username/password entered is incorrect!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            else if ((bool)validPassword)
            {
                //showing homepage if authentication success
                Application.Current.Resources.Add("currentUser", userEntered);

                SplashScreen splashLog = new SplashScreen("../images/splashLogging.png");
                splashLog.Show(true);
                (Application.Current.Resources["mainInstance"] as Window).Show();                
                (Application.Current.Resources["createProjInstance"] as CreateProject).LoadProjects();
                return true;
            }
            return false;
        }
    }
}
