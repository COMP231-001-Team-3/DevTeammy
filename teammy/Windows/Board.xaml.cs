using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teammy.Models;
using teammy.UserControls;

namespace teammy
{

    /// <summary>
    /// Interaction logic for ProjBoard.xaml
    /// </summary>
    public partial class Board : Window
    {
        private static ResourceDictionary globalItems = Application.Current.Resources;
        
        int left, top, right, bottom;
        int totalCats = 0;

        public string projName { get; set; }
        public int projectId { get; set; }

        private static IMongoDatabase dbContext = DBConnector.Connect();

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register("Categories", typeof(ObservableCollection<Category>), typeof(Board));

        public ObservableCollection<Category> Categories
        {
            get { return (ObservableCollection<Category>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        public User currentUser { get; set; } = globalItems["currentUser"] as User;


        public Board()
        {
            InitializeComponent();
            Categories = new ObservableCollection<Category>();
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
            Project proj = dbContext.GetCollection<Project>("projects")
                                          .Find(p => p.Name.Equals(projName))
                                          .Single();
            projectId = proj.ProjectId;
            string catName;
            Category toBeAdded = null;
            for (int i = 0; i < proj.Categories.Count; i++)
            {
                totalCats++;
                catName = proj.Categories[i];

                toBeAdded = new Category()
                {
                    CategoryName = catName,
                    Margin = new Thickness(left, top, right, bottom),
                    Project = proj
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
            if (++totalCats == 10)
            {
                totalCats--;
                MessageBox.Show("The maximum limit for categories per project is 9!", "Max categories completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Category newlyAdded = new Category()
            {
                Project = dbContext.GetCollection<Project>("projects")
                                      .Find(p => p.Name.Equals(projName))
                                      .Single()
            };
            Categories.Add(newlyAdded);

            string name = projName;
            AddCategory(name);
        }

        private async void AddCategory(string name)
        {
            await dbContext.GetCollection<Project>("projects")
                           .UpdateOneAsync(p => p.ProjectId == projectId,
                                           Builders<Project>.Update.Push(p => p.Categories, name));
        }
    }
}
