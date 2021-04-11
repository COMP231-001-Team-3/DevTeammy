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
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=dev; pwd=rds8w77c0ehnw2fx; port=25060;";
        
        int left, top, right, bottom;
        int catCount = 0;
        int totalCats = 0;
        NewCategory toBeInserted;
        MySqlConnection conn;


        public CardBox currentProj { get; set; } = Application.Current.Resources["currentProj"] as CardBox;
        public ProjBoard()
        {
            InitializeComponent();
            LoadCategorys();            
        }

        private void LoadCategorys()
        {

            ProjNameLable.Content = currentProj.FullName;
            Console.WriteLine(ProjNameLable.Content);
            left = 0;
            top = 0;
            right = 5;
            bottom = 0;
            catCount = 0;
            totalCats = 0;
            caStackPanel.Children.Clear();

            conn = new MySqlConnection(connectionString);            
            conn.Open();
            
            MySqlCommand getCategorys = new MySqlCommand("SELECT category_name FROM categories NATURAL JOIN projects where Proj_Name = @projName", conn);           
            getCategorys.Parameters.AddWithValue("projName", currentProj.FullName);
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
                    Application.Current.Resources["catName"] =  catName;

                    category = new NewCategory() { CategoryName = catName, Margin = new Thickness(left, top, right, bottom)};
                    
                    caStackPanel.Children.Add(category);                   
                }
            }
            conn.Close();
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
            Application.Current.Resources["catName"] = null;
            NewCategory newCategory = new NewCategory();
            caStackPanel.Children.Add(newCategory);           
        }
    }
}
