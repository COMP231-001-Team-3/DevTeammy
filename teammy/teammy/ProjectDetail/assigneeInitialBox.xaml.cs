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

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for assigneeInitialBox.xaml
    /// </summary>
    public partial class assigneeInitialBox : UserControl
    {
        public static readonly DependencyProperty AssigneeProfileBackProperty = DependencyProperty.Register("AssigneeProfileBack", typeof(Color), typeof(assigneeInitialBox));
        public Color AssigneeProfileBack
        {
            get { return (Color)GetValue(AssigneeProfileBackProperty); }
            set { SetValue(AssigneeProfileBackProperty, value); }
        }
        public assigneeInitialBox(Color c)
        {
            this.AssigneeProfileBack = c;
            InitializeComponent();
        }
    }
}
