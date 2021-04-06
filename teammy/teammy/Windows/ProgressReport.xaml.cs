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
        private teammyEntities dbContext = new teammyEntities();
        private List<string> projNames;
        private List<string> memNames;
        #endregion

        #region Properties

        //currently logged in user
        public user currentUser { get; set; } = Application.Current.Resources["currentUser"] as user;

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
            projNames = (from project in dbContext.projects
                         join team in dbContext.teams
                            on project.Team_ID equals team.Team_ID
                         join mate in dbContext.team_mates
                            on team.Team_ID equals mate.Team_ID
                         join user in dbContext.users 
                            on mate.user_id equals user.user_id
                         where currentUser.user_id.Equals(user.user_id)
                         select project.Proj_Name).ToList();

            cmbProjects.ItemsSource = projNames;
            cmbMemProjects.ItemsSource = projNames;

            cmbProjects.SelectedIndex = 0;
            cmbMemProjects.SelectedIndex = 0;

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

        #region ComboBox event handlers
        /// <summary>
        ///     Reloads the pie chart to reflect data of the newly selected 
        ///     project
        /// </summary>
        private void cmbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Runs DB query in parallel to the UI Thread
            Parallel.Invoke(() =>
            {
                //Loads progress status of all tasks associated with the project selected
                List<string> progress_codes = 
                (from tasks in dbContext.tasks
                    join projects in dbContext.projects 
                        on tasks.proj_id equals projects.Proj_ID
                 where projects.Proj_Name.Equals(cmbProjects.SelectedItem.ToString())
                 select tasks.progress_code).ToList();

                //Resets Pie chart values
                ProjectsPie[0].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("NS")).Count) };
                ProjectsPie[1].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("IP")).Count) };
                ProjectsPie[2].Values = new ChartValues<ObservableValue> { new ObservableValue(progress_codes.FindAll(code => code.Equals("CO")).Count) };
            });
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
                List<string> progress_codes =
                (from task in dbContext.tasks
                 join project in dbContext.projects
                     on task.proj_id equals project.Proj_ID
                 join assignee in dbContext.assignees
                    on task.assigned_group equals assignee.assigned_group
                 join mate in dbContext.team_mates
                    on assignee.mate_id equals mate.mate_id
                 join user in dbContext.users
                    on mate.user_id equals user.user_id
                 where project.Proj_Name.Equals(cmbMemProjects.SelectedItem.ToString()) && user.user_name.Equals(cmbMembers.SelectedItem.ToString())
                 select task.progress_code).ToList();

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
                memNames = (from project in dbContext.projects
                            join team in dbContext.teams
                            on project.Team_ID equals team.Team_ID
                            join mate in dbContext.team_mates
                            on team.Team_ID equals mate.Team_ID
                            join user in dbContext.users
                            on mate.user_id equals user.user_id
                            where project.Proj_Name.Equals(cmbMemProjects.SelectedItem.ToString())
                            select user.user_name).ToList();

                Dispatcher.Invoke(() =>
                {
                    cmbMembers.ItemsSource = memNames;
                    cmbMembers.SelectedIndex = 0;
                });
            });
        }
        #endregion

        #region Page Navigation
        /// <summary>
        ///     Redirects to the home page
        /// </summary>
        private void homeMenuItem_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (Application.Current.Resources["mainInstance"] as Window).Show();
        }

        /// <summary>
        ///     Redirects to boards page
        /// </summary>
        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (Application.Current.Resources["createProjInstance"] as Window).Show();
        }

        /// <summary>
        ///     Redirects to teams list page
        /// </summary>
        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["teamsListInstance"] as Window).Show();
        }
        #endregion

        private void schedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["scheduleInstance"] as Window).Show();
        }
    }
}