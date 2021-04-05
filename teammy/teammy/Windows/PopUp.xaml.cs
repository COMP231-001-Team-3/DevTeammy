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
using System.Windows.Shapes;

namespace teammy
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public int date, year;
        public string month;

        public PopUp()
        {
            InitializeComponent();            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void popupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblDateHeader.Content = month + " " + date + ", " + year;
        }

        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }
    }
}
