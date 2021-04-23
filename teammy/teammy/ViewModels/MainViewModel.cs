using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using teammy.Models;

namespace teammy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        public user currentUser { get; set; } = globalItems["currentUser"] as user;

        public ObservableCollection<task> myTasksData { get; set; } = new ObservableCollection<task>();
        public ObservableCollection<task> dueWeekData { get; set; } = new ObservableCollection<task>();

        private teammyEntities dbContext = globalItems["dbContext"] as teammyEntities;

        public MainViewModel()
        {
            Display_AssignToMe();
            Display_ComingUp();
        }

        public void Display_AssignToMe()
        {
            List<task> assignedData = (from task in dbContext.tasks
                           join assignee in dbContext.assignees
                              on task.assigned_group equals assignee.assigned_group
                           join mate in dbContext.team_mates
                              on assignee.mate_id equals mate.mate_id
                           join user in dbContext.users
                              on mate.user_id equals user.user_id
                           where user.user_name.Equals(currentUser.user_name)
                           select task).ToList();

            assignedData.ForEach(myTasksData.Add);
        }

        public void Display_ComingUp()
        {
            List<task> dueThisWeek = myTasksData.ToList().FindAll(task => task.due_date <= DateTime.Now.AddDays(7) && task.due_date >= DateTime.Now);

            dueThisWeek.ForEach(dueWeekData.Add);
        }
    }
}
