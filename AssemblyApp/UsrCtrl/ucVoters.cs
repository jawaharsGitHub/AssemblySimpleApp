using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVoters : UserControl
    {
        List<VoterDetail> voterDetails;
        public ZonalDistrict _selectedDistrict;
        string filePath = @"E:\NTK\jawa - 2021\Voters List\PanchayatVoters\P-211-Ramanathapuram\Nagaratchi\Rameswaram\Ward8\json\MergedJson\Rmm ward8.json";

        public ucVoters(ZonalDistrict selectedDistrict = null)
        {
            InitializeComponent();

            _selectedDistrict = selectedDistrict;

            string jsonText = File.ReadAllText(filePath);
            List<VoterDetail> list = JsonConvert.DeserializeObject<List<VoterDetail>>(jsonText) ?? new List<VoterDetail>();

            lblRecordCount.Text = $"Total Voters: {list.Count()}";

            voterDetails = list.OrderBy(o => o.Sno).ToList();

            dgvVoters.DataSource = voterDetails;
            dgvVoters.Columns["R"].DisplayIndex = 4;

            LoadFilter();
            LoadDistrict();
            FormatGrid();

        }

        private void ProcessVoterList(string fileName)
        {
            var fileNamewithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var processedFileName = fileNamewithoutExt.Split('-');

            txtStartingNo.Text = processedFileName[3].ToInt32().ToString();
            txtPageNo.Text = processedFileName[1].ToInt32().ToString();


            if (string.IsNullOrEmpty(txtPageNo.Text) || string.IsNullOrEmpty(txtStartingNo.Text))
            {
                MessageBox.Show("page no and strating no is needed");
                return;
            }

            var content = File.ReadAllLines(fileName).Where(w => string.IsNullOrEmpty(w.Trim()) == false).ToList();

            var voters = new List<VoterDetail>();


            for (int i = 0; i < content.Count() - 1; i = i + 5)
            {
                var items = content.Skip(i).Take(5).ToList();
                // Do something with 5 or remaining items


                var ageAndGender = items[4].Split(' ');

                var voterId = items[0].Split('.').Last().Replace("எண்", "").Trim();
                var address = items[3].Split(':').Last().Trim();

                var voter = new VoterDetail()
                {
                    VoterId = voterId,
                    CorrectedVoterId = CorrectVoterId(voterId).Trim(),
                    Name = items[1].Split(':').Last().Trim(),
                    Fname = items[2].Split(':').Last().Trim(),
                    Address = address,
                    CorrectedAddress = CorrectAddress(address.Trim()),
                    Age = ageAndGender[2],
                    Gender = ageAndGender[4].Replace(":", ""),
                    R = "H"
                };

                voters.Add(voter);

            }

            int nextStartingNO = txtStartingNo.Text.ToInt32(); // Starting serail no of that page
            int startingNo = txtStartingNo.Text.ToInt32(); // Starting serail no of that page
            int pageNo = txtPageNo.Text.ToInt32();



            int rowCount = 3;

            int fullrowCount = voters.Count / rowCount;
            if (voters.Count % rowCount > 0)
                fullrowCount += 1;

            int extraDataInRow = voters.Count % rowCount;

            int fullcolumnCount = voters.Count >= 3 ? 3 : voters.Count;


            var checkCount = fullrowCount == 1 ? voters.Count() : voters.Count() - 1;

            int roundNumber = 0;
            int fetchCount = fullrowCount;

            int skipRecordCount = 0;


            for (int i = 0; i < checkCount; i = i + fullrowCount)
            {

                roundNumber += 1;

                if (extraDataInRow != 0 && extraDataInRow < roundNumber)
                    fetchCount = fullrowCount - 1;

                var items = voters.Skip(skipRecordCount).Take(fetchCount).ToList();
                skipRecordCount += fetchCount;

                // Do something with taken items
                foreach (var item in items)
                {
                    item.Sno = startingNo;
                    item.PageNumber = pageNo;

                    startingNo += (fullrowCount == 1 ? 1 : fullcolumnCount);
                }
                nextStartingNO += 1;
                startingNo = nextStartingNO;

            }

            string sJSONResponse = JsonConvert.SerializeObject(voters.OrderBy(o => o.Sno), Formatting.Indented);


            var jsonFilePath = Path.Combine(Path.GetDirectoryName(fileName), fileNamewithoutExt);

            File.WriteAllText($"{jsonFilePath}_json.json", sJSONResponse);

            MessageBox.Show($"Done for {fileNamewithoutExt}");

        }

        private string CorrectAddress(string givenAddress)
        {
            return givenAddress.Replace("&", "A").Replace("8", "B").Replace("ப.எ.144", "ப.எ.NA").Replace("ப.எ.14", "ப.எ.NA");
        }

        private string CorrectVoterId(string givenVoterId)
        {
            if (givenVoterId.Contains("/34/201/") || (givenVoterId.Contains("/34") && givenVoterId.Contains("201/")))
                return "TN/34/201/" + givenVoterId.Substring(givenVoterId.Length - 7);
            else if (givenVoterId.Contains("₹") || givenVoterId.Contains("?") || givenVoterId.Contains("₹)") || givenVoterId.Contains("7)") || givenVoterId.Contains("%") || givenVoterId.Contains("["))
                return "FXJ" + givenVoterId.Substring(givenVoterId.Length - 7);
            else
                return "WRM" + givenVoterId.Substring(givenVoterId.Length - 7);
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

        private void button1_Click(object sender, EventArgs e)
        {


            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.InitialDirectory = @"E:\NTK\jawa - 2021\Voters List";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            foreach (String file in openFileDialog1.FileNames)
            {
                ProcessVoterList(file);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            //var data = voterDetails.Where(w => w.Gender.Trim() == "ஆண்‌").ToList();

            //var data = voterDetails.Count;

            sb.AppendLine($"Total Voters: {voterDetails.Count}");

            // by R
            var byR = voterDetails.GroupBy(g => g.R).Select(s => new { s.Key, s.ToList().Count }).OrderByDescending(o => o.Count).ToList();

            // by Age
            var byAge = voterDetails.GroupBy(g => g.Age).Select(s => new { s.Key, s.ToList().Count }).OrderByDescending(o => o.Count).ToList();

            sb.AppendLine($"AGE - VOTERS");
            byAge.ForEach(f =>
            {
                sb.AppendLine($"{f.Key} - {f.Count}");
            });

            // male voters
            var maleVoters = voterDetails.Where(g => g.Gender.First() == 'ஆ').ToList();

            // ladies voters
            var ladiesVoters = voterDetails.Where(g => g.Gender.Substring(0, 2) == "பெ").ToList();

            // other voters
            var otherVoters = voterDetails.Where(g => g.Gender.Substring(0, 2) != "பெ" && g.Gender.First() != 'ஆ').ToList();

            // Voters by Families.
            //var byFamilyAddress = voterDetails.GroupBy(g => g.CorrectedAddress)
            //    .Select(s => new
            //    {
            //        Key = s.Key + String.Join(",", s.Select(d => d.Sno).ToList(),
            //    s.ToList().Count
            //});


            var d = (from v in voterDetails
                     group v by v.CorrectedAddress into newGroup
                     select new
                     {
                         Key = newGroup.Key + String.Join(",", newGroup.Select(ss => ss.Sno).ToList()),
                         newGroup.ToList().Count
                     }).ToList();


            /*total voters
             * voters by age
             * voters by gender
             * number of families
             * voters by panchayat
             * voters by ondrium
             * voters by caste
             * voters by street or THERU.
             * voyers of younster/middle age/old
             * */

            MessageBox.Show(sb.ToString());


        }

        private void button3_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderDialog1 = new FolderBrowserDialog();
            folderDialog1.SelectedPath = @"E:\NTK\jawa - 2021\Voters List";
            folderDialog1.ShowNewFolderButton = false;
            folderDialog1.ShowDialog();

            var folderPath = folderDialog1.SelectedPath; // @"E:\NTK\jawa - 2021\Voters List\json";
            var allVoters = new List<VoterDetail>();


            var allJsonFiles = Directory.EnumerateFiles(folderPath, "*.json");

            var mergedFileName = allJsonFiles.First();

            var splittedFileName = Path.GetFileNameWithoutExtension(mergedFileName).Split(' ');

            mergedFileName = $"{splittedFileName[0].Trim()} {splittedFileName[1].Trim()}"; // $"Rmm ward8 OCR PNo-3-StartNo-1_json";

            foreach (string file in Directory.EnumerateFiles(folderPath, "*.json"))
            {
                string jsonText = File.ReadAllText(file);
                List<VoterDetail> list = JsonConvert.DeserializeObject<List<VoterDetail>>(jsonText) ?? new List<VoterDetail>();
                allVoters.AddRange(list);
            }

            var singleJonFile = JsonConvert.SerializeObject(allVoters.OrderBy(o => o.Sno).ToList(), Formatting.Indented);


            var fileDir = $"{folderPath}/MergedJson/";

            if (Directory.Exists(fileDir) == false)
                Directory.CreateDirectory(fileDir);

            File.WriteAllText($"{fileDir}/{mergedFileName}.json", singleJonFile);


            lblRecordCount.Text = $"Total Voters: {allVoters.Count()}";

            voterDetails = allVoters.OrderBy(o => o.Sno).ToList();
            dgvVoters.DataSource = voterDetails;

            MessageBox.Show("Done");
        }

        private void button4_Click(object sender, EventArgs e)
        {

            OpenFileDialog fileDial = new OpenFileDialog();
            fileDial.InitialDirectory = @"E:\NTK\jawa - 2021\Voters List";
            fileDial.ShowDialog();

            var file = fileDial.FileName; // @"E:\NTK\jawa - 2021\Voters List\json";
            var allVoters = new List<VoterDetail>();


            string jsonText = File.ReadAllText(file);
            List<VoterDetail> list = JsonConvert.DeserializeObject<List<VoterDetail>>(jsonText) ?? new List<VoterDetail>();

            var neededData = (from l in list
                              select new
                              { l.Sno, l.CorrectedVoterId, l.CorrectedAddress }).OrderBy(o => o.Sno).ToList();

            var singleWrongJson = JsonConvert.SerializeObject(neededData, Formatting.Indented);

            var Worngfile = $"{Path.GetDirectoryName(file)}/{Path.GetFileNameWithoutExtension(file)}_Wrong.json";

            File.WriteAllText(Worngfile, singleWrongJson);

            MessageBox.Show("Done");

        }

        private void dgvVoters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var owningColumnName = grid.CurrentCell.OwningColumn.Name;

            var cellValue = GetGridCellValue(grid, rowIndex, owningColumnName);

            if (cellValue == null) return;

            var cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as VoterDetail;

            var updatedCustomer = new VoterDetail()
            {
                Sno = cus.Sno
            };


            if (owningColumnName == "R" && cellValue != null)
            {
                updatedCustomer.R = cellValue;
                UpdateR(updatedCustomer);
                return;
            }
        }

        public static string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            var cellValue = Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);
            return (cellValue == string.Empty) ? null : cellValue;
        }

        public void UpdateR(VoterDetail updatedCustomer)
        {
            try
            {

                var jsonText = File.ReadAllText(filePath);
                var list = JsonConvert.DeserializeObject<List<VoterDetail>>(jsonText) ?? new List<VoterDetail>();
                //return list;

                //List<VoterDetail> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.Sno == updatedCustomer.Sno).FirstOrDefault();
                u.R = updatedCustomer.R;

                //WriteObjectsToFile(list, JsonFilePath);

                string jsonString = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
