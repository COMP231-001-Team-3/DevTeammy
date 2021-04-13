using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace teammy
{
    /// <summary>
    ///     Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
        #region Fields
        private static ResourceDictionary globalItems = Application.Current.Resources;

        //Colors for project cards
        private Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        private teammyEntities dbContext = new teammyEntities();

        //Margins indicate position of each box to be placed
        private int left, top, right, bottom;
        private int boxCount = 0;
        private int totalBoxes = 0;
        private CardBox toBeInserted;

        private TextBox txtNameInput;
        private string prevSelection;
        private List<ProjBoard> projectBrds = new List<ProjBoard>();
        #endregion

        #region Properties
        public user currentUser { get; set; } = globalItems["currentUser"] as user;
        #endregion

        #region Constructor
        public CreateProject()
        {
            InitializeComponent();
            LoadProjects();
            cmbTeams.DropDownClosed += new EventHandler(cmbTeams_DropDownClosed);
        }
        #endregion

        #region Miscellaneous
        /// <summary>
        ///     Loads projects from the database
        /// </summary>
        public void LoadProjects(string sender = "")
        {
            string currSelection = cmbTeams.SelectedItem?.ToString();
            
            if (cmbTeams.Items.Count == 0)
            {
                List<string> teamNames = (from team in dbContext.teams
                                          join mate in dbContext.team_mates
                                            on team.Team_ID equals mate.Team_ID
                                          join currUser in dbContext.users
                                            on mate.user_id equals currUser.user_id
                                          where currUser.user_name.Equals(currentUser.user_name)
                                          select team.Team_Name).ToList();
                cmbTeams.ItemsSource = teamNames;
                cmbTeams.SelectedIndex = 0;
                currSelection = cmbTeams.SelectedItem.ToString();
            }
            else if ((bool)prevSelection?.Equals(currSelection) && sender.Equals(""))
            {
                return;
            }

            left = 0;
            right = 361;
            top = 0;
            bottom = 260;
            boxCount = 0;
            totalBoxes = 0;
            projGrid.Children.Clear();                    

            List<string> projNames = (from proj in dbContext.projects
                                      join team in dbContext.teams 
                                        on proj.Team_ID equals team.Team_ID
                                      where team.Team_Name.Equals(currSelection)
                                     select proj.Proj_Name).ToList();

            CardBox project;
            //Variables for usage in loop declared beforehand for performance reasons
            Random rd = new Random();
            string projName;

            //Loop to read through results from query
            for (int i = 0; i < projNames.Count; i++)
            {
                totalBoxes++;
                projName = projNames[i].ToString();

                //Creation & Initialization of ProjectBox
                project = new CardBox() { Name = "crdBox" + i, FullName = projName, Margin = new Thickness(left, top, right, bottom), ProfileBack = backColors[rd.Next(0, 18)] };

                project.CardClick += new RoutedEventHandler(project_CardClick);

                project_CardClick(project, null);

                //Adds the newly created ProjectBox to the Grid within the ScrollViewer
                projGrid.Children.Add(project);

                //Updates margin for the next box
                left += 175;
                right -= 175;

                //If 3 boxes have been created...then
                if (boxCount == 2)
                {
                    //Margin updates for a new ProjectBox in a new row
                    top += 132;
                    bottom -= 132;
                    left = 0;
                    right = 361;
                    boxCount = 0;
                }
                else
                {
                    boxCount++;
                }
            }
            prevSelection = currSelection;
        }

        private void project_CardClick(object sender, RoutedEventArgs e)
        {
            CardBox current;
            if (e == null)
            {
                current = sender as CardBox;
                ProjBoard projDetails = new ProjBoard() { projName = current.FullName };
                projDetails.LoadCategories();
                projectBrds.Add(projDetails);
            }
            else
            {
                current = ((sender as Button).Parent as Grid).Parent as CardBox;
                int indexOfDetails = projGrid.Children.IndexOf(current);

                string item;
                for (int i = 0; i < cmbTeams.SelectedIndex; i++)
                {
                    item = cmbTeams.Items[i].ToString();
                    indexOfDetails += (from team in dbContext.teams
                                      where item.Equals(team.Team_Name)
                                      select team.projects.Count).Single();
                }

                projectBrds[indexOfDetails].Show();
                Hide();
            }
        }

        /// <summary>
        ///     Inserts project into database when user presses 'Enter' and
        ///     input TextBox has focus
        /// </summary>
        private void txtNameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                toBeInserted.FullName = txtNameInput.Text;
                txtNameInput.Visibility = Visibility.Hidden;

                List<project> existent = (from project in dbContext.projects
                                          where project.Proj_Name.Equals(txtNameInput.Text)
                                          select project).ToList();

                if (existent.Count == 0 && !txtNameInput.Equals(""))
                {
                    dbContext.projects.Add(new project()
                    {
                        Proj_Name = txtNameInput.Text,
                        Team_ID = (from team in dbContext.teams
                                   where team.Team_Name.Equals(cmbTeams.SelectedItem.ToString())
                                   select team.Team_ID).Single()
                    });

                    dbContext.SaveChanges();
                    btnDone.Visibility = Visibility.Hidden;
                    btnCancel.Visibility = Visibility.Hidden;
                    btnCreateProj.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                    project_CardClick(toBeInserted, null);
                    toBeInserted = null;
                    return;
                }                
                MessageBox.Show("The project name that you have entered has already been used! Please try again!", "Duplicate Project Name", MessageBoxButton.OK, MessageBoxImage.Error);
                btnCancel_Click(new object(), new RoutedEventArgs());
                toBeInserted = null;
            }
        }
        #endregion

        #region Title Bar Button Event Handlers

        /// <summary>
        ///     Shuts down the application
        /// </summary>
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
        #endregion

        #region Title Pane Event Handlers

        /// <summary>
        ///     Moves the window along with the title pane when it is dragged
        /// </summary>
        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }        
        #endregion
        
        #region Button Event Handlers

        /// <summary>
        ///     Creates the project box and the textbox for input
        /// </summary>
        private void btnCreateProj_Click(object sender, RoutedEventArgs e)
        {
            btnCreateProj.Visibility = Visibility.Hidden;
            btnDone.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;

            if (++totalBoxes == 10)
            {
                totalBoxes--;
                MessageBox.Show("The maximum limit for projects per team is 9!", "Max projects completed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Random rd = new Random();
            toBeInserted = new CardBox() { ProfileBack = backColors[rd.Next(0, backColors.Length - 1)], Margin = new Thickness(left, top, right, bottom) };
            toBeInserted.CardClick += new RoutedEventHandler(project_CardClick);            

            txtNameInput = new TextBox() { Height = 25, Width = 120, FontSize = 16, Margin = new Thickness(left, top + 93, right, bottom - 3) };

            if (boxCount == 2)
            {
                //Margin updates for a new ProjectBox in a new row
                top += 132;
                bottom -= 132;
                left = 0;
                right = 361;
                boxCount = 0;
            }
            else
            {
                //Updates margin for the next box
                left += 175;
                right -= 175;
                boxCount++;
            }

            projGrid.Children.Add(toBeInserted);
            projGrid.Children.Add(txtNameInput);
            txtNameInput.Focus();
            txtNameInput.GotFocus += new RoutedEventHandler(txtNameInput_GotFocus);
            txtNameInput.LostFocus += new RoutedEventHandler(txtNameInput_LostFocus);
            txtNameInput.KeyUp += new KeyEventHandler(txtNameInput_KeyUp);

            btnCreateProj.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Inserts the project into the DB.
        /// </summary>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if(toBeInserted != null)
            {
                toBeInserted.FullName = txtNameInput.Text;
                txtNameInput.Visibility = Visibility.Hidden;
                List<project> existent = (from project in dbContext.projects
                                          where project.Proj_Name.Equals(txtNameInput.Text)
                                          select project).ToList();

                if (existent.Count == 0 && !txtNameInput.Text.Equals("Enter Name"))
                {
                    dbContext.projects.Add(new project()
                    {
                        Proj_Name = txtNameInput.Text,
                        Team_ID = (from team in dbContext.teams
                                   where team.Team_Name.Equals(cmbTeams.SelectedItem.ToString())
                                   select team.Team_ID).Single()
                    });

                    dbContext.SaveChanges();
                    toBeInserted = null;
                    btnDone.Visibility = Visibility.Hidden;
                    btnCancel.Visibility = Visibility.Hidden;
                    btnCreateProj.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                    return;
                }
                
                MessageBox.Show("The project name that you have entered has already been used! Please try again!", "Duplicate Project Name", MessageBoxButton.OK, MessageBoxImage.Error);
                btnCancel_Click(new object(), new RoutedEventArgs());
                toBeInserted = null;
                return;
            }
            else
            {
                CardBox crdBox;
                for (int i = 0; i < projGrid.Children.Count; i++)
                {
                    if(projGrid.Children[i].GetType() == typeof(CardBox))
                    {
                        crdBox = projGrid.Children[i] as CardBox;
                        if(crdBox.Selected)
                        {
                            project selected = (from proj in dbContext.projects
                                               where proj.Proj_Name.Equals(crdBox.FullName)
                                               select proj).Single();
                            dbContext.tasks.RemoveRange(selected.tasks);
                            dbContext.categories.RemoveRange(selected.categories);
                            dbContext.projects.Remove(selected);

                            dbContext.SaveChanges();                        
                        }
                    }
                }
                LoadProjects("Reset");
            }

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateProj.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Resets window to the state before pressing create project
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(toBeInserted != null)
            {
                projGrid.Children.Remove(toBeInserted);
                projGrid.Children.Remove(txtNameInput);

                if (boxCount == 0)
                {
                    boxCount = 2;
                    left = 350;
                    right = 11;
                    top -= 132;
                    bottom += 132;
                }
                else
                {
                    left -= 175;
                    right += 175;
                    --boxCount;
                }
                --totalBoxes;
            }
            else
            {
                UIElement currElement;
                for (int i = 0; i < projGrid.Children.Count; i++)
                {
                    currElement = projGrid.Children[i];
                    if (currElement.GetType() == typeof(CardBox))
                    {
                        CardBox crdProj = currElement as CardBox;
                        crdProj.Selected = false;
                        crdProj.SelectorVisible = false;
                    }
                }
            }

            btnDone.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnCreateProj.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hover effect (Icon background) for the cancel button
        /// </summary>
        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            cancelbtnIcon.Background = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.7 };
        }

        /// <summary>
        ///     Hover effect (Icon background) for the cancel button
        /// </summary>
        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            cancelbtnIcon.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            UIElement currElement;
            for (int i = 0; i < projGrid.Children.Count; i++)
            {
                currElement = projGrid.Children[i];
                if (currElement.GetType() == typeof(CardBox))
                {
                    CardBox crdBox = currElement as CardBox;
                    crdBox.SelectorVisible = true;
                }
            }
            btnCreateProj.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Visible;
            btnDone.Visibility = Visibility.Visible;
        }
        #endregion

        #region ComboBox Event Handlers

        /// <summary>
        ///     Reloads projects based on team selected
        /// </summary>
        private void cmbTeams_DropDownClosed(object sender, EventArgs e)
        {
            LoadProjects();
        }
        #endregion

        #region Placeholder management

        /// <summary>
        ///     Resets TextBox text to placeholder text when empty
        /// </summary>
        private void txtNameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Equals(""))
            {
                txtNameInput.Text = "Enter Name";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        /// <summary>
        ///     Removes placeholder text
        /// </summary>
        private void txtNameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Equals("Enter Name"))
            {
                txtNameInput.Text = "";
                txtNameInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        #endregion
    }
}