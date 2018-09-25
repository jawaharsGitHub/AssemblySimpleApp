using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmInHand : UserControl
    {

        private DailyCollectionDetail dailyTxn;

        //private int givenAmount;
        //private int interest;
        public frmInHand()
        {
            InitializeComponent();
            GetDailyTxn(DateTime.Today.AddDays(-1), true);
            GetDailyTxn(DateTime.Today, true);

        }

        private void GetDailyTxn(DateTime date, bool isOnLoad)
        {
            dailyTxn = DailyCollectionDetail.GetDailyTxn(date, isOnLoad);
            if (dailyTxn == null)
            {
                dailyTxn = DailyCollectionDetail.GetDailyTxn(date.AddDays(-1), isOnLoad);
                lblDate1.Text = lblDate2.Text = $"{dateTimePicker1.Value.ToShortDateString()} NOT FOUND";
                btnAdd.Text = "ADD";
                return;
            }

            lblDate1.Text = lblDate2.Text = $"Data For {dailyTxn.Date}";
            btnAdd.Text = "UPDATE";

            
            txtSentFromUSA.Text = dailyTxn.SentFromUSA.ToString();

            txtBankTxnOut.Text = dailyTxn.BankTxnOut.ToString();
            txtTakenFromBank.Text = dailyTxn.TakenFromBank.ToString();
            txtCollectionAmount.Text = dailyTxn.CollectionAmount.ToString();
            txtGivenAmount.Text = dailyTxn.GivenAmount.ToString();
            txtInterest.Text = dailyTxn.Interest.ToString();
            txtClosed.Text = dailyTxn.ClosedAccounts.ToString();
            txtOpened.Text = dailyTxn.OpenedAccounts.ToString();
            txtTmrNeeded.Text = dailyTxn.TomorrowNeed.ToString();

            txtOtherExpenditure.Text = Convert.ToString(dailyTxn.OtherExpenditire);
            txtOtherInvestment.Text = Convert.ToString(dailyTxn.OtherInvestment);

            btnYesterdayInHand.Text = dailyTxn.YesterdayAmountInHand.TokFormat();
            btnTodayInHand.Text = dailyTxn.TodayInHand.TokFormat();
            btnInBank.Text = dailyTxn.InBank.TokFormat();
            btnTmrWanted.Text = (dailyTxn.TomorrowDiff > 0) ? dailyTxn.TomorrowDiff.ToString() : $"0 --- (Have Extra {Math.Abs(Convert.ToInt32(dailyTxn.TomorrowDiff))} )";
            btnTmrWanted.BackColor = (dailyTxn.TomorrowDiff > 0) ? Color.Red : Color.Green;
            btnCanGive.Text = (dailyTxn.TodayInHand + dailyTxn.InBank).TokFormat();



            txtComments.Text = dailyTxn.Comments;

            btnCollection.Text = Convert.ToString(Transaction.GetDailyCollectionDetails_V0(DateTime.Today).Sum(s => s.AmountReceived));
        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (dailyTxn == null) dailyTxn = new DailyCollectionDetail();

            dailyTxn.Date = dateTimePicker1.Value.ToShortDateString();
            dailyTxn.YesterdayAmountInHand = dailyTxn.TodayInHand;
            dailyTxn.SentFromUSA = Convert.ToDecimal(txtSentFromUSA.Text);
            dailyTxn.BankTxnOut = Convert.ToDecimal(txtBankTxnOut.Text);
            dailyTxn.TakenFromBank = Convert.ToInt32(txtTakenFromBank.Text);
            dailyTxn.InBank = (dailyTxn.InBank + dailyTxn.SentFromUSA - dailyTxn.TakenFromBank - dailyTxn.BankTxnOut);

            dailyTxn.CollectionAmount = Convert.ToInt32(txtCollectionAmount.Text);
            dailyTxn.GivenAmount = Convert.ToInt32(txtGivenAmount.Text);
            dailyTxn.Interest = Convert.ToInt32(txtInterest.Text);
            dailyTxn.ClosedAccounts = Convert.ToInt32(txtClosed.Text);
            dailyTxn.OpenedAccounts = Convert.ToInt32(txtOpened.Text);
            dailyTxn.TomorrowNeed = Convert.ToInt32(txtTmrNeeded.Text);

            dailyTxn.OtherExpenditire = Convert.ToInt32(txtOtherExpenditure.Text);
            dailyTxn.OtherInvestment = Convert.ToInt32(txtOtherInvestment.Text);

            dailyTxn.TodayInHand = (dailyTxn.YesterdayAmountInHand + dailyTxn.CollectionAmount + dailyTxn.TakenFromBank - dailyTxn.GivenAmount + dailyTxn.Interest + dailyTxn.OtherInvestment - dailyTxn.OtherExpenditire);

            dailyTxn.TomorrowDiff = (Convert.ToInt32(txtTmrNeeded.Text) - Convert.ToInt32((dailyTxn.TodayInHand + dailyTxn.InBank)));
            dailyTxn.Comments = txtComments.Text;


            DailyCollectionDetail.AddOrUpdateDaily(dailyTxn);

            

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void txtGivenAmount_Leave(object sender, EventArgs e)
        {
            var amt = Convert.ToDecimal(txtGivenAmount.Text);

            txtInterest.Text = (amt / 10).ToString();

            //givenAmount = Convert.ToInt32((amt * 1000));
            //interest = Convert.ToInt32((amt * 1000) / 10);
            //label19.Text = givenAmount.ToString();
            //label20.Text = interest.ToString();
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
            GetDailyTxn(dateTimePicker1.Value, false);

        }
    }
}
