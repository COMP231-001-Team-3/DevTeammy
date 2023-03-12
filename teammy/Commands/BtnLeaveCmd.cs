using System;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class BtnLeaveCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ScheduleVM schedVM;
        TeamsVM teamVM;
        public BtnLeaveCmd(ScheduleVM schedVM = null, TeamsVM teamVM = null) 
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
            Button btnOutFocus = parameter as Button;

            switch (btnOutFocus.Name)
            {
                case "btnPrevious":
                    schedVM.MouseOverPrevBtn = false;
                    break;
                case "btnNext":
                    schedVM.MouseOverNextBtn = false;
                    break;
                case "btnCancel":
                    teamVM.MouseOverCancelBtn = false; 
                    break;
                case "btnDone":
                    teamVM.MouseOverDoneBtn = false;
                    break;
                case "btnCreateTeam":
                    teamVM.MouseOverCreateTeamBtn = false;
                    break;
            }
        }
    }
}
