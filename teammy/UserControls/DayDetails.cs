using System.Collections.Generic;
using teammy.Models;

namespace teammy.UserControls
{
    public class DayDetails
    {
        public int Date { get; set; }
        public string DisplayTask { get; set; }
        public string Status { get; set; }
        public List<TaskToDo> Tasks { get; set; }
        public bool CurrentMonth { get; set; }
        public DayDetails(int date = 0, string taskTitle = null, string status = null, List<TaskToDo> tasks = null, bool currentMonth = false)
        {
            Date = date;
            DisplayTask = taskTitle;
            Status = status;
            Tasks = tasks;
            CurrentMonth = currentMonth;
        }
    }
}
