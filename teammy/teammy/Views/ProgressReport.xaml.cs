using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Threading.Tasks;
using teammy.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace teammy
{
    /// <summary>
    /// Interaction logic for ProgressReport.xaml
    /// </summary>
    public partial class ProgressReport : Window
    {

        #region Constructor
        public ProgressReport()
        {
            InitializeComponent();
        }
        #endregion
    }
}