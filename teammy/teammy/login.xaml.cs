using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace teammy
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        private void signinBtn_Click(object sender, RoutedEventArgs e)
        {
            //getting input from userIDtextbox and User Passwordtextbox
            string idinput=usernameInput.Text;
            string passwordinput= passwordInput.Text;

            //connecting DB
           string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            //Getting password by user_id
            MySqlCommand cmd = new MySqlCommand("SELECT password FROM users where user_id=@idinput", conn);
            cmd.Parameters.AddWithValue("@idinput", idinput);

            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                //comparing password with userinputpassword
                string password = reader["password"].ToString();
               
               if (passwordinput!=password)
                {//password error
                    MessageBox.Show("Password does not match. Please Try Again", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
         
                else
                {//showing homepage if authentication succees
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Hide();

                }
                

            }
            else {//showing invalid user id error
                MessageBox.Show("UserID does not exist", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error); }



        }
    }
}
