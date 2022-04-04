using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using teammy.Models;

namespace teammy
{
    /// <summary>
    /// Interaction logic for PopTaskBox.xaml
    /// </summary>
    public partial class PopTaskBox : UserControl
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Task", typeof(TaskToDo), typeof(PopTaskBox));

        private IMongoDatabase dbContext = DBConnector.Connect();
        public TaskToDo Task
        {
            get => GetValue(TaskProperty) as TaskToDo;
            set => SetValue(TaskProperty, value);
        }

        public PopTaskBox()
        {
            InitializeComponent();
        }

        private void popTaskBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<User> assignees = dbContext.GetCollection<TaskToDo>("tasks")
                                                .Find(t => t.TaskId == Task.TaskId)
                                                .Project(t => t.Assignees)
                                                .Single();

            Random rd = new Random();
            foreach (var assignee in assignees)
            {
                pnlAssignees.Children.Add(new AssigneeEllipse() { User = assignee.Username, BackColor = backColors[rd.Next(0, backColors.Length - 1)] });

                pnlAssignees.Width += 42;
                pnlAssignees.Margin = new Thickness(pnlAssignees.Margin.Left - 40, pnlAssignees.Margin.Top, pnlAssignees.Margin.Right - 40, pnlAssignees.Margin.Bottom);
            }
        }
    }
}
