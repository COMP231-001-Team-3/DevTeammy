using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using teammy.Models;
using MongoDB.Driver;

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

        private IMongoDatabase dbContext = DBConnector.Connect();

        //Margins indicate position of each box to be placed
        private int left, top, right, bottom;
        private int boxCount = 0;
        private int totalBoxes = 0;
        private CardBox toBeInserted;

        private TextBox txtNameInput;
        private List<Team> teams;
        #endregion

        #region Properties
        public User currentUser { get; set; } = globalItems["currentUser"] as User;
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

            teams = dbContext.GetCollection<Team>("teams")
                                .Find(Builders<Team>.Filter.ElemMatch(t => t.Members, m => m.UserId == currentUser.UserId))
                                .ToList();

            CardBox teamBox;

            //Variables for usage in loop declared beforehand for performance reasons
            Random rd = new Random();
            string teamName;

            //Loop to read through results from query
            for (int i = 0; i < teams.Count; i++)
            {
                totalBoxes++;
                teamName = teams[i].TeamName;

                //Creation & Initialization of teamBox
                teamBox = new CardBox() { FullName = teamName, Margin = new Thickness(left, top, right, bottom), ProfileBack = backColors[rd.Next(0, 18)]};
                teamBox.CardClick += new RoutedEventHandler(teamBox_CardClick);

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

        public void teamBox_CardClick(object sender, RoutedEventArgs e)
        {
            CardBox current = ((sender as Button).Parent as Grid).Parent as CardBox;

            TeamsContactlist contactPage = new TeamsContactlist()
            { 
                currentTeam = 
                    (from team in teams
                    where team.TeamName.Equals(current.FullName)
                    select team)
                    .Single() 
            };

            contactPage.ShowDialog();
        }

        /// <summary>
        ///     Adds team to database when user clicks 'Enter' key while
        ///     focus is on input TextBox
        /// </summary>
        private void txtNameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnDone_Click(sender, new RoutedEventArgs());
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
        private async void btnDone_Click(object sender, RoutedEventArgs e)
        {
            //Input used for display
            toBeInserted.FullName = txtNameInput.Text;
            txtNameInput.Visibility = Visibility.Hidden;

            string inputName = txtNameInput.Text;
            await Task.Run(() => AddTeam(inputName));

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateTeam.Visibility = Visibility.Visible;
        }

        private async void AddTeam(string inputName)
        {
            int teamId = dbContext.GetCollection<IDSequence>("idValues")
                                     .FindOneAndUpdate(i => i.myID.Equals("Sequence"), Builders<IDSequence>.Update.Inc(i => i.TeamId, 1))
                                     .TeamId;
            //Inserting team
            await dbContext.GetCollection<Team>("teams")
                            .InsertOneAsync(new Team()
                            {
                                TeamId = teamId,
                                TeamName = inputName,
                                Members = new List<User>() { currentUser }
                            });
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