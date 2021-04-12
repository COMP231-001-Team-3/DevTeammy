using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace teammy
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : UserControl
    {
        private teammyEntities dbContext = new teammyEntities();
        private int totalBoxes = 0;
        private TaskBox toBeInserted;      

        public NewCategory()
        {
            InitializeComponent();
            LoadTask();
        }       

        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(NewCategory));
        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public event EventHandler<EventArgs> txtCategroyNameChanged;
        private void txtCategoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            txtCategroyNameChanged?.Invoke(this, EventArgs.Empty);

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

            taStackPanel.Children.Add(toBeInserted);
        }

        private void btnCloseC_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StackPanel catPnlParent = Parent as StackPanel;
                catPnlParent.Children.Remove(this);
            }
        }
        

        private void LoadTask()
        {
            taStackPanel.Children.Clear();
            TaskBox taskBox;
            string taskName;
            string taskPrio;
            string taskProgre;
            DateTime taskDate;

            if (Application.Current.Resources["catName"].ToString() != null)
            {
                string category = Application.Current.Resources["catName"].ToString();

                List<task> tasksOfCategory = (from task in dbContext.tasks
                                             where task.category.category_name.Equals(category)
                                             select task).ToList();

                for (int i = 0; i < tasksOfCategory.Count; i++)
                {
                    totalBoxes++;
                    taskBox = new TaskBox()
                    {
                        Task = tasksOfCategory[i]
                    };
                    taStackPanel.Children.Add(taskBox);
                }
            }
        }
    }
}
