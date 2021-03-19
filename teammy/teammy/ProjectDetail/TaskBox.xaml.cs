using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        public static readonly DependencyProperty TaskNameProperty = DependencyProperty.Register("TaskName", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskPriorityProperty = DependencyProperty.Register("TaskPriority", typeof(string),  typeof(TaskBox));
        public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register("TaskProgress", typeof(string), typeof(TaskBox));
        public static readonly DependencyProperty TaskDueDateProperty = DependencyProperty.Register("TaskDueDate", typeof(DateTime), typeof(TaskBox));
        public static readonly DependencyProperty TaskAssigneeListProperty = DependencyProperty.Register("Assignee", typeof(List<string>), typeof(TaskBox));



        public string TaskName
        {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }
        public string TaskPriority
        {
            get { return (string)GetValue(TaskPriorityProperty); }
            set { SetValue(TaskPriorityProperty, value); }
        }
        public string TaskProgress
        {
            get { return (string)GetValue(TaskProgressProperty); }
            set { SetValue(TaskProgressProperty, value); }
        }
        public DateTime TaskDueDate
        {
            get { return (DateTime)GetValue(TaskDueDateProperty); }
            set { SetValue(TaskProgressProperty, value); }
        }

        public List<string> AssigneeList
        {
            get { return (List<string>)GetValue(TaskAssigneeListProperty); }
            set { SetValue(TaskAssigneeListProperty, value); }
        }


        //Events for change on the text properties
        public event EventHandler<EventArgs> TaskNameChanged;
        public event EventHandler<EventArgs> TaskPriorityChanged;
        public event EventHandler<EventArgs> TaskProgressChanged;
        public event EventHandler<EventArgs> TaskDueDateChanged;
        public event EventHandler<EventArgs> TaskAssigneeChanged;
        
        public TaskBox()
        {
            InitializeComponent();
        }

        private void btnEditDelete_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmThreeDots") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            //e.Handled = true;
            //TaskPriorityChanged?.Invoke(this, EventArgs.Empty);
        }
        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            e.Handled = true;
            TaskPriorityChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmStatus") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;

            e.Handled = true;
            TaskProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PriorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Brush chosenPriority = ((sender as MenuItem).Icon as Rectangle).Fill;
            priorityGrid.Background = chosenPriority;
        }

        private void StatusMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        private void StatusMenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            //The Button whose background is to be set
            Button btnIcon = MenuItem.Children[1] as Button;
            btnIcon.Background = null;
        }

        private void StatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //The Grid encompassing all the icon elements for the menu item
            Grid MenuItem = (sender as MenuItem).Icon as Grid;

            ImageSource iconSource = (MenuItem.Children[0] as Image).Source;
            statusGrid.Background = new ImageBrush(iconSource);
        }

        private void btnAssignee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void assigneeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

