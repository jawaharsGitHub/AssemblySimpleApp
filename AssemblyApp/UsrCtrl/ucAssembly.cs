using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucAssembly : UserControl
    {
        List<DistrictAssembly> assemblies;
        public ZonalDistrict _selectedDistrict;

        public ucAssembly(ZonalDistrict selectedDistrict = null)
        {
            InitializeComponent();

            _selectedDistrict = selectedDistrict;

            assemblies = (from a in Assembly.GetAll()
                         join d in DataAccess.PrimaryTypes.District.GetAll()
                         on a.DistrictId equals d.DistrictId
                          select new DataAccess.ExtendedTypes.DistrictAssembly
                         {
                             DistrictId = d.DistrictId,
                             DistrictName = d.Name,
                             AssemblyNo = a.AssemblyNo,
                             AssemblyName = a.AssemblyName,
                             Electors = a.Electors,
                             Category = a.Category
                         }).ToList();

            dgvAssembly.DataSource = assemblies;
            LoadFilter();
            LoadDistrict();
            FormatGrid();

        }

        private void FormatGrid()
        {
            dgvAssembly.Columns["DistrictId"].Visible = false;
            //dataGridView1.Columns["ReturnDay"].DisplayIndex = 4;
            //dataGridView1.Columns["ReturnType"].DisplayIndex = 5;
            //dataGridView1.Columns["CollectionSpotId"].DisplayIndex = 6;

            //dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            //dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            //dataGridView1.Columns["Name"].Width = 250;
        }

        private void LoadDistrict()
        {
            var district = DataAccess.PrimaryTypes.District.GetAll();
            district.Add(new DataAccess.PrimaryTypes.District(0, "ALL"));


            cmbDistrict.DataSource = district.OrderBy(o => o.ZonalId).ToList();
            this.cmbDistrict.SelectedIndexChanged += new System.EventHandler(this.cmbDistrict_SelectedIndexChanged);
            cmbDistrict.SelectedValue = _selectedDistrict == null ? 0 : _selectedDistrict.DistrictId;
        }

        private void LoadFilter()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By AssemblyNo"),
                   new KeyValuePair<int, string>(2, "By Assembly Name"),
                   new KeyValuePair<int, string>(3, "By Category"),
                   new KeyValuePair<int, string>(4, "By Electors"),
                   new KeyValuePair<int, string>(5, "By District Name"),

               };

            cmbFilter.DataSource = myKeyValuePair;
        }

        private void cmbFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbFilter.SelectedItem).Key;
            List<DataAccess.ExtendedTypes.DistrictAssembly> filteredAssemblies = assemblies;

            if (value == 1) filteredAssemblies = assemblies.OrderBy(o => o.AssemblyNo).ToList();

            else if (value == 2) filteredAssemblies = assemblies.OrderBy(o => o.AssemblyName).ToList();

            else if (value == 3) filteredAssemblies = assemblies.OrderByDescending(o => o.Category).ToList();

            else if (value == 4) filteredAssemblies = assemblies.OrderByDescending(o => o.Electors).ToList();

            else if (value == 5) filteredAssemblies = assemblies.OrderBy(o => o.DistrictName).ToList();

            filteredAssemblies.ForEach(a =>
            {
                a.SNo = filteredAssemblies.IndexOf(a) + 1;

            });


            dgvAssembly.DataSource = filteredAssemblies;

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

            dgvAssembly.DataSource = assemblies.Where(w => w.AssemblyName.ToLower().Contains(textBox1.Text.ToLower())).ToList();

        }

        private void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDistrict.SelectedValue.ToInt32() > 0)
                dgvAssembly.DataSource = assemblies.Where(w => w.DistrictId == cmbDistrict.SelectedValue.ToInt32()).ToList();
            else
                dgvAssembly.DataSource = assemblies;

        }

        private void dgvAssembly_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as DistrictAssembly);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm<ucPanchayat>(selectedCustomer);
        }
    }
}
