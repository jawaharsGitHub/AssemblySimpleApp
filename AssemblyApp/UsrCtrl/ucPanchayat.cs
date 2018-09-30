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
using Common.ExtensionMethod;

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
                          on p.AssemblyNo equals a.AssemblyNo
                          select new AssemblyPanchayat
                          {
                              AssemblyNo = a.AssemblyNo,
                              AssemblyName = a.AssemblyName,
                              PanchayatId = p.PanchayatId,
                              Name = p.Name,
                              BlockId = p.BlockId,
                              Population = p.Population,
                              Male = p.Male,
                              Female = p.Female,
                              SC = p.SC,
                              Scmale = p.Scmale,
                              ST = p.ST,
                              Stmale = p.Stmale,
                              StFemale = p.StFemale
                          }).ToList();

            dataGridView1.DataSource = panchayats;

            LoadFilter();
            LoadAssembly();
            FormatGrid();

        }

        private void FormatGrid()
        {
            //dataGridView1.Columns["DistrictId"].Visible = false;
        }

        private void LoadAssembly()
        {
            var district = Assembly.GetAll();
            district.Add(new Assembly(0, "ALL"));


            cmbAssembly.DataSource = district.OrderBy(o => o.AssemblyNo).ToList();
            this.cmbAssembly.SelectedIndexChanged += new System.EventHandler(this.cmbAssembly_SelectedIndexChanged);
            cmbAssembly.SelectedValue = _selectedAssembly == null ? 0 : _selectedAssembly.AssemblyNo;
        }

        private void LoadFilter()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By Assembly"),
                   new KeyValuePair<int, string>(2, "By Block"),
                   new KeyValuePair<int, string>(3, "By Poplation"),
                   new KeyValuePair<int, string>(4, "By SC"),
                   new KeyValuePair<int, string>(5, "By ST"),

               };

            cmbSort.DataSource = myKeyValuePair;
        }

        private void cmbAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAssembly.SelectedValue.ToInt32() > 0)
                dataGridView1.DataSource = panchayats.Where(w => w.AssemblyNo == cmbAssembly.SelectedValue.ToInt32()).ToList();
            else
                dataGridView1.DataSource = panchayats;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (cmbAssembly.SelectedValue.ToInt32() > 0)
                dataGridView1.DataSource = panchayats.Where(w => w.AssemblyNo == cmbAssembly.SelectedValue.ToInt32() && w.Name.ToLower().Contains(textBox1.Text.ToLower())).ToList();
            else
                dataGridView1.DataSource = panchayats.Where(w => w.Name.ToLower().Contains(textBox1.Text.ToLower())).ToList();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbSort.SelectedItem).Key;
            List<AssemblyPanchayat> sortedPanchayats = panchayats;

            if (value == 1) sortedPanchayats = panchayats.OrderBy(o => o.AssemblyName).ToList();

            else if (value == 2) sortedPanchayats = panchayats.OrderBy(o => o.BlockId).ToList();

            else if (value == 3) sortedPanchayats = panchayats.OrderByDescending(o => o.Population).ToList();

            else if (value == 4) sortedPanchayats = panchayats.OrderByDescending(o => o.SC).ToList();

            else if (value == 5) sortedPanchayats = panchayats.OrderByDescending(o => o.ST).ToList();

            if (cmbAssembly.SelectedValue.ToInt32() > 0)
                sortedPanchayats = sortedPanchayats.Where(w => w.AssemblyNo == cmbAssembly.SelectedValue.ToInt32()).ToList();

                //sortedPanchayats.ForEach(a =>
                //{
                //    a.SNo = sortedPanchayats.IndexOf(a) + 1;

                //});


                dataGridView1.DataSource = sortedPanchayats;
        }
    }
}
