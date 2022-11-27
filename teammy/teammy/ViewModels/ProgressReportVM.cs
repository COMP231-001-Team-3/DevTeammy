using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using teammy.Models;
using MongoDB.Bson;
using System.Windows.Controls;
using System.Windows.Threading;
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
            //Query to load all projects of the logged in person
            PipelineDefinition<Team, BsonDocument> pipeline = new[]
            {
                new BsonDocument("$match",
                new BsonDocument("members.userId", currentUser.UserId)),
                new BsonDocument("$unwind",
                new BsonDocument("path", "$projects")),
                new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "projects" },
                        { "localField", "projects" },
                        { "foreignField", "projectId" },
                        { "as", "proDetails" }
                    }),
                new BsonDocument("$unwind",
                new BsonDocument("path", "$proDetails")),
                new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                        { "projectName", "$proDetails.name" }
                    })
            };
            //List<int> projIDs =
            //    (List<int>)
            //    (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
            //    where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
            //    select team.Projects);

            projNames.Clear();
            projNames.AddRange(dbContext.GetCollection<Team>("teams")
                                        .Aggregate(pipeline)
                                        .ToEnumerable()
                                        .Select(t => t.GetValue("projectName").AsString)
                                        .ToList());
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
            int currProjectId = dbContext.GetCollection<Project>("projects")
                                                .Find(p => p.Name.Equals(txtMemberProject))
                                                .Project(p => p.ProjectId)
                                                .Single();
            PipelineDefinition<TaskToDo, BsonDocument> pipeline = new[]
            {
                new BsonDocument("$match",
                new BsonDocument("projectId", currProjectId)),
                new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                        { "assignees",
                new BsonDocument("$map",
                new BsonDocument
                            {
                                { "input", "$assignees" },
                                { "as", "a" },
                                { "in", "$$a.username" }
                            }) },
                        { "progress", 1 }
                    }),
                new BsonDocument("$match",
                new BsonDocument("assignees",
                new BsonDocument("$in",
                new BsonArray
                            {
                                txtCmbMembers
                            })))
            };
            List<string> progress_codes = dbContext.GetCollection<TaskToDo>("tasks")
                                                        .Aggregate(pipeline)
                                                        .ToEnumerable()
                                                        .Select(r => r.GetValue("progress").AsString)
                                                        .ToList();

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
            int currProjectId = dbContext.GetCollection<Project>("projects")
                                                .Find(p => p.Name.Equals(txtMemberProject))
                                                .Project(p => p.ProjectId)
                                                .Single();
            memNames.Clear();
            memNames.AddRange(dbContext.GetCollection<Team>("teams")
                                    .Find(t => t.Projects.Contains(currProjectId))
                                    .Project(t => t.Members)
                                    .Single()
                                    .Select(m => m.Username)
                                    .ToList());
            txtCmbMembers = memNames[1];
        }

        /// <summary>
        ///     Reloads the pie chart to reflect data of the newly selected 
        ///     project
        /// </summary>
        public void SelectProject()
        {
            //Loads progress status of all tasks associated with the project selected
            int currProjectId = dbContext.GetCollection<Project>("projects")
                                              .Find(p => p.Name.Equals(txtCmbProjects))
                                              .Project(p => p.ProjectId)
                                              .Single();
            List<string> progress_codes = dbContext.GetCollection<TaskToDo>("tasks")
                                                      .Find(t => t.ProjectId == currProjectId)
                                                      .Project(t => t.Progress)
                                                      .ToList();

            //Resets Pie chart values
            ProjectsPie[0].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("NS")).Count) };
            ProjectsPie[1].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("IP")).Count) };
            ProjectsPie[2].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("CO")).Count) };

        }

    }
}
