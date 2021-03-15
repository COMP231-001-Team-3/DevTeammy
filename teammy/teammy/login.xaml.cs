using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        //connecting DB
        string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        List<UserModel> users = new List<UserModel>();

        public login()
        {
            InitializeComponent();

            Application.Current.Resources["login"] = this;

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            //Getting password by user_id
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users", conn);
            //cmd.Parameters.AddWithValue("@idinput", nameinput);

            MySqlDataReader reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    users.Add(new UserModel(reader));
                }
            }

        }

        private void signinBtn_Click(object sender, RoutedEventArgs e)
        {
            //getting input from userIDtextbox and User Passwordtextbox
            string nameinput = usernameInput.Text;
            string passwordinput = passwordInput.Password;

            UserModel userEntered = users.Find((user) => user.Username.Equals(nameinput)); 
            
            if(userEntered == null)
            {
                //showing invalid user id error
                MessageBox.Show("The username entered is not valid!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(!userEntered.VerifyPassword(passwordinput))
            {
                MessageBox.Show("The username/password entered is incorrect!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {//showing homepage if authentication succees
                (Application.Current.Resources["mainInstance"] as Window).Show();
                Hide();
            }



        }

        private void usernameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameInput.Text.Equals("Enter your user name"))
            {
                usernameInput.Text = "";
                usernameInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void usernameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if(usernameInput.Text == null || usernameInput.Text.Equals(""))
            {
                usernameInput.Text = "Enter your user name";
                usernameInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void passwordPlaceholder_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = Visibility.Hidden;
            passwordInput.Visibility = Visibility.Visible;
            passwordInput.Focus();
        }

        private void passwordInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if(passwordInput.Password == null || passwordInput.Password.Equals(""))
            {
                passwordInput.Visibility = Visibility.Hidden;
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void loginWindow_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
