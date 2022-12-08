using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using teammy.Models;
using MongoDB.Driver;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        private IMongoDatabase dbContext = DBConnector.Connect();

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(TaskToDo), typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueProperty = DependencyProperty.Register("TaskDue", typeof(DateTime?), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("TaskAssigneeList", typeof(ObservableCollection<AssigneeEllipseTask>), typeof(TaskBox));
        public int LoadCounter = 0;

        public TaskToDo ToDoTask
        {
            get { return (TaskToDo)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public string TaskName
        {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }

        public DateTime? TaskDue
        {
            get { return (DateTime?)GetValue(TaskDueProperty); }
            set { SetValue(TaskDueProperty, value); }
        }

        public string TaskProgress
        {
            get { return (string)GetValue(TaskProgressProperty); }
            set { SetValue(TaskProgressProperty, value); }
        }

        public string TaskPriority
        {
            get { return (string)GetValue(TaskPriorityProperty); }
            set { SetValue(TaskPriorityProperty, value); }
        }

        public ObservableCollection<AssigneeEllipseTask> TaskAssigneeList
        {
            get { return (ObservableCollection<AssigneeEllipseTask>)GetValue(TaskAssigneeListProperty); }
            set { SetValue(TaskAssigneeListProperty, value); }
        }

        public ObservableCollection<MenuItem> TeamUsers { get; set; } = new ObservableCollection<MenuItem>();

        public static User currentUser { get; set; } = globalItems["currentUser"] as User;
        public DateTime DisplayDateStart { get; set; } = currentUser.Privilege.Equals("PM") ? DateTime.Now : DateTime.MaxValue;

        public TaskBox()
        {
            InitializeComponent();
            TaskAssigneeList = new ObservableCollection<AssigneeEllipseTask>();
        }

        public void LoadUsers()
        {
            List<string> teamMembers = dbContext.GetCollection<Team>("teams")
                                                .Find(t => t.TeamId == ToDoTask.TeamId)
                                                .Project(t => t.Members)
                                                .Single()
                                                .Select(m => m.Username)
                                                .ToList();

            teamMembers.ForEach(member => TeamUsers.Add(new MenuItem() { Header = member, HorizontalContentAlignment = HorizontalAlignment.Left, VerticalContentAlignment = VerticalAlignment.Center }));

            List<string> assignees = ToDoTask.Assignees != null ? ToDoTask.Assignees
                                        .Select(a => a.Username)
                                        .ToList()
                                        : new List<string>();

            foreach (var item in assignees)
            {
                CreateAssigneeBox(item);
            }     
        }

        private void CreateAssigneeBox(string assigneeName)
        {
            Random rd = new Random();

            //Creation & Initialization of InitialBox
            AssigneeEllipseTask epsAssignee = new AssigneeEllipseTask
            {
                User = assigneeName,
                BackColor = backColors[rd.Next(0, backColors.Length - 1)]
            };

            TaskAssigneeList.Add(epsAssignee);
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

        private async void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //initiate the button color to default
            btnPriority.Background = Brushes.Transparent;

            string priorName = (sender as MenuItem).Name.Replace("Item", "");
            string formatted = (priorName[0] + "").ToUpper() + priorName.Substring(1).ToLower();

            TaskPriority = formatted;
            await dbContext.GetCollection<TaskToDo>("tasks")
                        .UpdateOneAsync(t => t.TaskId == ToDoTask.TaskId, Builders<TaskToDo>.Update.Set(t => t.Priority, TaskPriority));
            //dbContext.tasks.Find(ToDoTask.TaskId).priority = TaskPriority;
            //await dbContext.SaveChangesAsync();
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

        private async void StatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string status = (sender as MenuItem).Header.ToString().ToUpper();
            string[] statusWords = status.Split(' ');
            if(statusWords.Length > 1)
            {
                ToDoTask.Progress = statusWords[0][0] + "" + statusWords[1][0];
            }
            else
            {
                ToDoTask.Progress = statusWords[0][0] + "" + statusWords[0][1];
            }
            TaskProgress = ToDoTask.Progress;

            await dbContext.GetCollection<TaskToDo>("tasks")
                        .UpdateOneAsync(t => t.TaskId == ToDoTask.TaskId, Builders<TaskToDo>.Update.Set(t => t.Progress, TaskProgress));
            //dbContext.tasks.Find(ToDoTask.task_id).progress_code = TaskProgress;
            //await dbContext.SaveChangesAsync();
        }

        private async void taskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(ToDoTask != null)
            {
                await dbContext.GetCollection<TaskToDo>("tasks")
                        .UpdateOneAsync(t => t.TaskId == ToDoTask.TaskId, Builders<TaskToDo>.Update.Set(t => t.Title, taskNameTextBox.Text));
                //dbContext.tasks.Find(ToDoTask.task_id).task_name = taskNameTextBox.Text;
                //await dbContext.SaveChangesAsync();
            }            
        }

        private void editItem_Click(object sender, RoutedEventArgs e)
        {
            EditTaskPage taskDetail = new EditTaskPage() 
            {
                TaskToBeEdited = ToDoTask,
                TaskName = TaskName,
                TaskDue = TaskDue,
                EditTaskPriority = TaskPriority,
                TeamUsers = TeamUsers
            };

            TaskAssigneeList.ToList().ForEach(eps =>
            {
                taskDetail.EditTaskAssignees.Add(new AssigneeEllipse()
                {
                    BackColor = eps.BackColor,
                    User = eps.User
                });
            });

            taskDetail.ShowDialog();            
        }

        private async void dlItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Category currentCat = Application.Current.Windows
                                             .OfType<Board>()
                                             .SingleOrDefault(window =>
                                                                window.projectId == ToDoTask.ProjectId)
                                             .Categories.ToList()
                                             .Find(cat => cat.CategoryName.Equals(ToDoTask.Category));

                currentCat.Tasks.Remove(this);
                await dbContext.GetCollection<TaskToDo>("tasks")
                                  .DeleteOneAsync(t => t.TaskId == ToDoTask.TaskId);
                //dbContext.tasks.Remove(dbContext.tasks.Find(ToDoTask.task_id));                
                //await dbContext.SaveChangesAsync();
            }                      
        }

        private void btnAddAssignee_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmAssignees") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        public void AssigneeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string username = (sender as MenuItem).Header.ToString();
            if (TaskAssigneeList.Count < 4)
            {
                List<string> currAssignees = ToDoTask.Assignees != null ? ToDoTask.Assignees
                                                        .Select(a => a.Username)
                                                        .ToList()
                                                        : new List<string>();
                User newAssignee = dbContext.GetCollection<User>("users")
                                                        .Find(u => u.Username.Equals(username))
                                                        .Single();
                CreateAssigneeBox(username);
                long teamID = ToDoTask.TeamId;
                AddAssignee(newAssignee);
            }
            else
            {
                MessageBox.Show("The Maximum number of assignees per task is 4.", "Max Assignees Exceeded!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void AddAssignee(User assignee)
        {
            await dbContext.GetCollection<TaskToDo>("tasks")
                           .UpdateOneAsync(t => t.TaskId == ToDoTask.TaskId,
                                        Builders<TaskToDo>.Update.AddToSet(t => t.Assignees, assignee));
        }

        private void AssigneeMenu_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmAssignees") as ContextMenu;

            if (LoadCounter == 0 && cm.Items.Count != 0)
            {
                foreach (MenuItem menuItem in cm.Items)
                {
                    menuItem.Click += new RoutedEventHandler(AssigneeMenuItem_Click);
                }
                LoadCounter++;
            }
        }

        private async void taskDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ToDoTask != null)
            {
                await dbContext.GetCollection<TaskToDo>("tasks")
                                  .UpdateOneAsync(t => t.TaskId == ToDoTask.TaskId,
                                            Builders<TaskToDo>.Update.Set(t => t.DueDate, taskDate.SelectedDate));
                //dbContext.tasks.Find(ToDoTask.TaskId).due_date = taskDate.SelectedDate;
                //await dbContext.SaveChangesAsync();
            }            
        }
    }
}

