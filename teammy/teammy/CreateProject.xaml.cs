﻿using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
        #endregion

        #region Constructor
        public CreateProject()
        {
            InitializeComponent();

            //Connection and data retrieval starts here
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT Proj_Name FROM projects", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            using (reader)
            {
                //Custom Control developed for this app
                ProjectBox project;

                //Margins indicate position of each box to be placed
                int left = 0, top = 0, right = 361, bottom = 260;
                int count = 0;

                //Variables for usage in loop declared beforehand for performance reasons
                Random rd = new Random();
                string projName, profChars;
                string[] nameWords;

                //Loop to read through results from query
                while (reader.Read())
                {
                    projName = reader[0].ToString();
                    nameWords = projName.Split(' ');

                    //If Project name has two or more words...then
                    if(nameWords.Length >= 2)
                    {
                        profChars = nameWords[0][0] + "" + nameWords[1][0];
                    }
                    else
                    {
                        profChars = nameWords[0][0] + "" + nameWords[0][1];
                    }

                    //Creation & Initialization of ProjectBox
                    project = new ProjectBox() { ProjectName = projName, Margin = new Thickness(left, top, right, bottom), ProjectProfileBack= backColors[rd.Next(0, 18)], ProjectProfile=profChars };

                    //Adds the newly created ProjectBox to the Grid within the ScrollViewer
                    projGrid.Children.Add(project);

                    //Updates margin for the next box
                    left += 175;
                    right -= 175;
                    
                    //If 3 boxes have been created...then
                    if(count == 2)
                    {
                        //Margin updates for a new ProjectBox in a new row
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
        #endregion
    }
}
