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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        List<string> users = new List<string>();

        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueDateProperty = DependencyProperty.Register("TaskDueDate", typeof(DateTime), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeProperty = DependencyProperty.Register("Assignee", typeof(int), typeof(TaskBox));

        

        public string TaskName
        {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }
        public string TaskPriority
        {
            get { return (string)GetValue(TaskPriorityProperty); }
            set { SetValue(TaskPriorityProperty, value); }
            
        }
        public string TaskProgress
        {
            get { return (string)GetValue(TaskProgressProperty); }
            set { SetValue(TaskProgressProperty, value); }
        }
        public DateTime TaskDueDate
        {
            get { return (DateTime)GetValue(TaskDueDateProperty); }
            set { SetValue(TaskDueDateProperty, value); }
        }

        public int TaskAssignee
        {
            get { return (int)GetValue(TaskAssigneeProperty); }
            set { SetValue(TaskAssigneeProperty, value); }
        }


        //Events for change on the text properties
        public event EventHandler<EventArgs> TaskNameChanged;
        public event EventHandler<EventArgs> TaskPriorityChanged;
        public event EventHandler<EventArgs> TaskProgressChanged;
        public event EventHandler<EventArgs> TaskDueDateChanged;
        public event EventHandler<EventArgs> TaskAssigneeChanged;

        public TaskBox()
        {

            InitializeComponent();
            LoadUsers();
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
            //UserListClass listclass = new UserListClass{TeamMembers=users};

        }
        private void createInitialBox(string assigneeName)
        {
            int left = 0, top = 0, right = 361, bottom = 260;
            int initialBoxCount = 0;
            //totalBoxes=0;
            taskGrid.Children.Clear();
            Random rd = new Random();
            string  assigneeInitial;
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
            assigneeInitialBox initialBox = new assigneeInitialBox();
            initialBox.txtInitial.Text = assigneeInitial;

            taskGrid.Children.Add(initialBox);

            left += 175;
            right -= 175;
            initialBoxCount++;
            
        }
        //MySqlConnection conn = new MySqlConnection(connectionString);
        //conn.Open();
        //MySqlCommand getAssignees = new MySqlCommand("SELECT user_name FROM users NATURAL JOIN teams NATURAL JOIN projects NATURAL JOIN tasks WHERE Task_Name = @nameTeam", conn);




        /*assisgneeName = */

    



        private void btnEditDelete_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmThreeDots") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
            
        }
        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            //e.Handled = true;
            //TaskPriorityChanged?.Invoke(this, EventArgs.Empty);
        }

       
        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmStatus") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            //e.Handled = true;
            //TaskProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Brush chosenPriority = ((sender as MenuItem).Icon as Rectangle).Fill;
            priorityGrid.Background = chosenPriority;
        }

        private void StatusMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        private void StatusMenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = null;
        }

        private void StatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            ImageSource iconSource = (MenuItem.Children[0] as Image).Source;
            statusGrid.Background = new ImageBrush(iconSource);
        }

        private void btnAssignee_Click(object sender, RoutedEventArgs e)
        {

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
                string currentItem = (currentComboBox.SelectedItem as UserListClass).TeamMembers;

                if (currentItem != null)
                {
                   createInitialBox(currentItem);
                }
            }
        }
    }
}

