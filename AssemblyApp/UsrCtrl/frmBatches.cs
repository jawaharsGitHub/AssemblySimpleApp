using Common;
using DataAccess.PrimaryTypes;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmBatches : UserControl
    {

        private string DailyBatchFile = AppConfiguration.DailyBatchFile;
        public frmBatches()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Directory.Exists(DailyBatchFile) == false)
            {
                Directory.CreateDirectory(DailyBatchFile);
            }

            var firstDate = new DateTime(2018, 1, 25);
            var lastDate = DateTime.Today.Date.AddDays(-1);


            while (firstDate <= lastDate)
            {
                // Search active and closed txn file.

                //1. Search Transaction.json
                var activeTxn = Transaction.GetTransactionForDate(lastDate);

                //2. Search Closed Notes.
                var closedTxn = Transaction.GetClosedTransactionForDate(lastDate);

                activeTxn.AddRange(closedTxn);

                Transaction.AddBatchTransactions(activeTxn, $"{lastDate.ToString("dd-MM-yyyy")}");

                // Set to previous day.
                lastDate = lastDate.AddDays(-1);
            }

            var option = MessageBox.Show("Generated Daily Txns - Completed!", "Daily Txn", MessageBoxButtons.OKCancel);

            if (option == DialogResult.OK)
                Process.Start(DailyBatchFile);
        }

        private void btnYearlyBatch_Click(object sender, EventArgs e)
        {
            // Need to create static data instead of each time creating dynamic data.
            // eg: Credit Report.
            // Flexible to use if need the closed txns.
            // CLsoed Txns.
        }
    }
}
