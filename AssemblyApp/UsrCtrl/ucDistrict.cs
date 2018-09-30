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
using DataAccess.ExtendedTypes;
using Common.ExtensionMethod;
using Common;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucDistrict : UserControl
    {

        List<ZonalDistrict> districts;
        public Zonal _selectedZonal;

        public ucDistrict(Zonal selectedZonal = null)
        {
            _selectedZonal = selectedZonal;

            InitializeComponent();



            districts = (from d in DataAccess.PrimaryTypes.District.GetAll()
                         join z in Zonal.GetAll()
                         on d.ZonalId equals z.ZonalId
                         select new ZonalDistrict
                         {
                             ZonalId = z.ZonalId,
                             ZonalName = z.Name,
                             DistrictId = d.DistrictId,
                             DistrictName = d.Name,
                             Code = d.Code,
                             Blocks = d.Blocks
                         }).ToList();

            dataGridView1.DataSource = districts;
            FormatGrid();

            LoadZonal();
        }

        private void FormatGrid()
        {
            dataGridView1.Columns["ZonalId"].Visible = false;
            dataGridView1.Columns["DistrictId"].Visible = false;
            //dataGridView1.Columns["ReturnDay"].DisplayIndex = 4;
            //dataGridView1.Columns["ReturnType"].DisplayIndex = 5;
            //dataGridView1.Columns["CollectionSpotId"].DisplayIndex = 6;

            //dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            //dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            //dataGridView1.Columns["Name"].Width = 250;
        }

        private void LoadZonal()
        {
            var zonal = Zonal.GetAll();
            zonal.Add(new Zonal(0, "ALL"));

            cmbZonal.DisplayMember = "Name";
            cmbZonal.ValueMember = "ZonalId";
            cmbZonal.DataSource = zonal.OrderBy(o => o.ZonalId).ToList();
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            cmbZonal.SelectedValue = _selectedZonal == null ? 0 : _selectedZonal.ZonalId;

            
        }

        private void cmbZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZonal.SelectedValue.ToInt32() > 0)
                dataGridView1.DataSource = districts.Where(w => w.ZonalId == cmbZonal.SelectedValue.ToInt32()).ToList();
            else
                dataGridView1.DataSource = districts;



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = districts.Where(w => w.DistrictName.ToLower().Contains(textBox1.Text.ToLower().Trim())).ToList();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as ZonalDistrict);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm<ucAssembly>(selectedCustomer);
        }
    }
}
