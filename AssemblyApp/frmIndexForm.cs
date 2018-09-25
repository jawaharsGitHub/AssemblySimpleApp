﻿using CenturyFinCorpApp.UsrCtrl;
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

            ShowForm<uclAddMember>(); // initial form to be loaded
        }

        private void CreateMenu()
        {
            // Menu
            menuStrip = new MenuStrip
            {
                Location = new Point(0, 0),
                Name = "MenuStrip"
            };


            //Member
            var mnuMember = new ToolStripMenuItem() { Name = "addMember", Text = "ADD-MEMBER" };
            mnuMember.Click += (s, e) => ShowForm<uclAddMember>(); ;
            menuStrip.Items.Add(mnuMember);
            

            this.Controls.Add(menuStrip);
        }

        
        public void ShowForm<T>(Division cus = null) where T : UserControl, new()
        {

            T ac = new T();

            if (isAdded && panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }


            isAdded = true;
            panel1.Controls.Add(ac);
            ac.Show();
        }

    }
}
