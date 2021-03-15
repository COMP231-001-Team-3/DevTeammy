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
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : UserControl
    {
        public NewCategory()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(NewCategory));
        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public event EventHandler<EventArgs> txtCategroyNameChanged;
        private void txtCategroyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            txtCategroyNameChanged?.Invoke(this, EventArgs.Empty);
        }
        private void addTask(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCloseC_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
