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

namespace teammy
{
    /// <summary>
    /// Interaction logic for ProjBoard.xaml
    /// </summary>
    public partial class ProjBoard : Window
    {
        #region Fields
        //Hosted DB connection string
        //private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";

        //Colors for project cards
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        #endregion
        public ProjBoard()
        {
            InitializeComponent();

            ////Connection and data retrieval starts here
            //MySqlConnection conn = new MySqlConnection(connectionString);
            //conn.Open();

            //string sql = "SELECT Proj_Name FROM projects";
            //MySqlCommand cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader reader = cmd.ExecuteReader();

            //using (reader)
            //{
            //    //Custom Control developed for this app
            //    //NewCategory addCategory;

            //    //Margins indicate position of each box to be placed
            //    int left = 0, top = 0, right = 361, bottom = 260;
            //    int count = 0;

            //    //Variables for usage in loop declared beforehand for performance reasons
            //    Random rd = new Random();
            //    string projName, profChars;
            //    string[] nameWords;

            //    //Loop to read through results from query
            //    while (reader.Read())
            //    {
            //        projName = reader[0].ToString();
            //        nameWords = projName.Split(' ');

            //        //If Project name has two or more words...then
            //        if (nameWords.Length >= 2)
            //        {
            //            profChars = nameWords[0][0] + "" + nameWords[1][0];
            //        }
            //        else
            //        {
            //            profChars = nameWords[0][0] + "" + nameWords[0][1];
            //        }

            //        //Creation & Initialization of ProjectBox
            //        //addCategory = new NewCategory() { CategoryName = projName, Margin = new Thickness(left, top, right, bottom)};

            //        //Adds the newly created ProjectBox to the Grid within the ScrollViewer
            //        //newCategoryGrid.Children.Add(addCategory);

            //        //Updates margin for the next box
            //        left += 175;
            //        right -= 175;

            //        //If 3 boxes have been created...then
            //        if (count == 2)
            //        {
            //            //Margin updates for a new ProjectBox in a new row
            //            top += 132;
            //            bottom -= 132;
            //            left = 0;
            //            right = 361;
            //            count = 0;
            //        }
            //        else
            //        {
            //            count++;
            //        }
            //    }
            //} // Data retrieval ends here
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
            //Margins indicate position of each box to be placed
            int left = 0, top = 0, right = 5, bottom = 0;
            NewCategory newCategory = new NewCategory() { Margin = new Thickness(left, top, right, bottom) };
            caStackPanel.Children.Add(newCategory);
        }
    }
}
