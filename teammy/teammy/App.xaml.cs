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
            Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive).Hide();
            (Current.Resources["progReportInstance"] as Window).Show();
        }

        /// <summary>
        ///     Redirects to the Teams List Page
        /// </summary>
        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive).Hide();
            (Current.Resources["teamsListInstance"] as Window).Show();
        }

        private void schedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive).Hide();
            (Current.Resources["scheduleInstance"] as Window).Show();
        }

        /// <summary>
        ///     Redirects to the Home Page
        /// </summary>
        private void homeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive).Hide();
            (Current.Resources["mainInstance"] as Window).Show();
        }

        private void boardsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive).Hide();
            (Current.Resources["createProjInstance"] as Window).Show();
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
    }
}
