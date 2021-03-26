using MySql.Data.MySqlClient;
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
using teammy.ProjectDetail;


namespace teammy
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : UserControl
    {
        private List<TaskBox> tasks = new List<TaskBox>();

        private string connectionString = @"server=db-mysql-tor1-21887-do-user-8838717-0.b.db.ondigitalocean.com; database=teammy; uid=admin; pwd=sxx0uix39f5ty52d; port=25060;";
        public NewCategory currentCat { get; set; } = Application.Current.Resources["currentCat"] as NewCategory;
        int totalBoxes = 0;
        TaskBox toBeInserted;
        MySqlConnection conn;       

        public NewCategory()
        {
            InitializeComponent();
            LoadTask();
        }

        public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(NewCategory));
        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public event EventHandler<EventArgs> txtCategroyNameChanged;
        private void txtCategoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            txtCategroyNameChanged?.Invoke(this, EventArgs.Empty);

            //conn = new MySqlConnection(connectionString);
            //conn.Open();
            //MySqlCommand insertCatName = new MySqlCommand("Insert INTO categories VALUES(category_id, @category_name, (SELECT Proj_ID FROM projects WHERE Proj_Name = @projName));", conn);
            //MySqlCommand commit = new MySqlCommand("COMMIT;", conn);
            //insertCatName.Parameters.AddWithValue("category_name", txtCategoryName.Text);
            //insertCatName.Parameters.AddWithValue("projName", projBoard.ProjNameLable.Content.ToString());

            //insertCatName.ExecuteNonQuery();
            //commit.ExecuteNonQuery();
        }
        private void addTask(object sender, RoutedEventArgs e)
        {
            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for task per category is 9!", "Max tasks completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            toBeInserted = new TaskBox();
            taStackPanel.Children.Add(toBeInserted);
        }

        private void btnCloseC_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void LoadTask()
        {
            //ProjBoard catNameB = new ProjBoard();

            taStackPanel.Children.Clear();

            string taskName;
            string taskPrio;
            string taskProgre;
            DateTime taskDate;
            int taskAssigneeNum;
            List<string> userName = new List<string>();

            conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand getTasks = new MySqlCommand("SELECT task_name, priority, due_date, progress_code, assigned_group FROM tasks NATURAL JOIN categories where category_name  = @catName", conn);
            getTasks.Parameters.AddWithValue("catName", Application.Current.Resources["catName"] as string);
            
            MySqlDataReader tasksReader = getTasks.ExecuteReader();

            using (tasksReader)
            {
                //TaskBox taskBox = new TaskBox();
                
                //Random rd = new Random();
                
                while (tasksReader.Read())
                {
                    totalBoxes++;
                    taskName = tasksReader[0].ToString();
                    taskPrio = tasksReader[1].ToString();
                    taskProgre = tasksReader[3].ToString();
                    taskDate = DateTime.Parse(tasksReader[2].ToString());
                    taskAssigneeNum = (int)tasksReader[4];
                    tasks.Add(new TaskBox { TaskName= taskName , TaskPriority = taskPrio, TaskProgress = taskProgre, TaskDueDate = taskDate, TaskAssignee = taskAssigneeNum});                    
                }
            }

            for (int signee = 0; signee < tasks.Count; signee++)
            {

                //TaskBox assigneeGroup = tasks.Find((assignee) => assignee.TaskAssignee.Equals(tasks[signee]));

                //MySqlCommand getAssingees = new MySqlCommand("SELECT user_name FROM Users NATURAL JOIN Team_Mates Natural JOIN Assignees Natural JOIN Tasks where assigned_group = @assigned_group", conn);
                //getAssingees.Parameters.AddWithValue("assigned_group", assigneeGroup);
                //MySqlDataReader assingeeReader = getAssingees.ExecuteReader();
                //using (assingeeReader)
                //{
                //    while (assingeeReader.Read())
                //    {
                //        userName.Add((string)assingeeReader["user_name"]);
                //    }
                //}

                //taskBox = new TaskBox()
                //{
                //    TaskName = taskName,
                //    Margin = new Thickness(left, top, right, bottom),
                //    TaskPriority = taskPrio,
                //    TaskProgress = taskProgre,
                //    TaskDueDate = taskDate,
                //    AssigneeList = userName
                //};
                //taStackPanel.Children.Add(taskBox);
            }
            
        }
    }
}
