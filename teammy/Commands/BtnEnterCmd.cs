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
        public BtnEnterCmd(ScheduleVM schedVM) 
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
            //(((btnInFocus.Content as DockPanel).Children[0] as Grid).Children[1] as Button).RaiseEvent(new RoutedEventArgs(Button.MouseEnterEvent));

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
