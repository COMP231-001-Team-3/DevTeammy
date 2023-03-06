using System;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class BtnClickCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ScheduleVM schedVM;
        public BtnClickCmd(ScheduleVM schedVM) 
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
                    schedVM.btnPrevious_Click();
                    break;
                case "btnNext":
                    schedVM.btnNext_Click();
                    break;
            }
        }
    }
}
