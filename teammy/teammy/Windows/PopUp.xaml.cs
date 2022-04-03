using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using teammy.Models;

namespace teammy
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public int date, year;
        public string month;
        public List<TaskToDo> Tasks;

        public PopUp()
        {
            InitializeComponent();            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void popupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblDateHeader.Content = month + " " + date + ", " + year;
            
            foreach(var task in Tasks)
            {
                pnlTasks.Children.Add(new PopTaskBox() { Task = task });
            }
        }

        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }
    }
}
