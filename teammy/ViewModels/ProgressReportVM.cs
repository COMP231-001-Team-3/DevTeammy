using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using teammy.Models;
using System.Windows.Input;
using teammy.Commands;

namespace teammy.ViewModels
{
    public class ProgressReportVM : ViewModelBase
    {
        #region Fields
        //alias for Application resources
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private IMongoDatabase dbContext = DBConnector.Connect();
        #endregion

        #region Properties
        public User currentUser { get; set; } = globalItems["currentUser"] as User;
        public SeriesCollection ProjectsPie { get; set; } = new SeriesCollection()
        {
            new PieSeries
            {
                Title = "Not started",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "In Progress",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Completed",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            }
        };
        public SeriesCollection ProjectsMemPie { get; set; } = new SeriesCollection()
        {
            new PieSeries
            {
                Title = "Not started",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "In Progress",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Completed",
                Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                DataLabels = true
            }
        };

        public ColorsCollection PieColors { get; set; } = new ColorsCollection()
        {
            Colors.Red,
            Colors.RoyalBlue,
            Colors.MediumSeaGreen
        };

        public List<string> projNames { get; set; } = new List<string>();
        public List<string> memNames { get; set; } = new List<string>();

        public string txtCmbMembers { get; set; }
        public string txtMemberProject { get; set; }
        public string txtCmbProjects { get; set; }
        public ICommand selectionChangedCmd { get; set; }
        #endregion   
        
        public ProgressReportVM()
        {

            var projIDs =
            (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
            where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
            from project in team.Projects
            select project).ToList();

            projNames =
            (from proj in dbContext.GetCollection<Project>("projects").AsQueryable()
             where projIDs.Contains(proj.ProjectId)
             select proj.Name).ToList();

            selectionChangedCmd = new SelectionChangedCmd(this);

            LoadCharts();
        }

        /// <summary>
        ///     Initializes charts
        /// </summary>
        private void LoadCharts()
        {
            txtCmbProjects = projNames[0];
            txtMemberProject = projNames[0];

            SelectProject();
            SelectMemberProject();

            txtCmbMembers = memNames[1];

            SelectTeamMember();
        }

        /// <summary>
        ///     Reloads pie chart to reflect progress status of newly 
        ///     selected member in the selected project 
        /// </summary>
        public void SelectTeamMember()
        {
            //Loads progress status of all tasks associated with the project for the member selected
            int currProjectId =
                (from proj in dbContext.GetCollection<Project>("projects").AsQueryable()
                 where proj.Name.Equals(txtMemberProject)
                 select proj.ProjectId).Single();

            List<string> progress_codes = 
            (from task in dbContext.GetCollection<TaskToDo>("tasks").AsQueryable()
             where task.ProjectId == currProjectId && task.Assignees.Select(a => a.Username).Contains(txtCmbMembers)
             select task.Progress).ToList();

            //Resets Pie Chart values
            ProjectsMemPie[0].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("NS")).Count) };
            ProjectsMemPie[1].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("IP")).Count) };
            ProjectsMemPie[2].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("CO")).Count) };
        }

        /// <summary>
        ///     Reloads team member data for the project selected
        /// </summary>
        public void SelectMemberProject()
        {
            int currProjectId =
                (from proj in dbContext.GetCollection<Project>("projects").AsQueryable()
                 where proj.Name.Equals(txtMemberProject)
                 select proj.ProjectId).Single();

            memNames =
                (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
                 where team.Projects.Contains(currProjectId)
                 from member in team.Members
                 select member.Username).ToList();
            txtCmbMembers = memNames[1];
        }

        /// <summary>
        ///     Reloads the pie chart to reflect data of the newly selected 
        ///     project
        /// </summary>
        public void SelectProject()
        {
            //Loads progress status of all tasks associated with the project selected
            int currProjectId = 
                (from proj in dbContext.GetCollection<Project>("projects").AsQueryable()
                where proj.Name.Equals(txtCmbProjects)
                select proj.ProjectId).Single();

            List<string> progress_codes =
                (from t in dbContext.GetCollection<TaskToDo>("tasks").AsQueryable()
                 where t.ProjectId == currProjectId
                 select t.Progress).ToList();


            //Resets Pie chart values
            ProjectsPie[0].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("NS")).Count) };
            ProjectsPie[1].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("IP")).Count) };
            ProjectsPie[2].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("CO")).Count) };

        }

    }
}
