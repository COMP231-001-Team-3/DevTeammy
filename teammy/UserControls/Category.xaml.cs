using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.Models;

namespace teammy
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class Category : UserControl
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private IMongoDatabase dbContext = DBConnector.Connect();
        private int totalBoxes = 0;
        private TaskBox toBeInserted;
        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(Category));
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(Project), typeof(Category));
        public static readonly DependencyProperty TasksProperty = DependencyProperty.Register("Tasks", typeof(ObservableCollection<TaskBox>), typeof(Category));

        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public ObservableCollection<TaskBox> Tasks
        {
            get { return (ObservableCollection<TaskBox>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }
        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }
        public User currentUser { get; set; } = globalItems["currentUser"] as User;

        public Category()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskBox>();
            LoadTasks();
        }

        private async void addTask(object sender, RoutedEventArgs e)
        {
            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for task per category is 9!", "Max tasks completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int taskId = dbContext.GetCollection<IDSequence>("idValues")
                                     .FindOneAndUpdate(i => i.myID.Equals("Sequence"), Builders<IDSequence>.Update.Inc(i => i.TaskId, 1))
                                     .TaskId;
            TaskToDo newTask = new TaskToDo()
            {
                TaskId = taskId,
                Category = CategoryName,
                ProjectId = Project.ProjectId,
                Progress = "NS",
                TeamId = Project.TeamId,
                Assignees = new List<User>(),
                DueDate = DateTime.Now.AddDays(7)
            };
            toBeInserted = new TaskBox() { ToDoTask = newTask };

            Tasks.Add(toBeInserted);
            await dbContext.GetCollection<TaskToDo>("tasks")
                              .InsertOneAsync(newTask);
            LoadTasks();
        }

        public void LoadTasks()
        {
            if(CategoryName != null)
            {
                TaskBox taskBox;
                List<TaskToDo> tasksOfCategory = dbContext.GetCollection<TaskToDo>("tasks")
                                                            .Find(t => t.Category.Equals(CategoryName) && t.ProjectId.Equals(Project.ProjectId))
                                                            .ToList();

                Tasks.Clear();
                for (int i = 0; i < tasksOfCategory.Count; i++)
                {
                    taskBox = new TaskBox()
                    {
                        ToDoTask = tasksOfCategory[i],
                        TaskName = tasksOfCategory[i].Title,
                        TaskDue = tasksOfCategory[i].DueDate,
                        TaskPriority = tasksOfCategory[i].Priority,
                        TaskProgress = tasksOfCategory[i].Progress
                    };
                    Tasks.Add(taskBox);
                    taskBox.LoadUsers();
                }
            }            
        }

        private async void btnCloseC_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await dbContext.GetCollection<TaskToDo>("tasks")
                                  .DeleteManyAsync(t => t.Category.Equals(CategoryName));

                await dbContext.GetCollection<Project>("projects")
                                  .UpdateOneAsync(p => p.ProjectId == Project.ProjectId, Builders<Project>.Update.Pull(p => p.Categories, CategoryName));

                Application.Current.Windows
                    .OfType<Board>()
                    .SingleOrDefault(window => window.IsActive).Categories
                    .Remove(this);
            }
        }

        public TaskBox FindTaskBox(long taskID)
        {
            return Tasks
                    .ToList()
                    .Find(taskbox => taskbox.ToDoTask.TaskId == taskID);
        }

        private void txtCategoryName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string catName = txtCategoryName.Text;
                AddCategory(catName);
            }
        }

        private async void AddCategory(string catName)
        {
            await dbContext
                    .GetCollection<Project>("projects")
                    .UpdateOneAsync(p => p.ProjectId == Project.ProjectId, Builders<Project>.Update.Push(p => p.Categories, catName));
        }
    }
}
