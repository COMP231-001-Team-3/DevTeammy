using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.UserControls;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class CardKeyUpCmd : ICommand
    {
        private TeamsVM teamVM;
        public CardKeyUpCmd(TeamsVM teamVM) 
        {
            this.teamVM = teamVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            EventArgs sent = parameter as EventArgs;
            teamVM.teamBox_KeyUp(sent);
        }
    }
}
