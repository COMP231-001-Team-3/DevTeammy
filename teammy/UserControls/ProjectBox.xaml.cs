using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace teammy
{
    /// <summary>
    /// Interaction logic for ProjectBox.xaml
    /// </summary>
    public partial class ProjectBox : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty ProjectNameProperty = DependencyProperty.Register("ProjectName", typeof(string), typeof(ProjectBox));
        public static readonly DependencyProperty ProjectProfileBackProperty = DependencyProperty.Register("ProjectProfileBack", typeof(Color), typeof(ProjectBox));
        public static readonly DependencyProperty ProjectProfileProperty = DependencyProperty.Register("ProjectProfile", typeof(string), typeof(ProjectBox));
        #endregion

        #region Public Properties of the Dependency properties
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
        #endregion

        //Events for change on the text properties
        public event EventHandler<EventArgs> ProjectNameChanged;
        public event EventHandler<EventArgs> ProjectProfileChanged;

        #region Constructor
        public ProjectBox()
        {
            InitializeComponent();            
        }
        #endregion

        #region Text Changed Event Handlers
        /// <summary>
        ///     Invokes any events related to the ProjectNameProperty when
        ///     Project Name Textbox's Text changes.
        /// </summary>
        private void txtProjName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            ProjectNameChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Invokes any events related to the ProjectProfileProperty when
        ///     Project Prof Textbox's Text changes.
        /// </summary>
        private void txtProfText_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            ProjectProfileChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            ProjectBox projectBox = new ProjectBox() { ProjectName = txtProjName.Text , ProjectProfile = txtProfText.Text};

            Application.Current.Resources.Add("currentProj", projectBox);            
            (Application.Current.Resources["BoardCatInstance"] as Window).Show();
            
        }
    }
}
