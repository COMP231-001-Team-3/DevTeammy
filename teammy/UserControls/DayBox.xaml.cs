using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using teammy.UserControls;


namespace teammy.UserControls
{
    /// <summary>
    /// Interaction logic for DayBox.xaml
    /// </summary>
   
    public partial class DayBox : UserControl
    {
        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register("Details", typeof(DayDetails), typeof(DayBox));

        public event RoutedEventHandler BoxClick
        {
            add { btnClicker.Click += value; }
            remove { btnClicker.Click -= value; }
        }

        
        public DayDetails Details
        {
            get => (DayDetails)GetValue(DetailsProperty);
            set => SetValue(DetailsProperty, value);
        }

        public DayBox()
        {
            InitializeComponent();
        }        
    }
}
