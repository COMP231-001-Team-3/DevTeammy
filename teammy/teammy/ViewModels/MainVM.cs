using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using teammy.Models;

namespace teammy.ViewModels
{
    public class MainVM : ViewModelBase
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public User currentUser { get; set; } = globalItems["currentUser"] as User;
        public ObservableCollection<TaskToDo> TasksAssigned { get; set; } = new ObservableCollection<TaskToDo>();
        public ObservableCollection<TaskToDo> TasksDue { get; set; } = new ObservableCollection<TaskToDo>();

        private IMongoDatabase dbContext = DBConnector.Connect();

        public MainVM()
        {
            DisplayTasksAssigned();
            DisplayTasksDue();
        }

        public void DisplayTasksAssigned()
        {
            List<TaskToDo> tasksAssigned =
                (from t in dbContext.GetCollection<TaskToDo>("tasks").AsQueryable()
                 where t.Assignees.Select(a => a.UserId).Contains(currentUser.UserId)
                 select t).ToList();

            tasksAssigned.ForEach(TasksAssigned.Add);
        }

        public void DisplayTasksDue()
        {
            List<TaskToDo> tasksDue = TasksAssigned
                                        .ToList()
                                        .FindAll(task => task.DueDate <= DateTime.Now.AddDays(7) && task.DueDate >= DateTime.Now);

            tasksDue.ForEach(TasksDue.Add);
        }
    }
}
