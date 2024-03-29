﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using teammy.Models;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TeamsContactlist.xaml
    /// </summary>
    public partial class TeamDetails : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public User currentUser { get; set; } = globalItems["currentUser"] as User;
        public Team currentTeam { get; set; }


        private List<User> contactinfo;
        public TeamDetails()
        {
            InitializeComponent();
        }

        private void contactWindow_Loaded(object sender, RoutedEventArgs e)
        {
            teamnamelabel.Content = currentTeam.TeamName;

            contactinfo = currentTeam.Members;

            dtgTeamMates.ItemsSource = contactinfo;
        }
        #region Title Bar Button Event Handlers
        /// <summary>
        ///     Shuts down the application
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Minimizes the current window
        /// </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Displays page menu when button is clicked
        /// </summary>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = globalItems["cmButton"] as ContextMenu;
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
        #endregion
        private void Mail_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:" + (dtgTeamMates.SelectedItem as User).Email);
        }
    }
}
