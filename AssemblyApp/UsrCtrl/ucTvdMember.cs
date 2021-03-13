using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucTvdMember : UserControl
    {

        List<TvdMember> assemblies;
        List<Pair> paguthiList;
        List<Pair> utPaguthiList;

        public ucTvdMember()
        {
            InitializeComponent();

            LoadGrid();

            paguthiList = GetPaguthi();
            utPaguthiList = GetUtPaguthi();


            comboBox1.DataSource = GetUtPaguthi();
            comboBox1.DisplayMember = "Display";
            comboBox1.ValueMember = "DisplayTamil";
            LoadRec(0);

            comboBox3.DataSource = GetOptions();

        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "Order By Vote"),
                   new KeyValuePair<int, string>(2, "Only money"),
                   new KeyValuePair<int, string>(31, "MO"),
                   new KeyValuePair<int, string>(32, "PO"),
                   new KeyValuePair<int, string>(33, "VEO"),
                   //new KeyValuePair<int, string>(34, "M+P"),
                   //new KeyValuePair<int, string>(35, "M+V"),
                   //new KeyValuePair<int, string>(36, "P+V"),
                   //new KeyValuePair<int, string>(37, "M+P+V"),
                   new KeyValuePair<int, string>(3, "wants meet"),
                   new KeyValuePair<int, string>(4, "Not Yet Contact"),
                   new KeyValuePair<int, string>(15, "Till Now Contacted"),
                    new KeyValuePair<int, string>(16, "VVIP"),
                    new KeyValuePair<int, string>(17, "TALK TO THEM"),
                   new KeyValuePair<int, string>(5, "HighOrder By Members Count"),
                   new KeyValuePair<int, string>(6, "LowOrder By Members Count"),
                   new KeyValuePair<int, string>(7, "Order By Panchayat Names And Count"),
                   new KeyValuePair<int, string>(8, "Same Phone Numbers"),
                   new KeyValuePair<int, string>(9, "Same Family"),
                   new KeyValuePair<int, string>(10, "NO Phone No. Member"),
                   new KeyValuePair<int, string>(11, "Only Female"),
                   new KeyValuePair<int, string>(12, "Female By Area"),
                   new KeyValuePair<int, string>(13, "By Ondrium Name"),                   
                   new KeyValuePair<int, string>(14, "By Ondrium Count")
               };

            return myKeyValuePair;

        }


        private void LoadGrid()
        {
            assemblies = TvdMember.GetAll().ToList();

            fData = assemblies.Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00").ToList();

            if (rdbFem.Checked)
            {
                fData = fData.Where(w => w.IsFemale == true).ToList();
            }
            else if (rdbMale.Checked)
            {
                fData = fData.Where(w => w.IsFemale == false).ToList();
            }

            int i = 1;
            fData.ForEach(f => f.Sno = i++);
            dataGridView1.DataSource = fData;
            ColumnVisibility();
            LoadRec(fData.Count);


        }
        private void ColumnOrder()
        {
            dataGridView1.Columns["Name"].DisplayIndex = 0;
            dataGridView1.Columns["Phone"].DisplayIndex = 1;
            dataGridView1.Columns["Vote"].DisplayIndex = 2;
            dataGridView1.Columns["Campaign"].DisplayIndex = 3;
            dataGridView1.Columns["BoothAgent"].DisplayIndex = 4;
            dataGridView1.Columns["Vehicle"].DisplayIndex = 5;
            dataGridView1.Columns["Money"].DisplayIndex = 6;
            dataGridView1.Columns["NoMore"].DisplayIndex = 7; 
            dataGridView1.Columns["UpdatedTime"].DisplayIndex = 8;


        }
        private void ColumnVisibility()
        {
            dataGridView1.Columns["NoMore"].Visible = true;
            dataGridView1.Columns["Sno"].Visible = false;
            dataGridView1.Columns["Address"].Visible = false;
            dataGridView1.Columns["Paguthi"].Visible = false;
            dataGridView1.Columns["UtPaguthi"].Visible = false;
            dataGridView1.Columns["PaguthiEng"].Visible = false;

            dataGridView1.Columns["UtPaguthiEng"].Visible = false;
            dataGridView1.Columns["Country"].Visible = false;
            dataGridView1.Columns["State"].Visible = false;
            dataGridView1.Columns["District"].Visible = false;
            dataGridView1.Columns["Assembly"].Visible = false;

            
            dataGridView1.Columns["MemberId"].Visible = false;
            dataGridView1.Columns["Name"].Visible = true;
            dataGridView1.Columns["Phone"].Visible = true;
            dataGridView1.Columns["Email"].Visible = false;
            dataGridView1.Columns["JustTalk"].Visible = false;

            dataGridView1.Columns["VVIP"].Visible = false;
            dataGridView1.Columns["MultiplePlace"].Visible = false;
            dataGridView1.Columns["WantsMeet"].Visible = false;
            dataGridView1.Columns["Campaign"].Visible = true;
            dataGridView1.Columns["BoothAgent"].Visible = true;


            dataGridView1.Columns["Vehicle"].Visible = true;
            dataGridView1.Columns["Money"].Visible = true;
            dataGridView1.Columns["Vote"].Visible = true;
            dataGridView1.Columns["IsFemale"].Visible = false;
            dataGridView1.Columns["UpdatedTime"].Visible = true;
            dataGridView1.Columns["NeedUpdatePagEng"].Visible = false;

            ColumnOrder();

            //dataGridView1.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadRec(int searchCount)
        {
            lblDetails.Text = $"{searchCount} out of {TvdMember.GetCount()}";
        }
        private List<Pair> GetPaguthi()
        {

            List<Pair> paguthi = new List<Pair>()
            {
                new Pair(1,"Rsm-K", "ஆர்.எஸ்.மங்களம் - கிழக்கு", 24000, "JAC"),
                new Pair(2,"Rsm-M", "ஆர்.எஸ்.மங்களம் - மேற்கு",18000, "DAN" ),
                //new Pair(3,"Tvd-T","திருவாடானை - தெற்கு"),
                new Pair(4,"Tvd-K", "திருவாடானை - கிழக்கு", 30000, "JAY"),
                new Pair(5,"Tvd-M", "திருவாடானை - மேற்கு",28000, "ARD"),
                //new Pair(6,"Tvd-V", "திருவாடானை - வடக்கு"),
                new Pair(7,"Rmd-K", "இராம்நாடு - கிழக்கு",35000,"SAG"),
                new Pair(8,"Rmd-m", "இராம்நாடு - மேற்கு", 34000,"RAB"),
                new Pair(9,"RSMNagar", "ஆர்.எஸ்.மங்களம் நகர்" , 3500,"MER"),
                new Pair(10,"Thondi", "தொண்டி", 4500, "SID"),
                new Pair(11,"Others","Other Assembly",0),
                new Pair(12,"Dont Know","Dont Know",0)
            };

            return paguthi;
        }


        private List<Pair> GetUtPaguthi()
        {

            List<Pair> utpaguthi = new List<Pair>()
            {
                // RSM-Kilakku
                new Pair(1,"Manakkudi", "அ.மணக்குடி", 1200),
                new Pair(1,"Alagarthevankottai","அழகர்தேவன்கோட்டை ",1000),
                new Pair(1,"Siththoorvaadi","சித்தூர்வாடி ",1300),
                new Pair(1,"Kadaloore","கடலூர் ",2500),
                new Pair(1,"Kallikkudi","கள்ளிக்குடி ", 800),
                new Pair(1,"Karungudi","கருங்குடி ", 2000),
                new Pair(1,"Koththidal","கொத்திடல் ", 400),
                new Pair(1,"Kottakkudi","கொட்டக்குடி ", 1100),
                new Pair(1,"Paaranoor","பாரனூர் ", 1500),
                new Pair(1,"Pichchankurichi","பிச்சங்குறிச்சி ", 700),
                new Pair(1,"Senkudi","செங்குடி ", 450),
                new Pair(1,"Solanthoore","சோழந்தூர் ", 1000),
                new Pair(1,"Thiruppaalakudi","திருப்பாலைக்குடி ", 5000),
                new Pair(1,"Thumbadaikkakottai","தும்படைக்காகோட்டை ", 1500),
                new Pair(1,"Ooranagkudi","ஊரணங்குடி ", 600),
                new Pair(1,"Varavani","வரவணி ", 1000),
                new Pair(1,"KavanoorRsm","காவனூர்-RSM", 1400),

                // RSM-Merku
                new Pair(2,"A.R.Mangalam", "A.R.மங்களம்",1300),
                new Pair(2,"Ananthoor","ஆனத்தூர் ",1700),
                new Pair(2,"Ayangudi","ஆயங்குடி ",800),
                new Pair(2,"Govindamangalam","கோவிந்தமங்களம் ",800),
                new Pair(2,"Koodaloore","கூடலூர் ",800),
                new Pair(2,"Karkaaththakudi","கற்காத்தகுடி ",1300),
                new Pair(2,"Kaavanakkottai","காவனக்கோட்டை ",700),
                new Pair(2,"Pullamadai","புள்ளமடை ",2000),
                new Pair(2,"MelPanaiyoor","மேல்பனையூர் ",600),
                new Pair(2,"Odaikkaal","ஓடைக்கால் ", 450),
                new Pair(2,"Radhanoor","இராதானூர் ",1500),
                new Pair(2,"Sanaveli","சனவேலி ",1600),
                new Pair(2,"Saththanoor","சத்தானூர் ",700),
                new Pair(2,"Seththidal","சேத்திடல் ",1200),
                new Pair(2,"Sevvaipettai","செவ்வாய்பேட்டை ",750),
                new Pair(2,"Sirunakudi","சிருநாகுடி ", 350),
                new Pair(2,"Thiruthervalai","திருத்தேர்வளை ",800),
                new Pair(2,"Vadakkaloor","வடக்கலூர்",500),

                // TVD-Kilakku (22-VT)
                new Pair(4,"Theloor", "தேளூர் ",1200),
                new Pair(4,"Thalirmarungoor","தளிர்மருங்கூர் ",1200),
                new Pair(4,"Aathiyur","ஆதியூர் ",1200),
                new Pair(4,"Arumboor","அரும்பூர் ",1100),
                new Pair(4,"Kulaththoor","குளத்தூர் ",1200),
                new Pair(4,"Thiruvetriyur","திருவெற்றியூர் ",1000),
                new Pair(4,"Mugilthanagam","முகிழ்த்தகம் ",1900),
                new Pair(4,"Nambuthalai","நம்புதாளை ",3500),
                new Pair(4,"Puthupattinam","புதுப்பட்டினம் ",900),
                new Pair(4,"Mullimunai","முள்ளிமுனை ",1200),
                new Pair(4,"Karangaadu","காரங்காடு",900),
                new Pair(4,"Arasaththoor","அரசத்தூர் ",1300),
                new Pair(4,"Kodipangu","கொடிப்பங்கு ",1000),
                new Pair(4,"Maavoor","மாவூர் ",1000),
                new Pair(4,"Kattavilagam", "கட்டவிளாகம் ",2),
                new Pair(4,"Vallaiyaapuram","வெள்ளையபுரம் ",1400),
                new Pair(4,"Oriyur","ஓரியூர் ",1800),
                new Pair(4,"S.P.Pattinam","S.P. பட்டிணம்  ", 2500),
                new Pair(4,"Panachayal","பனஞ்சாயல் ",1000),
                new Pair(4,"Vattanam","வட்டானம் ",1000),
                new Pair(4,"Kaliyanagari","கலியநகரி ",1500),
                new Pair(4,"Pullakadamban","புல்லக்கடம்பன் ",1200),                 

                // TVD-Merku (25-AD)
                new Pair(5,"Mangalakudi", "மங்களக்குடி ",1500),
                new Pair(5,"Nilamalagiyamangalam","நிலமழகியமங்களம் ",900),
                new Pair(5,"Kattivayal","கட்டிவயல் ",850),
                new Pair(5,"Kunjangulam","குஞ்சங்குளம் ",1000),
                new Pair(5,"Anjukottai","அஞ்சுகோட்டை ",1800),
                new Pair(5,"Kodanoor","கோடனூர் ",1700),
                new Pair(5,"Pandukudi","பாண்டுகுடி ",650),
                new Pair(5,"Nagarikathan","நகரிகாத்தான் ",600),
                new Pair(5,"Achchangudi","அச்சங்குடி ",1200),
                new Pair(5,"Neivayal", "நெய்வயல் ",1400),
                new Pair(5,"karumoli","கருமொழி ",900),
                new Pair(5,"Palangulam","பழங்குளம் ",800),
                new Pair(5,"Kadamboor","கடம்பூர் ",2),
                new Pair(5,"Kookudi","கூகுடி ",1300),
                new Pair(5,"Thuththaakudi","துத்தாக்குடி ",630),
                new Pair(5,"Sirumalaikottai","சிறுமலைக்கோட்டை ", 850),
                new Pair(5,"T.Nagini","தி.நாகனி ",1300),
                new Pair(5,"Orikottai","ஓரிக்கோட்டை ", 380),
                new Pair(5,"Periakeeramangalam","பெரியகீரமங்களம் ", 1800),
                new Pair(5,"Kalloore","கல்லூர் ", 2800),
                new Pair(5,"Thiruvadanai","திருவாடானை ", 1350),
                new Pair(5,"Aandaavoorani","ஆண்டாவூரணி ", 1000),
                new Pair(5,"Paaganur","பாகனூர் ", 700),
                new Pair(5,"Sirukambaiyur","சிறுகம்பையூர் ", 850),
                new Pair(5,"Pathanakudi","பதனக்குடி ", 400),

                // RMD-Kilakku
                new Pair(7,"Alagankulam", "அழகன்குளம்"),
                new Pair(7,"Panaikulam","பனைக்குளம்"),
                new Pair(7,"Puthuvalasai","புதுவலசை"),
                new Pair(7,"Therbogi","தேர்போகி"),
                new Pair(7,"Athiyuthu","அத்தியூத்து "),
                new Pair(7,"Siththaarkottai","சித்தார்க்கோட்டை "),
                new Pair(7,"Devipattinam","தேவிப்பட்டிணம் "),
                new Pair(7,"Kalanikudi","கழினிக்குடி "),
                new Pair(7,"Vennathoor","வெண்ணத்தூர் "),
                new Pair(7,"Naranamangalam","நாரணமங்கலம் "),
                new Pair(7,"Pandamangalam","பாண்டமங்களம் "),
                new Pair(7,"Maathavanur","மாதவனூர் "),
                new Pair(7,"Peravoor","பேராவூர் "),
                new Pair(7,"Ilamanur","இளமனூர் "),
                new Pair(7,"Kalugoorani","கழுகூரணி "),
                new Pair(7,"Madakottan","மாடகொட்டான் "),

                // RMD-Merku
                new Pair(8,"Kavanur-RMD", "காவனூர்-RMD"),
                new Pair(8,"Karukudi","காருகுடி "),
                new Pair(8,"Thoruvalur","தொருவலூர் "),
                new Pair(8,"Siththoor","சித்தூர் "),
                new Pair(8,"Kallikkudi","கள்ளிக்குடி"),
                new Pair(8,"Soorankottai","சூரங்கோட்டை "),
                new Pair(8,"Puththenthal","புத்தேந்தல் "),
                new Pair(8,"PattinamKathan","பட்டிணம்காத்தான்"),
                new Pair(8,"Sakkarakottai","சக்கரக்கோட்டை"),
                new Pair(8,"Therkutharavai","தெற்குதாரவை "),
                new Pair(8,"Lanthai","லாந்தை"),
                new Pair(8,"Ekkakudi","எக்கக்குடி"),
                new Pair(8,"Panaikulam-TPL","பனைக்குளம்"),
                new Pair(8,"Mallal","மல்லல்"),
                new Pair(8,"Peruvayal","பெறுவயல் "),
                new Pair(8,"Karenthal","காரேந்தல் "),
                new Pair(8,"Pullangudi","புல்லங்குடி "),
                new Pair(8,"Achunthavayal","அச்சுந்தவயல்"),

                new Pair(9,"RSM-Nagar","ஆர்.எஸ்.மங்களம்"),
                new Pair(10,"Thondi","தொண்டி"),

                new Pair(11,"Others","Other Assembly"),
                new Pair(12,"Dont Know","Dont Know"),
                new Pair(13,",",",")


            };

            return utpaguthi.OrderBy(o => o.Display).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> snos = new List<int>();

            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                var tvd = (r.DataBoundItem as TvdMember);

                snos.Add(tvd.Sno);

                var memberId = tvd.MemberId;

                var utp = (comboBox1.SelectedItem as Pair);

                var utPagu = (from t in utPaguthiList
                              where t.Display == utp.Display
                              select t).FirstOrDefault();

                var Pagu = (from t in paguthiList
                            where t.Value == utp.Value
                            select t).FirstOrDefault();

                if (Pagu != null && utPagu != null)
                {
                    TvdMember.UpdateMemberDetails(memberId, Pagu.DisplayTamil, utPagu.DisplayTamil, Pagu.Display, utPagu.Display);
                }

            }

            LoadGrid();
            OnlyEmpty(snos.Max());

            MessageBox.Show("Done");

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Trim() == string.Empty)
            {
                var utp = (comboBox1.SelectedItem as Pair);
                dataGridView1.DataSource = assemblies.Where(w =>
                w.Address.Contains(utp.DisplayTamil.Trim()) ||
                w.Address.Contains(utp.Display.Trim())).ToList();
            }
            else
            {
                dataGridView1.DataSource = assemblies.Where(w =>
                w.Address.Contains(textBox1.Text.Trim())).ToList();
            }

            dataGridView1.SelectAll();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.SelectedIndex + 1;
            var utp = (comboBox1.SelectedItem as Pair);

            dataGridView1.DataSource = assemblies.Where(w => w.Address.Contains(utp.DisplayTamil.Trim())).ToList();

            dataGridView1.SelectAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TvdMember.ClearAllUpdate();
            LoadGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += (s, o) =>
            {
                int i = 0;
                utPaguthiList.ForEach(fe =>
                {

                    var dataToUpdate = assemblies.Where(w => w.Address.Contains(fe.DisplayTamil.Trim())).Select(ss => ss.MemberId).ToList();

                    if (dataToUpdate.Count > 0)
                    {

                        var utPagu = (from t in utPaguthiList
                                      where t.Display == fe.Display
                                      select t).FirstOrDefault();

                        var Pagu = (from t in paguthiList
                                    where t.Value == fe.Value
                                    select t).FirstOrDefault();

                        if (Pagu != null && utPagu != null)
                        {
                            foreach (string memberId in dataToUpdate)
                            {
                                TvdMember.UpdateMemberDetails(memberId, Pagu.DisplayTamil, utPagu.DisplayTamil, Pagu.Display, utPagu.Display);
                            }
                        }
                    }


                    i = i + 1;

                    // label1.BeginInvoke(new Action(() => label1.Text = $"{i} - {fe.DisplayTamil.Trim()}"));
                });

                MessageBox.Show("ALL DONE");
            };



            bw.RunWorkerAsync();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OnlyEmpty();
        }

        private void OnlyEmpty(int max = 0)
        {
            assemblies = TvdMember.GetAll();

            var x = assemblies.Where(w => w.UtPaguthiEng == null || w.UtPaguthiEng.Trim() == string.Empty).ToList();

            if (max > 0)
            {
                x = x.Where(w => w.Sno >= max).ToList();
            }

            dataGridView1.DataSource = x;

            LoadRec(x.Count);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var x = assemblies.Where(w => w.UtPaguthi.Trim().Contains(",")).ToList();
            dataGridView1.DataSource = x;
            LoadRec(x.Count);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadGrid();

        }

        List<TvdMember> fData;

        string selectedPan = "";

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectedPan = (comboBox1.SelectedItem as Pair).Display;

            var data = assemblies.Where(w => string.IsNullOrEmpty(w.UtPaguthiEng) == false && w.UtPaguthiEng.Contains(selectedPan)).ToList();

            if (rdbFem.Checked)
            {
                data = data.Where(w => w.IsFemale == true).ToList();
            }
            else if (rdbMale.Checked)
            {
                data = data.Where(w => w.IsFemale == false).ToList();
            }

            if (txtPhone.Text.Trim() != string.Empty)
            {
                data = data.Where(w => w.Phone.Contains(txtPhone.Text)).ToList();
            }

            fData = new List<TvdMember>();
            fData = data.OrderByDescending(o => o.Money)
                .ThenBy(o => o.Phone)
                .ToList();

            dataGridView1.DataSource = fData;
            ColumnVisibility();

            LoadRec(fData.Count);
        }

        private string GetFileNameFIlter()
        {

            StringBuilder filterName = new StringBuilder();

            if (chkExpAll.Checked == true) filterName.Append("_ALL");

            if (rdbMale.Checked == true) filterName.Append("_M");
            else if (rdbFem.Checked == true) filterName.Append("_F");

            return filterName.ToString();

        }

        private string Status(TvdMember tvdm)
        {
            StringBuilder sb = new StringBuilder();

            if (tvdm.Campaign) sb.Append("பிரச்சாரம் - march 6 கூட்டம்?  ||");
            if (tvdm.Vehicle) sb.Append("வாகனம் - ins. copy, RC, License, Photo    ||");
            if (tvdm.BoothAgent) sb.Append("பூத் ஏஜென்ட் - voter id?    ||");
            if (tvdm.Vote > 19) sb.Append($"vote:{tvdm.Vote} - நன்றி    ||");
            if (tvdm.Money) sb.Append("money - account details?");

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            if (fData == null || fData.Count == 0)
            {
                MessageBox.Show("No Data to export, please try search grid");
                return;
            }
            StringBuilder sb = new StringBuilder();
            int i = 0;
            ///*

            
            fData.ForEach(fe =>
            {

                if (fe.Campaign == true || fe.Vehicle == true || fe.BoothAgent == true || fe.Vote > 19 || fe.Money == true)
                {
                    i = i + 1;
                    // sb.AppendLine($"({i})  {fe.Name} [ {fe.Phone} ]{Environment.NewLine}{fe.Address}");
                    sb.AppendLine($"({i})  {fe.Name} [ {fe.Phone} ] - {Status(fe)} ");
                }
                
                // sb.AppendLine($"$$$$$$$$$$$$$$$$$$$$$$$$${Environment.NewLine}");
            });

            File.WriteAllText($@"F:\NTK\jawa - 2021\members\CallUpdate-2 march.txt", sb.ToString());
            MessageBox.Show($"All Members Exported Successfully!");
            return;
            ///


            string filterName = GetFileNameFIlter();

            if (chkExpAll.Checked == false)
            {
                var d = fData.Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00").OrderBy(o => o.Phone).ToList();

                if (rdbFem.Checked)
                {
                    d = d.Where(w => w.IsFemale).ToList();
                }
                else if (rdbMale.Checked)
                {
                    d = d.Where(w => w.IsFemale == false).ToList();
                }

                //sb.AppendLine($"-------------------");
                //sb.AppendLine($"{fData[0].UtPaguthi} ({fData.Count})");
                //sb.AppendLine($"-------------------");

                //d.ForEach(fe =>
                fData.ForEach(fe =>
                {
                    i = i + 1;
                    // sb.AppendLine($"({i})  {fe.Name} [ {fe.Phone} ]{Environment.NewLine}{fe.Address}");
                    sb.AppendLine($"({i})  {fe.Name} [ {fe.Phone} ]");
                    if (rdbMale.Checked)
                        sb.AppendLine($@"1. பயணிப்பதற்கு நன்றி! --- 2. எடுக்கவில்லை!  ---3. வெற்றி நம்பிக்கை! --- 4. வாக்கு?   --- 5. பரப்புரை?--- 6. பூத் ஏகென்ட்?  -- 7. வாகனம்?     --- 6. நிதி/வேறு உதவி?");
                    else if (rdbFem.Checked)
                        sb.AppendLine($@"1. பயணிப்பதற்கு நன்றி ! --- 2. தொடர்பில் இல்லை!   ---3. வெற்றி நம்பிக்கை! --- 4. வாக்கு?   --- 5. பரப்புரை?  -- 7. வாகனம்?     --- 6. நிதி/வேறு உதவி?");

                    sb.AppendLine($"$$$$$$$$$$$$$$$$$$$$$$$$${Environment.NewLine}");
                });

                File.WriteAllText($@"F:\NTK\jawa - 2021\members\{selectedPan}({d.Count}){filterName}.txt", sb.ToString());
                MessageBox.Show($"{selectedPan}({d.Count} Exported Successfully!");
            }
            else
            {

                string fg = "_ALL";
                // ALL
                var allData = (from t in TvdMember.GetAll().Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00")
                               where t.UpdatedTime.ToString() == "01-01-0001 00:00:00" &&
                                  t.UtPaguthiEng.Contains(',') == false &&
                                  t.UtPaguthiEng.Contains("Others") == false &&
                                 t.UtPaguthiEng.Contains("Dont") == false
                               group t by t.UtPaguthiEng into newGrp
                               select new
                               {
                                   Key = newGrp.Key,
                                   Data = newGrp.ToList()
                               }).ToList();

                int panNo = 0;
                int recCount = allData.Sum(s => s.Data.Count);

                if (rdbFem.Checked)
                {
                    fg = "_F";
                    allData = (from t in TvdMember.GetAll().Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00")
                               where t.UpdatedTime.ToString() == "01-01-0001 00:00:00" && t.IsFemale == true &&
                                  t.UtPaguthiEng.Contains(',') == false &&
                                  t.UtPaguthiEng.Contains("Others") == false &&
                                 t.UtPaguthiEng.Contains("Dont") == false
                               group t by t.UtPaguthiEng into newGrp
                               select new
                               {
                                   Key = newGrp.Key,
                                   Data = newGrp.ToList()
                               }).ToList();

                    recCount = allData.Sum(s => s.Data.Count);
                }
                else if (rdbMale.Checked)
                {
                    fg = "_M";
                    allData = (from t in TvdMember.GetAll().Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00")
                               where t.UpdatedTime.ToString() == "01-01-0001 00:00:00" && t.IsFemale == false &&
                                  t.UtPaguthiEng.Contains(',') == false &&
                                  t.UtPaguthiEng.Contains("Others") == false &&
                                 t.UtPaguthiEng.Contains("Dont") == false
                               group t by t.UtPaguthiEng into newGrp
                               select new
                               {
                                   Key = newGrp.Key,
                                   Data = newGrp.ToList()
                               }).ToList();
                    recCount = allData.Sum(s => s.Data.Count);
                }

                allData.OrderByDescending(o => o.Data.Count).ToList().ForEach(fe =>
                {
                    panNo = panNo + 1;

                    sb.AppendLine($"-------------------");
                    sb.AppendLine($"{panNo}. {fe.Data[0].UtPaguthi} ({fe.Data.Count})");
                    sb.AppendLine($"-------------------");

                    int panchMemberNo = 0;
                    fe.Data.OrderBy(o => o.Phone).ToList().ForEach(fed =>
                    {
                        panchMemberNo = panchMemberNo + 1;
                        sb.AppendLine($"({panchMemberNo})  {fed.Name} [ {fed.Phone} ]{Environment.NewLine}{fed.Address}");
                        sb.AppendLine(Environment.NewLine);
                    });

                });

                File.WriteAllText($@"F:\NTK\jawa - 2021\members\AllPanchayatMember{fg} ({recCount}).txt", sb.ToString());
                MessageBox.Show($"All Members Exported Successfully!");
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var file = ofd.FileName;
                var con = File.ReadAllText(file);

                string s = con.RemoveLines(2);

                s = "[" + Environment.NewLine + s;
                s = s.Remove(s.LastIndexOf('}'));

                s = s.Replace("\"உறுப்பினர் எண்\"", "\"MemberId\"")
                     .Replace("\"முகவரி\"", "\"Address\"")
                     .Replace("\"நாடு\"", "\"Country\"")
                     .Replace("\"மாநிலம்\"", "\"State\"")
                     .Replace("\"மாவட்டம்\"", "\"District\"")
                     .Replace("\"தொகுதி\"", "\"Assembly\"")
                     .Replace("\"பெயர்\"", "\"Name\"")
                     .Replace("\"தொடர்பு எண்\"", "\"Phone\"")
                     .Replace("\"மின்னஞ்சல்\"", "\"Email\"");


                var myList = JsonConvert.DeserializeObject<List<TvdMember>>(s).ToList();

                var existingMembersId = TvdMember.GetAll().Select(ss => ss.MemberId.Trim()).ToList();

                var allMembersId = myList.Select(ss => ss.MemberId.Trim()).ToList();


                var newMembersId = allMembersId.Except(existingMembersId).ToList();

                var newList = new List<TvdMember>();

                foreach (var item in newMembersId)
                {

                    if (existingMembersId.Contains(item))
                    {
                        MessageBox.Show($"{item} already exist");
                    }
                    else
                    {
                        var d = myList.Where(w => w.MemberId == item).ToList();

                        if (d.Count() == 1)
                        {
                            newList.Add(d.First());
                        }
                        else
                        {
                            MessageBox.Show($"{item} more than 1 member ids");
                        }
                    }

                }

                TvdMember.AddTvdMembers(newList);

                MessageBox.Show($"{newList.Count} new members added");

                LoadRec(0);

            }


        }

        private void button11_Click(object sender, EventArgs e)
        {
            //if (txtPhone.Text.Trim() != string.Empty)
            //{
            //////var data = assemblies.Where(w => w.Phone.EndsWith(txtPhone.Text)).ToList();

            //////if (rdbFem.Checked)
            //////{
            //////    data = assemblies.Where(w => w.IsFemale && w.UtPaguthiEng.Contains(selectedPan)).ToList();
            //////}
            //////else if (rdbMale.Checked)
            //////{
            //////    data = assemblies.Where(w => w.IsFemale == false && w.UtPaguthiEng.Contains(selectedPan)).ToList();
            //////}
            ///
            var data = assemblies.Where(w => w.UpdatedTime.ToString() != "01-01-0001 00:00:00").ToList();

            if (txtPhone.Text.Trim() != string.Empty)
            {
                data = assemblies.Where(w => w.Phone.EndsWith(txtPhone.Text)).ToList();
            }

           fData = new List<TvdMember>();
            fData = data.OrderByDescending(o => o.Money)
                .ThenBy(o => o.Phone)
                .ToList();

            dataGridView1.DataSource = fData;
            ColumnVisibility();
            ColumnOrder();
            LoadRec(fData.Count);

            if (dataGridView1.Rows.Count == 0) return;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Vote"];
            //cellCurrentValue = Transaction.GetLastTransactionAmount(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].DataBoundItem as Customer);
            //dataGridView1.CurrentCell.Value = cellCurrentValue;
            dataGridView1.BeginEdit(true);

            //}

        }


        bool IsEnterKey = false;
        private void EditSuccess()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.LightGreen;
            dataGridView1.CurrentCell.Style.ForeColor = Color.White;
            this.dataGridView1.ClearSelection();
            IsEnterKey = false; // reset flag after ecery success edit.
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView grid = (sender as DataGridView);
            int rowIndex = grid.CurrentCell.RowIndex;
            string owningColumnName = grid.CurrentCell.OwningColumn.Name;
            string cellValue = FormGeneral.GetGridCellValue(grid, rowIndex, owningColumnName);
            TvdMember cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as TvdMember;



            if (owningColumnName != "UtPaguthiEng" && (string.IsNullOrEmpty(cellValue) || cellValue == "0"))
            {
                EditCancel();
                return;
            }

            var updatedMember = new TvdMember()
            {
                MemberId = cus.MemberId
            };

            if (owningColumnName == "WantsMeet")
            {
                TvdMember.UpdateMeets(cus.MemberId, Convert.ToBoolean(cellValue));
            }
            else if (owningColumnName == "VVIP")
            {
                TvdMember.UpdateVVIP(cus.MemberId, Convert.ToBoolean(cellValue));
            }

            else if (owningColumnName == "Money")
            {
                TvdMember.UpdateMoney(cus.MemberId, Convert.ToBoolean(cellValue));
            }

            else if (owningColumnName == "Campaign")
            {
                TvdMember.UpdateCampaign(cus.MemberId, Convert.ToBoolean(cellValue));
            }

            else if (owningColumnName == "Vehicle")
            {
                TvdMember.UpdateVehicle(cus.MemberId, Convert.ToBoolean(cellValue));
            }

            else if (owningColumnName == "BoothAgent")
            {
                TvdMember.UpdateBoothAgent(cus.MemberId, Convert.ToBoolean(cellValue));
            }

            else if (owningColumnName == "Vote")
            {
                TvdMember.UpdateVotes(cus.MemberId, cellValue.ToInt32());
            }

            else if (owningColumnName == "NoMore")
            {
                TvdMember.UpdateNoMore(cus.MemberId, Convert.ToBoolean(cellValue));
            }
            

            else if (owningColumnName == "JustTalk")
            {
                TvdMember.UpdateTalk(cus.MemberId, Convert.ToBoolean(cellValue));
            }
            else if (owningColumnName == "UtPaguthiEng")
            {

                if (cellValue != null && utPaguthiList.Select(s => s.Display).Contains(cellValue) == false)
                {
                    MessageBox.Show("Wrong Panchayats!");
                    return;
                }

                TvdMember.UpdateUtEngPaguthi(cus.MemberId, cellValue ?? "");
            }

            EditSuccess();
        }

        private void EditCancel()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.Red;
            dataGridView1.CurrentCell.Style.ForeColor = Color.Yellow;
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            IsEnterKey = (keyData == Keys.Enter);
            return false;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TvdMember> data = TvdMember.GetAll();
            string detail = "";

            //if (checkBox1.Checked)
            //{
            //    detail = "தொகுதியில்";
            //}
            //else
            //{
            //    var selectedPan = (comboBox1.SelectedItem as Pair).Display;
               //data = data.Where(w => w.UtPaguthiEng.Contains(selectedPan)).ToList();
            //    detail = (comboBox1.SelectedItem as Pair).DisplayTamil + "-யில்";
            //}

            var value = ((KeyValuePair<int, string>)comboBox3.SelectedItem).Key;
            List<TvdMember> searchedMember = null;

            int day = 14;
            if (value == 1)
            {
                searchedMember = data.OrderByDescending(o => o.Vote).ToList();

                lblDetails.Text = $"{detail} {searchedMember.Sum(s => s.Vote)} ஓட்டுகள் உறுதியானது.";
            }
            else if (value == 2)
            {
                searchedMember = data.Where(w => w.Money).ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுகள் நிதி அளிக்க விரும்பிகிறார்கள்.";
            }
            else if (value == 3)
            {
                searchedMember = data.Where(w => w.WantsMeet).ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுகள் வேட்பாளரை  சந்திக்க விரும்பிகிறார்கள்.";
            }
            else if (value == 4)
            {
                searchedMember = data.Where(w => w.UpdatedTime.ToString() == "01-01-0001 00:00:00").ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுககளை தொடர்புகொள்ளவில்லை.";
            }
            else if (value == 15)
            {
                searchedMember = data.Where(w => w.UpdatedTime.ToString() != "01-01-0001 00:00:00").ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுககளை தொடர்பு கொண்டு பேசிவிட்டோம்!!.";
            }

            else if (value == 16)
            {
                searchedMember = data.Where(w => w.VVIP).ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுககளை கண்டிப்பாக தொடர்பு கொள்ளுங்கள்!!!";
            }
            else if (value == 17)
            {
                searchedMember = data.Where(w => w.JustTalk).ToList();
                lblDetails.Text = $"{detail} {searchedMember.Count} உறவுகளை தொடர்புகொள்ளுங்கள்!!!";
            }
            else if (value == 5 || value == 6)
            {
                var localData = (from d in data
                                 where
                                 d.UtPaguthiEng.Contains(',') == false &&
                                 d.UtPaguthiEng.Contains("Others") == false &&
                                 d.UtPaguthiEng.Contains("Dont") == false

                                 group d by d.UtPaguthiEng into ng
                                 select new { panchayat = ng.Key, MembersCount = ng.Count() }).ToList();

                var noMemberPanchayat = utPaguthiList.Where(
                    w => w.Display.Contains(',') == false &&
                    w.Display.Contains("Others") == false &&
                    w.Display.Contains("Dont") == false).Select(s => s.Display).ToList().Except(localData.Select(d => d.panchayat)).ToList();

                noMemberPanchayat.ForEach(fe => { localData.Add(new { panchayat = fe, MembersCount = 0 }); });

                if (value == 5)
                    localData = localData.OrderByDescending(o => o.MembersCount).ToList();
                else if (value == 6)
                    localData = localData.OrderBy(o => o.MembersCount).ToList();
                else
                    localData = localData.OrderBy(o => o.panchayat).ToList();

                localData.Insert(0, new { panchayat = "Multple", MembersCount = data.Where(d => d.UtPaguthiEng.Contains(',')).Count() });
                localData.Insert(0, new { panchayat = "Others", MembersCount = data.Where(d => d.UtPaguthiEng.Contains("Others")).Count() });
                localData.Insert(0, new { panchayat = "DontKnow", MembersCount = data.Where(d => d.UtPaguthiEng.Contains("Dont")).Count() });

                dataGridView1.DataSource = localData;

                lblDetails.Text = $"{localData.Count} ஊராட்சியின் உறுப்பினர் எண்ணிக்கை!";
                return;
            }

            else if (value == 7)
            {

                var allData = (from a in TvdMember.GetAll()
                               group a by a.UtPaguthiEng into newGroup
                               select new { newGroup.Key, count = newGroup.Count() }
                               ).OrderByDescending(o => o.count).ToList();

                StringBuilder sb = new StringBuilder();
                int i = 0;

                dataGridView1.DataSource = allData;
            }
            else if (value == 8)
            {
                dataGridView1.DataSource = (from d in data
                                            group d by d.Phone.Trim() into ng
                                            select new
                                            {
                                                Phone = ng.Key,
                                                Count = ng.Count()
                                            }).Where(w => w.Count > 1).OrderByDescending(o => o.Count).ToList();


                lblDetails.Text = $"ஒரே தொடர்பு எண் கொண்ட உறுப்பினர்கள்!";
                return;
            }
            else if (value == 9)
            {
                dataGridView1.DataSource = (from d in data
                                            group d by d.Address.Trim() into ng
                                            select new
                                            {
                                                Address = ng.Key,
                                                Count = ng.Count(),
                                                Panchayat = ng.First().UtPaguthiEng
                                            }).Where(w => w.Count > 1).OrderByDescending(o => o.Count).ToList();



                lblDetails.Text = $"ஒரே குடும்ப உறுப்பினர்கள்!";
                return;
            }

            else if (value == 10)
            {
                searchedMember = (from d in data
                                  where string.IsNullOrEmpty(d.Phone.Trim())
                                  select d).ToList();

                lblDetails.Text = $"தொடர்பு எண் இல்லாதவர்கள்!";

            }

            else if (value == 11)
            {
                searchedMember = (from d in data
                                  where d.IsFemale
                                  select d).OrderBy(o => o.UtPaguthiEng).ThenBy(t => t.Phone).ToList();

                lblDetails.Text = $"{searchedMember.Count} மகளிர் உறுப்பினர் உள்ளார்கள்";
            }


            else if (value == 12)
            {
                dataGridView1.DataSource = (from d in data
                                            where d.IsFemale
                                            group d by d.UtPaguthi into ng
                                            select new
                                            {
                                                Area = ng.Key,
                                                Count = ng.Count()
                                            }).OrderByDescending(o => o.Count).ToList();

                lblDetails.Text = $"Area wise - மகளிர் உறுப்பினர் உள்ளார்கள்";
                return;

            }

            else if (value == 13 || value == 14)
            {
                var myLocalData = from d in data
                                  group d by d.PaguthiEng.Trim() into ng
                                  select new
                                  {
                                      Ondrium = ng.Key,
                                      Panchayat = ng.Where(w => w.UtPaguthiEng.Contains(',') == false).DistinctBy(d => d.UtPaguthiEng.Trim()).ToList().Count,
                                      SplitCount = ng.Where(w => w.UtPaguthiEng.Contains(',') == false).Count() + "+" + ng.Where(w => w.UtPaguthiEng.Contains(',') == true).Count(),
                                      TotalCount = ng.Count()
                                  };

                if (value == 13) dataGridView1.DataSource = myLocalData.OrderByDescending(o => o.TotalCount).ToList();
                else dataGridView1.DataSource = myLocalData.OrderByDescending(o => o.Panchayat).ToList();

                lblDetails.Text = $"ஒன்றியம் வாரியாக உறுப்பினர் எண்ணிக்கை!";
                return;
            }
            else if (value == 31)
            {
                searchedMember = data.Where(w => 
                w.UpdatedTime.ToString() != "01-01-0001 00:00:00" &&
                w.UpdatedTime.Day <= day &&
                w.Money == true).ToList();
                LoadRec(searchedMember.Count);

                //new KeyValuePair<int, string>(31, "MO"),
                //   new KeyValuePair<int, string>(32, "PO"),
                //   new KeyValuePair<int, string>(33, "VEO"),
            }
            else if (value == 32)
            {
                searchedMember = data.Where(w =>
                w.UpdatedTime.ToString() != "01-01-0001 00:00:00" &&
                w.UpdatedTime.Day <= day &&
                w.Campaign == true).ToList();
                LoadRec(searchedMember.Count);
                

            }

            else if (value == 33)
            {
                searchedMember = data.Where(w =>
                w.UpdatedTime.ToString() != "01-01-0001 00:00:00" &&
                w.UpdatedTime.Day <= day &&
                w.Vehicle == true).ToList();
                LoadRec(searchedMember.Count);
                
            }

            else if (value == 34)
            {
                searchedMember = data.Where(w =>
                w.UpdatedTime.ToString() != "01-01-0001 00:00:00" &&
                w.UpdatedTime.Day <= day &&
                w.Campaign == true).ToList();
                LoadRec(searchedMember.Count);
               
            }

            dataGridView1.DataSource = searchedMember;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var file = ofd.FileName;
                var con = File.ReadAllText(file);

                string s = con.RemoveLines(2);

                s = "[" + Environment.NewLine + s;
                s = s.Remove(s.LastIndexOf('}'));

                s = s.Replace("\"உறுப்பினர் எண்\"", "\"MemberId\"")
                     .Replace("\"முகவரி\"", "\"Address\"")
                     .Replace("\"நாடு\"", "\"Country\"")
                     .Replace("\"மாநிலம்\"", "\"State\"")
                     .Replace("\"மாவட்டம்\"", "\"District\"")
                     .Replace("\"தொகுதி\"", "\"Assembly\"")
                     .Replace("\"பெயர்\"", "\"Name\"")
                     .Replace("\"தொடர்பு எண்\"", "\"Phone\"")
                     .Replace("\"மின்னஞ்சல்\"", "\"Email\"");


                var myList = JsonConvert.DeserializeObject<List<TvdMember>>(s).ToList();

                var femMemId = myList.Select(ss => ss.MemberId.Trim()).ToList();

                TvdMember.BulkUpdateFemaleFlag(femMemId);

                MessageBox.Show($"{femMemId.Count} female flag updated!");

                LoadRec(femMemId.Count);

            }
        }

        private void btnUpdatePE_Click(object sender, EventArgs e)
        {
            var ourData = TvdMember.GetAll();

            List<TvdMember> needCorrectionData = new List<TvdMember>();

            ourData.ForEach(fe =>
            {

                var dd = utPaguthiList.Where(ww => ww.Display == fe.UtPaguthiEng).FirstOrDefault();

                if (dd != null)
                {
                    var correctData = paguthiList.Where(w => w.Value == dd.Value).First();

                    fe.PaguthiEng = correctData.Display;
                    fe.Paguthi = correctData.DisplayTamil;

                    needCorrectionData.Add(fe);
                }

            });

            TvdMember.BulkUpdatePaguthiEng(needCorrectionData);

        }

        bool hasInternet = false;

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {

            try
            {
                hasInternet = CheckForInternetConnection();

                if (hasInternet == false)
                {
                    if (DialogResult.No == MessageBox.Show("No Internet Available, Please Connect to your WiFi", "", MessageBoxButtons.YesNo))
                        return;
                }

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (s, ee) =>
                {


                    SendEmailForSendBalance();

                    //AppCommunication.SendBalanceEmail(allBalances, currentBalanceDate, activeCus.Count(), "Jeyam Finance Balance Report");
                    MessageBox.Show("Balance Report have been send to your email");
                };
                bw.RunWorkerAsync();



            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void SendEmailForSendBalance()
        {

            if (hasInternet == false) return;
            var htmlString = FileContentReader.MemberContactHtml;


            var total = TvdMember.GetCount();
            var cn = TvdMember.ConAndNot();
            var vote = TvdMember.TotalExpectedVote();
            var meet = TvdMember.MeetCount();
            var note = TvdMember.NithiCount();

            var meetList = string.Join(Environment.NewLine, TvdMember.MeetContact().Select(s => $"{s.Name} - {s.Phone}"));
            var noteList = string.Join(Environment.NewLine, TvdMember.NithiContact().Select(s => $"{s.Name} - {s.Phone}"));

            StringBuilder rowData = new StringBuilder();

            var dat = DateTime.Today.ToShortDateString();

            var dailyCheckHTML = htmlString
                                .Replace("[DATE]", dat)
                                .Replace("[TOTAL]", total)
                                .Replace("[CONTACTED]", cn.C)
                                .Replace("[TOBECONTACT]", cn.NC)
                                .Replace("[VOTE]", vote)
                                .Replace("[MEET]", meet)
                                .Replace("[MEETNO]", meetList)
                                .Replace("[NU]", note)
                                .Replace("[NUNO]", noteList);

            if (hasInternet)
            {

                //var smtp = General.GetMailMessage($"{dat} : {total} - திருவாடானை உறுப்பினர் தகவல்",
                //    dailyCheckHTML,
                //    "ntkthiruvadanai@gmail.com",
                //    "ntkthiruvadanai210");


                var smtp = General.GetMailMessage($"{dat} : {total} - திருவாடானை உறுப்பினர் தகவல்",
                    dailyCheckHTML);

                try
                {
                    smtp.Item2.Send(smtp.Item1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }



            }

        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [உறுப்பினர் தகவல்$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable 

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return dtexcel;
        }

        private void button10_Click(object sender, EventArgs e)
        {

            string ass = "";

            var allData = new List<TvdMember>();

            if (chkExcel.Checked)
            {
                ass = "Rmd";
                string filePath = string.Empty;
                string fileExt = string.Empty;
                OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
                if (file.ShowDialog() == DialogResult.OK) //if there is a file choosen by the user  
                {
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                    {
                        try
                        {
                            DataTable dtExcel = new DataTable();
                            dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                            //dataGridView1.Visible = true;
                            //dataGridView1.DataSource = dtExcel;

                            List<TvdMember> studentList = new List<TvdMember>();
                            for (int d = 1; d < dtExcel.Rows.Count; d++)
                            {
                                TvdMember mem = new TvdMember();
                                mem.Phone = Convert.ToString(dtExcel.Rows[d][2]); // "தொடர்பு எண்"
                                mem.Name = dtExcel.Rows[d][1].ToString(); // "பெயர்"
                                //mem.Address = dt.Rows[i]["Address"].ToString();
                                //mem.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                                studentList.Add(mem);
                            }

                            allData = studentList;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                    }
                }

            }
            else
            {
                allData = TvdMember.GetAll();

                ass = "Tvd";
            }

            int i = 1;

            allData.ForEach(fe =>
            {
                fe.Sno = i;
                i = i + 1;
            });

            int groupNo = 0;
            for (int j = 0; j < allData.Count; j += 250)
            {
                var slicedData = allData.Skip(j).Take(250).ToList();
                groupNo += 1;

                StringBuilder sb = new StringBuilder();
                slicedData.ForEach(fe =>
                {
                    sb.AppendLine($"BEGIN: VCARD");
                    sb.AppendLine($"VERSION:3.0");
                    sb.AppendLine($"KIND: org");
                    sb.AppendLine($"FN:NTK{groupNo}-{fe.Sno.ToString().PadLeft(3, '0')}-{fe.Name}");
                    sb.AppendLine($"TEL; type = Mobile:{fe.Phone}");
                    sb.AppendLine($"ORG: G{groupNo}");
                    sb.AppendLine($"END:VCARD");
                });

                File.WriteAllText($@"F:\NTK\jawa - 2021\members\All-{ass}Phone{slicedData[0].Sno.ToString().PadLeft(4, '0')}-{slicedData.Last().Sno.ToString().PadLeft(4, '0')}-XX{groupNo.ToString().PadLeft(2, '0')}.vcf", sb.ToString());

            }

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q) // char.IsDigit(e.KeyChar) &&
            {
                txtPhone.SelectAll();
                txtPhone.Focus();
            }
        }

       
    }

    public class Pair
    {
        public Pair()
        {

        }

        public Pair(int val, string dis)
        {
            Value = val;
            Display = dis;
        }

        public Pair(int val, string dis, string disTamil, int voteCount = 0, string sey = "")
        {
            Value = val;
            Display = dis;
            DisplayTamil = disTamil;
            Seyalaalar = sey;
            Vote = voteCount;
        }

        public int Value { get; set; }
        public string Display { get; set; }

        public string DisplayTamil { get; set; }

        public string Seyalaalar { get; set; }

        public int Vote { get; set; }

        public override string ToString()
        {
            return $"{Value}-{Display}-{DisplayTamil}";
        }
    }
}

