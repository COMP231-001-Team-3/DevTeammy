using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using teammy.ViewModels;

namespace teammy.Commands
{
    public class LoadProjectsCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private CreateProjViewModel CpModel;
        public LoadProjectsCmd(CreateProjViewModel cpModel)
        {
            CpModel = cpModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string currSelection = cmbTeams.SelectedItem?.ToString();

            if (teams.Count == 0)
            {
                BsonDocument[] pipeline = new BsonDocument[]
                {
                    new BsonDocument("$match",
                    new BsonDocument("members",
                    new BsonDocument("$elemMatch",
                    new BsonDocument("userId", 103))))
                };
                List<string> teams = dbContext.GetCollection<Team>("teams")
                    .Aggregate<Team>(pipeline)
                    .ToList()
                    .Select(team => team.teamName)
                    .ToList();

                for (int i = 0; i < teams.Count; i++)
                {
                    projectBrds.Add(new List<ProjBoard>());
                }

                cmbTeams.SelectedIndex = 0;
                currSelection = cmbTeams.SelectedItem.ToString();
            }
            else if ((bool)prevSelection?.Equals(currSelection) && sender.Equals(""))
            {
                return;
            }
            else if (sender.Equals("Reset"))
            {
                projectBrds[cmbTeams.SelectedIndex].Clear();
            }

            left = 0;
            right = 361;
            top = 0;
            bottom = 260;
            boxCount = 0;
            totalBoxes = 0;
            projGrid.Children.Clear();

            BsonDocument[] pipeProjNames = new BsonDocument[]
            {
                new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "teams" },
                        { "localField", "teamId" },
                        { "foreignField", "teamId" },
                        { "as", "team" }
                    }),
                new BsonDocument("$match",
                new BsonDocument("team",
                new BsonDocument("$elemMatch",
                new BsonDocument("teamName", "DevTeam"))))
            };

            List<string> projNames = dbContext.GetCollection<Project>("projects")
                .Aggregate<BsonDocument>(pipeProjNames)
                .ToList()
                .Select(proj => proj.GetValue("name", null).AsString)
                .ToList();
            //List<string> projNames = (from proj in dbContext.projects
            //                          join team in dbContext.teams 
            //                            on proj.Team_ID equals team.Team_ID
            //                          where team.Team_Name.Equals(currSelection)
            //                         select proj.Proj_Name).ToList();

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
                project = new CardBox()
                {
                    Name = "crdBox" + i,
                    FullName = projName,
                    Margin = new Thickness(left, top, right, bottom),
                    ProfileBack = backColors[rd.Next(0, 18)]
                };

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
    }
}
