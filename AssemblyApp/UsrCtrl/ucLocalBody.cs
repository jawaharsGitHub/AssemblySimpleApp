using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.ExtensionMethod;
using Newtonsoft.Json;
using System.IO;
using DataAccess.PrimaryTypes;

namespace CenturyFinCorpApp.UsrCtrl
{


    public partial class ucLocalBody : UserControl
    {

        private int disId;
        private string disName;
        public ucLocalBody()
        {
            InitializeComponent();
            cmbSubItems.DataSource = GetOptions();
            cmbSubItems.SelectedIndex = 1; // for ondrium
            LoadZonal();
        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(0, "--Select--"),
                   new KeyValuePair<int, string>(1, "Add-Ondrium"),
                   new KeyValuePair<int, string>(2, "Add-Panchayat"),
               };

            return myKeyValuePair;

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {

            var value = ((KeyValuePair<int, string>)cmbSubItems.SelectedItem).Key;

            if(value == 0)
            {
                MessageBox.Show("pls select sub items");
                return;
            }

            var data = txtData.Text;

            var allLines = data.Split('=').ToList();

            allLines.RemoveRange(0, 2);

            var fnalData = new StringBuilder();
            var d = new List<PanchayatData>();

            allLines.ForEach(fe =>
            {

                var NeededData = fe.Split('<')[0].Split('>');

                var id = NeededData[0].Replace("\"", "");
                var name = NeededData[1];

                d.Add(new PanchayatData()
                {

                    DistrictNoId = disId,
                    DistrictName = disName.Trim(),
                    OndriumId = Convert.ToInt32(id),
                    OndriumName = name
                });
            });


            string path = @"e:\json\MyTest.txt";

            //if (!File.Exists(path))
            //{
            //    // Create a file to write to.
            //    using (StreamWriter sw = File.CreateText(path))
            //    {
                    
            //    }
            //}

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(d, Formatting.Indented).Replace("[","").Replace("]", "") + ",");
            }



            MessageBox.Show($"{disName} done!", "DONE!");

            txtData.Text = "";

        }

        private void cmbSubItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbSubItems.SelectedItem).Key;

            if(value == 1)
            {
                grpOndrium.Visible = false;

            }
            else if (value == 2)
            {
                grpOndrium.Visible = true;
            }
        }

        private void LoadZonal()
        {
            var zonal = Zonal.GetAll();
            zonal.Add(new Zonal(0, "ALL"));

            cmbZonal.DisplayMember = "Name";
            cmbZonal.ValueMember = "ZonalId";
            cmbZonal.DataSource = zonal.OrderBy(o => o.ZonalId).ToList();
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            //cmbZonal.SelectedValue = _selectedZonal == null ? 0 : _selectedZonal.ZonalId;


        }

        private void cmbZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZonal.SelectedValue.ToInt32() > 0)
            {
                disId = (cmbZonal.SelectedItem as Zonal).ZonalId;
                disName = (cmbZonal.SelectedItem as Zonal).Name;

                LoadOndrium(disId);
            }
            
        }

        private void LoadOndrium(int disId)
        {
            var zonal = Zonal.GetAll();
            zonal.Add(new Zonal(0, "ALL"));

            cmbZonal.DisplayMember = "Name";
            cmbZonal.ValueMember = "ZonalId";
            cmbZonal.DataSource = zonal.OrderBy(o => o.ZonalId).ToList();
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
        }


    }

    

    public class PanchayatData
    {

        public int DistrictNoId { get; set; }

        public string DistrictName { get; set; }

        public int OndriumId { get; set; }

        public string OndriumName { get; set; }

        public int OoratchiId { get; set; }

        public string OoratchiName { get; set; }

    }
}
