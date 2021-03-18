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
using MySql.Data.MySqlClient;
using System.Windows.Shapes;
using teammy.ProjectDetail;

namespace teammy
{

    /// <summary>
    /// Interaction logic for ProjBoard.xaml
    /// </summary>
    public partial class ProjBoard : Window
    {
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";

        int left, top, right, bottom;
        int catCount = 0;
        int totalCats = 0;
        NewCategory toBeInserted;
        MySqlConnection conn;


        //Colors for project cards
        //Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
       
        public ProjBoard()
        {
            InitializeComponent();
            LoadCategorys();
        }

        private void LoadCategorys()
        {
            left = 0;
            top = 0;
            right = 5;
            bottom = 0;
            catCount = 0;
            totalCats = 0;
            caStackPanel.Children.Clear();

            conn = new MySqlConnection(connectionString);
            
            conn.Open();
            
            MySqlCommand getCategorys = new MySqlCommand("SELECT category_name FROM categories NATURAL JOIN projects where proj_name = @projName", conn);
            getCategorys.Parameters.AddWithValue("projName", ProjNameLable.Content.ToString());
            MySqlDataReader categorysReader = getCategorys.ExecuteReader();
                       
            using (categorysReader)
            {
                NewCategory category;

                //Random rd = new Random();
                string catName;
                while (categorysReader.Read())
                {
                    totalCats++;
                    catName = categorysReader[0].ToString();

                    category = new NewCategory() { CategoryName = catName, Margin = new Thickness(left, top, right, bottom)};
                    caStackPanel.Children.Add(category);                   
                }
            }
        }
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
            Hide();
            (Application.Current.Resources["mainInstance"] as Window).Show();
        }

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            add_newCategory();
            
        }
        private void add_newCategory()
        {
            if (++totalCats == 10)
            {
                totalCats--;
                MessageBox.Show("The maximum limit for categorys per project is 9!", "Max categorys completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Margins indicate position of each box to be placed
            int left = 0, top = 0, right = 5, bottom = 0;
            NewCategory newCategory = new NewCategory() { Margin = new Thickness(left, top, right, bottom) };
            caStackPanel.Children.Add(newCategory);

            //conn = new MySqlConnection(connectionString);
            //conn.Open();
            //MySqlCommand insertCatName = new MySqlCommand("Insert INTO categories VALUES(category_id, @category_name, (SELECT Proj_ID FROM projects WHERE Proj_Name = @projName));", conn);
            //MySqlCommand commit = new MySqlCommand("COMMIT;", conn);
            //insertCatName.Parameters.AddWithValue("category_name", toBeInserted.txtCategoryName.Text);
            //insertCatName.Parameters.AddWithValue("projName", ProjNameLable.Content.ToString());

            //insertCatName.ExecuteNonQuery();
            //commit.ExecuteNonQuery();
        }
    }
}
