using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace teammy
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class ProjCategory : UserControl
    {
        private teammyEntities dbContext = new teammyEntities();
        private int totalBoxes = 0;
        private TaskBox toBeInserted;
        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(ProjCategory));

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

        public ProjCategory()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskBox>();
        }

        private void addTask(object sender, RoutedEventArgs e)
        {
            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for task per category is 9!", "Max tasks completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Application.Current.Resources["assigneeNum"] = null;
            Application.Current.Resources["priority"] = null;
            Application.Current.Resources["status"] = null;
            toBeInserted = new TaskBox() { Task = new task()};

            Tasks.Add(toBeInserted);
        }

        public void LoadTasks()
        {
            if(CategoryName != null)
            {
                TaskBox taskBox;
                List<task> tasksOfCategory = (from task in dbContext.tasks
                                              where task.category.category_name.Equals(CategoryName)
                                              select task).ToList();

                for (int i = 0; i < tasksOfCategory.Count; i++)
                {
                    taskBox = new TaskBox()
                    {
                        Task = tasksOfCategory[i]
                    };
                    Tasks.Add(taskBox);
                    taskBox.LoadUsers();
                }
            }            
        }

        private void btnCloseC_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StackPanel catPnlParent = Parent as StackPanel;
                catPnlParent.Children.Remove(this);
            }
        }
    }
}
