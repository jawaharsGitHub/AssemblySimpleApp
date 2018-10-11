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

            SetWrap("Location", "PollingArea");


        }

        private void cmbAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ps = pollingStations.Where(w => w.AssemblyNo == cmbAssembly.SelectedValue.ToInt32()).ToList();

            dataGridView1.DataSource = ps;


            label1.Text = $"{ps.Count} Polling Stations";
        }

        private void SetWrap(params string[] columnNames)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                // Console.Write(list[i] + " ");


                dataGridView1.Columns[columnNames[i]].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dataGridView1.Columns[columnNames[i]].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            }

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
    }
}
