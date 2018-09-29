using CenturyFinCorpApp.UsrCtrl;
using Common;
using DataAccess;
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

            //var customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();


            //var activeTxn = customers.Count(c => c.IsActive == true);
            //var closedTxn = customers.Count(c => c.IsActive == false);
            //var totalTxn = activeTxn + closedTxn;

            this.Text = "WELCOME - 2019 NTK";


            //this.TopMost = true;
            this.AutoScrollOffset = new Point(0, 0);


            CreateMenu();

            panel1.Width = 1300;
            panel1.Height = this.Height;

            ShowForm<ucZonal>(); // initial form to be loaded
        }

        private void CreateMenu()
        {
            // Menu
            menuStrip = new MenuStrip
            {
                Location = new Point(0, 0),
                Name = "MenuStrip"
            };




            var mnuZonal = new ToolStripMenuItem() { Name = "zonal", Text = "ZONAL" };
            mnuZonal.Click += (s, e) => ShowForm<ucZonal>(); ;
            menuStrip.Items.Add(mnuZonal);

            var mnuDistrict = new ToolStripMenuItem() { Name = "district", Text = "DISTRICT(SUB-ZONAL)" };
            mnuDistrict.Click += (s, e) => ShowForm<ucDistrict>(); ;
            menuStrip.Items.Add(mnuDistrict);


            var mnuAssembly = new ToolStripMenuItem() { Name = "assembly", Text = "ASSEMBLY" };
            mnuAssembly.Click += (s, e) => ShowForm<ucAssembly>(); ;
            menuStrip.Items.Add(mnuAssembly);

            var mnuBlock = new ToolStripMenuItem() { Name = "block", Text = "UNION-BLOCK" };
            mnuBlock.Click += (s, e) => ShowForm<ucBlock>(); ;
            menuStrip.Items.Add(mnuBlock);

            var mnuPanchayat = new ToolStripMenuItem() { Name = "kilai", Text = "PANCHAYAT" };
            mnuPanchayat.Click += (s, e) => ShowForm<ucPanchayat>(); ;
            menuStrip.Items.Add(mnuPanchayat);






            //Member
            var mnuMember = new ToolStripMenuItem() { Name = "addMember", Text = "ADD-MEMBER" };
            mnuMember.Click += (s, e) => ShowForm<uclAddMember>(); ;
            menuStrip.Items.Add(mnuMember);

            var mnuDataHelper = new ToolStripMenuItem() { Name = "dataHelper", Text = "DATA-HELPER" };
            mnuDataHelper.Click += (s, e) => ShowForm<DataHelper>(); ;
            menuStrip.Items.Add(mnuDataHelper);

            var mnuInternational = new ToolStripMenuItem() { Name = "international", Text = "INTER-NATIONAL" };
            mnuInternational.Click += (s, e) => ShowForm<ucInternational>(); ;
            menuStrip.Items.Add(mnuInternational);

            var mnuLocalbody = new ToolStripMenuItem() { Name = "localbody", Text = "LOCAL-BODY" };
            mnuLocalbody.Click += (s, e) => ShowForm<ucLocalBody>(); ;
            menuStrip.Items.Add(mnuLocalbody);


            this.Controls.Add(menuStrip);
        }


        public void ShowForm<T>(BaseClass cus = null) where T : UserControl
        {

            //UserControl ac = null;

            if (panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }

            if (cus == null)
            {
                isAdded = false;
                //T t = new T();
                ucZonal ucZ = new ucZonal();
                panel1.Controls.Add(ucZ);
                ucZ.Show();
            }
            

            if (typeof(T) == typeof(ucDistrict))
            {
                var zonal = (Zonal)cus;
                var ucd = new ucDistrict(zonal);
                //ucd._selectedZonal = zonal;
                isAdded = true;
                panel1.Controls.Add(ucd);
                ucd.Show();
            }

           

        }

    }
}
