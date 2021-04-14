﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private teammyEntities dbContext = new teammyEntities();
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

        private void addTask(object sender, RoutedEventArgs e)
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
            toBeInserted = new TaskBox() { Task = newTask };

            Tasks.Add(toBeInserted);
            dbContext.tasks.Add(newTask);
            dbContext.SaveChanges();
            LoadTasks("new");
        }

        public void LoadTasks(string taskAge = "")
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
                        Task = tasksOfCategory[i],
                        TaskName = tasksOfCategory[i].task_name,
                        TaskDue = tasksOfCategory[i].due_date,
                        TaskPriority = tasksOfCategory[i].priority,
                        TaskProgress = tasksOfCategory[i].progress_code
                    };
                    Tasks.Add(taskBox);
                    taskBox.LoadUsers(taskAge);
                }
            }            
        }

        private void btnCloseC_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StackPanel catPnlParent = Parent as StackPanel;
                catPnlParent.Children.Remove(this);

                dbContext.tasks.RemoveRange((from task in dbContext.tasks
                                             where task.category.category_name.Equals(CategoryName)
                                             select task).ToList());
                dbContext.categories.Remove((from category in dbContext.categories
                                            where category.category_name.Equals(CategoryName)
                                            select category).Single());
                dbContext.SaveChanges();

            }
        }

        public TaskBox FindTaskBox(string taskName)
        {
            return Tasks.ToList().Find(taskbox => taskbox.Task.task_name.Equals(taskName));
        }

        private void txtCategoryName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                dbContext.categories.Add(new category()
                {
                    category_name = txtCategoryName.Text,
                    Proj_ID = Project.Proj_ID
                });
                dbContext.SaveChanges();
            }            
        }
    }
}