using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using teammy.Models;
using teammy.UserControls;

namespace teammy
{
    /// <summary>
    /// Interaction logic for EditTaskPage.xaml
    /// </summary>
    public partial class EditTaskPage : Window, INotifyPropertyChanged
    {
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };
        private IMongoDatabase dbContext = DBConnector.Connect();
        private int LoadCounter = 0;
        public TaskToDo TaskToBeEdited { get; set; }
        public User currentUser { get; set; } = globalItems["currentUser"] as User;

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
        }
        public ObservableCollection<MenuItem> TeamUsers { get; set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            Category catOfTask = Application.Current.Windows
                                    .OfType<Board>()
                                    .SingleOrDefault(window => window.projectId == TaskToBeEdited.ProjectId).Categories
                                    .ToList()
                                    .Find(ctg => TaskToBeEdited.Category.Equals(ctg.CategoryName));
            TaskBox boxOfTask = catOfTask.FindTaskBox(TaskToBeEdited.TaskId);

            List<string> assignees = EditTaskAssignees
                                                .Select(a => a.User)
                                                .ToList();
            List<User> currentAssignees = 
            dbContext.GetCollection<User>("users")
                        .Find(u => assignees.Contains(u.Username))
                        .ToList();
            await dbContext.GetCollection<TaskToDo>("tasks")
                              .UpdateOneAsync(t => t.TaskId == TaskToBeEdited.TaskId,
                                              Builders<TaskToDo>.Update.Set(t => t.DueDate, TaskDue)
                                                                      .Set(t => t.Priority, EditTaskPriority)
                                                                      .Set(t => t.Title, txtTaskName.Text)
                                                                      .Set(t => t.Assignees, currentAssignees));

            boxOfTask.ToDoTask = TaskToBeEdited;
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
            ContextMenu cm = FindResource("cmEdAssignees") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void CreateAssigneeBox(string assigneeName)
        {
            Random rd = new Random();
            List<string> assignees = EditTaskAssignees
                                        .Select(a => a.User)
                                        .ToList();

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
            ContextMenu cm = FindResource("cmEdAssignees") as ContextMenu;

            if (LoadCounter == 0 && cm.Items.Count != 0)
            {
                Category catOfTask = Application.Current.Windows
                                        .OfType<Board>()
                                        .SingleOrDefault(window => window.projectId == TaskToBeEdited.ProjectId).Categories
                                        .ToList()
                                        .Find(ctg => TaskToBeEdited.Category.Equals(ctg.CategoryName));
                TaskBox boxOfTask = catOfTask.FindTaskBox(TaskToBeEdited.TaskId);

                foreach (MenuItem menuItem in cm.Items)
                {
                    menuItem.Click += new RoutedEventHandler(AssigneeEdMenuItem_Click);
                    if(boxOfTask.LoadCounter == 1)
                    {
                        menuItem.Click -= new RoutedEventHandler(boxOfTask.AssigneeMenuItem_Click);
                    }                    
                }
                LoadCounter++;
            }
        }

        private void AssigneeEdMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string username = (sender as MenuItem).Header.ToString();
            if (EditTaskAssignees.Count < 4)
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
