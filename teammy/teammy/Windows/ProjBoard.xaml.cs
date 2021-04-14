using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public ObservableCollection<ProjCategory> Categories
        {
            get { return (ObservableCollection<ProjCategory>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        public user currentUser { get; set; } = globalItems["currentUser"] as user;

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register("Categories", typeof(ObservableCollection<ProjCategory>), typeof(ProjBoard));

        public ProjBoard()
        {
            InitializeComponent();
            Categories = new ObservableCollection<ProjCategory>();
        }

        public void LoadCategories()
        {
            lblProjName.Content = projName;
            left = 0;
            top = 0;
            right = 3;
            bottom = 0;
            totalCats = 0;
            Categories.Clear();

            List<category> projCategories = (from project in dbContext.projects
                                            where project.Proj_Name.Equals(projName)
                                            select project.categories).Single().ToList();

            string catName;
            ProjCategory toBeAdded = null;
            for (int i = 0; i < projCategories.Count; i++)
            {
                totalCats++;
                catName = projCategories[i].category_name?.ToString();

                toBeAdded = new ProjCategory() 
                {
                    CategoryName = catName,
                    Margin = new Thickness(left, top, right, bottom), 
                    Project = (from project in dbContext.projects
                               where project.Proj_Name.Equals(projName)
                               select project).Single() 
                };

                Categories.Add(toBeAdded);
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
            ProjCategory newlyAdded = new ProjCategory() 
            {
                Project = (from project in dbContext.projects
                           where project.Proj_Name.Equals(projName)
                           select project).Single()
            };
            Categories.Add(newlyAdded);
            dbContext.categories.Add(new category()
            {
                project = (from project in dbContext.projects
                           where project.Proj_Name.Equals(projName)
                           select project).Single()
            });
            dbContext.SaveChanges();
        }   
    }
}
