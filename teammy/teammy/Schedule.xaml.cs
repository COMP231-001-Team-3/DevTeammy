using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace teammy
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
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

        private void LoadTasks()
        {
            tasks = (from task in dbContext.tasks
                     select task).ToList();
        }

        private void LoadDates(int year, int month)
        {
            displayMonth = month;
            displayYear = year;

            lblMonthName.Content = (Months)month + " " + year;

            DateTime monthStart = new DateTime(year, month, 1);
            int totalDays = DateTime.DaysInMonth(year, month);

            List<task> dueThisMonth = tasks.FindAll(task => task.due_date >= monthStart && task.due_date <= new DateTime(year, month, totalDays));

            int startDay = (int) monthStart.DayOfWeek;
            int date = startDay != 0 ? DateTime.DaysInMonth(year, month != 1 ? month - 1 : 12) - startDay + 1 : 1;

            DayBox dayBox;
            UIElementCollection dateBoxes = containerDates.Children;
            bool isCurrentMonth = date == 1;
            List<task> dueThisDay;

            for (int i = 0; i < dateBoxes.Count; ++i)
            {
                dayBox = dateBoxes[i] as DayBox;
                dayBox.Date = date++;
                dayBox.Status = null;
                dayBox.DisplayTask = null;

                if(dayBox.CurrentMonth = isCurrentMonth)
                {
                    dueThisDay = dueThisMonth.FindAll(task => task.due_date.Value.Day == date);

                    if(dueThisDay.Count != 0)
                    {
                        dayBox.DisplayTask = dueThisDay[0].task_name;
                        dayBox.Tasks = dueThisDay;
                        dayBox.Status = dueThisDay[0].progress_code;
                    }
                    
                }              

                if (i == startDay - 1 || i == totalDays + startDay - 1)
                {
                    date = 1;
                    isCurrentMonth = !isCurrentMonth;
                }
            }
        }

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

        private void btnNext_MouseEnter(object sender, MouseEventArgs e)
        {
            nextbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        private void btnNext_MouseLeave(object sender, MouseEventArgs e)
        {
            nextbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnPrevious_MouseEnter(object sender, MouseEventArgs e)
        {
            prevbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        private void btnPrevious_MouseLeave(object sender, MouseEventArgs e)
        {
            prevbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (displayMonth == 1)
            {
                displayYear--;
                displayMonth = 13;
            }

            displayMonth--;
            LoadDates(displayYear, displayMonth);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (displayMonth == 12)
            {
                displayYear++;
                displayMonth = 0;
            }

            displayMonth++;
            LoadDates(displayYear, displayMonth);
        }
    }
}
