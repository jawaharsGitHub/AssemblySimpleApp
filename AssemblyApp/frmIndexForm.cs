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

            var customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();


            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            this.Text = $"WELCOME - CENTURY FIN CORP. Running Notes({activeTxn}) Closed Notes({closedTxn}) Total Notes({totalTxn})";


            //this.TopMost = true;
            this.AutoScrollOffset = new Point(0, 0);

            LoadAllData();


            CreateMenu();

            usingMenu = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);

            if (usingMenu)
            {

                button1.Visible = false;
                btnCustomers.Visible = false;
                btnRefresh.Visible = false;
                btnClosedTxn.Visible = false;
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                panel1.Visible = true;

            }
            else
            {
                menuStrip.Visible = false;
                panel1.Visible = false;
            }

            panel1.Width = 1300;
            panel1.Height = this.Height;

            ShowForm<frmCustomers>(); // initial form to be loaded
        }

        private void CreateMenu()
        {
            // Menu
            menuStrip = new MenuStrip
            {
                Location = new Point(0, 0),
                Name = "MenuStrip"
            };


            //Customer
            var mnuCustomer = new ToolStripMenuItem() { Name = "Customer", Text = "CUSTOMERS" };
            mnuCustomer.Click += (s, e) => ShowForm<frmCustomers>(); ;
            menuStrip.Items.Add(mnuCustomer);
            //Add customer
            var mnuAddCustomer = new ToolStripMenuItem() { Name = "AddCustomer", Text = "ADD-CUSTOMER" };
            menuStrip.Items.Add(mnuAddCustomer);
            mnuAddCustomer.Click += (s, e) => ShowForm<frmAddCustomer>(); ;
            //Daily Collection
            var mnuDailyCollection = new ToolStripMenuItem() { Name = "DailyColl", Text = "DAILY-COLLECTION" };
            menuStrip.Items.Add(mnuDailyCollection);
            mnuDailyCollection.Click += (s, e) => ShowForm<frmDailyEntry>(); ;
            //ShowInHand
            var mnuShowInHand = new ToolStripMenuItem() { Name = "InHand", Text = "INHAND-DETAILS" };
            menuStrip.Items.Add(mnuShowInHand);
            mnuShowInHand.Click += (s, e) => ShowForm<frmInHand>(); ;
            //General Reports
            var mnuGeneralReport = new ToolStripMenuItem() { Name = "GenReport", Text = "GENERAL-REPORT" };
            menuStrip.Items.Add(mnuGeneralReport);
            mnuGeneralReport.Click += (s, e) => ShowForm<frmGeneralReport>();
            //Dynamic Reports
            var mnuReport = new ToolStripMenuItem() { Name = "DynReport", Text = "DYNAMIC-REPORT" };
            menuStrip.Items.Add(mnuReport);
            mnuReport.Click += (s, e) => ShowForm<frmDynamicReport>();
            //Data Check Report
            var mnuDataCheckRpt = new ToolStripMenuItem() { Name = "dataCheckReport", Text = "DATA-CHECK-REPORT" };
            menuStrip.Items.Add(mnuDataCheckRpt);
            mnuDataCheckRpt.Click += (s, e) => ShowForm<frmDataCheck>();
            //Credit Report
            var mnuCreditReport = new ToolStripMenuItem() { Name = "CreditReport", Text = "CREDIT-REPORT" };
            menuStrip.Items.Add(mnuCreditReport);
            mnuCreditReport.Click += (s, e) => ShowForm<frmCreditReport>();
            //Batches
            var mnuBatch = new ToolStripMenuItem() { Name = "batch", Text = "BATCH-JOBS" };
            menuStrip.Items.Add(mnuBatch);
            mnuBatch.Click += (s, e) => ShowForm<frmBatches>();
            //Outstanding
            var mnuOutstanding = new ToolStripMenuItem() { Name = "Outstanding", Text = "OUTSTANDING" };
            menuStrip.Items.Add(mnuOutstanding);
            mnuOutstanding.Click += (s, e) => ShowForm<frmOutstanding>();
            //Health
            var mnuHealth = new ToolStripMenuItem() { Name = "health", Text = "HEALTH" };
            menuStrip.Items.Add(mnuHealth);
            mnuHealth.Click += (s, e) => ShowForm<frmHealth>();
            //Politics
            var mnuPolitics = new ToolStripMenuItem() { Name = "politics", Text = "POLITICS" };
            menuStrip.Items.Add(mnuPolitics);
            mnuPolitics.Click += (s, e) => ShowForm<frmPolitics>();
            //Petrol
            var mnuPetrol = new ToolStripMenuItem() { Name = "petrol", Text = "PETROL" };
            menuStrip.Items.Add(mnuPetrol);
            mnuPetrol.Click += (s, e) => ShowForm<frmPetrol>();

            this.Controls.Add(menuStrip);
        }

        public void ShowForm(int transactionId)
        {
            var txn = Transaction.GetTransactionDetail(transactionId);
            var customer = Customer.GetCustomerDetails(txn);

            ShowForm<frmCustomerTransaction>(customer);
        }
        public void ShowForm<T>(Customer cus = null) where T : UserControl, new()
        {

            T ac;
            if (cus == null)
            {
                ac = new T();
            }
            else
            {
                var txns = Transaction.GetTransactionDetails(cus);

                if (txns == null)
                {
                    MessageBox.Show("May be you didn't run Closed Batch, Please run and try this!");
                    return;
                }

                ac = new frmCustomerTransaction(cus, this) as T;
            }

            if (isAdded && panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }


            isAdded = true;
            panel1.Controls.Add(ac);
            ac.Show();
        }

        private void LoadAllData()
        {
            GetNumberOfClients();
            GetNumberOfActiveClients();
            JawaInvestment();
            ComPanyInv();
            IntOnly();
            TotalAssets();
            InHandMoney();
            Profit();
            GetExpenditure();
            Outstanding();

            ClosedTxn();

            TotalNote();
        }

        private void TotalNote()
        {
            var noOfDistinctClients = GetNumberOfClients();
            var closedNotesCount = Directory.GetFiles(AppConfiguration.ClosedNotesFile, "*.*", SearchOption.AllDirectories).Length;
            btnNote.Text = "Total Note: " + (noOfDistinctClients + closedNotesCount);
        }

        private void ClosedTxn()
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDailyEntry ac = new frmDailyEntry();
            //ac.ShowDialog();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers ac = new frmCustomers();
            //ac.ShowDialog();
        }


        private void btnClosedTxn_Click(object sender, EventArgs e)
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            if (list == null || list.Count == 0) return;

            var closedIds = list.Where(w => w.Balance == 0).ToList();

            foreach (var item in closedIds)
            {
                var closedTxn = new List<Transaction>();
                closedTxn.AddRange(list.Where(w => w.CustomerId == item.CustomerId && w.CustomerSequenceNo == item.CustomerSequenceNo));
                // Back up closed txn
                Transaction.AddClosedTransaction(closedTxn);

                // Delete Transactions data
                Transaction.DeleteTransactionDetails(item.CustomerId, item.CustomerSequenceNo);

                // Customer.UpdateCustomerDetails(new Customer() { CustomerId = item.CustomerId, CustomerSeqNumber = item.CustomerSequenceNo, IsActive = false });

            }


        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            //InHand.AddInHand(Convert.ToInt32(txtJawaInvestment.Text), fromJawahar: true);
        }

        private void btnAddExpen_Click(object sender, EventArgs e)
        {
            Expenditure.AddExpenditure(new Expenditure() { Amount = Convert.ToInt32(txtExpenditure.Text), Reason = txtReason.Text });
        }

        private void btnAllTransaction_Click(object sender, EventArgs e)
        {

        }


        private void GetNumberOfActiveClients()
        {
            var cus = Customer.GetAllCustomer() ?? new List<Customer>();

            var activeCustomers = cus.Where(w => w.IsActive).Count();

            btnActiveClients.Text = $"Active Customers: {Environment.NewLine}{activeCustomers}";

        }

        private int GetNumberOfClients()
        {
            var allCustomers = Customer.GetAllCustomer();

            var customerCount = allCustomers == null ? 0 : allCustomers.Select(s => s.CustomerId).Distinct().Count();
            btnNoClients.Text = $"All Customers: {Environment.NewLine}{customerCount}";
            return customerCount;


        }

        private void JawaInvestment()
        {
            var jawaInvestment = GetJawaInvestment();
            btnJawaInvestment.Text = $"Outgoing From Jawahar Investment: {Environment.NewLine}{jawaInvestment}";
        }

        private void ComPanyInv()
        {
            var jawaInvestment = (Investment.GetAllInvestmet() ?? new List<Investment>()).Where(w => w.InvestType == InvestmentFrom.Company).Sum(s => s.Amount);
            btnComPanyInv.Text = $"OutGoing From Company: {Environment.NewLine}{jawaInvestment}";
        }

        private void IntOnly()
        {
            var jawaInvestment = (Investment.GetAllInvestmet() ?? new List<Investment>()).Sum(s => s.Interest);
            btnIntOnly.Text = $"Interest Only Income: {Environment.NewLine}{jawaInvestment}";

        }

        private int TotalAssets()
        {
            var os = Transaction.GetAllOutstandingAmount();
            var assets = (os.includesProfit + InHandAndBank.GetAllhandMoney().InHandAmount) - GetAllExpenditure();
            btnTotalAssets.Text = $"Total Assets Amount: {Environment.NewLine}{assets }";
            return assets;

        }

        private void InHandMoney()
        {
            var jawaInvestment = InHandAndBank.GetAllhandMoney();
            btnInHand.Text = $"InHand Amount: {Environment.NewLine}{jawaInvestment.InHandAmount}";

        }

        private void Profit()
        {

            var profitOnly = (TotalAssets() - GetJawaInvestment() - GetExpenditure());
            btnProfit.Text = $"Profit Only: {Environment.NewLine}{profitOnly}";


        }

        private int GetAllExpenditure()
        {
            int exp = GetExpenditure();
            btnExpenditure.Text = $"Expenditure Only: {Environment.NewLine}{exp}";
            return exp;

        }

        private int GetJawaInvestment()
        {
            return 0;
            //return InHandAndBank.GetAllhandMoney().JawaharShare; //.GetAllInvestmet().Where(w => w.InvestType == InvestmentFrom.Jawahar).Sum(s => s.Amount); 

        }

        private int GetExpenditure()
        {
            return Expenditure.GetTotalExpenditure();

        }

        private void Outstanding()
        {
            int outStanding = Transaction.GetAllOutstandingAmount().includesProfit;
            btnOutstanding.Text = $"OutStanding Amount: {Environment.NewLine}{outStanding}";

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllData();
        }
    }
}
