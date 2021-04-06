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
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        

        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueDateProperty = DependencyProperty.Register("TaskDueDate", typeof(DateTime), typeof(TaskBox));
        //public static readonly DependencyProperty TaskAssigneeProperty = DependencyProperty.Register("Assignee", typeof(int), typeof(TaskBox));
        //public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("AssigneeList", typeof(List<string>), typeof(TaskBox));


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

        //public int TaskAssignee
        //{
        //    get { return (int)GetValue(TaskAssigneeProperty); }
        //    set { SetValue(TaskAssigneeProperty, value); }
        //}

        public List<string> TaskAssigneeList = new List<string>();
        public TaskBox(List<string> assigneesL)
        {
            TaskAssigneeList = assigneesL;
        }


        //Events for change on the text properties
        public event EventHandler<EventArgs> TaskNameChanged;
        public event EventHandler<EventArgs> TaskPriorityChanged;
        public event EventHandler<EventArgs> TaskProgressChanged;
        public event EventHandler<EventArgs> TaskDueDateChanged;
        //public event EventHandler<EventArgs> TaskAssigneeChanged;
        

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

            //Get All Assigneded people
            string signStr = Application.Current.Resources["assigneeNum"] as string;
            int signNum = Int32.Parse(signStr);
            List<string> userName = new List<string>();

            MySqlCommand getAssingees = new MySqlCommand("SELECT user_name FROM users NATURAL JOIN team_mates Natural JOIN assignees Natural JOIN tasks where assigned_group = @assigned_group", conn);
            getAssingees.Parameters.AddWithValue("assigned_group", signNum);
            MySqlDataReader assingeeReader = getAssingees.ExecuteReader();
            using (assingeeReader)
            {
                while (assingeeReader.Read())
                {

                    userName.Add((string)assingeeReader["user_name"]);
                }
            }

            if (userName != null)
            {

                foreach (var item in userName)
                {
                    createInitialBox(item.ToString());
                }
            }
            loadPriorityPicture();
            loadStatusPicture();



        }
        private List<assigneeInitialBox> assignees = new List<assigneeInitialBox>();

        private void loadStatusPicture()
        {

            if (Application.Current.Resources["status"] as string == "NS")
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\Desktop\COMP231\comp231-001_team3\teammy\teammy\images\notStarted.png"));
                statusGrid.Background=imgBrush;


            }

            if (Application.Current.Resources["status"] as string == "IP")
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\Desktop\COMP231\comp231-001_team3\teammy\teammy\images\progressIcon.jpg"));
                statusGrid.Background = imgBrush;
                //statusGrid.Background.SetValue(ImageBrush.ImageSourceProperty, new BitmapImage() { UriSource = new Uri(@"../images/progressIcon.jpg") });



            }
            if (Application.Current.Resources["status"] as string == "CO")
            {
                var imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"C:\Users\user\Desktop\COMP231\comp231-001_team3\teammy\teammy\images\complete.png"));
                statusGrid.Background = imgBrush;
                //statusGrid.Background.SetValue(ImageBrush.ImageSourceProperty, new BitmapImage() { UriSource = new Uri(@"../images/complete.png") });


            }


        }
        
        private void loadPriorityPicture()
        { 
             
            if (Application.Current.Resources["priority"] as string== "High")
                    {
                btnPriority.Background = Brushes.Red;
                        

                    }

                    if (Application.Current.Resources["priority"] as string == "Medium")
                    {
                btnPriority.Background = Brushes.Yellow;



            }
                    if (Application.Current.Resources["priority"] as string == "Low")
                    {
                btnPriority.Background = Brushes.Blue;


            }

                
        }
        private void createInitialBox(string assigneeName)
        {
            int left = 0, top = 0, right = 0, bottom = 0;
            int initialBoxCount = 0;
            int totalBoxes=0;           
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

            if (++totalBoxes == 3)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for assignees per a task is 3!", "Max Assigning completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            assigneeStackPanel.Children.Add(initialBox);


            left += 25;
            right -= 25;
            initialBoxCount++;
            
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
        public event EventHandler<EventArgs> taskNameChanged;
        public NewCategory currentCategory { get; set; } = Application.Current.Resources["currentCategory"] as NewCategory;
        public TaskBox currentTask { get; set; } = Application.Current.Resources["currentTask"] as TaskBox;
        private void taskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            //e.Handled = true;
            //taskNameChanged?.Invoke(this, EventArgs.Empty);


            //MySqlConnection conn = new MySqlConnection(connectionString);
            //conn.Open();
            //MySqlCommand getTaskName = new MySqlCommand("select task_name from tasks", conn);
            //// getTaskName.Parameters.AddWithValue("tskName", currentTask.TaskName);
            //MySqlDataReader reader = getTaskName.ExecuteReader();
            //reader.Read();
            //string taskNameInDatabase = reader[0].ToString();
            //reader.Close();

            //if (taskNameInDatabase == null)                   //if task name of current taskbox is null in database
            //{
            //    MySqlCommand insertTaskName = new MySqlCommand("Insert INTO tasks(task_name) VALUES(@tskName) NATURAL JOIN categories Where category_name = @catName;", conn);
            //    insertTaskName.Parameters.AddWithValue("catName", Application.Current.Resources["catName"] as string);
            //    insertTaskName.Parameters.AddWithValue("tskName", taskNameTextBox.Text);

            //    MySqlCommand commit = new MySqlCommand("COMMIT;", conn);
            //    insertTaskName.ExecuteNonQuery();                                   //ERROR OCCURED HERE
            //    commit.ExecuteNonQuery();
            //}
            //else                //if the task name of the current taskbox already exists in database
            //{

            //    MySqlCommand updateTaskName = new MySqlCommand("Update tasks set task_name = @tskName NATURAL JOIN categories Where category_name = @catName;", conn);
            //    updateTaskName.Parameters.AddWithValue("catName", Application.Current.Resources["catName"] as string);
            //    updateTaskName.Parameters.AddWithValue("tskName", taskNameTextBox.Text);
            //    MySqlCommand commit = new MySqlCommand("COMMIT;", conn);
            //    updateTaskName.ExecuteNonQuery();                                   //ERROR OCCURED HERE
            //    commit.ExecuteNonQuery();


            //}
        }

        private void etItem_Click(object sender, RoutedEventArgs e)
        {
            EditTaskPage editTaskPage = new EditTaskPage();
            editTaskPage.Show();
        }

        private void svItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

