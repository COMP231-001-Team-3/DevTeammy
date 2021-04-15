using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for EditTaskPage.xaml
    /// </summary>
    public partial class EditTaskPage : Window, INotifyPropertyChanged
    {
       Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        private teammyEntities dbContext = globalItems["dbContext"] as teammyEntities;
        private int LoadCounter = 0;
        public task TaskToBeEdited { get; set; }

        private string _taskPriority;
        public string EditTaskPriority
        {
            get => _taskPriority;
            set 
            {
                _taskPriority = value;
                RaisePropertyChanged();
            } 
        }

        private string _taskName;
        public string TaskName 
        {
            get => _taskName;
            set
            {
                _taskName = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _taskDue;
        public DateTime? TaskDue
        {
            get => _taskDue;
            set
            {
                _taskDue = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<AssigneeEllipse> EditTaskAssignees { get; set; } = new ObservableCollection<AssigneeEllipse>();
        private static ResourceDictionary globalItems = Application.Current.Resources;

        public event PropertyChangedEventHandler PropertyChanged;

        public static user currentUser { get; set; } = globalItems["currentUser"] as user;

        public EditTaskPage()
        {
            InitializeComponent();            
        }

        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //initiate the button color to default
            btnPriority.Background = Brushes.Transparent;

            string priorName = (sender as MenuItem).Name.Replace("Item", "");
            string formatted = (priorName[0] + "").ToUpper() + priorName.Substring(1).ToLower();

            EditTaskPriority = formatted;
            dbContext.tasks.Find(TaskToBeEdited.task_id).priority = EditTaskPriority;
        }
        public ObservableCollection<MenuItem> TeamUsers { get; set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            TaskToBeEdited = dbContext.tasks.Find(TaskToBeEdited.task_id);
            EditTaskPriority = dbContext.tasks.Find(TaskToBeEdited.task_id).progress_code;
            TaskName = dbContext.tasks.Find(TaskToBeEdited.task_id).task_name;
            TaskDue = dbContext.tasks.Find(TaskToBeEdited.task_id).due_date;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            ProjCategory catOfTask = Application.Current.Windows.OfType<ProjBoard>().SingleOrDefault(window => window.projName.Equals(TaskToBeEdited.project.Proj_Name)).Categories.ToList().Find(ctg => TaskToBeEdited.category.category_name.Equals(ctg.CategoryName));
            TaskBox boxOfTask = catOfTask.FindTaskBox(TaskToBeEdited.task_name);
            TaskToBeEdited.due_date = TaskDue;
            TaskToBeEdited.priority = EditTaskPriority;
            TaskToBeEdited.task_name = TaskName;            
            
            dbContext.tasks.Find(TaskToBeEdited.task_id).priority = EditTaskPriority;
            dbContext.tasks.Find(TaskToBeEdited.task_id).task_name = txtTaskName.Text;
            dbContext.tasks.Find(TaskToBeEdited.task_id).due_date = TaskDue;

            string username;
            List<string> assignees = (from assignee in dbContext.assignees
                                      where assignee.assigned_group == TaskToBeEdited.assigned_group
                                      select assignee.team_mates.user.user_name).ToList();

            foreach (AssigneeEllipse epsAssignee in EditTaskAssignees)
            {
                username = epsAssignee.User;
                if (!assignees.Contains(username))
                {
                    dbContext.assignees.Add(new assignee()
                    {
                        assigned_group = TaskToBeEdited.assigned_group,
                        mate_id = (from mate in dbContext.team_mates
                                   where mate.user.user_name.Equals(username) && mate.Team_ID == TaskToBeEdited.project.Team_ID
                                   select mate.mate_id).Single()
                    });
                }
            }

            dbContext.SaveChanges();

            boxOfTask.Task = TaskToBeEdited;
            boxOfTask.TaskName = TaskName;
            boxOfTask.TaskDue = TaskDue;
            boxOfTask.TaskPriority = EditTaskPriority;

            boxOfTask.TaskAssigneeList = new ObservableCollection<AssigneeEllipseTask>();

            EditTaskAssignees.ToList().ForEach(eps =>
            {
                boxOfTask.TaskAssigneeList.Add(new AssigneeEllipseTask()
                {
                    BackColor = eps.BackColor,
                    User = eps.User
                });
            });
            Close();
        }

        private void btnAddAssignee_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmAssignees") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void CreateAssigneeBox(string assigneeName)
        {
            Random rd = new Random();
            List<string> assignees = (from assignee in dbContext.assignees
                                      where assignee.assigned_group == TaskToBeEdited.assigned_group
                                      select assignee.team_mates.user.user_name).ToList();
            if (assignees.Contains(assigneeName))
            {
                MessageBox.Show("This member is already assigned to the task!", "Duplicate assignee entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Creation & Initialization of InitialBox
            AssigneeEllipse epsAssignee = new AssigneeEllipse
            {
                User = assigneeName,
                BackColor = backColors[rd.Next(0, backColors.Length - 1)]
            };

            EditTaskAssignees.Add(epsAssignee);
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
        private void AssigneeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string username = (sender as MenuItem).Header.ToString();
            if (EditTaskAssignees.Count <= 4)
            {
                CreateAssigneeBox(username);
            }
            else
            {
                MessageBox.Show("The Maximum number of assignees per task is 4.", "Max Assignees Exceeded!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
