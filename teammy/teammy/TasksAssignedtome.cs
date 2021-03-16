using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy
{
    class TasksAssignedtome
    {
        string Progress;
        public string taskname
        {
            set;
            get;
        }
        public string duedate
        {
            set;
            get;
          
        }
        public string progress
        {
            set
            {
                if (value == "NS")
                {Progress= "images/notstarted.png"; }
                else if (value == "CO")
                { Progress = "images/checkmark.png"; }
                else
                { Progress = "images/Progress.png"; }
            }
            get {return Progress; }
        }
        


    }
}
