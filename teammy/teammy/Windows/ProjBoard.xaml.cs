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
using MySql.Data.MySqlClient;
using System.Windows.Shapes;

namespace teammy
{

    /// <summary>
    /// Interaction logic for ProjBoard.xaml
    /// </summary>
    public partial class ProjBoard : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        
        int left, top, right, bottom;
        int totalCats = 0;

        public string projName { get; set; }

        private teammyEntities dbContext = new teammyEntities();

        public ProjBoard()
        {
            InitializeComponent();
        }

        public void LoadCategories()
        {
            lblProjName.Content = projName;
            left = 0;
            top = 0;
            right = 5;
            bottom = 0;
            totalCats = 0;
            caStackPanel.Children.Clear();

            List<category> projCategories = (from project in dbContext.projects
                                            where project.Proj_Name.Equals(projName)
                                            select project.categories).Single().ToList();

            string catName;
            ProjCategory toBeAdded = null;
            for (int i = 0; i < projCategories.Count; i++)
            {
                totalCats++;
                catName = projCategories[i].category_name.ToString();

                toBeAdded = new ProjCategory() { CategoryName = catName, Margin = new Thickness(left, top, right, bottom) };

                caStackPanel.Children.Add(toBeAdded);
                toBeAdded.LoadTasks();
            }            
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     Minimizes the current window
        /// </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = globalItems["cmButton"] as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }      

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory();            
        }

        private void AddCategory()
        {
            if (++totalCats == 10)
            {
                totalCats--;
                MessageBox.Show("The maximum limit for categories per project is 9!", "Max categories completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            Application.Current.Resources["catName"] = null;
            ProjCategory newlyAdded = new ProjCategory();
            caStackPanel.Children.Add(newlyAdded);           
        }   
    }
}
