using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace teammy.ProjectDetail
{
    /// <summary>
    /// Interaction logic for TaskBox.xaml
    /// </summary>
    public partial class TaskBox : UserControl
    {
        public TaskBox()
        {
            
            this.DataContext = this;
            StatusImages = new ObservableCollection<PicClass>();
            PriorityImages = new ObservableCollection<PicClass>();
            AssigneeImages = new ObservableCollection<PicClass>();

            InitializeComponent();

            //StatusImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\notStarted.png")),
            //    //ImgName = "Not started"

            //});
            //StatusImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\underProgress.png")),
            //    //ImgName = "Under Progress"

            //});
            //StatusImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\done.png")),
            //    //ImgName = "Done"

            //});

            //PriorityImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority 1.png")),
            //    //ImgName = "High"

            //});
            //PriorityImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority2.png")),
            //    //ImgName = "Midium"

            //});
            //PriorityImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority3.png")),
            //    //ImgName = "Low"

            //});

            //AssigneeImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority 1.png")),
            //    ImgName = "assignee1"

            //});
            //AssigneeImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority2.png")),
            //    ImgName = "assignee2"

            //});
            //AssigneeImages.Add(new PicClass
            //{
            //    Img = new BitmapImage(new Uri(@"C:\Users\user\Desktop\Programming 3\comp231-001_team3\teammy\teammy\images\priority3.png")),
            //    ImgName = "assignee3"

            //});
        }

        public ObservableCollection<PicClass> StatusImages { get; set; }
        public ObservableCollection<PicClass> PriorityImages { get; set; }
        public ObservableCollection<PicClass> AssigneeImages { get; set; }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)         //when you click duedate button
        {
            

        }

        private void assigneeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = FindResource("cmPriority") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Brush chosenPriority = ((sender as MenuItem).Icon as Rectangle).Fill;
            priorityGrid.Background = chosenPriority;
        }
    }


    public class PicClass
    {

        public BitmapImage Img { get; set; }
        public string ImgName { get; set; }


    }
}

