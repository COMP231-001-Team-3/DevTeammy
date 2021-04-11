using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using teammy.ProjectDetail;

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=dev; pwd=rds8w77c0ehnw2fx; port=25060;";
        List<string> users = new List<string>();
        static List<TaskBox> objTask = new List<TaskBox>();
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        private teammyEntities dbContext = new teammyEntities();

        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueDateProperty = DependencyProperty.Register("TaskDueDate", typeof(DateTime), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeProperty = DependencyProperty.Register("Assignee", typeof(int), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("AssigneeList", typeof(List<string>), typeof(TaskBox));


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
        public List<string> TaskAssigneeList
        {
            get { return (List<string>)GetValue(TaskAssigneeListProperty); }
            set { SetValue(TaskAssigneeListProperty, value); }
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
            TaskAssigneeList = new List<string>();            
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

            //Get All Assigneded people
            string signStr = Application.Current.Resources["assigneeNum"] as string;
            if (signStr != null)
            {
                int signNum = Int32.Parse(signStr);
                

                MySqlCommand getAssingees = new MySqlCommand("SELECT user_name FROM users NATURAL JOIN team_mates Natural JOIN assignees Natural JOIN tasks where assigned_group = @assigned_group", conn);
                getAssingees.Parameters.AddWithValue("assigned_group", signNum);
                MySqlDataReader assingeeReader = getAssingees.ExecuteReader();
                using (assingeeReader)
                {
                    while (assingeeReader.Read())
                    {

                        TaskAssigneeList.Add(assingeeReader["user_name"].ToString());
                    }
                }

                if (TaskAssigneeList != null)
                {

                    foreach (var item in TaskAssigneeList)
                    {
                        createInitialBox(item.ToString());
                    }
                }
            }
            TaskPriority = Application.Current.Resources["priority"] as string;
            TaskProgress = Application.Current.Resources["status"] as string;
            loadPriorityPicture();
            loadStatusPicture();
            conn.Close();            
            objTask.Add(this); // Taskassigneelist = userNma
        }
        private List<assigneeInitialBox> assignees = new List<assigneeInitialBox>();

        private void loadStatusPicture()
        {
            if (TaskProgress == "IP")
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/progressIcon.jpg"));
                statusGrid.Background = imgBrush;
                 

            }
            else if (TaskProgress == "CO")
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/complete.png"));
                statusGrid.Background = imgBrush;
                
            }
            else
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/notStarted.png"));
                statusGrid.Background = imgBrush;
            }

        }
        
        private void loadPriorityPicture()
        { 
             
            if (TaskPriority == "High")
            {
                btnPriority.Background = Brushes.Red;
            }
            if (TaskPriority == "Medium")
            {
                btnPriority.Background = Brushes.Yellow;
            }
            if (TaskPriority == "Low")
            {
                btnPriority.Background = Brushes.Blue;
            }                
        }
        private void createInitialBox(string assigneeName)
        {
            int left = 0, top = 0, right = 0, bottom = 0;        
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
            assigneeInitialBox initialBox = new assigneeInitialBox(backColors[rd.Next(0, 18)]);
            initialBox.Margin = new Thickness(left, top, right, bottom);
            initialBox.txtInitial.Padding = new Thickness(0, 0, 0, 0);
            
            initialBox.txtInitial.Text = assigneeInitial;
            assigneeStackPanel.Children.Add(initialBox);  
        }
       

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
        }

       
        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmStatus") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //initiate the button color to default
            btnPriority.Background = Brushes.Transparent;

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
            //initiate the grid background image to default
            var imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\Desktop\COMP231\comp231-001_team3\teammy\teammy\images\notStarted.png"));
            statusGrid.Background = imgBrush;

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
        public event EventHandler<EventArgs> taskNameChanged;
       
        private void taskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void etItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedName = taskNameTextBox.Text;
            
            for (int i = 0; i < objTask.Count; i++) 
            {             

                if (selectedName == objTask[i].TaskName)
                {
                    EditTaskPage taskDetail = new EditTaskPage() 
                    { EditTaskName = objTask[i].TaskName, 
                      EditTaskDueDate = objTask[i].TaskDueDate,
                      EditTaskPriority = objTask[i].TaskPriority,
                      EditTaskAssignee = objTask[i].TaskAssigneeList
                    };
                    taskDetail.ShowDialog();
                }
            }
            
        }

        private void svItem_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void dlItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StackPanel taskPnlParent = this.Parent as StackPanel;
                taskPnlParent.Children.Remove(this);
            }
                      
        }

    }
}

