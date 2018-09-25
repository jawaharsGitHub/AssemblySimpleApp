using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmPetrol : UserControl
    {
        public frmPetrol()
        {
            InitializeComponent();
            LoadPetrolDetails();
        }

        private void LoadPetrolDetails()
        {
            var data = BaseClass.ReadFileAsObjects<Petrol>(Petrol.JsonFilePath).OrderBy(o => o.Date).ToList(); ;
            dgvPetrol.DataSource = data;

            var firstDate = data.First(f => f.Speedometer > 0);
            var lastDate = data.Last(f => f.Speedometer > 0);

            var totalRunKm = lastDate.Speedometer - firstDate.Speedometer;

            var runningDays = (lastDate.Date - firstDate.Date).TotalDays + 1;

            lblAvgKm.Text = $"Average Km PerDay : {(totalRunKm/runningDays).ToInt32()} between {firstDate.Date.ToShortDateString()} and {lastDate.Date.ToShortDateString()} {Environment.NewLine}" +
                $"Running Kms : {totalRunKm} RunningDays : {runningDays}";


        }
    }
}
