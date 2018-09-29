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

            cmbZonal.DisplayMember = "Name";
            cmbZonal.ValueMember = "ZonalId";

            LoadZonal();

            districts = (from d in District.GetAll()
                         join z in Zonal.GetAll()
                         on d.ZonalId equals z.ZonalId
                         select new ZonalDistrict
                         {
                             ZonalId = z.ZonalId,
                             ZonalName = z.Name,
                             DistrictName = d.Name,
                             Code = d.Code,
                             Blocks = d.Blocks
                         }).ToList();



            dataGridView1.DataSource = districts;



        }

        //public ucDistrict()
        //{
        //    Init();
            
        //}


        private void Init()
        {
            InitializeComponent();

            cmbZonal.DisplayMember = "Name";
            cmbZonal.ValueMember = "ZonalId";

            LoadZonal();

            districts = (from d in District.GetAll()
                         join z in Zonal.GetAll()
                         on d.ZonalId equals z.ZonalId
                         select new ZonalDistrict
                         {
                             ZonalId = z.ZonalId,
                             ZonalName = z.Name,
                             DistrictName = d.Name,
                             Code = d.Code,
                             Blocks = d.Blocks
                         }).ToList();



            dataGridView1.DataSource = districts;
        }

        private void LoadZonal()
        {
            var zonal = Zonal.GetAll();
            cmbZonal.DataSource = zonal;
            cmbZonal.SelectedValue = _selectedZonal != null ? _selectedZonal.ZonalId : 0;
        }

        private void cmbZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (districts != null)
                dataGridView1.DataSource = districts.ToList().Where(w => w.ZonalId == cmbZonal.SelectedValue.ToInt32()).ToList();


        }
    }
}
