﻿using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{


    public partial class ucLocalBody : UserControl
    {

        private int selectedDisId;
        private string selectedDisName;

        private int selectedOndId;
        private string selectedOndName;
        public ucLocalBody()
        {
            InitializeComponent();
            cmbSubItems.DataSource = GetOptions();
            //cmbSubItems.SelectedIndex = 1; // for ondrium
            cmbSubItems.SelectedIndex = 2; // for panchayat
            LoadZonal();
            LoadAssembly();
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

            //var copiedData = Clipboard.GetDataObject();

            //if (Clipboard.ContainsText(TextDataFormat.Text))
            //{
            //    //string clipboardText = Clipboard.GetText(TextDataFormat.Rtf);

            //    var ddd = txtData.Text;
            //    // Do whatever you need to do with clipboardText
            //}



            ProcessPanchayat();

            //ProcessOndrium();

            txtData.Text = "";
            txtData.SelectAll();
            txtData.Focus();

        }


        private void ProcessPanchayat()
        {
            var value = ((KeyValuePair<int, string>)cmbSubItems.SelectedItem).Key;

            if (value == 0)
            {
                MessageBox.Show("pls select sub items");
                return;
            }

            var data = txtData.Text;

            var allLines = data.Split('=').ToList();

            allLines.RemoveRange(0, 2);

            var fnalData = new StringBuilder();
            var d = new List<BaseData>();

            allLines.ForEach(fe =>
            {

                var NeededData = fe.Split('<')[0].Split('>');

                var id = NeededData[0].Replace("\"", "");
                var name = NeededData[1];

                d.Add(new BaseData()
                {

                    DistrictId = selectedDisId,
                    DistrictName = selectedDisName.Trim(),
                    OndriumId = selectedOndId,
                    OndriumName = selectedOndName,
                    PanchayatId = Convert.ToInt32(id),
                    PanchayatName = name
                });
            });


            string path = @"e:\json\PanchayatData.json";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(d, Formatting.Indented).Replace("[", "").Replace("]", "") + ",");
            }



            MessageBox.Show($"{selectedOndName}-{selectedDisName} done!", "DONE!");


            if (cmbOndrium.SelectedIndex + 1 <= cmbOndrium.Items.Count - 1)
            {
                cmbOndrium.SelectedIndex += 1;
            }
            else
            {
                MessageBox.Show("COMPLETE DONE!!!!!");

                if (cmbZonal.SelectedIndex + 1 <= cmbZonal.Items.Count - 1)
                {
                    cmbZonal.SelectedIndex += 1;
                }
                else
                {
                    MessageBox.Show("ALL DONE!!!!!");

                }

            }


        }

        private void ProcessOndrium()
        {
            var value = ((KeyValuePair<int, string>)cmbSubItems.SelectedItem).Key;

            if (value == 0)
            {
                MessageBox.Show("pls select sub items");
                return;
            }

            var data = txtData.Text;

            var allLines = data.Split('=').ToList();

            allLines.RemoveRange(0, 2);

            var fnalData = new StringBuilder();
            var d = new List<BaseData>();

            allLines.ForEach(fe =>
            {

                var NeededData = fe.Split('<')[0].Split('>');

                var id = NeededData[0].Replace("\"", "");
                var name = NeededData[1];

                d.Add(new BaseData()
                {

                    DistrictId = selectedDisId,
                    DistrictName = selectedDisName.Trim(),
                    OndriumId = Convert.ToInt32(id),
                    OndriumName = name
                });
            });


            string path = @"e:\json\MyTest.txt";

            ////if (!File.Exists(path))
            ////{
            ////    // Create a file to write to.
            ////    using (StreamWriter sw = File.CreateText(path))
            ////    {

            ////    }
            ////}

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(d, Formatting.Indented).Replace("[", "").Replace("]", "") + ",");
            }



            MessageBox.Show($"{selectedDisName} done!", "DONE!");


        }

        private void LoadZonal()
        {
            var zonal = Zonal.GetAll();
            zonal.Add(new Zonal(0, "ALL"));

            cmbZonal.DisplayMember = "ZonalFullName";
            cmbZonal.ValueMember = "ZonalId";
            cmbZonal.DataSource = zonal.OrderBy(o => o.ZonalId).ToList();
            this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            //cmbZonal.SelectedValue = _selectedZonal == null ? 0 : _selectedZonal.ZonalId;


        }

        private void LoadAssembly()
        {
            var assembly = Assembly.GetAll();
            //zonal.Add(new Zonal(0, "ALL"));

            cmbAssembly.DisplayMember = "AssemblyName";
            cmbAssembly.ValueMember = "AssemblyNo";
            cmbAssembly.DataSource = assembly.OrderBy(o => o.AssemblyName).ToList();
            //this.cmbZonal.SelectedIndexChanged += new System.EventHandler(this.cmbZonal_SelectedIndexChanged);
            //cmbZonal.SelectedValue = _selectedZonal == null ? 0 : _selectedZonal.ZonalId;


        }

        private void cmbZonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZonal.SelectedValue.ToInt32() > 0)
            {
                selectedDisId = (cmbZonal.SelectedItem as Zonal).ZonalId;
                selectedDisName = (cmbZonal.SelectedItem as Zonal).Name;
                LoadOndrium(selectedDisId);
            }

        }

        private void LoadOndrium(int disId)
        {
            var ondriums = BaseData.GetOndrium(disId);

            cmbOndrium.DataSource = ondriums;

            lblCount.Text = $"{ondriums.Count} in {selectedDisName}";

            cmbOndrium.DisplayMember = "OndriumFullName";
            cmbOndrium.ValueMember = "OndriumId";
            this.cmbOndrium.SelectedIndexChanged += CmbOndrium_SelectedIndexChanged;
        }

        private void CmbOndrium_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOndrium.SelectedValue.ToInt32() > 0)
            {
                selectedOndId = (cmbOndrium.SelectedItem as BaseData).OndriumId;
                selectedOndName = (cmbOndrium.SelectedItem as BaseData).OndriumName;

                LoadPanchayat(selectedOndId);
                //lblDone.Text = $"{cmbOndrium.SelectedIndex} done";
            }
        }


        private void LoadPanchayat(int ondriumId)
        {
            var panchayats = BaseData.GetPanchayat(ondriumId);

            cmbPanchayat.DataSource = panchayats;

            lblCount.Text = $"{panchayats.Count} in {selectedOndName}";

            cmbPanchayat.DisplayMember = "PanchayatName";
            cmbPanchayat.ValueMember = "PanchayatId";
            //this.cmbOndrium.SelectedIndexChanged += CmbOndrium_SelectedIndexChanged;
        }

        private void ucLocalBody_Load(object sender, EventArgs e)
        {
            ParentForm.AcceptButton = btnProcess;
        }

        private void btnPSProcess_Click(object sender, EventArgs e)
        {
            var url = $"https://www.elections.tn.gov.in/SSR2019/ac{cmbAssembly.SelectedValue}.htm";

            var sourceCode = GetHTMLSource(url);

            var data = sourceCode.Substring(sourceCode.IndexOf("lb_AC_NAME"));
            data = data.Remove(data.IndexOf("<span id="));
            data = data.Substring(data.IndexOf("<input type"));
            data = data.Replace("<tr>", "~");

            var neededData = data.Split('~').Where(w => w.Trim() != "").ToList();

            var psList = new List<MyPollingStation>();

            neededData.ForEach(fe =>
            {

                var ps = new MyPollingStation();

                var row = fe;

                // place Name
                var pn = row.Substring(row.IndexOf("<a"));
                pn = pn.Remove(pn.IndexOf("</a>"));
                pn = pn.Substring(pn.IndexOf(">") + 1);
                ps.Name = pn;

                // place Address
                var address = row.Substring(row.IndexOf("<br><br><br>"));
                address = address.Remove(address.IndexOf("<br />"));
                address = address.Replace("<br>", ""); //.Substring(pn.IndexOf(">") + 1);
                ps.Address = address;

                // Area Voted
                var votingArea = row.Substring(row.IndexOf("<br />"));
                votingArea = votingArea.Substring(votingArea.IndexOf("<p>"));
                votingArea = votingArea.Remove(votingArea.IndexOf("</p>"));
                votingArea = votingArea.Replace("<p>", "").Replace("<br />", "");
                ps.AreaVoting = votingArea.Trim();

                psList.Add(ps);


            });


            string path = @"e:\json\ac" + cmbAssembly.SelectedValue + ".txt";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw1 = File.CreateText(path))
                {


                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            //using (StreamWriter sw2 = File.AppendText(path))
            //{
            File.WriteAllText(path,JsonConvert.SerializeObject(psList, Formatting.Indented));
            //}

        }

        private string GetHTMLSource(string urlAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string data = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();


            }

            return data;
        }
    }

    public class MyPollingStation
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string AreaVoting { get; set; }

    }

}
