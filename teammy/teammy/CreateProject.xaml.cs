using System;
using MySql.Data.MySqlClient;
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
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
        public string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        List<string> ProjectNames = new List<string>();

        public CreateProject()
        {
            InitializeComponent();
            MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT Proj_Name FROM projects", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            using (reader)
            {
                ProjectBox project;
                int left = 0, top = 0, right = 361, bottom = 260;
                int count = 0;
                Random rd = new Random();
                string projName;
                string profChars;
                string[] nameWords;
                while (reader.Read())
                {
                    projName = reader[0].ToString();
                    ProjectNames.Add(projName);
                    nameWords = projName.Split(' ');
                    if(nameWords.Length >= 2)
                    {
                        profChars = nameWords[0][0] + "" + nameWords[1][0];
                    }
                    else
                    {
                        profChars = nameWords[0][0] + "" + nameWords[0][1];
                    }
                    project = new ProjectBox() { ProjectName = projName, Margin = new Thickness(left, top, right, bottom), ProjectProfileBack= backColors[rd.Next(0, 18)], ProjectProfile=profChars };
                    projGrid.Children.Add(project);
                    left += 175;
                    right -= 175;
                    
                    if(count == 2)
                    {
                        top += 132;
                        bottom -= 132;
                        left = 0;
                        right = 361;
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
 }

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
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity= 0.7};
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
    }
}
