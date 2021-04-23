using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace teammy.Commands
{
    public class MenuClickCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Window currWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
            Button clicked = parameter as Button;
            Window toDisplay = null;

            switch (clicked.Name)
            {
                case "homeIconBtn":
                    toDisplay = Application.Current.Resources["mainInstance"] as Window;
                    break;
                case "BoardsIconBtn":
                    toDisplay = Application.Current.Resources["createProjInstance"] as Window;
                    break;
                case "TeamsIconBtn":
                    toDisplay = Application.Current.Resources["teamsListInstance"] as Window;
                    break;
                case "ProgressIconBtn":
                    toDisplay = Application.Current.Resources["progReportInstance"] as Window;
                    break;
                case "ScheduleIconBtn":
                    toDisplay = Application.Current.Resources["scheduleInstance"] as Window;
                    break;
            }

            if (!toDisplay.Name.Equals(currWindow.Name))
            {
                toDisplay.Show();
                currWindow.Hide();
            }
        }
    }
}
