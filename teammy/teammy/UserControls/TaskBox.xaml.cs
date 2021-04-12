using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        static List<TaskBox> objTask = new List<TaskBox>();
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        private teammyEntities dbContext = new teammyEntities();

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(task), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("TaskAssigneeList", typeof(List<string>), typeof(TaskBox));

        public task Task
        {
            get { return (task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public List<string> TaskAssigneeList
        {
            get { return (List<string>)GetValue(TaskAssigneeListProperty); }
            set { SetValue(TaskAssigneeListProperty, value); }
        }


        //Events for change
        //public event EventHandler<EventArgs> TaskNameChanged;
        //public event EventHandler<EventArgs> TaskPriorityChanged;
        //public event EventHandler<EventArgs> TaskProgressChanged;
        //public event EventHandler<EventArgs> TaskDueDateChanged;
        //public event EventHandler<EventArgs> TaskAssigneeChanged;

        public ObservableCollection<string> TeamUsers { get; set; } = new ObservableCollection<string>();

        public TaskBox()
        {
            InitializeComponent();
            TaskAssigneeList = new List<string>();        
        }        
               
        private void LoadUsers()
        {
            List<string> teamMembers = (from mate in dbContext.team_mates
                                     where mate.Team_ID == Task.project.Team_ID
                                     select mate.user.user_name).ToList();

            teamMembers.ForEach(TeamUsers.Add);

            int? assigned_group = (from task in dbContext.tasks
                                 where task.task_name.Equals(Task.task_name) && task.assigned_group.HasValue
                                 select task.assigned_group).Single();

            if(assigned_group != null)
            {
                List<string> assignees = (from assignee in dbContext.assignees
                                       where assignee.assigned_group == assigned_group
                                       select assignee.team_mates.user.user_name).ToList();
                assignees.ForEach(TaskAssigneeList.Add);
                foreach (var item in TaskAssigneeList)
                {
                    CreateAssigneeBox(item.ToString());
                }
            }

            objTask.Add(this);
        }

        private void CreateAssigneeBox(string assigneeName)
        {
            Random rd = new Random();      
            
            //Creation & Initialization of InitialBox
            AssigneeEllipseTask epsAssignee = new AssigneeEllipseTask();
            epsAssignee.User = assigneeName;
            epsAssignee.BackColor = backColors[rd.Next(0, backColors.Length - 1)];
            assigneeStackPanel.Children.Add(epsAssignee);  
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
            string status = (sender as MenuItem).Header.ToString().ToUpper();
            string[] statusWords = status.Split(' ');
            if(statusWords.Length > 1)
            {
                Task.progress_code = statusWords[0][0] + "" + statusWords[1][0];
            }
            else
            {
                Task.progress_code = statusWords[0][0] + "" + statusWords[0][1];
            }
            string imagePath = null;
            switch (Task.progress_code)
            {
                case "NS":
                    imagePath = "pack://application:,,,/images/notstarted.png";
                    break;
                case "IP":
                    imagePath = "pack://application:,,,/images/progressIcon.jpg";
                    break;
                case "CO":
                    imagePath = "pack://application:,,,/images/complete.png";
                    break;
            }

            statusGrid.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(imagePath)) };
        }

        private void btnAssignee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void assigneeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox currentComboBox = sender as ComboBox;
            if (currentComboBox != null)
            {
                string currentItem = currentComboBox.SelectedItem.ToString();
                if (currentItem != null)
                {
                    CreateAssigneeBox(currentItem);
                }
            }
        }
       
        private void taskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void editItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedName = taskNameTextBox.Text;
            
            for (int i = 0; i < objTask.Count; i++) 
            {
                if (selectedName.Equals(objTask[i].Task.task_name))
                {
                    EditTaskPage taskDetail = new EditTaskPage() 
                    { 
                        EditTaskName = objTask[i].Task.task_name, 
                        EditTaskDueDate = objTask[i].Task.due_date.Value,
                        EditTaskPriority = objTask[i].Task.priority,
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
                StackPanel taskPnlParent = Parent as StackPanel;
                taskPnlParent.Children.Remove(this);
            }                      
        }

        private void taskBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }
    }
}

