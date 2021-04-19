using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace teammy
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class ProjCategory : UserControl
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private teammyEntities dbContext = globalItems["dbContext"] as teammyEntities;
        private int totalBoxes = 0;
        private TaskBox toBeInserted;
        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(ProjCategory));
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(project), typeof(ProjCategory));

        public static readonly DependencyProperty TasksProperty = DependencyProperty.Register("Tasks", typeof(ObservableCollection<TaskBox>), typeof(ProjCategory));

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
        public project Project
        {
            get { return (project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }
        public user currentUser { get; set; } = globalItems["currentUser"] as user;

        public ProjCategory()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskBox>();
        }

        private async void addTask(object sender, RoutedEventArgs e)
        {
            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for task per category is 9!", "Max tasks completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            task newTask = new task()
            {
                category = (from cat in dbContext.categories
                           where cat.category_name.Equals(CategoryName)
                           select cat).Single(),
                project = (from cat in dbContext.categories
                           where cat.category_name.Equals(CategoryName)
                           select cat.project).Single(),
                progress_code = "NS"
            };
            toBeInserted = new TaskBox() { ToDoTask = newTask };

            Tasks.Add(toBeInserted);
            dbContext.tasks.Add(newTask);
            await dbContext.SaveChangesAsync();
            LoadTasks();
        }

        public void LoadTasks()
        {
            if(CategoryName != null)
            {
                TaskBox taskBox;
                List<task> tasksOfCategory = (from task in dbContext.tasks
                                              where task.category.category_name.Equals(CategoryName)
                                              select task).ToList();

                Tasks.Clear();
                for (int i = 0; i < tasksOfCategory.Count; i++)
                {
                    taskBox = new TaskBox()
                    {
                        ToDoTask = tasksOfCategory[i],
                        TaskName = tasksOfCategory[i].task_name,
                        TaskDue = tasksOfCategory[i].due_date,
                        TaskPriority = tasksOfCategory[i].priority,
                        TaskProgress = tasksOfCategory[i].progress_code
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
                List<task> toBeRemoved = (from task in dbContext.tasks
                                          where task.category.category_name.Equals(CategoryName)
                                          select task).ToList();

                int? taskAssigneeNum;
                Parallel.ForEach(toBeRemoved, (curr) =>
                {
                    taskAssigneeNum = curr.assigned_group;
                    dbContext.assignees.RemoveRange((from assignee in dbContext.assignees
                                                     where assignee.assigned_group == taskAssigneeNum
                                                     select assignee).ToList());
                });

                string catName = CategoryName;
                await Task.Run(() => RemoveCategory(toBeRemoved, catName));

                Application.Current.Windows.OfType<ProjBoard>().SingleOrDefault(window => window.IsActive).Categories.Remove(this);

            }
        }

        private async void RemoveCategory(List<task> toBeRemoved, string catName)
        {
            dbContext.tasks.RemoveRange(toBeRemoved);

            dbContext.categories.Remove(dbContext.categories.Find((from category in dbContext.categories
                                                                   where category.category_name.Equals(catName)
                                                                   select category.category_id).Single()));

            await dbContext.SaveChangesAsync();
        }

        public TaskBox FindTaskBox(long taskID)
        {
            return Tasks.ToList().Find(taskbox => taskbox.ToDoTask.task_id.Equals(taskID));
        }

        private async void txtCategoryName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string catName = txtCategoryName.Text;
                long projID = Project.Proj_ID;
                await Task.Run(() => AddCategory(catName, projID));
            }
        }

        private async void AddCategory(string catName, long projID)
        {
            dbContext.categories.Add(new category()
            {
                category_name = catName,
                Proj_ID = projID
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
