using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.ViewModels;
using teammy.Views;

namespace teammy.Commands
{
    public class DropDownClosedCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ProgressReportVM prModel;

        public DropDownClosedCmd(ProgressReportVM prModel)
        {
            this.prModel = prModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string cmbName = (string)parameter;

            switch (cmbName)
            {
                case "cmbMembers":
                    prModel.SelectTeamMember();
                    break;
                case "cmbMemProjects":
                    prModel.SelectMemberProject();
                    break;
                case "cmbProjects":
                    prModel.SelectProject();
                    break;
            }           
        }
    }
}
