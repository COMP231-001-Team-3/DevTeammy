using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TeamsList.xaml
    /// </summary>
    public partial class TeamsList : Window
    {
        #region Fields
        private static ResourceDictionary globalItems = Application.Current.Resources;

        //Colors for team cards
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        private teammyEntities dbContext = new teammyEntities();

        //Margins indicate position of each box to be placed
        private int left, top, right, bottom;
        private int boxCount = 0;
        private int totalBoxes = 0;
        private CardBox toBeInserted;

        private TextBox txtNameInput;
        #endregion

        #region Properties
        public user currentUser { get; set; } = globalItems["currentUser"] as user;
        #endregion

        #region Constructor
        public TeamsList()
        {
            InitializeComponent();
            LoadTeams();
        }
        #endregion

        #region Miscellaneous

        /// <summary>
        ///     Loads teams from the database
        /// </summary>
        private void LoadTeams()
        {
            left = 0;
            right = 361;
            top = 0;
            bottom = 260;
            boxCount = 0;
            totalBoxes = 0;
            teamsGrid.Children.Clear();

            List<string> teamNames = (from team in dbContext.teams
                                      join mate in dbContext.team_mates
                                        on team.Team_ID equals mate.Team_ID
                                      join user in dbContext.users
                                        on mate.user_id equals user.user_id
                                      where user.user_id == currentUser.user_id
                                      select team.Team_Name).ToList();

            CardBox teamBox;

            //Variables for usage in loop declared beforehand for performance reasons
            Random rd = new Random();
            string teamName;

            //Loop to read through results from query
            for (int i = 0; i < teamNames.Count; i++)
            {
                totalBoxes++;
                teamName = teamNames[i].ToString();

                //Creation & Initialization of teamBox
                teamBox = new CardBox() { FullName = teamName, Margin = new Thickness(left, top, right, bottom), ProfileBack = backColors[rd.Next(0, 18)] };

                //Adds the newly created teamBox to the Grid within the ScrollViewer
                teamsGrid.Children.Add(teamBox);

                //Updates margin for the next box
                left += 175;
                right -= 175;

                //If 3 boxes have been created...then
                if (boxCount == 2)
                {
                    //Margin updates for a new teamBox in a new row
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
        }

        /// <summary>
        ///     Adds team to database when user clicks 'Enter' key while
        ///     focus is on input TextBox
        /// </summary>
        private void txtNameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                toBeInserted.FullName = txtNameInput.Text;
                txtNameInput.Visibility = Visibility.Hidden;
                
                dbContext.teams.Add(new team()
                {
                    Team_Name = txtNameInput.Text
                });

                dbContext.team_mates.Add(new team_mates()
                {
                    Team_ID = (from team in dbContext.teams
                               where team.Team_Name == txtNameInput.Text
                               select team.Team_ID).Single(),
                    user_id = currentUser.user_id
                });

                dbContext.SaveChanges();
                btnDone.Visibility = Visibility.Hidden;
                btnCancel.Visibility = Visibility.Hidden;
                btnCreateTeam.Visibility = Visibility.Visible;
            }
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

        /// <summary>
        ///     Displays page menu when button is clicked
        /// </summary>
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

        #region Button Event Handlers
        /// <summary>
        ///     Creates a team box and a textbox for team name input
        /// </summary>
        private void btnCreateTeam_Click(object sender, RoutedEventArgs e)
        {
            btnCreateTeam.Visibility = Visibility.Hidden;
            btnDone.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;

            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for teams per account is 9!", "Max teams completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Random rd = new Random();
            toBeInserted = new CardBox() { ProfileBack = backColors[rd.Next(0, backColors.Length - 1)] };
            txtNameInput = new TextBox() { Height = 25, Width = 120, FontSize = 16 };

            toBeInserted.Margin = new Thickness(left, top, right, bottom);
            txtNameInput.Margin = new Thickness(left, top + 93, right, bottom - 3);

            if (boxCount == 2)
            {
                //Margin updates for a new teamBox in a new row
                top += 132;
                bottom -= 132;
                left = 0;
                right = 361;
                boxCount = 0;
            }
            else
            {
                //Updates margin for the next box
                left += 175;
                right -= 175;
                boxCount++;
            }

            //creation of team box and text box to accept user input
            teamsGrid.Children.Add(toBeInserted);
            teamsGrid.Children.Add(txtNameInput);
            txtNameInput.Focus();
            txtNameInput.GotFocus += new RoutedEventHandler(txtNameInput_GotFocus);
            txtNameInput.LostFocus += new RoutedEventHandler(txtNameInput_LostFocus);
            txtNameInput.KeyUp += new KeyEventHandler(txtNameInput_KeyUp);

            btnCreateTeam.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Inserts a team into the DB
        /// </summary>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            //Input used for display
            toBeInserted.FullName = txtNameInput.Text;
            txtNameInput.Visibility = Visibility.Hidden;

            //Inserting team
            dbContext.teams.Add(new team()
            {
                Team_Name = txtNameInput.Text
            });

            dbContext.SaveChanges();

            dbContext.team_mates.Add(new team_mates()
            {
                Team_ID = (from team in dbContext.teams
                           where team.Team_Name.Equals(txtNameInput.Text)
                           select team.Team_ID).Single(),
                user_id = currentUser.user_id
            });

            dbContext.SaveChanges();

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateTeam.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Resets app to the state before clicking the create team 
        ///     button
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            teamsGrid.Children.Remove(toBeInserted);
            teamsGrid.Children.Remove(txtNameInput);
            //margin updates undone
            if (boxCount == 0)
            { 
                
                boxCount = 2;
                left = 350;
                right = 11;
                top -= 132;
                bottom += 132;
            }
            else
            {
                left -= 175;
                right += 175;
                --boxCount;
            }
            --totalBoxes;

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateTeam.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hover effect (Icon background) for cancel button
        /// </summary>
        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            cancelbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Hover effect (Icon background) for cancel button
        /// </summary>
        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            cancelbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }
        #endregion

        #region Page Navigation
        /// <summary>
        ///     Redirects to Progress Report Page
        /// </summary>
        private void progMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (globalItems["progReportInstance"] as Window).Show();
            Hide();
        }

        /// <summary>
        ///     Redirects to Boards Page
        /// </summary>
        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (globalItems["createProjInstance"] as Window).Show();
            Hide();
        }

        /// <summary>
        ///     Redirects to Home page
        /// </summary>
        private void homeMenuItem_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["mainInstance"] as Window).Show();
        }
        #endregion       

        #region Placeholder management
        /// <summary>
        ///     Resets TextBox text to placeholder text when empty
        /// </summary>
        private void txtNameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Equals(""))
            {
                txtNameInput.Text = "Enter Name";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        /// <summary>
        ///     Removes placeholder text
        /// </summary>
        private void txtNameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Equals("Enter Name"))
            {
                txtNameInput.Text = "";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        #endregion
    }
}