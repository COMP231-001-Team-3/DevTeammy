using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using teammy.Models;

namespace teammy
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

    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;

        private int displayYear = DateTime.Now.Year;
        private int displayMonth = DateTime.Now.Month;
        private List<task> tasks;
        private teammyEntities dbContext = globalItems["dbContext"] as teammyEntities;
        private user currentUser = Application.Current.Resources["currentUser"] as user;

        public Schedule()
        {
            InitializeComponent();
            LoadTasks();
            LoadDates(displayYear, displayMonth);
        }

        #region Miscellaneous

        /// <summary>
        ///     Gets all tasks from the Database
        /// </summary>
        private void LoadTasks()
        {
            if(!currentUser.privilege_code.Equals("PM"))
            {
                tasks = (from task in dbContext.tasks
                         join assignee in dbContext.assignees
                            on task.assigned_group equals assignee.assigned_group
                         join mate in dbContext.team_mates
                            on assignee.mate_id equals mate.mate_id
                         where mate.user.user_id == currentUser.user_id
                         select task).ToList();
            }
            else
            {
                var teamsOfPm = (from user in dbContext.users
                                 join mate in dbContext.team_mates
                                    on user.user_id equals mate.user_id
                                 join team in dbContext.teams
                                    on mate.Team_ID equals team.Team_ID
                                 where user.user_id == currentUser.user_id
                                 select team.Team_ID).ToList();

                tasks = (from task in dbContext.tasks
                         select task).ToList();

                tasks = tasks.FindAll(task => teamsOfPm.Contains(task.project.team.Team_ID));
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
            displayMonth = month;
            displayYear = year;

            lblMonthName.Content = (Months)month + " " + year;

            DateTime monthStart = new DateTime(year, month, 1);
            int totalDays = DateTime.DaysInMonth(year, month);

            int startDay = (int) monthStart.DayOfWeek;
            int date = startDay != 0 ? DateTime.DaysInMonth(year, month != 1 ? month - 1 : 12) - startDay + 1 : 1;

            //Fields for use in the loop declared beforehand for performance reasons
            DayBox dayBox;
            UIElementCollection dateBoxes = containerDates.Children;
            bool isCurrentMonth = date == 1;
            List<task> dueThisDay;

            //Loop for telling each DayBox what its date is and what tasks are due on that date
            for (int i = 0; i < dateBoxes.Count; ++i)
            {
                dayBox = dateBoxes[i] as DayBox;
                dayBox.Date = date;
                dayBox.Status = null;
                dayBox.DisplayTask = null;
                dayBox.Tasks = null;
                dayBox.BoxClick -= new RoutedEventHandler(dayBox_Click);

                //If the date is of the current month...then
                if(dayBox.CurrentMonth = isCurrentMonth)
                {
                    dueThisDay = tasks.FindAll(task => task.due_date.HasValue && task.due_date.Value.Month == month && task.due_date.Value.Year == year && task.due_date.Value.Day == date);

                    //If atleast one task is due on this date...then
                    if(dueThisDay.Count != 0)
                    {
                        dayBox.DisplayTask = dueThisDay[0].task_name;
                        dayBox.Tasks = dueThisDay;
                        dayBox.Status = dueThisDay[0].progress_code;
                        dayBox.BoxClick += new RoutedEventHandler(dayBox_Click);
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

        #region Title Bar Button Event Handlers
        /// <summary>
        ///     Shuts down the application
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     Minimizes the current window
        /// </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Displays page menu when button is clicked
        /// </summary>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = globalItems["cmButton"] as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
        #endregion

        #region Title Pane Event Handlers

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }
        #endregion

        #region Hover handlers

        /// <summary>
        ///     Matches the Hover style of the 'Next image' to that of the 'Next 
        ///     Button'
        /// </summary>
        private void btnNext_MouseEnter(object sender, MouseEventArgs e)
        {
            nextbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Resets the background of the 'Next Image' to normal.
        /// </summary>
        private void btnNext_MouseLeave(object sender, MouseEventArgs e)
        {
            nextbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        ///     Matches the Hover style of the 'Previous image' to that of the 'Next 
        ///     Button'
        /// </summary>
        private void btnPrevious_MouseEnter(object sender, MouseEventArgs e)
        {
            prevbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Resets the background of the 'Previous Image' to normal.
        /// </summary>
        private void btnPrevious_MouseLeave(object sender, MouseEventArgs e)
        {
            prevbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }
        #endregion

        #region Button Click Event Handlers

        /// <summary>
        ///     Shifts the calendar to the previous month
        /// </summary>
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
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
        private void btnNext_Click(object sender, RoutedEventArgs e)
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

        private void dayBox_Click(object sender, RoutedEventArgs e)
        {
            Button sent = sender as Button;
            DayBox dateClicked = (sent.Parent as Grid).Parent as DayBox;

            PopUp taskDetail = new PopUp() { date = dateClicked.Date.Value, month = ((Months)displayMonth).ToString(), year = displayYear, Tasks = dateClicked.Tasks};

            taskDetail.ShowDialog();
        }
    }
}