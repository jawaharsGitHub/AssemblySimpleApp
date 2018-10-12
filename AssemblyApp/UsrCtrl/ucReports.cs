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
    public partial class ucReports : UserControl
    {
        public ucReports()
        {
            InitializeComponent();

            LoadAssembly();

        }

        private void LoadAssembly()
        {
            var district = Assembly.GetAll();
            district.Add(new Assembly(0, "ALL"));


            cmbAssembly.DataSource = district.OrderBy(o => o.AssemblyNo).ToList();
            this.cmbAssembly.SelectedIndexChanged += new System.EventHandler(this.cmbAssembly_SelectedIndexChanged);
            cmbAssembly.SelectedValue = 0; // _selectedAssembly == null ? 0 : _selectedAssembly.AssemblyNo;
        }

        private void cmbAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedAssemblyNo = cmbAssembly.SelectedValue.ToInt32();

            // Gets all data for that assembly.
            // panchayats, wards.

            var panchayats = Panchayat.GetAll().Where(w => w.AssemblyNo == selectedAssemblyNo).ToList();

            var pollingBooths = PollingStation.GetAll().Where(w => w.AssemblyNo == selectedAssemblyNo).ToList();

            List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

            data.Add(new KeyValuePair<string, int>("Panchayats",  panchayats.Count()));
            data.Add(new KeyValuePair<string, int>("polling Station", pollingBooths.Count()));

            dataGridView1.DataSource = data;

        }
    }
}
