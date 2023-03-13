using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using teammy.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using teammy.UserControls;
using teammy.Commands;

namespace teammy.ViewModels
{
    public class TeamsVM : ViewModelBase
    {

        #region Fields
        private static ResourceDictionary globalItems = Application.Current.Resources;

        public IMongoDatabase dbContext = DBConnector.Connect();
        public List<Team> teams;
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

        private bool _btnDoneVisible;
        public bool BtnDoneVisible 
        {
            get
            {
                return _btnDoneVisible;
            }
            set
            {
                _btnDoneVisible = value;
                OnPropertyChanged(nameof(BtnDoneVisible));
            }
        }

        private bool _btnCancelVisible;
        public bool BtnCancelVisible
        {
            get
            {
                return _btnCancelVisible;
            }
            set
            {
                _btnCancelVisible = value;
                OnPropertyChanged(nameof(BtnCancelVisible));
            }
        }

        private bool _btnCreateTeamVisible;
        public bool BtnCreateTeamVisible
        {
            get
            {
                return _btnCreateTeamVisible;
            }
            set
            {
                _btnCreateTeamVisible = value;
                OnPropertyChanged(nameof(BtnCreateTeamVisible));
            }
        }


        #endregion

        public ICommand btnClickCmd { get; set; }
        public ICommand teamClickCmd { get; set; }
        public ICommand teamKeyUpCmd { get; set; }
        public TeamsVM()
        {
            btnClickCmd = new BtnClickCmd(null, this);
            teamClickCmd = new CardClickCmd(this);
            teamKeyUpCmd = new CardKeyUpCmd(this);

            BtnCreateTeamVisible = currentUser.Privilege.Equals("PM");          
            LoadTeams();
        }

        #region Miscellaneous

        /// <summary>
        ///     Loads teams from the database
        /// </summary>
        public void LoadTeams()
        {
            teams =
                (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
                 where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
                 select team).ToList();

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

        #endregion

        /// <summary>
        ///     Resets app to the state before clicking the create team 
        ///     button
        /// </summary>
        public void btnCancel_Click()
        {
            BtnDoneVisible = false;
            BtnCancelVisible = false;
            BtnCreateTeamVisible = true;

            int unwantedTeam = Cards.IndexOf(Cards.Last(cd => cd.IsVisible));
            Cards.RemoveAt(unwantedTeam);
            Cards.Insert(unwantedTeam, new CardDetails(null, false, false, false) { IsInputVisible = false });
        }

        /// <summary>
        ///     Creates a team box and a textbox for team name input
        /// </summary>
        public void btnCreateTeam_Click()
        {
            int firstHiddenBox = Cards.IndexOf(Cards.First(cd => !cd.IsVisible));

            if (firstHiddenBox == 9)
            {
                MessageBox.Show("The maximum limit for teams per account is 9!", "Max teams completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BtnCreateTeamVisible = false;
            BtnDoneVisible = true;
            BtnCancelVisible = true;
            

            Cards.RemoveAt(firstHiddenBox);
            Cards.Insert(firstHiddenBox, new CardDetails("", true, true, false) { IsInputVisible = true });
        }

        /// <summary>
        ///     Inserts a team into the DB
        /// </summary>
        public async void btnDone_Click()
        {
            CardDetails newlyCreatedTeam = Cards.Last(cd => cd.IsVisible);

            string teamName = newlyCreatedTeam.InputName;
            await Task.Run(() => {
                AddTeam(teamName);
                teams =
                (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
                 where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
                 select team).ToList();
            });

            BtnCreateTeamVisible = true;
            BtnDoneVisible = false;
            BtnCancelVisible = false;

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
        ///     Adds team to database when user clicks 'Enter' key while
        ///     focus is on input TextBox
        /// </summary>
        public void teamBox_KeyUp(EventArgs sent)
        {
            KeyEventArgs e = sent as KeyEventArgs;
            if (e.Key == Key.Enter)
            {
                btnDone_Click();
            }
        }

        public void teamBox_CardClick(CardBox sender)
        {
            try
            {
                TeamDetails contactPage = new TeamDetails()
                {
                    currentTeam =
                    (from team in teams
                     where team.TeamName.Equals(sender.Details.FullName)
                     select team)
                    .Single()
                };
                contactPage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The team {sender.Details.FullName} does not have any members as of now. Check back later! 😊", "No team members");
            }


        }
    }
}
