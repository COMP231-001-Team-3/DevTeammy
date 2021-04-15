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
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TeamsContactlist.xaml
    /// </summary>
    public partial class TeamsContactlist : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public user currentUser { get; set; } = globalItems["currentUser"] as user;
        public team currentTeam { get; set; }


        private List<user> contactinfo;
        private teammyEntities dbContext = globalItems["dbContext"] as teammyEntities;
        public TeamsContactlist()
        {
            InitializeComponent();
        }

        private void contactWindow_Loaded(object sender, RoutedEventArgs e)
        {
            teamnamelabel.Content = currentTeam.Team_Name;
            
            contactinfo = (from user in dbContext.users
                           join mate in dbContext.team_mates
                              on user.user_id equals mate.user_id
                           join teams in dbContext.teams
                              on mate.Team_ID equals teams.Team_ID
                           where teams.Team_Name == currentTeam.Team_Name
                           select user).ToList();



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
            Process.Start("mailto:" + (dtgTeamMates.SelectedItem as user).email_address);
        }
    }
}
