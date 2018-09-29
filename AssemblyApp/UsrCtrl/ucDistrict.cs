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
    public partial class ucDistrict : UserControl
    {
        public ucDistrict()
        {
            InitializeComponent();

            var dist = (from d in District.GetAll()
                        join z in Zonal.GetAll()
                        on d.ZonalId equals z.ZonalId
                        select new {
                            ZonalName = z.Name,
                            DistrictName = d.Name,
                            d.Code,
                            d.Blocks
                        }).ToList();



            dataGridView1.DataSource = dist;
        }
    }
}
