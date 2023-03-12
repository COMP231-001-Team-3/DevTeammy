using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace teammy.UserControls
{
    /// <summary>
    /// Interaction logic for CardBox.xaml
    /// </summary>
    public partial class CardBox : UserControl
    {

        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register("Details", typeof(CardDetails), typeof(CardBox));

        public CardDetails Details
        {
            get { return (CardDetails)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        } 

        public event RoutedEventHandler CardClick
        {
            add { btnDetails.Click += value; }
            remove { btnDetails.Click -= value; }
        }
        public event RoutedEventHandler CardSelected
        {
            add { chkSelector.Checked += value; }
            remove { chkSelector.Checked -= value; }
        }
        public event RoutedEventHandler CardUnselected
        {
            add { chkSelector.Unchecked += value; }
            remove { chkSelector.Unchecked -= value; }
        }
        public event KeyEventHandler CardKeyUp
        {
            add { txtInput.KeyUp += value; }
            remove { txtInput.KeyUp -= value; }
        }

        #region Constructor
        public CardBox()
        {
            InitializeComponent();
        }
        #endregion

        #region Placeholder management
        /// <summary>
        ///     Resets TextBox text to placeholder text when empty
        /// </summary>
        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Equals(""))
            {
                txtInput.Text = "Enter Name";
                txtInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        /// <summary>
        ///     Removes placeholder text
        /// </summary>
        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Equals("Enter Name"))
            {
                txtInput.Text = "";
                txtInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        
        #endregion
    }
}
