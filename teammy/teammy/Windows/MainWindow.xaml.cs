using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public user currentUser { get; set; } = globalItems["currentUser"] as user;

        List<task> myTasksData;
        List<task> dueWeekData;
        teammyEntities dbContext = new teammyEntities();

        public MainWindow()
        {
            InitializeComponent();
            Display_AssignToMe();
            Display_ComingUp();
        }

        public void Display_AssignToMe()
        {
            myTasksData = (from task in dbContext.tasks 
                           join assignee in dbContext.assignees
                              on task.assigned_group equals assignee.assigned_group
                           join mate in dbContext.team_mates
                              on assignee.mate_id equals mate.mate_id
                           join user in dbContext.users
                              on mate.user_id equals user.user_id
                           where user.user_name.Equals(currentUser.user_name)
                           select task).ToList();

            AssignedtomeDatagrid.ItemsSource = myTasksData;
        }

        public void Display_ComingUp()
        {
            dueWeekData = myTasksData.FindAll(task => task.due_date <= DateTime.Now.AddDays(7) && task.due_date >= DateTime.Now);
            ComingDatagrid.ItemsSource = dueWeekData;
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (globalItems["createProjInstance"] as Window).Show();
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
            ContextMenu cm = FindResource("cmButton") as ContextMenu;
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

        private void progMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["progReportInstance"] as Window).Show();
        }

        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["teamsListInstance"] as Window).Show();
        }
    }
}