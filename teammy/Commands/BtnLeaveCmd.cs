using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class BtnLeaveCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ScheduleVM schedVM;
        public BtnLeaveCmd(ScheduleVM schedVM) 
        {
            this.schedVM = schedVM;            
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
                    schedVM.btnPrevious_MouseLeave();
                    break;
                case "btnNext":
                    schedVM.btnNext_MouseLeave();
                    break;
            }
        }
    }
}
