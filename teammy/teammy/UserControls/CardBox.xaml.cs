using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for ProjectBox.xaml
    /// </summary>
    public partial class CardBox : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty FullNameProperty = DependencyProperty.Register("FullName", typeof(string), typeof(CardBox));
        public static readonly DependencyProperty ProfileBackProperty = DependencyProperty.Register("ProfileBack", typeof(Color), typeof(CardBox));
        public static readonly DependencyProperty ProfileProperty = DependencyProperty.Register("Profile", typeof(string), typeof(CardBox));
        #endregion

        #region Public Properties of the Dependency properties
        public string FullName
        {
            get { return (string)GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }
        public string Profile
        {
            get { return (string)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }
        public Color ProfileBack
        {
            get { return (Color)GetValue(ProfileBackProperty); }
            set { SetValue(ProfileBackProperty, value); }
        }
        #endregion

        //Events for change on the text properties
        public event EventHandler<EventArgs> FullNameChanged;
        public event EventHandler<EventArgs> ProfileChanged;

        public event RoutedEventHandler CardClick
        {
            add { btnDetails.Click += value; }
            remove { btnDetails.Click -= value; }
        }

        #region Constructor
        public CardBox()
        {
            InitializeComponent();
        }
        #endregion

        #region Text Changed Event Handlers
        /// <summary>
        ///     Invokes any events related to the ProjectNameProperty when
        ///     Project Name Textbox's Text changes.
        /// </summary>
        private void txtFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            FullNameChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Invokes any events related to the ProjectProfileProperty when
        ///     Project Prof Textbox's Text changes.
        /// </summary>
        private void txtProfText_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            ProfileChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
