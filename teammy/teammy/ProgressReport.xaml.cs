using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Charts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Threading.Tasks;

namespace teammy
{
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class ProgressReport : Window
    {

        #region Fields
        //Hosted DB connection string
        private static string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        MySqlConnection conn = new MySqlConnection(connectionString);
        #endregion

        //public UserModel currentUser { get; set; } = Application.Current.Resources["currentUser"] as UserModel;
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

        public ColorsCollection PieColors { get; set; } = new ColorsCollection()
        {
            Colors.Red,
            Colors.RoyalBlue,
            Colors.MediumSeaGreen
        };

        #region Constructor
        public ProgressReport()
        {
            InitializeComponent();
            Parallel.Invoke(() =>
            {
                conn.Open();
                try
                {
                    MySqlCommand getProjects = new MySqlCommand("SELECT Proj_Name FROM projects;", conn);


                    MySqlDataReader reader = getProjects.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Dispatcher.Invoke(() => cmbProjects.Items.Add(reader[0]));
                        }
                    }
                }
                catch (MySqlException) { }
                conn.Close();

                cmbProjects.SelectedIndex = 0;
            });
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

        private void homeMenu_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (Application.Current.Resources["mainInstance"] as Window).Show();
        }

        private void cmbTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parallel.Invoke(() => 
            {
                int nsCount = 0, ipCount = 0, coCount = 0;

                conn.Open();
                try
                {
                    MySqlCommand getTasks = new MySqlCommand("SELECT progress_code FROM tasks NATURAL JOIN projects WHERE Proj_Name = @projectName;", conn);
                    getTasks.Parameters.AddWithValue("projectName", cmbProjects.SelectedItem.ToString());
                    MySqlDataReader reader = getTasks.ExecuteReader();

                    using (reader)
                    {
                        string progress;
                        while (reader.Read())
                        {
                            progress = reader[0].ToString();
                            if (progress.Equals("NS"))
                            {
                                nsCount++;
                            }
                            else if (progress.Equals("IP"))
                            {
                                ipCount++;
                            }
                            else
                            {
                                coCount++;
                            }
                        }
                    }
                }
                catch (MySqlException m) 
                {
                    MessageBox.Show(m.Message, "MySqlException Thrown", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                ProjectsPie[0].Values = new ChartValues<ObservableValue> { new ObservableValue(nsCount) };
                ProjectsPie[1].Values = new ChartValues<ObservableValue> { new ObservableValue(ipCount) };
                ProjectsPie[2].Values = new ChartValues<ObservableValue> { new ObservableValue(coCount) };

                conn.Close();
            });
        }
    }
}