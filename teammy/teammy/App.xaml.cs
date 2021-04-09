using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace teammy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Page Navigation

        /// <summary>
        ///     Redirects to the Progress Report Page
        /// </summary>
        private void progMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window currWindow = Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Window toDisplay = Current.Resources["progReportInstance"] as Window;

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }

        /// <summary>
        ///     Redirects to the Teams List Page
        /// </summary>
        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window currWindow = Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Window toDisplay = Current.Resources["teamsListInstance"] as Window;

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }

        private void schedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window currWindow = Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Window toDisplay = Current.Resources["scheduleInstance"] as Window;

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }

        /// <summary>
        ///     Redirects to the Home Page
        /// </summary>
        private void homeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window currWindow = Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Window toDisplay = Current.Resources["mainInstance"] as Window;

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }

        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window currWindow = Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Window toDisplay = Current.Resources["createProjInstance"] as Window;

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }
        #endregion



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

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Current.Resources["mainInstance"] = Current.Resources["mainInstance"] as Window;
            Current.Resources["createProjInstance"] = Current.Resources["createProjInstance"] as Window;
            Current.Resources["teamsListInstance"] = Current.Resources["teamsListInstance"] as Window;
            Current.Resources["scheduleInstance"] = Current.Resources["scheduleInstance"] as Window;
            Current.Resources["progReportInstance"] = Current.Resources["progReportInstance"] as Window;
        }
    }
}
