using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.UserControls;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class CardClickCmd : ICommand
    {
        private TeamsVM teamVM;
        public CardClickCmd(TeamsVM teamVM) 
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
            CardBox source = parameter as CardBox;
            teamVM.teamBox_CardClick(source);
        }
    }
}
