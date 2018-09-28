using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DataAccess.PrimaryTypes;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class uclAddMember : UserControl
    {
        public uclAddMember()
        {
            InitializeComponent();

            cmbDivision.DataSource = Zonal.GetAll();
        }
    }
}
