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
        public static readonly DependencyProperty ProjectImageProperty = DependencyProperty.Register("ProjectImage", typeof(ImageSource), typeof(ProjectBox));

        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set { SetValue(ProjectNameProperty, value); }
        }
        public ImageSource ProjectImage
        {
            get { return (ImageSource)GetValue(ProjectImageProperty); }
            set { SetValue(ProjectImageProperty, value); }
        }

        public event EventHandler<EventArgs> ProjectNameChanged;
        public event EventHandler<EventArgs> ProjectImageChanged;
        public ProjectBox()
        {
            InitializeComponent();
        }

        private void imgProjProfile_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            //e.Handled = true;
            ProjectImageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void txtProjName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //e.Handled = true;
            ProjectNameChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
