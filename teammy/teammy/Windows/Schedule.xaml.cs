using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private teammyEntities dbContext = new teammyEntities();

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
            tasks = (from task in dbContext.tasks
                     select task).ToList();
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

                //If the date is of the current month...then
                if(dayBox.CurrentMonth = isCurrentMonth)
                {
                    dueThisDay = tasks.FindAll(task => task.due_date.Value.Month == month && task.due_date.Value.Year == year && task.due_date.Value.Day == date);

                    //If atleast one task is due on this date...then
                    if(dueThisDay.Count != 0)
                    {
                        dayBox.DisplayTask = dueThisDay[0].task_name;
                        dayBox.Tasks = dueThisDay;
                        dayBox.Status = dueThisDay[0].progress_code;
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

        //**TBD An event handler to determine what happens when a date is clicked
        public void dbRowBox_BoxClick(object sender, RoutedEventArgs e)
        {

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
            ContextMenu cm = FindResource("cmButton") as ContextMenu;
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

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it were hovered 
        ///     upon.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     The method sets the background of the Button it contains to the same color as if it lost focus.
        /// </summary>
        /// <param name="sender">The MenuItem triggering this event</param>
        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = null;
        }
        #endregion

        #region Page Navigation
        /// <summary>
        ///     Redirects to Progress Report Page
        /// </summary>
        private void progMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (globalItems["progReportInstance"] as Window).Show();
            Hide();
        }

        /// <summary>
        ///     Redirects to Boards Page
        /// </summary>
        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (globalItems["createProjInstance"] as Window).Show();
            Hide();
        }

        /// <summary>
        ///     Redirects to Home page
        /// </summary>
        private void homeMenuItem_click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["mainInstance"] as Window).Show();
        }

        /// <summary>
        ///     Redirects to Teams page
        /// </summary>
        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            (globalItems["teamsListInstance"] as Window).Show();
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
    }
}
