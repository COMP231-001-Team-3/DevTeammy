using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for EditTaskPage.xaml
    /// </summary>
    public partial class EditTaskPage : Window
    {
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=dev; pwd=rds8w77c0ehnw2fx; port=25060;";
        List<string> users = new List<string>();
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        public static readonly DependencyProperty EditTaskNameProperty = DependencyProperty.Register("EditTaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty EditTaskPriorityProperty = DependencyProperty.Register("EditTaskPriority", typeof(string), typeof(TaskBox));
       
        public static readonly DependencyProperty EditTaskDueDateProperty = DependencyProperty.Register("EditTaskDueDate", typeof(DateTime), typeof(TaskBox));
        //public static readonly DependencyProperty EditTaskAssigneeProperty = DependencyProperty.Register("Assignee", typeof(int), typeof(TaskBox));

        public string EditTaskName
        {
            get { return (string)GetValue(EditTaskNameProperty); }
            set { SetValue(EditTaskNameProperty, value); }
        }
        public string EditTaskPriority
        {
            get { return (string)GetValue(EditTaskPriorityProperty); }
            set { SetValue(EditTaskPriorityProperty, value); }

        }

        public DateTime EditTaskDueDate
        {
            get { return (DateTime)GetValue(EditTaskDueDateProperty); }
            set { SetValue(EditTaskDueDateProperty, value); }
        }

        public List<string> EditTaskAssignee { get; set; }


        public EditTaskPage()
        {
            InitializeComponent();
            
        }

        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            //e.Handled = true;
            //TaskPriorityChanged?.Invoke(this, EventArgs.Empty);
        }
        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string chosenPriority = (sender as MenuItem).Header.ToString();
            btnPriority.Content = chosenPriority;
            Brush chosenPriorityColour = ((sender as MenuItem).Icon as Rectangle).Fill;
            priorityGrid.Background = chosenPriorityColour;
        }
        public ObservableCollection<UserListClass> TeamUsers { get; set; }
        public class UserListClass
        {
            public string TeamMembers { get; set; }
        }
        private void LoadUsers()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MySqlCommand getTUsers = new MySqlCommand("SELECT user_name  FROM users", conn);
            //getTUsers.Parameters.AddWithValue("nameTeam", "DevTeam");
            MySqlDataReader reader = getTUsers.ExecuteReader();
            this.DataContext = this;
            TeamUsers = new ObservableCollection<UserListClass>();
            using (reader)
            {
                while (reader.Read())
                {
                    users.Add(reader[0].ToString());

                }
                for (int i = 0; i < users.Count; i++)
                {
                    TeamUsers.Add(new UserListClass
                    {

                        TeamMembers = users[i]

                    });
                }
            }
            if (EditTaskAssignee != null)
            { 
             
            }
        }
        

        private void createInitialBox(string assigneeName)
        {
            int left = 0, top = 0, right = 0, bottom = 0;
            int initialBoxCount = 0;
            int totalBoxes = 0;
            //assigneeStackPanel.Children.Clear();
            Random rd = new Random();
            string assigneeInitial;
            string[] nameWords;

            nameWords = assigneeName.Split(' ');

            if (nameWords.Length >= 2)
            {
                assigneeInitial = nameWords[0][0] + "" + nameWords[1][0];
            }
            else
            {
                assigneeInitial = nameWords[0][0] + "" + nameWords[0][1];
            }
            //Creation & Initialization of InitialBox
            assigneeInitialBox initialBox = new assigneeInitialBox(backColors[rd.Next(0, 18)]);
            initialBox.Margin = new Thickness(left, top, right, bottom);
            initialBox.txtInitial.Padding = new Thickness(0, 0, 0, 0);

            initialBox.txtInitial.Text = assigneeInitial;

            if (++totalBoxes == 3)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for assignees per a task is 3!", "Max Assigning completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            assigneeStackPanel.Children.Add(initialBox);

        }
        private void assigneeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox currentComboBox = sender as ComboBox;
            if (currentComboBox != null)
            {
                string currentItem = (currentComboBox.SelectedItem as UserListClass).TeamMembers;
                if (currentItem != null)
                {
                    //MessageBox.Show(currentItem.Content.ToString());
                    createInitialBox(currentItem);
                }
            }
        }
        private void assigneeCombo_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox currentComboBox = sender as ComboBox;
            if (currentComboBox != null)
            {
                if(currentComboBox.SelectedIndex >= 0)
                {
                    string currentItem = (currentComboBox.SelectedItem as UserListClass).TeamMembers;

                    if (currentItem != null)
                    {
                        createInitialBox(currentItem);
                    }
                }
                
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }
    }
}
