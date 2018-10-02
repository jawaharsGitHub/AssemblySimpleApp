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
    public partial class ucAnalysis : UserControl
    {
        public ucAnalysis()
        {
            InitializeComponent();

            dataGridView1.DataSource = VoteData.GetAll();
        }
    }
}
