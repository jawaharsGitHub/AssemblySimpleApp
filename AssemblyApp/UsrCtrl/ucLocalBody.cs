using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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



            //ProcessPanchayat();

            //ProcessOndrium();

            ProcessNagaratchi();

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


            if (lstOndrium.SelectedIndex + 1 <= lstOndrium.Items.Count - 1)
            {
                lstOndrium.SelectedIndex += 1;
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

        private void ProcessNagaratchi()
        {

            //BaseData.SaveAll();

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


            if (lstOndrium.SelectedIndex + 1 <= lstOndrium.Items.Count - 1)
            {
                lstOndrium.SelectedIndex += 1;
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

            //cmbOndrium.DataSource = ondriums;
            lstOndrium.DataSource = ondriums;

            lblCount.Text = $"{ondriums.Count} in {selectedDisName}";

            lstOndrium.DisplayMember = "OndriumFullName";
            lstOndrium.ValueMember = "OndriumId";
            this.lstOndrium.SelectedIndexChanged += CmbOndrium_SelectedIndexChanged;
        }

        private void CmbOndrium_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOndrium.SelectedValue.ToInt32() > 0)
            {
                selectedOndId = (lstOndrium.SelectedItem as BaseData).OndriumId;
                selectedOndName = (lstOndrium.SelectedItem as BaseData).OndriumName;

                LoadPanchayat(selectedOndId);
                //lblDone.Text = $"{cmbOndrium.SelectedIndex} done";
            }
        }


        private void LoadPanchayat(int ondriumId)
        {
            var panchayats = BaseData.GetPanchayat(ondriumId);

            lstPanchayat.DataSource = panchayats;

            lblCount.Text = $"{panchayats.Count} in {selectedOndName}";

            lstPanchayat.DisplayMember = "PanchayatName";
            lstPanchayat.ValueMember = "PanchayatId";
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
            File.WriteAllText(path, JsonConvert.SerializeObject(psList, Formatting.Indented));
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                var filePath = ofd1.FileName;

                var fileText = File.ReadAllLines(filePath).ToList();

                // voter id
                var onlyepicNo = (from ft in fileText
                                  where ft.Contains(":") == false &&
                                        string.IsNullOrEmpty(ft) == false &&
                                        ft.Contains("Photo") == false &&
                                        ft.Contains("is") == false &&
                                        ft.Contains("Available") == false &&
                                        (char.IsUpper(ft.First()) == true || char.IsNumber(ft.First()) == true)
                                  select ft.Trim()).ToList();

                var voters = new List<VoterData>();

                onlyepicNo.ForEach(fe =>
                {
                    voters.Add(new VoterData() { VoterId = fe });
                }
                );


                // home number
                var homeNos = (from ft in fileText
                               where ft.Contains("வீட்டு")
                               select ft).ToList();

                string captche = textBox1.Text; // "vEKkJm";

                for (int i = 0; i < homeNos.Count; i++)
                {
                    if (homeNos[i].Contains(":"))
                    {
                        voters[i].HomeNo = homeNos[i].Split(':')[1];
                    }
                    else
                    {
                        voters[i].IsDeleted = true;
                    }

                    var url = "https://electoralsearch.in/Home/searchVoter?epic_no=" + voters[i].VoterId + "&txtCaptcha=" + captche + "&page_no=1&results_per_page=10&reureureired=ca3ac2c8-4676-48eb-9129-4cdce3adf6ea&search_type=epic&state=S22";


                    //webBrowser1.Navigate(new Uri(url));

                    //var tes = webBrowser1.DocumentText;

                    //MessageBox.Show(tes);

                    //MessageBox.Show(url);

                    // return;


                    //webBrowser1.Url = url;
                    //using (var client = new HttpClient())
                    //{
                    //    HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call!  
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        Console.WriteLine("Request Message Information:- \n\n" + response.RequestMessage + "\n");
                    //        Console.WriteLine("Response Message Header \n\n" + response.Content.Headers + "\n");
                    //        // Get the response
                    //        var customerJsonString = response.Content.ReadAsStringAsync();
                    //        Console.WriteLine("Your response data is: " + customerJsonString);

                    //        // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                    //        //var deserialized = JsonConvert.DeserializeObject<IEnumerable<Customer>>(custome‌​rJsonString);
                    //        // Do something with it
                    //    }
                    //}


                    //System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    //document.Load(url);
                    //string allText = document.InnerText;


                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    //using (Stream stream = response.GetResponseStream())
                    //using (StreamReader reader = new StreamReader(stream))
                    //{
                    //    var data =  reader.ReadToEnd();
                    //}

                    using (WebClient wc = new WebClient())
                    {
                        //var json = wc.DownloadString(url);

                        WebRequest request = WebRequest.Create(url);

                        var reqHeaders = new WebHeaderCollection();
                        //reqHeaders.Add("", "");

                        reqHeaders.Add("electoralSearchId", "ax1brxqo14gozflcvjoafavz");
                        reqHeaders.Add("__RequestVerificationToken", "JSIrx2i7uArQE-e1ob91r0NvNj9XMmUCAApH7mPluWHPk3W2iSm6nCc98eve9OtUQTqQhJHQEsndfF3spRj-VV6R59GXfbdNDVq1nEA5DkY1	");
                        reqHeaders.Add("ServerAffinity", "a916ee78d08daa7d38242ee3b334b6aec768ab5e622167d3ebc9b179bf12c839	");
                        reqHeaders.Add("_ga", "GA1.2.1267771172.1579806954");
                        reqHeaders.Add("_gid", "GA1.2.1062821620.1589918604");
                        reqHeaders.Add("runOnce", "true");

                        request.Headers = reqHeaders;
                        //request.ho

                        //request.TryAddCookie(new Cookie("electoralSearchId", "ax1brxqo14gozflcvjoafavz"));
                        //request.TryAddCookie(new Cookie("__RequestVerificationToken", "JSIrx2i7uArQE-e1ob91r0NvNj9XMmUCAApH7mPluWHPk3W2iSm6nCc98eve9OtUQTqQhJHQEsndfF3spRj-VV6R59GXfbdNDVq1nEA5DkY1	"));
                        //request.TryAddCookie(new Cookie("ServerAffinity", "a916ee78d08daa7d38242ee3b334b6aec768ab5e622167d3ebc9b179bf12c839	"));
                        //request.TryAddCookie(new Cookie("_ga", "GA1.2.1267771172.1579806954"));
                        //request.TryAddCookie(new Cookie("_gid", "GA1.2.1062821620.1589918604"));
                        //request.TryAddCookie(new Cookie("runOnce", "true"));
                        //request.TryAddCookie(new Cookie("electoralSearchId", "ax1brxqo14gozflcvjoafavz"));
                        //request.TryAddCookie(new Cookie("electoralSearchId", "ax1brxqo14gozflcvjoafavz"));
                        WebResponse response = request.GetResponse();


                    }

                }


                // all voter details.




            }
        }

        private void btnUpdateBooth_Click(object sender, EventArgs e)
        {
            if (cmbZonal.SelectedIndex == 0 || cmbAssembly.SelectedIndex == 0 ||
                lstOndrium.SelectedIndices.Count == 0 || lstPanchayat.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select all fields");
                return;
            }

            var selectedPanchayats = lstPanchayat.SelectedItems.Cast<BaseData>().Select(s => s.PanchayatId).ToList();

            
            //selectedPanchayats.Cast<BaseData>().ToList()

            BaseData.UpdateBooth(cmbZonal.SelectedValue.ToInt32(), 
                                cmbAssembly.SelectedValue.ToInt32(), 
                                lstOndrium.SelectedValue.ToInt32(),
                                selectedPanchayats);



        }
    }

    public class VoterData
    {
        public string VoterId { get; set; }

        public string HomeNo { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class MyPollingStation
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string AreaVoting { get; set; }

    }

    public static class MyWebExtension
    {
        public static bool TryAddCookie(this WebRequest webRequest, Cookie cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            cookie.Domain = "electoralsearch.in";

            httpRequest.CookieContainer.Add(cookie);
            return true;
        }
    }

}


