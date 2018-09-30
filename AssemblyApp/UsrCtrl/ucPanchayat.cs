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
using DataAccess.ExtendedTypes;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucPanchayat : UserControl
    {

        List<AssemblyPanchayat> panchayats;
        public DistrictAssembly _selectedAssembly;

        public ucPanchayat(DistrictAssembly selectedAssembly = null)
        {
            InitializeComponent();
            _selectedAssembly = selectedAssembly;

            // TODO: Update AssemblyId as AssemblyNo
            panchayats = (from p in Panchayat.GetAll()
                          join a in Assembly.GetAll()
                          on p.AssemblyId equals a.AssemblyNo
                          select new AssemblyPanchayat
                          {
                              AssemblyNo = a.AssemblyNo,
                              AssemblyName = a.AssemblyName,
                              PanchayatId = p.PanchayatId,
                              
                              
                              //Electors = p.Electors,
                              //Category = p.Category
                          }).ToList();

            dataGridView1.DataSource = Panchayat.GetAll();
        }
    }
}
