using CenturyFinCorpApp.UsrCtrl;
using Common;
using DataAccess;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmIndexForm : Form
    {

        bool usingMenu = false;
        bool isAdded = false; // for child forms
        public static MenuStrip menuStrip;

        public frmIndexForm()
        {

            InitializeComponent();
            this.Text = "நாம் தமிழர் - 2019";
            this.AutoScrollOffset = new Point(0, 0);

            CreateMenu();

            panel1.Width = 1300;
            panel1.Height = this.Height;

            ShowForm<ucTvdMember>(); // initial form to be loaded

            //ShowForm<ucReports>(); // initial form to be loaded
        }

        private void CreateMenu()
        {
            // Menu
            menuStrip = new MenuStrip
            {
                Location = new Point(0, 0),
                Name = "MenuStrip"
            };




            var mnuZonal = new ToolStripMenuItem() { Name = "zonal", Text = "மண்டலம்" };
            mnuZonal.Click += (s, e) => ShowForm<ucZonal>(); ;
            menuStrip.Items.Add(mnuZonal);

            var mnuDistrict = new ToolStripMenuItem() { Name = "district", Text = "மாவட்டம்" };
            mnuDistrict.Click += (s, e) => ShowForm<ucDistrict>(); ;
            menuStrip.Items.Add(mnuDistrict);


            var mnuAssembly = new ToolStripMenuItem() { Name = "assembly", Text = "சட்டமன்ற-தொகுதி" };
            mnuAssembly.Click += (s, e) => ShowForm<ucAssembly>(); ;
            menuStrip.Items.Add(mnuAssembly);

            var mnuPanchayat = new ToolStripMenuItem() { Name = "kilai", Text = "பஞ்சாயத்து-கிளைகள்" };
            mnuPanchayat.Click += (s, e) => ShowForm<ucPanchayat>(); ;
            menuStrip.Items.Add(mnuPanchayat);

            var mnuPollingStation = new ToolStripMenuItem() { Name = "pollingstation", Text = "POLLING_STATION" };
            mnuPollingStation.Click += (s, e) => ShowForm<ucPollingStation>(); ;
            menuStrip.Items.Add(mnuPollingStation);



            var mnuBlock = new ToolStripMenuItem() { Name = "block", Text = "ஒன்றியம்" };
            mnuBlock.Click += (s, e) => ShowForm<ucBlock>(); ;
            menuStrip.Items.Add(mnuBlock);   


            //Member
            var mnuMember = new ToolStripMenuItem() { Name = "addMember", Text = "பொறுப்பாளர்கள்" };
            mnuMember.Click += (s, e) => ShowForm<uclAddMember>(); ;
            menuStrip.Items.Add(mnuMember);

            //var mnuDataHelper = new ToolStripMenuItem() { Name = "dataHelper", Text = "DATA-HELPER" };
            //mnuDataHelper.Click += (s, e) => ShowForm<DataHelper>(); ;
            //menuStrip.Items.Add(mnuDataHelper);

            var mnuInternational = new ToolStripMenuItem() { Name = "international", Text = "Member-Verification" };
            mnuInternational.Click += (s, e) => ShowForm<ucMemberVerify>(); ;
            menuStrip.Items.Add(mnuInternational);

            var mnuLocalbody = new ToolStripMenuItem() { Name = "localbody", Text = "உள்ளாட்சி-தேர்தல்-2020" };
            mnuLocalbody.Click += (s, e) => ShowForm<ucLocalBody>(); ;
            menuStrip.Items.Add(mnuLocalbody);

            var mnuAnalysis = new ToolStripMenuItem() { Name = "localbody", Text = "2016-Election" };
            mnuAnalysis.Click += (s, e) => ShowForm<ucVotes>();
            menuStrip.Items.Add(mnuAnalysis);

            var mnuVoterAnalysis = new ToolStripMenuItem() { Name = "VoteAnalysis", Text = "Vote-Analysis" };
            mnuVoterAnalysis.Click += (s, e) => ShowForm<ucVoterAnalysis>(); ;
            menuStrip.Items.Add(mnuVoterAnalysis);

            var mnuVoters = new ToolStripMenuItem() { Name = "voters", Text = "VOTERS-Rmm" };
            mnuVoters.Click += (s, e) => ShowForm<ucVoters>(); ;
            menuStrip.Items.Add(mnuVoters);


            var mnuTvd = new ToolStripMenuItem() { Name = "TVD", Text = "TVD" };
            mnuTvd.Click += (s, e) => ShowForm<ucTvdMember>(); ;
            menuStrip.Items.Add(mnuTvd);

            var mnuReports = new ToolStripMenuItem() { Name = "reports", Text = "Reports" };
            mnuReports.Click += (s, e) => ShowForm<ucReports>(); ;
            menuStrip.Items.Add(mnuReports);

            

            this.Controls.Add(menuStrip);
        }


        public void ShowForm<T>(object cus = null) where T : UserControl
        {

            //UserControl ac = null;

            if (panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }

            if (typeof(T) == typeof(ucZonal))
            {
                ucZonal ucZ = new ucZonal();
                panel1.Controls.Add(ucZ);
            }
            else if (typeof(T) == typeof(ucDistrict))
            {
                var zonal = (Zonal)cus;
                var ucd = new ucDistrict(zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucAssembly))
            {
                var dist = (ZonalDistrict)cus;
                var ucd = new ucAssembly(dist); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucBlock))
            {
                //var zonal = (Assembly)cus;
                var ucd = new ucBlock(); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucBlock))
            {
                //var zonal = (Assembly)cus;
                var ucd = new ucBlock(); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucPanchayat))
            {
                var da = (DistrictAssembly)cus;
                var ucd = new ucPanchayat(da); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(uclAddMember))
            {
                //var da = (DistrictAssembly)cus;
                var ucd = new uclAddMember(); // (zonal);
                panel1.Controls.Add(ucd);
            }

            else if (typeof(T) == typeof(ucVotes))
            {
                //var da = (DistrictAssembly)cus;
                var ucd = new ucVotes(); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucPollingStation))
            {
                //var da = (DistrictAssembly)cus;
                var ucd = new ucPollingStation(); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucReports))
            {
                //var da = (DistrictAssembly)cus;
                var ucd = new ucReports(); // (zonal);
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucVoters))
            {
                var ucd = new ucVoters(); 
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucMemberVerify))
            {
                var ucd = new ucMemberVerify(); 
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucLocalBody))
            {
                var ucd = new ucLocalBody();
                panel1.Controls.Add(ucd);
            }
            else if (typeof(T) == typeof(ucVoterAnalysis))
            {
                var ucd = new ucVoterAnalysis();
                panel1.Controls.Add(ucd);
            }

            else if (typeof(T) == typeof(ucTvdMember))
            {
                var ucd = new ucTvdMember();
                panel1.Controls.Add(ucd);
            }
        }

    }
}
