using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmGeneralReport : UserControl
    {
        (int actual, int includesProfit) outstandingMoney;

        public frmGeneralReport()
        {
            InitializeComponent();
            chkAddSalary.Checked = true; // will callCalculateIncome(true);
            CustomerGrowth();

            ShowRemaingDays();

            GetCustomerCount();

            ShowOutstandingMoney();
            ShowTotalAssetMoney();
        }

        private void GetCustomerCount()
        {
            var count = Customer.GetAllCustomer().Select(s => s.CustomerId).Distinct().Count();
            btnCustomerCount.Text = $"Customer Count - {count.ToString()}";

        }

        private void ShowRemaingDays()
        {

            var data = DateHelper.RemaingDaysToNextCycle;

            label1.Text = $"Remaining Days to start next month ({data.MonthName}) cycle: {data.NoOfDays}";
            label2.Text = $"Remaining Days in this month: {DateHelper.RemaingDaysOfMonth}";
        }

        private void CalculateIncome(bool considerSalary = false)
        {

            // Closed Account
            var closedCustomers = (from c in Customer.GetAllCustomer()
                                   where c.IsActive == false
                                   group c by Convert.ToDateTime(c.ClosedDate).ToShortDateString() into newGroup
                                   select new
                                   {
                                       ClosedDate = newGroup.Key,
                                       TotalInterest = newGroup.Sum(s => s.Interest),
                                       IsExpectedIncome = false
                                   }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();

            // Running Account (Expected Income)
            var runningCustomers = (from c in Customer.GetAllCustomer()
                                    where c.IsActive == true
                                    group c by Convert.ToDateTime(c.AmountGivenDate).ToShortDateString() into newGroup
                                    select new
                                    {
                                        ClosedDate = (Convert.ToDateTime(newGroup.Key).AddDays(100)).ToShortDateString(), // TODO: it should be group by given data + 100 days not by key(July 2018)
                                        TotalInterest = newGroup.Sum(s => s.Interest),
                                        IsExpectedIncome = true
                                    }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();

            // Merge Both result
            closedCustomers.AddRange(runningCustomers);

            // group by eclosed date and income type
            var data = (from x in closedCustomers
                        group x by new { ClosedMonth = Convert.ToDateTime(x.ClosedDate).ToString("Y"), IsExpectedIncome = x.IsExpectedIncome } into newGroup
                        select newGroup).ToList();

            var finalData = new List<IncomeReport>();


            data.ForEach(f =>
            {

                f.ToList().ForEach(d =>
                {
                    var existData = finalData.Where(w => w.Month == Convert.ToDateTime(d.ClosedDate).ToString("Y")).FirstOrDefault();

                    if (existData == null) // Not exist
                    {
                        existData = new IncomeReport() { Month = Convert.ToDateTime(d.ClosedDate).ToString("Y") };
                        finalData.Add(existData);
                    }

                    if (d.IsExpectedIncome)
                        existData.ExpectedIncome += d.TotalInterest;
                    else
                        existData.ActualIncome += d.TotalInterest;


                });


            });

            finalData.Insert(0, new IncomeReport()
            {
                Month = "Feb 2018",
                ActualIncome = 0
            });

            // Move past month expected to current month expected.
            var pastMonthExpectedIncome = (from pm in finalData
                                           where Convert.ToDateTime(pm.Month).Month < DateTime.Now.Month
                                           select pm);


            var currentMonthExpectedIncome = finalData.Where(w => Convert.ToDateTime(w.Month).Month == DateTime.Now.Month).FirstOrDefault();
            if (currentMonthExpectedIncome != null) currentMonthExpectedIncome.ExpectedIncome += pastMonthExpectedIncome.Sum(s => s.ExpectedIncome);

            pastMonthExpectedIncome.ToList().ForEach(f => f.ExpectedIncome = 0);

            // Consider if salary also!
            if (considerSalary)
            {
                finalData.ForEach(fd =>
                {
                    if (Convert.ToDateTime(fd.Month).Month == 2)
                    {
                        fd.ActualIncome = (fd.ExpectedIncome - fd.MonthlySalary); // Always fr feb, actualincome is -salary
                    }
                    else if ((fd.ActualIncome > 0 && fd.ExpectedIncome > 0) || fd.ActualIncome > 0)
                    {
                        fd.ActualIncome = (fd.ActualIncome - fd.MonthlySalary);
                    }
                    else
                    {
                        fd.ExpectedIncome = (fd.ExpectedIncome - fd.MonthlySalary);
                    }

                });

            }
            // Bindng the real/actual source finalData (will be updated using by ref by various data)
            dgvIncome.DataSource = finalData;

            // Years Expected and Actual Salary

            var actual = finalData.Sum(w => w.ActualIncome);
            var expected = finalData.Sum(w => w.ExpectedIncome);
            var salary = finalData.Sum(w => w.MonthlySalary);
            var total = (actual + expected);

            lblActual.Text = $"Actual :  {actual.ToMoney()} (Per Month: { (actual / DateTime.Today.Month).ToMoney()})";
            lblExpected.Text = $"Ëxpected : {expected.ToMoney()} (Per Month: { (expected / DateTime.Today.Month).ToMoney()})";
            lblTotal.Text = $"TOTAL : {total.ToMoney()} (Per Month: { (total / DateTime.Today.Month).ToMoney()})";

            lblSalary.Text = $"Salary : {salary}";
            lblSalary.Visible = considerSalary;


        }

        private void chkAddSalary_CheckedChanged(object sender, EventArgs e)
        {
            CalculateIncome(chkAddSalary.Checked);
        }

        private void CustomerGrowth()
        {
            var customers = (from c in Customer.GetAllCustomer()
                             orderby c.AmountGivenDate
                             group c by c.AmountGivenDate.Value.ToString("Y") into newGroup

                             select new
                             {
                                 Month = newGroup.Key,
                                 Count = newGroup.Count(),
                                 LoanAmount = newGroup.Sum(s => s.LoanAmount),
                                 GivenAmount = newGroup.Sum(s => (s.LoanAmount - s.Interest)),
                                 FutureInterest = newGroup.Sum(s => s.Interest)
                             }).ToList();

            dgvNotePerMonth.DataSource = customers;

            label6.Text = $"LA: {customers.Sum(s => s.LoanAmount).ToMoney()} {Environment.NewLine}" +
                $"GA: {customers.Sum(s => s.GivenAmount).ToMoney()} {Environment.NewLine}" +
                $"FI: {customers.Sum(s => s.FutureInterest).ToMoney()} {Environment.NewLine}" +
                $"C: {customers.Sum(s => s.Count)} {Environment.NewLine}";





        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.includesProfit.ToMoney();
        }

        private void ShowTotalAssetMoney()
        {
            var inHandAndBank = InHandAndBank.GetAllhandMoney();
            lblTotalAsset.Text = $"{(outstandingMoney.includesProfit + inHandAndBank.InHandAmount + inHandAndBank.InBank).ToMoney()} (OS: {outstandingMoney.includesProfit.ToMoney()} IH: {inHandAndBank.InHandAmount.ToMoney()} IB: {inHandAndBank.InBank.ToMoney()} Actual Outstanding: {outstandingMoney.actual.ToMoney()})";
        }
    }
}
