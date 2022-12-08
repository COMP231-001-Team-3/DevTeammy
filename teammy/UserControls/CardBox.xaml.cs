using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for CardBox.xaml
    /// </summary>
    public partial class CardBox : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty FullNameProperty = DependencyProperty.Register("FullName", typeof(string), typeof(CardBox));
        public static readonly DependencyProperty ProfileBackProperty = DependencyProperty.Register("ProfileBack", typeof(Color), typeof(CardBox));
        public static readonly DependencyProperty ProfileProperty = DependencyProperty.Register("Profile", typeof(string), typeof(CardBox));
        public static readonly DependencyProperty SelectorVisibleProperty = DependencyProperty.Register("SelectorVisible", typeof(bool), typeof(CardBox));
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(CardBox));
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
        public bool SelectorVisible
        {
            get { return (bool)GetValue(SelectorVisibleProperty); }
            set { SetValue(SelectorVisibleProperty, value); }
        }
        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
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
