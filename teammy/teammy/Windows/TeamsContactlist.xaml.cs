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

namespace teammy.Windows
{
    /// <summary>
    /// Interaction logic for TeamsContactlist.xaml
    /// </summary>
    /// 
  

    public partial class TeamsContactlist : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public user currentUser { get; set; } = globalItems["currentUser"] as user;
        public team currentTeam { get; set; } = globalItems["currentTeam"] as team;
        string proname;

        List<user> contactinfo;
         teammyEntities dbContext = new teammyEntities();
        public TeamsContactlist()
        {
            InitializeComponent();
            showingcontact();
        }
        public void showingcontact()
        {
            
    
                contactinfo = (from user in dbContext.users
                               join mate in dbContext.team_mates
                               on user.user_id equals mate.user_id
                               join teams in dbContext.teams
                               on mate.Team_ID equals teams.Team_ID
                               where teams.Team_Name == proname 
                               select user).ToList();

 

            datagrid.ItemsSource = contactinfo;
        }
    }
}
