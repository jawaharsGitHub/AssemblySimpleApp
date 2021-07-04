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
using Common.ExtensionMethod;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucReports : UserControl
    {
        List<OtherPartyData> otherData = null;
        public ucReports()
        {
            InitializeComponent();

            LoadAssembly();

            cmbAssembly.DataSource = GetOptions();

            otherData = OtherPartyData.GetAll();
        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "DMK strong"),
                   new KeyValuePair<int, string>(2, "ADMK strong"),
                   new KeyValuePair<int, string>(3, "TTV strong"),
                   new KeyValuePair<int, string>(4, "BOTH STRONG"),
                   new KeyValuePair<int, string>(5, "BY PANCHAYAT NAME"),
                   new KeyValuePair<int, string>(6, ">>TTV"),
                   new KeyValuePair<int, string>(7, ">>MNM"),
                   new KeyValuePair<int, string>(8, "<<TTV"),
                   new KeyValuePair<int, string>(9, "<<MNM"),
                   new KeyValuePair<int, string>(10, "JOHN"),
                   new KeyValuePair<int, string>(11, "DEVA")

               };

            return myKeyValuePair;

        }

        private void LoadAssembly()
        {
            var district = Assembly.GetAll();
            district.Add(new Assembly(0, "ALL"));


            cmbAssembly.DataSource = district.OrderBy(o => o.AssemblyNo).ToList();
            this.cmbAssembly.SelectedIndexChanged += new System.EventHandler(this.cmbAssembly_SelectedIndexChanged_1);
            cmbAssembly.SelectedValue = 0; // _selectedAssembly == null ? 0 : _selectedAssembly.AssemblyNo;
        }


       private decimal GetPerc(int den, int nom)
        {

            return Math.Round((Convert.ToDecimal(den) / Convert.ToDecimal(nom)) * 100, 1);

        }

        private void cmbAssembly_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbAssembly.SelectedItem).Key;
            //List<OtherPartyData> searchedData = null;

            if (value == 1)
            {
                var localData = (from d in otherData
                                group d by d.ooratchi into ng
                                select new { 
                                    Ooratchi = ng.Key,
                                    Ondrium = ng.ToList().First().ondrium,
                                    DMK = ng.Sum(s => s.dmk), 
                                    ADMK = ng.Sum(s => s.admk),
                                    DIFF = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)) - GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                    DMKP = GetPerc(ng.Sum(s => s.dmk),ng.Sum(s => s.total)),
                                    ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                    TTV = ng.Sum(s => s.ttv),
                                    NTK = ng.Sum(s => s.ntkvote),
                                    MNM = ng.Sum(s => s.mnm),
                                    TOTAL = ng.Sum(s => s.total),
                                }
                                
                                ).ToList();

                var r = localData.OrderByDescending(o => (o.DMKP - o.ADMKP)).Where(w => w.DMK > w.ADMK && (w.DMKP - w.ADMKP) > 5).ToList();

                dataGridView1.DataSource = r;
                //lblDetails.Text = $"{detail} {searchedMember.Sum(s => s.Vote)} ஓட்டுகள் உறுதியானது.";
                
            }

            else if(value == 2)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     DIFF = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)) - GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     TTV = ng.Sum(s => s.ttv),
                                     NTK = ng.Sum(s => s.ntkvote),
                                     MNM = ng.Sum(s => s.mnm),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                var r = localData.OrderByDescending(o => (o.ADMKP - o.DMKP)).Where(w => w.ADMK > w.DMK && (w.ADMKP - w.DMKP) > 5).ToList();

                dataGridView1.DataSource = r;
            }

            else if (value == 3)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     TTV = ng.Sum(s => s.ttv),
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     DIFF = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)) - GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     NTK = ng.Sum(s => s.ntkvote),
                                     MNM = ng.Sum(s => s.mnm),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                var r = localData.Where(w => w.TTV > w.DMK && w.TTV > w.ADMK).OrderByDescending(o => (o.TTV)).ToList();

                dataGridView1.DataSource = r;
            }

            else if(value == 4)
            {
                    var localData = (from d in otherData
                                     group d by d.ooratchi into ng
                                     select new
                                     {
                                         Ooratchi = ng.Key,
                                         Ondrium = ng.ToList().First().ondrium,
                                         ADMK = ng.Sum(s => s.admk),
                                         DMK = ng.Sum(s => s.dmk),
                                         DIFF = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)) - GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                         ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                         DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                         TTV = ng.Sum(s => s.ttv),
                                         NTK = ng.Sum(s => s.ntkvote),
                                         MNM = ng.Sum(s => s.mnm),
                                         TOTAL = ng.Sum(s => s.total),
                                     }

                                    ).ToList();

                    var r = localData.OrderByDescending(o => (o.ADMKP - o.DMKP)).Where(w => (w.ADMKP - w.DMKP) <= 5 || (w.DMKP - w.ADMKP) <= 5).ToList();

                    dataGridView1.DataSource = r;
               

            }
            else if (value == 5)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     //DIFF = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)) - GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     TTV = ng.Sum(s => s.ttv),
                                     NTK = ng.Sum(s => s.ntkvote),
                                     MNM = ng.Sum(s => s.mnm),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                var r = localData.Where(w=> 
                (w.ADMKP - w.DMKP > 0 && w.ADMKP - w.DMKP <= 5) || 
                (w.DMKP - w.ADMKP > 0 && w.DMKP - w.ADMKP <= 5)).OrderBy(o => o.Ooratchi).ToList();

                dataGridView1.DataSource = r;


            }

            else if (value == 6 || value == 8)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     NTK = ng.Sum(s => s.ntkvote),
                                     TTV = ng.Sum(s => s.ttv),                                     
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),                                     
                                     MNM = ng.Sum(s => s.mnm),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                if(value == 6)
                {
                    dataGridView1.DataSource = localData.Where(w => w.NTK > w.TTV).OrderByDescending(o => o.NTK).ToList();
                }

                if (value == 8)
                {
                    dataGridView1.DataSource = localData.Where(w =>  w.NTK < w.TTV).OrderByDescending(o => o.TTV).ToList();
                }

            }

            else if (value == 7 || value == 9)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     NTK = ng.Sum(s => s.ntkvote),
                                     MNM = ng.Sum(s => s.mnm),
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     TTV = ng.Sum(s => s.ttv),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                if (value == 7)
                {
                    dataGridView1.DataSource = localData.Where(w => w.NTK > w.MNM).OrderByDescending(o => o.NTK).ToList();
                }

                if (value == 9)
                {
                    dataGridView1.DataSource = localData.Where(w => w.NTK < w.MNM).OrderByDescending(o => o.MNM).ToList();
                }


            }
            else if (value == 10)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     TTV = ng.Sum(s => s.ttv),
                                     NTK = ng.Sum(s => s.ntkvote),
                                     John = ng.Sum(s => s.john),
                                     Deva = ng.Sum(s => s.deva),
                                     JOHNP = GetPerc(ng.Sum(s => s.john), ng.Sum(s => s.total)),
                                     DEVAP = GetPerc(ng.Sum(s => s.deva), ng.Sum(s => s.total)),
                                     TOTAL = ng.Sum(s => s.total),
                                 }

                                ).ToList();

                var r = localData.OrderByDescending(o => o.JOHNP).ToList();

                dataGridView1.DataSource = r;


            }
            else if (value == 11)
            {
                var localData = (from d in otherData
                                 group d by d.ooratchi into ng
                                 select new
                                 {
                                     Ooratchi = ng.Key,
                                     Ondrium = ng.ToList().First().ondrium,
                                     ADMK = ng.Sum(s => s.admk),
                                     DMK = ng.Sum(s => s.dmk),
                                     ADMKP = GetPerc(ng.Sum(s => s.admk), ng.Sum(s => s.total)),
                                     DMKP = GetPerc(ng.Sum(s => s.dmk), ng.Sum(s => s.total)),
                                     TTV = ng.Sum(s => s.ttv),
                                     NTK = ng.Sum(s => s.ntkvote),
                                     John = ng.Sum(s => s.john),
                                     Deva = ng.Sum(s => s.deva),
                                     JOHNP = GetPerc(ng.Sum(s => s.john), ng.Sum(s => s.total)),
                                     DEVAP = GetPerc(ng.Sum(s => s.deva), ng.Sum(s => s.total)),
                                     TOTAL = ng.Sum(s => s.total)
                                 }

                                ).ToList();

                var r = localData.OrderByDescending(o => o.DEVAP).ToList();

                dataGridView1.DataSource = r;


            }

        }
    }


    
}
