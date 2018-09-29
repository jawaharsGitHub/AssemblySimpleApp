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

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucZonal : UserControl
    {
        List<Zonal> zonals;
       

        public ucZonal()
        {
            InitializeComponent();
            zonals = Zonal.GetAll();
            dgvZonal.DataSource = zonals;
            LoadFilter();
        }

        private void LoadFilter()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By ZonalId"),
                   new KeyValuePair<int, string>(2, "By Name"),
                   new KeyValuePair<int, string>(3, "By Code"),
                   new KeyValuePair<int, string>(4, "By Headquarters"),
                   new KeyValuePair<int, string>(5, "By Established"),
                   new KeyValuePair<int, string>(6, "By FormedFrom"),
                   new KeyValuePair<int, string>(7, "By AreakmSquare"),
                   new KeyValuePair<int, string>(8, "By Population"),
                   new KeyValuePair<int, string>(9, "By PopulationDensity"),
                   new KeyValuePair<int, string>(10, "By Taluk")

               };

            cmbFilter.DataSource = myKeyValuePair;
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbFilter.SelectedItem).Key;
            List<Zonal> filteredZonals = zonals;

            if (value == 1) filteredZonals = zonals.OrderBy(o => o.ZonalId).ToList(); 

            else if (value == 2) filteredZonals = zonals.OrderBy(o => o.Name).ToList();

            else if(value == 3) filteredZonals = zonals.OrderBy(o => o.Code).ToList();

            else if (value == 4) filteredZonals = zonals.OrderBy(o => o.Headquarters).ToList();

            else if (value == 5) filteredZonals = zonals.OrderByDescending(o => o.Established).ToList();

            else if (value == 6) filteredZonals = zonals.OrderBy(o => o.FormedFrom).ToList();

            else if (value == 7) filteredZonals = zonals.OrderByDescending(o => o.AreakmSquare).ToList();

            else if (value == 8) filteredZonals = zonals.OrderByDescending(o => o.Population).ToList();

            else if (value == 9) filteredZonals = zonals.OrderByDescending(o => o.PopulationDensity).ToList();

            else if (value == 10) filteredZonals = zonals.OrderByDescending(o => o.Taluk).ToList(); 



            dgvZonal.DataSource = filteredZonals;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dgvZonal.DataSource = zonals.Where(w => w.Name.ToLower().Contains(textBox1.Text.ToLower().Trim())).ToList();

        }

        private void dgvZonal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as Zonal);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm<ucDistrict>(selectedCustomer);
        }
    }
}
