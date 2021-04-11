using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace teammy
{
    /// <summary>
    /// Interaction logic for DayBox.xaml
    /// </summary>
   
    public partial class DayBox : UserControl
    {
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(int?), typeof(DayBox));

        public static readonly DependencyProperty DisplayTaskProperty = DependencyProperty.Register("DisplayTask", typeof(string), typeof(DayBox));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(DayBox));

        public static readonly DependencyProperty TasksProperty = DependencyProperty.Register("Tasks", typeof(List<task>), typeof(DayBox));

        public static readonly DependencyProperty CurrentMonthProperty = DependencyProperty.Register("CurrentMonth", typeof(bool), typeof(DayBox));

        public event RoutedEventHandler BoxClick
        {
            add { btnClicker.Click += value; }
            remove { btnClicker.Click -= value; }
        }

        public int? Date
        {
            get => (int)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }
        public string DisplayTask
        {
            get => GetValue(DisplayTaskProperty).ToString();
            set => SetValue(DisplayTaskProperty, value);
        }
        public string Status
        {
            get => GetValue(StatusProperty).ToString();
            set => SetValue(StatusProperty, value);
        }
        public List<task> Tasks
        {
            get => GetValue(TasksProperty) as List<task>;
            set => SetValue(TasksProperty, value);
        }
        public bool CurrentMonth
        {
            get => (bool)GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        public DayBox()
        {
            InitializeComponent();
        }        
    }
}
