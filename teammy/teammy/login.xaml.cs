using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //connecting DB
        string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        List<UserModel> users = new List<UserModel>();

        public LoginWindow()
        {
            InitializeComponent();

            Application.Current.Resources["loginInstance"] = this;

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
                    users.Add(new UserModel(reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
                }
            }

        }

        private void signinBtn_Click(object sender, RoutedEventArgs e)
        {
            //getting input from userIDtextbox and User Passwordtextbox
            string nameinput = usernameInput.Text;
            string passwordinput = passwordInput.Password;

            UserModel userEntered = users.Find((user) => user.Username.Equals(nameinput));

            if ( userEntered == null || !userEntered.VerifyPassword(passwordinput))
            {
                MessageBox.Show("The username/password entered is incorrect!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {//showing homepage if authentication success
                Application.Current.Resources.Add("currentUser", userEntered);
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
            if (usernameInput.Text == null || usernameInput.Text.Equals(""))
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
            if (passwordInput.Password == null || passwordInput.Password.Equals(""))
            {
                passwordInput.Visibility = Visibility.Hidden;
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void loginWindow_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
        #region Title Bar Button Event Handlers

        /// <summary>
        ///     Shuts down the application
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     Minimizes the current window
        /// </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Title Pane Event Handlers

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it were hovered 
        ///     upon.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it lost focus.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = null;
        }
        #endregion
    }
}