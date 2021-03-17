﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace teammy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserModel currentUser { get; set; } = Application.Current.Resources["currentUser"] as UserModel;
        public MainWindow()
        {
            InitializeComponent();
            displaying_assigntome();
            displaying_comingup();

        }

        public void displaying_assigntome()
        {
            ObservableCollection<TasksAssignedtome> list = new ObservableCollection<TasksAssignedtome>();
            //DB connection
            string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            //Getting list of assigned tasks by userid
            MySqlCommand cmd = new MySqlCommand("Select task_name, due_date,progress_code FROM tasks NATURAL JOIN assignees NATURAL JOIN team_mates NATURAL JOIN users WHERE user_name = @nameUser", conn);
            cmd.Parameters.AddWithValue("nameUser", currentUser.Username);
            MySqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {


                while (reader.Read())
                {

                    list.Add(new TasksAssignedtome { taskname = (string)reader["task_name"], progress = (string)reader["progress_code"] });
                }
            }
            AssignedtomeDatagrid.ItemsSource = list;
        }

        public void displaying_comingup()
        {
            ObservableCollection<TasksAssignedtome> list = new ObservableCollection<TasksAssignedtome>();
            //DB connection
            string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            //Getting list of assigned tasks by userid
            MySqlCommand cmd = new MySqlCommand("Select task_name, due_date,progress_code FROM tasks NATURAL JOIN assignees NATURAL JOIN team_mates NATURAL JOIN users WHERE user_name = @nameUser AND due_date < (NOW() + INTERVAL 7 DAY)", conn);
            cmd.Parameters.AddWithValue("nameUser", currentUser.Username);

            MySqlDataReader reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new TasksAssignedtome { taskname = (string)reader["task_name"], progress = (string)reader["progress_code"], duedate = "Due on " + reader["due_date"].ToString() });
                }
            }
            ComingDatagrid.ItemsSource = list;
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.Resources["createProjInstance"] as Window).Show();
            Hide();
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
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
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

        private void homeMenu_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (Application.Current.Resources["mainInstance"] as Window).Show();
        }
    }
}