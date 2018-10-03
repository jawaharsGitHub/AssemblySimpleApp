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
using DataAccess.ExtendedTypes;
using Common.ExtensionMethod;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class ucVotes : UserControl
    {

        List<AssemblyAndVotes> votes;
        public ucVotes()
        {
            InitializeComponent();

           votes = (from v in VoteData.GetAll()
                                                join a in Assembly.GetAll()
                                                on v.ACno equals a.AssemblyNo
                                                select new AssemblyAndVotes()
                                                {
                                                    ACno = a.AssemblyNo,
                                                    AssemblyName = a.AssemblyName,
                                                    CandidateName = v.CandidateName,
                                                    PartyAbbreviation = v.PartyAbbreviation,
                                                    PercVotesPolled = v.PercVotesPolled,
                                                    Rank = v.Rank,
                                                    VotesPolled = v.VotesPolled
                                                }).ToList();

            SetDataSource(votes);

            cmbAssembly.DataSource = Assembly.GetAll();

            LoadFilter();
        }

        private void LoadFilter()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By AssemblyNo"),
                   new KeyValuePair<int, string>(2, "Only NTK By Votes"),
                   new KeyValuePair<int, string>(3, "Winners"),
                   new KeyValuePair<int, string>(4, "NOTA only"),
                   new KeyValuePair<int, string>(5, "Only NTK By Votes %"),
                   //new KeyValuePair<int, string>(5, "By District Name"),

               };

            cmbFilters.DataSource = myKeyValuePair;
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)cmbFilters.SelectedItem).Key;
            List<AssemblyAndVotes> filteredVotes = votes;

            if (value == 1) filteredVotes = votes.OrderBy(o => o.ACno).ToList();

            else if (value == 2) filteredVotes = votes.Where(w => w.PartyAbbreviation.ToLower() == "ntk").OrderByDescending(o => o.VotesPolled).ToList();

            else if (value == 3) filteredVotes = votes.Where(w => w.Rank == 1).OrderByDescending(o => o.VotesPolled).ToList();

            else if (value == 4) filteredVotes = votes.Where(w => w.PartyAbbreviation.ToLower() == "nota").OrderByDescending(o => o.VotesPolled).ToList();

            else if (value == 5) filteredVotes = votes.Where(w => w.PartyAbbreviation.ToLower() == "ntk").OrderByDescending(o => o.PercVotesPolled).ToList();


            filteredVotes.ForEach(a =>
            {
                a.SNo = filteredVotes.IndexOf(a) + 1;

            });


            SetDataSource(filteredVotes);
        }

        private void cmbAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDataSource(votes.Where(w => w.ACno == cmbAssembly.SelectedValue.ToInt32()).ToList());

        }


        private void SetDataSource(List<AssemblyAndVotes> va)
        {

            dataGridView1.DataSource = va;

            label1.Text = $"Row Count: {va.Count} {Environment.NewLine} TotalVotes : {va.Sum(c => c.VotesPolled)}";

        }
    }
}
