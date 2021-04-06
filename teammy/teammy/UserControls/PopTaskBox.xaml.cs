using System;
using System.Collections.Generic;
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

namespace teammy
{
    /// <summary>
    /// Interaction logic for PopTaskBox.xaml
    /// </summary>
    public partial class PopTaskBox : UserControl
    {
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(task), typeof(PopTaskBox));

        private teammyEntities dbContext = new teammyEntities();

        public task Task
        {
            get => GetValue(TaskProperty) as task;
            set => SetValue(TaskProperty, value);
        }

        public PopTaskBox()
        {
            InitializeComponent();
        }

        private void popTaskBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<user> assignees = (from task in dbContext.tasks
                                   join assignee in dbContext.assignees
                                      on task.assigned_group equals assignee.assigned_group
                                   join mate in dbContext.team_mates
                                      on assignee.mate_id equals mate.mate_id
                                   where task.task_id == Task.task_id
                                   select mate.user).ToList();

            Random rd = new Random();
            foreach (var assignee in assignees)
            {
                pnlAssignees.Children.Add(new AssigneeEllipse() { User = assignee.user_name, BackColor = backColors[rd.Next(0, backColors.Length - 1)] });

                pnlAssignees.Width += 42;
                pnlAssignees.Margin = new Thickness(pnlAssignees.Margin.Left - 40, pnlAssignees.Margin.Top, pnlAssignees.Margin.Right - 40, pnlAssignees.Margin.Bottom);
            }
        }
    }
}
