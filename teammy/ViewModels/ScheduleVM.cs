using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using teammy.Commands;
using teammy.Models;
using teammy.UserControls;

namespace teammy.ViewModels
{
    public class ScheduleVM : ViewModelBase
    {
        /// <summary>
        ///     Maps an integer to the corresponding month of a year
        /// </summary>
        public enum Months
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        private static ResourceDictionary globalItems = Application.Current.Resources;

        private int displayYear = DateTime.Now.Year;
        private int displayMonth = DateTime.Now.Month;
        private List<TaskToDo> tasks;
        private IMongoDatabase dbContext = DBConnector.Connect();
        public User currentUser { get; set; } = globalItems["currentUser"] as User;

        private string _lblMonth = string.Empty;
        public string lblMonth 
        {
            get
            {
                return _lblMonth;
            }
            set 
            {
                _lblMonth = value;
                OnPropertyChanged(nameof(lblMonth));
            }
        } 
        public SolidColorBrush nextbtnIconBG { get; set; } = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush prevbtnIconBG { get; set; } = new SolidColorBrush(Colors.Transparent);

        
        public ObservableCollection<DayDetails> Dates { get; set; } = new ObservableCollection<DayDetails>() {
            new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails(), new DayDetails()
        };

        public ICommand btnEnterCmd { get; set; }
        public ICommand btnLeaveCmd { get; set; }
        public ICommand btnClickCmd { get; set; }
        public ICommand boxClickCmd { get; set; }

        public ScheduleVM() 
        {
            LoadTasks();
            LoadDates(displayYear, displayMonth);
            btnEnterCmd = new BtnEnterCmd(this);
            btnLeaveCmd = new BtnLeaveCmd(this);
            btnClickCmd = new BtnClickCmd(this);
            boxClickCmd = new BoxClickCmd(this);

        }
        #region Miscellaneous

        /// <summary>
        ///     Gets all tasks from the Database
        /// </summary>
        private void LoadTasks()
        {
            if (!currentUser.Privilege.Equals("PM"))
            {
                tasks =
                    (from t in dbContext.GetCollection<TaskToDo>("tasks").AsQueryable()
                     where t.Assignees.Select(a => a.UserId).Contains(currentUser.UserId)
                     select t).ToList();
            }
            else
            {

                tasks =
                    (from team in dbContext.GetCollection<Team>("teams").AsQueryable()
                     join t in dbContext.GetCollection<TaskToDo>("tasks").AsQueryable()
                     on team.TeamId equals t.TeamId
                     where team.Members.Select(m => m.UserId).Contains(currentUser.UserId)
                     select t).ToList();
            }
        }

        /// <summary>
        ///     Prepares the calendar to display dates of the current month and loads
        ///     and displays tasks on due dates.
        /// </summary>
        /// <param name="year">Represents the year to be shown</param>
        /// <param name="month">Represents the month to be shown</param>
        private void LoadDates(int year, int month)
        {
            lblMonth = (Months)month + " " + year;

            DateTime monthStart = new DateTime(year, month, 1);
            int totalDays = DateTime.DaysInMonth(year, month);

            int startDay = (int)monthStart.DayOfWeek;
            int date = startDay != 0 ? DateTime.DaysInMonth(year, month != 1 ? month - 1 : 12) - startDay + 1 : 1;

            bool isCurrentMonth = date == 1;
            List<TaskToDo> dueThisDay;

            //Loop for telling each DayBox what its date is and what tasks are due on that date
            for (int i = 0; i < 42; i++)
            {
                Dates.RemoveAt(i);
                Dates.Insert(i, new DayDetails(date, "", "", new List<TaskToDo>(), isCurrentMonth));
                //If the date is of the current month...then
                if (isCurrentMonth)
                {
                    dueThisDay = tasks.FindAll(task => task.DueDate.Month == month && task.DueDate.Year == year && task.DueDate.Day == date);

                    //If atleast one task is due on this date...then
                    if (dueThisDay.Count != 0)
                    {
                        Dates.RemoveAt(i);
                        Dates.Insert(i, new DayDetails(date, dueThisDay[0].Title, dueThisDay[0].Progress, dueThisDay, isCurrentMonth));
                    }
                }

                date++;

                //If i has reached the end of the prev month or the end of the current month...then
                if (i == startDay - 1 || i == totalDays + startDay - 1)
                {
                    date = 1;
                    isCurrentMonth = !isCurrentMonth;
                }
            }
        }

        #endregion

        #region Hover handlers

        /// <summary>
        ///     Matches the Hover style of the 'Next image' to that of the 'Next 
        ///     Button'
        /// </summary>
        public void btnNext_MouseEnter()
        {
            nextbtnIconBG = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Resets the background of the 'Next Image' to normal.
        /// </summary>
        public void btnNext_MouseLeave()
        {
            nextbtnIconBG = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        ///     Matches the Hover style of the 'Previous image' to that of the 'Next 
        ///     Button'
        /// </summary>
        public void btnPrevious_MouseEnter()
        {
            prevbtnIconBG = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Resets the background of the 'Previous Image' to normal.
        /// </summary>
        public void btnPrevious_MouseLeave()
        {
            prevbtnIconBG = new SolidColorBrush(Colors.Transparent);
        }
        #endregion

        #region Button Click Event Handlers

        /// <summary>
        ///     Shifts the calendar to the previous month
        /// </summary>
        public void btnPrevious_Click()
        {
            //If current month is January...then
            if (displayMonth == 1)
            {
                displayYear--;
                displayMonth = 13;
            }

            displayMonth--;
            LoadDates(displayYear, displayMonth);
        }

        /// <summary>
        ///     Shifts the calendar to the next month
        /// </summary>
        public void btnNext_Click()
        {
            //If current month is December...then
            if (displayMonth == 12)
            {
                displayYear++;
                displayMonth = 0;
            }

            displayMonth++;
            LoadDates(displayYear, displayMonth);
        }
        #endregion        

        public void dayBox_Click(DayBox dateClicked)
        {
            PopUp taskDetail = new PopUp() { date = dateClicked.Details.Date, month = ((Months)displayMonth).ToString(), year = displayYear, Tasks = dateClicked.Details.Tasks };

            taskDetail.ShowDialog();
        }
    }
}
