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
using teammy.UserControls;
using System.Collections.ObjectModel;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TeamsList.xaml
    /// </summary>
    public partial class Teams : Window
    {
        #region Fields
        private static ResourceDictionary globalItems = Application.Current.Resources;

        private IMongoDatabase dbContext = DBConnector.Connect();

        //Margins indicate position of each box to be placed
        private int left, top, right, bottom;
        private int boxCount = 0;
        private int totalBoxes = 0;

        private TextBox txtNameInput;
        private List<Team> teams;
        #endregion

        #region Properties
        public User currentUser { get; set; } = globalItems["currentUser"] as User;

        public ObservableCollection<CardDetails> Cards { get; set; } = new ObservableCollection<CardDetails>() { 
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false),
            new CardDetails(null, false, false, false)
        };

        #endregion

        #region Constructor
        public Teams()
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
            teams =
                (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
                 where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
                 select team).ToList();
            totalBoxes = teams.Count;

            //Variables for usage in loop declared beforehand for performance reasons
            string teamName;

            //Loop to read through results from query
            for (int i = 0; i < teams.Count; i++)
            {
                teamName = teams[i].TeamName;
                Cards.RemoveAt(i);
                Cards.Insert(i, new CardDetails(teamName, true, false, false));

            }
        }

        public void teamBox_CardClick(object sender, RoutedEventArgs e)
        {
            CardBox current = ((sender as Button).Parent as Grid).Parent as CardBox;
            try
            {
                TeamDetails contactPage = new TeamDetails()
                {
                    currentTeam =
                    (from team in teams
                     where team.TeamName.Equals(current.Details.FullName)
                     select team)
                    .Single()
                };
                contactPage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The team {current.Details.FullName} does not have any members as of now. Check back later! 😊", "No team members");
            }

            
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
            int firstHiddenBox = Cards.IndexOf(Cards.First(cd => !cd.IsVisible));

            if (firstHiddenBox == -1) 
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for teams per account is 9!", "Max teams completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Cards.RemoveAt(firstHiddenBox);
            Cards.Insert(firstHiddenBox, new CardDetails("", true, true, false) { IsInputVisible = true });
        }

        /// <summary>
        ///     Inserts a team into the DB
        /// </summary>
        private async void btnDone_Click(object sender, RoutedEventArgs e)
        {
            CardDetails newlyCreatedTeam = Cards.Last(cd => cd.IsVisible);

            string teamName = newlyCreatedTeam.InputName;
            await Task.Run(() => AddTeam(teamName));

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateTeam.Visibility = Visibility.Visible;

            int i = Cards.IndexOf(newlyCreatedTeam);
            Cards.RemoveAt(i);
            Cards.Insert(i, new CardDetails(teamName, true, false, false) { IsInputVisible = false });
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
            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateTeam.Visibility = Visibility.Visible;

            int unwantedTeam = Cards.IndexOf(Cards.Last(cd => cd.IsVisible));
            Cards.RemoveAt(unwantedTeam);
            Cards.Insert(unwantedTeam, new CardDetails(null, false, false, false) { IsInputVisible = false });
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


        /// <summary>
        ///     Adds team to database when user clicks 'Enter' key while
        ///     focus is on input TextBox
        /// </summary>
        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnDone_Click(sender, new RoutedEventArgs());
            }
        }

    }
}