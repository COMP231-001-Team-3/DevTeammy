using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.ViewModels;
using System.Windows;

namespace teammy.Commands
{
    public class BtnEnterCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ScheduleVM schedVM;
        TeamsVM teamVM;
        public BtnEnterCmd(ScheduleVM schedVM = null, TeamsVM teamVM = null) 
        {
            this.schedVM = schedVM;
            this.teamVM = teamVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //The Button whose background is to be set
            Button btnInFocus = parameter as Button;

            switch (btnInFocus.Name)
            {
                case "btnPrevious":
                    schedVM.MouseOverPrevBtn = true;
                    break;
                case "btnNext":
                    schedVM.MouseOverNextBtn = true;
                    break;
            }
        }
    }
}
