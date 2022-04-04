using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Threading.Tasks;
using teammy.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace teammy
{
    /// <summary>
    /// Interaction logic for ProgressReport.xaml
    /// </summary>
    public partial class ProgressReport : Window
    {
        #region Fields
        //alias for Application resources
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private IMongoDatabase dbContext = DBConnector.Connect();
        private List<string> projNames;
        private List<string> memNames;
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
        #endregion       

        #region Constructor
        public ProgressReport()
        {
            InitializeComponent();

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
            projNames = dbContext.GetCollection<Team>("teams")
                                    .Aggregate(pipeline)
                                    .ToEnumerable()
                                    .Select(t => t.GetValue("projectName").AsString)
                                    .ToList();

            cmbProjects.ItemsSource = projNames;
            cmbMemProjects.ItemsSource = projNames;

            cmbProjects.SelectedIndex = 0;
            cmbMemProjects.SelectedIndex = 1;

            cmbMemProjects.SelectionChanged += new SelectionChangedEventHandler(cmbMem_SelectionChanged);
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

        #region ComboBox event handlers
        /// <summary>
        ///     Reloads the pie chart to reflect data of the newly selected 
        ///     project
        /// </summary>
        private void cmbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                //Loads progress status of all tasks associated with the project selected
                int currProjectId = dbContext.GetCollection<Project>("projects")
                                                  .Find(p => p.Name.Equals(cmbProjects.SelectedItem.ToString()))
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

        /// <summary>
        ///     Reloads pie chart to reflect progress status of newly 
        ///     selected member in the selected project 
        /// </summary>
        private void cmbMem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Runs DB query in parallel to the UI Thread
            Parallel.Invoke(() =>
            {
                //Loads progress status of all tasks associated with the project for the member selected
                int currProjectId = dbContext.GetCollection<Project>("projects")
                                                  .Find(p => p.Name.Equals(cmbMemProjects.SelectedItem.ToString()))
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
                                    cmbMembers.SelectedItem.ToString()
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
            });
        }

        /// <summary>
        ///     Reloads team member data for the project selected
        /// </summary>
        private void cmbMemProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Runs DB query in parallel to the UI Thread
            Parallel.Invoke(() =>
            {
                int currProjectId = dbContext.GetCollection<Project>("projects")
                                                  .Find(p => p.Name.Equals(cmbMemProjects.SelectedItem.ToString()))
                                                  .Project(p => p.ProjectId)
                                                  .Single();
                memNames = dbContext.GetCollection<Team>("teams")
                                       .Find(t => t.Projects.Contains(currProjectId))
                                       .Project(t => t.Members)
                                       .Single()
                                       .Select(m => m.Username)
                                       .ToList();

                Dispatcher.Invoke(() =>
                {
                    cmbMembers.ItemsSource = memNames;
                    cmbMembers.SelectedIndex = 0;
                });
            });
        }
        #endregion
    }
}