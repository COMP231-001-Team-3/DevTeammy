using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace teammy.UserControls
{
    /// <summary>
    /// Interaction logic for AssigneeEllipse.xaml
    /// </summary>
    public partial class AssigneeEllipseTask : UserControl
    {
        public static readonly DependencyProperty BackColor1Property = DependencyProperty.Register("BackColor", typeof(Color), typeof(AssigneeEllipseTask));
        public static readonly DependencyProperty User1Property = DependencyProperty.Register("User", typeof(string), typeof(AssigneeEllipseTask));

        public string User
        {
            get { return (string)GetValue(User1Property); }
            set { SetValue(User1Property, value); }
        }
        public Color BackColor
        {
            get { return (Color)GetValue(BackColor1Property); }
            set { SetValue(BackColor1Property, value); }
        }
        public AssigneeEllipseTask()
        {
            InitializeComponent();
        }
    }
}
