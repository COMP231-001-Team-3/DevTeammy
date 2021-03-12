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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ProjectBox : UserControl
    {
        public static readonly DependencyProperty ProjectNameProperty = DependencyProperty.Register("ProjectName", typeof(string), typeof(ProjectBox));
        public static readonly DependencyProperty ProjectProfileBackProperty = DependencyProperty.Register("ProjectProfileBack", typeof(Color), typeof(ProjectBox));
        public static readonly DependencyProperty ProjectProfileProperty = DependencyProperty.Register("ProjectProfile", typeof(string), typeof(ProjectBox));

        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set { SetValue(ProjectNameProperty, value); }
        }
        public string ProjectProfile
        {
            get { return (string)GetValue(ProjectProfileProperty); }
            set { SetValue(ProjectProfileProperty, value); }
        }
        public Color ProjectProfileBack
        {
            get { return (Color)GetValue(ProjectProfileBackProperty); }
            set { SetValue(ProjectProfileBackProperty, value); }
        }
        
        public event EventHandler<EventArgs> ProjectNameChanged;
        public event EventHandler<EventArgs> ProjectProfileChanged;
        public ProjectBox()
        {
            InitializeComponent();
        }

        private void txtProjName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            ProjectNameChanged?.Invoke(this, EventArgs.Empty);
        }

        private void txtProfText_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            ProjectProfileChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
