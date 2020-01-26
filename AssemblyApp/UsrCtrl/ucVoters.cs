using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoters : UserControl
    {
        List<VoterDetail> assemblies;
        public ZonalDistrict _selectedDistrict;

        public ucVoters(ZonalDistrict selectedDistrict = null)
        {
            InitializeComponent();


            //ProcessVoterList();

            _selectedDistrict = selectedDistrict;

            assemblies = (from a in VoterDetail.GetAll()
                          //join d in DataAccess.PrimaryTypes.District.GetAll()
                          //on a.DistrictId equals d.DistrictId
                          select a).ToList();

            dgvVoters.DataSource = assemblies;
            LoadFilter();
            LoadDistrict();
            FormatGrid();

        }

        private void ProcessVoterList()
        {
            var fileName = @"E:\NTK\jawa - 2021\Voters List\newocr.com-Single Columns.txt";

            var content = File.ReadAllLines(fileName).Where(w => string.IsNullOrEmpty(w.Trim()) == false).ToList();

            //var content2 = File.ReadAllLines(fileName);

            var pageDetails = content[50];
            var pageTitle = content[101];
            var areaDetails = content[102];
            var zoneDetails = content[153];

            content.RemoveAt(50);
            content.RemoveAt(100);
            content.RemoveAt(100);
            content.RemoveAt(150);
            

            var voters = new List<VoterDetail>();


            for (int i = 0; i < content.Count() - 1; i = i + 5)
            {
                var items = content.Skip(i).Take(5).ToList();
                // Do something with 100 or remaining items


                var ageAndGender = items[4].Split(' ');

                var voter = new VoterDetail()
                {
                    VoterId = items[0].Split('.').Last().Trim(),
                    CorrectedVoterId = CorrectVoterId(items[0].Split('.').Last()).Trim(),
                    Name = items[1].Split(':').Last().Trim(),
                    Fname = items[2].Split(':').Last().Trim(),
                    Address = items[3].Split(':').Last().Trim(),
                    CorrectedAddress = CorrectAddress(items[3].Split(':').Last().Trim()),
                    Age = ageAndGender[2],
                    Gender = ageAndGender[4].Replace(":", ""),
                    R = "H"
                };

                voters.Add(voter);

            }

            int nextStartingNO = 1; // Starting serail no of that page
            int startingNo = 1; // Starting serail no of that page
            int pageNo = 3;
            
            
            for (int i = 0; i < voters.Count() - 1; i = i + 10)
            {
                var items = voters.Skip(i).Take(10).ToList();

                // Do something with 100 or remaining items
                foreach (var item in items)
                {
                    item.Sno = startingNo;
                    item.PageNumber = pageNo;
                    
                    startingNo += 3;
                }
                nextStartingNO += 1;
                startingNo = nextStartingNO;
            }

            string sJSONResponse = JsonConvert.SerializeObject(voters.OrderBy(o => o.Sno));

        }

        private string CorrectAddress(string givenAddress)
        {
            return givenAddress.Replace("&", "A").Replace("8", "B").Replace("ப.எ.144", "ப.எ.NA").Replace("ப.எ.14", "ப.எ.NA");
        }

        private string CorrectVoterId(string givenVoterId)
        {
            if (givenVoterId.Contains("/"))
                return givenVoterId.Replace("111/34/201/", "TN/34/201/").Replace("714/34/201/", "TN/34/201/");
            else
                return string.Format("WRM{0}", givenVoterId.Substring(3));






        }

        private void FormatGrid()
        {
            //dgvVoters.Columns["DistrictId"].Visible = false;
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
            //var value = ((KeyValuePair<int, string>)cmbFilter.SelectedItem).Key;
            //List<DataAccess.ExtendedTypes.DistrictAssembly> filteredAssemblies = assemblies;

            //if (value == 1) filteredAssemblies = assemblies.OrderBy(o => o.AssemblyNo).ToList();

            //else if (value == 2) filteredAssemblies = assemblies.OrderBy(o => o.AssemblyName).ToList();

            //else if (value == 3) filteredAssemblies = assemblies.OrderByDescending(o => o.Category).ToList();

            //else if (value == 4) filteredAssemblies = assemblies.OrderByDescending(o => o.Electors).ToList();

            //else if (value == 5) filteredAssemblies = assemblies.OrderBy(o => o.DistrictName).ToList();

            //filteredAssemblies.ForEach(a =>
            //{
            //    a.SNo = filteredAssemblies.IndexOf(a) + 1;

            //});


            //dgvVoters.DataSource = filteredAssemblies;

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

            //dgvVoters.DataSource = assemblies.Where(w => w.AssemblyName.ToLower().Contains(textBox1.Text.ToLower())).ToList();

        }

        private void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbDistrict.SelectedValue.ToInt32() > 0)
            //    dgvVoters.DataSource = assemblies.Where(w => w.DistrictId == cmbDistrict.SelectedValue.ToInt32()).ToList();
            //else
            //    dgvVoters.DataSource = assemblies;

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
