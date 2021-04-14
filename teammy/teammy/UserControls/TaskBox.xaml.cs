using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace teammy
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        private teammyEntities dbContext = new teammyEntities();

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(task), typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueProperty = DependencyProperty.Register("TaskDue", typeof(DateTime?), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("TaskAssigneeList", typeof(ObservableCollection<AssigneeEllipseTask>), typeof(TaskBox));
        private int LoadCounter = 0;

        public task Task
        {
            get { return (task)GetValue(TaskProperty); }
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
            get { return GetValue(TaskPriorityProperty).ToString(); }
            set { SetValue(TaskPriorityProperty, value); }
        }

        public ObservableCollection<AssigneeEllipseTask> TaskAssigneeList
        {
            get { return (ObservableCollection<AssigneeEllipseTask>)GetValue(TaskAssigneeListProperty); }
            set { SetValue(TaskAssigneeListProperty, value); }
        }

        public ObservableCollection<MenuItem> TeamUsers { get; set; } = new ObservableCollection<MenuItem>();

        public static user currentUser { get; set; } = globalItems["currentUser"] as user;
        public DateTime DisplayDateStart { get; set; } = currentUser.privilege_code.Equals("PM") ? DateTime.Now : DateTime.MaxValue;

        public TaskBox()
        {
            InitializeComponent();
            TaskAssigneeList = new ObservableCollection<AssigneeEllipseTask>();
        }

        public void LoadUsers()
        {
            if(TaskName != null)
            {
                List<string> teamMembers = (from mate in dbContext.team_mates
                                            where mate.Team_ID == Task.project.Team_ID
                                            select mate.user.user_name).ToList();

                teamMembers.ForEach(member => TeamUsers.Add(new MenuItem() { Header = member, HorizontalContentAlignment = HorizontalAlignment.Left, VerticalContentAlignment = VerticalAlignment.Center }));

                int? assigned_group = (from task in dbContext.tasks
                                       where task.task_name.Equals(TaskName) && task.assigned_group.HasValue
                                       select task.assigned_group).Single();

                if (assigned_group.HasValue)
                {
                    List<string> assignees = (from assignee in dbContext.assignees
                                              where assignee.assigned_group == assigned_group
                                              select assignee.team_mates.user.user_name).ToList();

                    foreach (var item in assignees)
                    {
                        CreateAssigneeBox(item);
                    }
                }
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

        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //initiate the button color to default
            btnPriority.Background = Brushes.Transparent;

            string priorName = (sender as MenuItem).Name.Replace("Item", "");
            string formatted = (priorName[0] + "").ToUpper() + priorName.Substring(1).ToLower();

            TaskPriority = formatted;
            dbContext.tasks.Find(Task.task_id).priority = TaskPriority;
            dbContext.SaveChanges();
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
            TaskProgress = Task.progress_code;
            dbContext.tasks.Find(Task.task_id).progress_code = TaskProgress;
            dbContext.SaveChanges();
        }

        private void taskNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Task != null)
            {
                dbContext.tasks.Find(Task.task_id).task_name = taskNameTextBox.Text;
                dbContext.SaveChanges();
            }            
        }

        private void editItem_Click(object sender, RoutedEventArgs e)
        {
            EditTaskPage taskDetail = new EditTaskPage() 
            {
                TaskToBeEdited = Task,
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

        private void dlItem_Click(object sender, RoutedEventArgs e)
        {            
            if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ProjCategory currentCat = ProjBoard.categoryBoxes.Find(cat => cat.CategoryName.Equals(Task.category.category_name));

                currentCat.Tasks.Remove(this);
                dbContext.tasks.Remove(dbContext.tasks.Find(Task.task_id));
                dbContext.SaveChanges();
            }                      
        }

        private void btnAddAssignee_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmAssignees") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void AssigneeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string username = (sender as MenuItem).Header.ToString();
            if(TaskAssigneeList.Count <= 4)
            {
                CreateAssigneeBox(username);
                dbContext.assignees.Add(new assignee()
                {
                    assigned_group = Task.assigned_group,
                    mate_id = (from mate in dbContext.team_mates
                               where mate.user.user_name.Equals(username) && mate.Team_ID == Task.project.Team_ID
                               select mate.mate_id).Single()
                });
            }
            else
            {
                MessageBox.Show("The Maximum number of assignees per task is 4.", "Max Assignees Exceeded!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
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

        private void taskDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Task != null)
            {
                dbContext.tasks.Find(Task.task_id).due_date = taskDate.SelectedDate;
                dbContext.SaveChanges();
            }            
        }
    }
}

