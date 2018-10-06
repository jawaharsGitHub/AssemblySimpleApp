using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess.PrimaryTypes;
using Common.ExtensionMethod;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucPollingStation : UserControl
    {
        List<PollingStation> pollingStations;

        public ucPollingStation()
        {
            InitializeComponent();

            pollingStations = PollingStation.GetAll();

            dataGridView1.DataSource = pollingStations;

            cmbAssembly.DataSource = Assembly.GetAll();

        }

        private void cmbAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ps = pollingStations.Where(w => w.AssemblyNo == cmbAssembly.SelectedValue.ToInt32()).ToList();

            dataGridView1.DataSource = ps;


            label1.Text = $"{ps.Count} Polling Stations";
        }
    }
}
