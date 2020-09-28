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


            dataGridView1.Columns["State"].Visible = false;
            dataGridView1.Columns["District"].Visible = false;
            dataGridView1.Columns["Assembly"].Visible = false;
            dataGridView1.Columns["MemberId"].Visible = false;
            dataGridView1.Columns["Name"].Visible = false;
            dataGridView1.Columns["Phone"].Visible = false;
            dataGridView1.Columns["Email"].Visible = false;

            dataGridView1.Columns["Paguthi"].Visible = false;
            dataGridView1.Columns["UtPaguthi"].Visible = false;

            dataGridView1.Columns["PaguthiEng"].Visible = false;
            //dataGridView1.Columns["UtPaguthiEng"].Visible = false;
            //dataGridView1.Columns["UtPaguthiEng"].DisplayIndex = 3;

            dataGridView1.Columns["Country"].Visible = false;

            dataGridView1.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            comboBox1.DataSource = GetUtPaguthi();
            comboBox1.DisplayMember = "Display";
            comboBox1.ValueMember = "DisplayTamil";

        }


        private void LoadGrid()
        {
            assemblies = TvdMember.GetAll().ToList();
            int i = 1;
            assemblies.ForEach(f => f.Sno = i++);
            dataGridView1.DataSource = assemblies;
        }
        private List<Pair> GetPaguthi()
        {

            List<Pair> paguthi = new List<Pair>()
            {
                new Pair(1,"Rsm-K", "ஆர்.எஸ்.மங்களம் - கிழக்கு"),
                new Pair(2,"Rsm-M", "ஆர்.எஸ்.மங்களம் - மேற்கு"),
                new Pair(3,"Tvd-T","திருவாடானை - தெற்கு"),
                new Pair(4,"Tvd-K", "திருவாடானை - கிழக்கு"),
                new Pair(5,"Tvd-M", "திருவாடானை - மேற்கு"),
                new Pair(6,"Tvd-V", "திருவாடானை - வடக்கு"),
                new Pair(7,"Rmd-K", "இராம்நாடு - கிழக்கு"),
                new Pair(8,"Rmd-m", "இராம்நாடு - மேற்கு"),
                new Pair(9,"RSMNagar", "ஆர்.எஸ்.மங்களம் நகர்"),
                new Pair(10,"Thondi", "தொண்டி")
            };

            return paguthi;
        }


        private List<Pair> GetUtPaguthi()
        {

            List<Pair> utpaguthi = new List<Pair>()
            {

                new Pair(9,"RSM-Nagar","ஆர்.எஸ்.மங்களம் நகர்"),
                new Pair(10,"Thondi","தொண்டி"),


                new Pair(1,"Manakkudi", "அ.மணக்குடி"),
                new Pair(1,"Alagarthevankottai","அழகர்தேவன்கோட்டை "),
                new Pair(1,"Siththoorvaadi","சித்தூர்வாடி "),
                new Pair(1,"Kadaloore","கடலூர் "),
                new Pair(1,"Kallikkudi","கள்ளிக்குடி "),
                new Pair(1,"Karungudi","கருங்குடி "),
                new Pair(1,"Koththidal","கொத்திடல் "),
                new Pair(1,"Kottakkudi","கொட்டக்குடி "),
                new Pair(1,"Paaranoor","பாரனூர் "),
                new Pair(1,"Pichchankurichi","பிச்சங்குறிச்சி "),
                new Pair(1,"Senkudi","செங்குடி "),
                new Pair(1,"Solanthoore","சோழந்தூர் "),
                new Pair(1,"Thiruppaalakudi","திருப்பாலைக்குடி "),
                new Pair(1,"Thumbadaikkakottai","தும்படைக்காகோட்டை "),
                new Pair(1,"Ooranagkudi","ஊரணங்குடி "),
                new Pair(1,"Varavani","வரவணி "),
                new Pair(1,"KavanoorRsm","காவனூர்-RSM"),

                // RSM-Merku
                new Pair(2,"A.R.Mangalam", "A.R.மங்களம்"),
                new Pair(2,"Ananthoor","ஆனத்தூர் "),
                new Pair(2,"Ayangudi","ஆயங்குடி "),
                new Pair(2,"Govindamangalam","கோவிந்தமங்களம் "),
                new Pair(2,"Koodaloore","கூடலூர் "),
                new Pair(2,"Karkaaththakudi","கற்காத்தகுடி "),
                new Pair(2,"Kaavanakkottai","காவனக்கோட்டை "),
                new Pair(2,"Pullamadai","புள்ளமடை "),
                new Pair(2,"MelPanaiyoor","மேல்பனையூர் "),
                new Pair(2,"Odaikkaal","ஓடைக்கால் "),
                new Pair(2,"Radhanoor","இராதானூர் "),
                new Pair(2,"Sanaveli","சனவேலி "),
                new Pair(2,"Saththanoor","சத்தானூர் "),
                new Pair(2,"Seththidal","சேத்திடல் "),
                new Pair(2,"Sevvaipettai","செவ்வாய்பேட்டை "),
                new Pair(2,"Sirunakudi","சிருநாகுடி "),
                new Pair(2,"Thiruthervalai","திருத்தேர்வளை "),
                new Pair(2,"Vadakkaloor","வடக்கலூர்"),

                // TVD-Therku
                new Pair(3,"Theloor", "தேளூர் "),
                new Pair(3,"Thalirmarungoor","தளிர்மருங்கூர் "),
                new Pair(3,"Aathiyur","ஆதியூர் "),
                new Pair(3,"Arumboor","அரும்பூர் "),
                new Pair(3,"Kulaththoor","குளத்தூர் "),
                new Pair(3,"Thiruvetriyur","திருவெற்றியூர் "),
                new Pair(3,"Mugilthanagam","முகிழ்த்தகம் "),
                new Pair(3,"Nambuthalai","நம்புதாளை "),
                new Pair(3,"Puthupattinam","புதுப்பட்டினம் "),
                new Pair(3,"Mullimunai","முள்ளிமுனை "),
                new Pair(3,"Karangaadu","காரங்காடு"),

                // TVD-Kilakku
                new Pair(4,"Mangalakudi", "மங்களக்குடி "),
                new Pair(4,"Arasaththoor","அரசத்தூர் "),
                new Pair(4,"Nilamalagiyamangalam","நிலமழகியமங்களம் "),
                new Pair(4,"Kattivayal","கட்டிவயல் "),
                new Pair(4,"Kunjangulam","குஞ்சங்குளம் "),
                new Pair(4,"Anjukottai","அஞ்சுகோட்டை "),
                new Pair(4,"Kodanoor","கோடனூர் "),
                new Pair(4,"Pandukudi","பாண்டுகுடி "),
                new Pair(4,"Nagarikathan","நகரிகாத்தான் "),
                new Pair(4,"Kodipangu","கொடிப்பங்கு "),
                new Pair(4,"Maavoor","மாவூர் "),
                new Pair(4,"Achchangudi","அச்சங்குடி "),
                
                // TVD-MErku
                new Pair(5,"Neivayal", "நெய்வயல் "),
                new Pair(5,"karumoli","கருமொழி "),
                new Pair(5,"Palangulam","பழங்குளம் "),
                new Pair(5,"Kadamboor","கடம்பூர் "),
                new Pair(5,"Kookudi","கூகுடி "),
                new Pair(5,"Thuththaakudi","துத்தாக்குடி "),
                new Pair(5,"Sirumalaikottai","சிறுமலைக்கோட்டை "),
                new Pair(5,"T.Nagini","தி.நாகனி "),
                new Pair(5,"Orikottai","ஓரிக்கோட்டை "),
                new Pair(5,"Periakeeramangalam","பெரியகீரமங்களம் "),
                new Pair(5,"Kalloore","கல்லூர் "),
                new Pair(5,"Thiruvadanai","திருவாடானை "),
               
                // TVD-Vadakku
                new Pair(6,"Kattavilagam", "கட்டவிளாகம் "),
                new Pair(6,"Aandaavoorani","ஆண்டாவூரணி "),
                new Pair(6,"Paaganur","பாகனூர் "),
                new Pair(6,"Sirukambaiyur","சிறுகம்பையூர் "),
                new Pair(6,"Pathanakudi","பதனக்குடி "),
                new Pair(6,"Vallaiyaapuram","வெள்ளையபுரம் "),
                new Pair(6,"Oriyur","ஓரியூர் "),
                new Pair(6,"S.P.Pattinam","S.P. பட்டிணம்  "),
                new Pair(6,"Panachayal","பனஞ்சாயல் "),
                new Pair(6,"Vattanam","வட்டானம் "),
                new Pair(6,"Kaliyanagari","கலியநகரி "),
                new Pair(6,"Pullakadamban","புல்லக்கடம்பன் "),
               

                // RMD-Kilakku
                new Pair(7,"Alagankulam-MDB", "அழகன்குளம் (மண்டபம் ஒன்றியம்)"),
                new Pair(7,"Panaikulam-MDB","பனைக்குளம் (மண்டபம் ஒன்றியம்)"),
                new Pair(7,"Puthuvalasai-MDB","புதுவலசை (மண்டபம் ஒன்றியம்)"),
                new Pair(7,"Therbogi-MDB","தேர்போகி (மண்டபம் ஒன்றியம்)"),
                new Pair(7,"Athiyuthu","அத்தியூத்து "),
                new Pair(7,"Siththaarkottai","சித்தார்க்கோட்டை "),
                new Pair(7,"Devipattinam","தேவிப்பட்டிணம் "),
                new Pair(7,"Kalanikudi","கழினிக்குடி "),
                new Pair(7,"Vennathoor","வெண்ணத்தூர் "),
                new Pair(7,"Nafranamangalam","நாரணமங்கலம் "),
                new Pair(7,"Pandamangalam","பாண்டமங்களம் "),
                new Pair(7,"Maathavanur","மாதவனூர் "),
                new Pair(7,"Peravoor","பேராவூர் "),
                new Pair(7,"Ilamanur","இளமனூர் "),
                new Pair(7,"Kalugoorani","கழுகூரணி "),
                new Pair(7,"Madakottan","மாடகொட்டான் "),

                // RMD-Merku
                new Pair(8,"Kavanur-RMD", "காவனூர் "),
                new Pair(8,"Karukudi","காருகுடி "),
                new Pair(8,"Thoruvalur","தொருவலூர் "),
                new Pair(8,"Siththoor","சித்தூர் "),
                new Pair(8,"Kallikkudi","சூரங்கோட்டை "),
                new Pair(8,"Soorankottai","அச்சுந்தவயல் "),
                new Pair(8,"Puththenthal","புத்தேந்தல் "),
                new Pair(8,"PattinamKathanb-MDP","பட்டிணம்காத்தான்(மண்டபம் ஒன்றியம்)"),
                new Pair(8,"Sakkarakottai","சக்கரக்கோட்டை"),
                new Pair(8,"Therkutharavai","தெற்குதாரவை "),
                new Pair(8,"Lanthai-TPL","லாந்தை (திருப்புல்லாணி ஒன்றியம்)"),
                new Pair(8,"Ekkakudi-TPL","எக்கக்குடி (திருப்புல்லாணி ஒன்றியம்)"),
                new Pair(8,"Panaikulam-TPL","பனைக்குளம் (திருப்புல்லாணி ஒன்றியம்)"),
                new Pair(8,"Mallal-TPL","மல்லல் (திருப்புல்லாணி ஒன்றியம்)"),
                new Pair(8,"Peruvayal","பெறுவயல் "),
                new Pair(8,"Karenthal","காரேந்தல் "),
                new Pair(8,"Pullangudi","புல்லங்குடி "),


            };

            return utpaguthi.OrderBy(o => o.Display).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                var memberId = (r.DataBoundItem as TvdMember).MemberId;

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
            MessageBox.Show("Done");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var utp = (comboBox1.SelectedItem as Pair);

            //dataGridView1.DataSource = assemblies.Where(w => w.Address.Contains(utp.DisplayTamil.Trim())).ToList();

            //dataGridView1.SelectAll();

            //dataGridView1.DataSource = assemblies.Where(w => w.Address.Contains(textBox1.Text.Trim())).ToList();

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
        }
    }

    public class Pair
    {

        public Pair(int val, string dis)
        {
            Value = val;
            Display = dis;
        }

        public Pair(int val, string dis, string disTamil)
        {
            Value = val;
            Display = dis;
            DisplayTamil = disTamil;
        }

        public int Value { get; set; }
        public string Display { get; set; }

        public string DisplayTamil { get; set; }

        public override string ToString()
        {
            return $"{Value}-{Display}-{DisplayTamil}";
        }
    }
}
