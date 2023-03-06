using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.UserControls;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class BoxClickCmd : ICommand
    {
        private ScheduleVM schedVM;
        public BoxClickCmd(ScheduleVM scheduleVM) 
        {
            schedVM = scheduleVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DayBox source = parameter as DayBox;
            schedVM.dayBox_Click(source);
        }
    }
}
