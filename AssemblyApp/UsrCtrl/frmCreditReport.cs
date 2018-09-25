using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmCreditReport : UserControl
    {

        private static List<CreditReport> _fullCreditReport;

        public frmCreditReport()
        {
            InitializeComponent();
            comboBox1.DataSource = GetOptions();
        }


        private void CreditScore()
        {
            try
            {
                var data = (from c in Customer.GetAllCustomer()
                            select Customer.GetCreditScore(c)).ToList();

                _fullCreditReport = (from d in data
                                     group d by new { d.CustomerId } into ng
                                     select new CreditReport()
                                     {
                                         Count = ng.ToList().Count,
                                         CustomerId = ng.Key.CustomerId,
                                         Name = ng.ToList().First().Name,
                                         CreditScore = ng.ToList().Average(s => s.CreditScore).RoundPoints(),
                                         InterestRate = ng.ToList().Average(s => s.InterestRate).RoundMoney(),
                                         PercGainPerMonth = ng.ToList().Average(s => s.PercGainPerMonth).RoundMoney(),
                                         InterestPerMonth = ng.ToList().Average(s => s.InterestPerMonth).RoundMoney(),
                                         DaysTaken = ng.ToList().Average(s => s.DaysTaken).ToInt32(),
                                         MissingDays = ng.ToList().Average(s => s.MissingDays).ToInt32()
                                     }).OrderBy(o => o.CreditScore).ToList();

                dataGridView1.DataSource = _fullCreditReport;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BadCredit()
        {
            try
            {
                var data = (from c in _fullCreditReport
                            where c.CreditScore < 0
                            select c).OrderBy(o => o.CreditScore).ToList();

                dataGridView1.DataSource = data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GoodCredit()
        {
            try
            {
                var data = (from c in _fullCreditReport
                            where c.CreditScore >= 0
                            select c).OrderByDescending(o => o.CreditScore).ToList();

                dataGridView1.DataSource = data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "CREDIT REPORT"),
                   new KeyValuePair<int, string>(2, "BAD CREDIT"),
                   new KeyValuePair<int, string>(3, "GOOD CREDIT")
               };

            return myKeyValuePair;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                CreditScore();
            }
            else if (value == 2)
            {
                BadCredit();
            }
            else if (value == 3)
            {
                GoodCredit();
            }
        }
    }
}
