﻿using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
<<<<<<< HEAD
=======
using System.Windows.Controls;
using System.Windows.Navigation;
>>>>>>> 11e363c... Menu Completed

namespace teammy
{
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
        
        #region Fields
        //Hosted DB connection string
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";

        //Colors for project cards
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        //Margins indicate position of each box to be placed
        int left = 0, top = 0, right = 361, bottom = 260;
        int boxCount = 0;
        ProjectBox toBeInserted;
        MySqlConnection conn;
        #endregion

        #region Constructor
        public CreateProject()
        {
            InitializeComponent();

            //Connection and data retrieval starts here    
            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT Proj_Name FROM projects", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            using (reader)
            {
                //Custom Control developed for this app
                ProjectBox project;

                //Variables for usage in loop declared beforehand for performance reasons
                Random rd = new Random();
                string projName;

                //Loop to read through results from query
                while (reader.Read())
                {
                    projName = reader[0].ToString();
                    
                    //Creation & Initialization of ProjectBox
                    project = new ProjectBox() { ProjectName = projName, Margin = new Thickness(left, top, right, bottom), ProjectProfileBack= backColors[rd.Next(0, 18)] };

                    //Adds the newly created ProjectBox to the Grid within the ScrollViewer
                    projGrid.Children.Add(project);

                    //Updates margin for the next box
                    left += 175;
                    right -= 175;
                    
                    //If 3 boxes have been created...then
                    if(boxCount == 2)
                    {
                        //Margin updates for a new ProjectBox in a new row
                        top += 132;
                        bottom -= 132;
                        left = 0;
                        right = 361;
                        boxCount = 0;
                    }
                    else
                    {
                        boxCount++;
                    }
                }
            } // Data retrieval ends here
        }
        #endregion

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
<<<<<<< HEAD
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
 }
=======

        }
        #endregion

        #region Title Pane Event Handlers
>>>>>>> 438712a... Added comments for readability

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }
<<<<<<< HEAD

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
=======
        #endregion
<<<<<<< HEAD
>>>>>>> 438712a... Added comments for readability
=======

        private void homeMenu_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (Application.Current.Resources["mainInstance"] as Window).Show();
        }
<<<<<<< HEAD
>>>>>>> 11e363c... Menu Completed
=======

        private void btnCreateProj_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random();
            toBeInserted = new ProjectBox() { ProjectProfileBack=backColors[rd.Next(0, backColors.Length - 1)]};
            TextBox txtNameInput = new TextBox() { Height = 25, Width = 120, FontSize = 16};
            //{ Text = "Enter Project Name", Foreground = new SolidColorBrush(Colors.Gray)}
            if (boxCount == 0)
            {
                //Margin updates for a new ProjectBox in a new row
                top += 132;
                bottom -= 132;
                left = 0;
                right = 361;
                boxCount = 0;
            }
            toBeInserted.Margin = new Thickness(left, top, right, bottom);
            txtNameInput.Margin = new Thickness(left, top + 93, right, bottom - 3);

            projGrid.Children.Add(toBeInserted);
            projGrid.Children.Add(txtNameInput);
            txtNameInput.Focus();
            txtNameInput.GotFocus += new RoutedEventHandler(txtNameInput_GotFocus);
            txtNameInput.LostFocus += new RoutedEventHandler(txtNameInput_LostFocus);
            txtNameInput.KeyUp += new KeyEventHandler(txtNameInput_KeyUp);
        }

        private void txtNameInput_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txtNameInput = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                toBeInserted.ProjectName = txtNameInput.Text;
                txtNameInput.Visibility = Visibility.Hidden;

                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand insert = new MySqlCommand("INSERT INTO projects VALUES(Proj_ID, @Proj_Name, @Team_Num);", conn);
                MySqlCommand commit = new MySqlCommand("COMMIT;", conn);
                insert.Parameters.AddWithValue("Proj_Name", txtNameInput.Text);
                insert.Parameters.AddWithValue("Team_Num", null);

                insert.ExecuteNonQuery();
                commit.ExecuteNonQuery();
            }
        }

        private void txtNameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtNameInput = sender as TextBox;
            if (txtNameInput.Text.Equals(""))
            {
                txtNameInput.Text = "Enter Name";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void txtNameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtNameInput = sender as TextBox;
            if(txtNameInput.Text.Equals("Enter Name"))
            {
                txtNameInput.Text = "";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
>>>>>>> c07ea48... Create Project function complete
    }
}
