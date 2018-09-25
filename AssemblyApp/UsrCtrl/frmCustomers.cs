using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmCustomers : UserControl
    {
        private List<Customer> customers;

        public frmCustomers()
        {
            InitializeComponent();

            cmbFilters.DataSource = GetOptions();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]) == true)
                btnAddCustomer.Visible = false;
            else
                btnAddCustomer.Visible = true;

            // Get the table from the data set
            SetCustomers();

            if (customers == null) return;

            //Create New DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.HeaderText = "Collection Amount";
            textboxColumn.Name = "CollectionAmt";

            dataGridView1.DataSource = customers;

            //Add TextBoxColumn dynamically to DataGridView
            dataGridView1.Columns.Add(textboxColumn);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            AdjustColumnOrder();
            chkAllColumns.Checked = false;


            switch (GlobalValue.NoteOption)
            {
                case "AN":
                    rdbAll.Checked = true;
                    break;
                case "CN":
                    rdbClosed.Checked = true;
                    break;
                case "RL":
                default:
                    rdbActive.Checked = true;
                    break;
            }


            txtSearch.Text = GlobalValue.SearchText;


        }


        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By Amount"),
                   new KeyValuePair<int, string>(2, "By Sequence No"),
                   new KeyValuePair<int, string>(3, "By Customer Id"),
                   new KeyValuePair<int, string>(4, "By Customer Name"),
                   new KeyValuePair<int, string>(10, "Return By Yesterday"),
                   new KeyValuePair<int, string>(5, "Return By Today"),
                   new KeyValuePair<int, string>(6, "Return By Tomorrow"),
                   new KeyValuePair<int, string>(7, "By Return Day"),
                   new KeyValuePair<int, string>(8, "By Return Type"),
                   new KeyValuePair<int, string>(9, "By CollectionSpot")

               };

            return myKeyValuePair;

        }


        private void SetCustomers()
        {
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();

            var allGivenAmount = customers.Sum(s => s.LoanAmount);

            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            this.Text = $"WELCOME TO Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";

            rdbActive.Text = $"RUNNING NOTES ({activeTxn})";
            rdbClosed.Text = $"CLOSED NOTES ({closedTxn})";
            rdbAll.Text = $"ALL NOTES ({totalTxn})";

            // Customer day and count ratio.
            var days = (DateTime.Today - new DateTime(2018, 1, 25)).TotalDays;

            label1.Text = $"{totalTxn} notes in {days} days {Environment.NewLine} " +
                $"{DateHelper.DaysToMonth("Running Days", new DateTime(2018, 1, 25), DateTime.Today)} {Environment.NewLine} " +
                $"{Math.Round(totalTxn / days, 2)} note(s) per day {Environment.NewLine} " +
                $"{Math.Round(allGivenAmount / days).TokFormat()} Rs. per day ({((Math.Round(allGivenAmount / days) / 10) * 30).TokFormat()} per month) {Environment.NewLine}" +
                $"  need {365 - totalTxn} in {365 - days} days [Shortage: {days - totalTxn}] {Environment.NewLine} " +
                $"{DateHelper.DaysToMonth(" Days Left", DateTime.Today, new DateTime(2019, 1, 24))}";
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CollectionAmt"].DisplayIndex = 3;
            dataGridView1.Columns["ReturnDay"].DisplayIndex = 4;
            dataGridView1.Columns["ReturnType"].DisplayIndex = 5;
            dataGridView1.Columns["CollectionSpotId"].DisplayIndex = 6;

            dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["Name"].Width = 250;
        }

        private void SetColumnVisibility(bool show = false)
        {
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["PhoneNumber"].Visible = false;
            dataGridView1.Columns["IdAndName"].Visible = false;
            dataGridView1.Columns["CollectionSpotId"].Visible = show;
            dataGridView1.Columns["ReturnDay"].Visible = show;
            dataGridView1.Columns["ReturnType"].Visible = show;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as Customer);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm<frmCustomerTransaction>(selectedCustomer);

        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);
        }

        private void rdbClosed_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);

        }

        private void rdbActive_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);
        }

        private void NoteOptionChanged(object sender)
        {
            List<Customer> searchedCustomer;

            if (rdbAll.Checked)
            {
                searchedCustomer = customers;
                GlobalValue.NoteOption = rdbAll.Tag.ToString();
            }
            else if (rdbClosed.Checked)
            {
                searchedCustomer = customers.Where(w => w.IsActive == false).OrderBy(o => o.ClosedDate).ToList();
                GlobalValue.NoteOption = rdbClosed.Tag.ToString();
            }
            else
            {
                searchedCustomer = customers.Where(w => w.IsActive == true).ToList();
                GlobalValue.NoteOption = rdbActive.Tag.ToString();
            }

            searchedCustomer = string.IsNullOrEmpty(txtSearch.Text) ? searchedCustomer : searchedCustomer.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            dataGridView1.DataSource = searchedCustomer;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (rdbActive.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == true).ToList();
            if (rdbClosed.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == false).ToList();
            if (rdbAll.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            GlobalValue.SearchText = txtSearch.Text;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //frmAddCustomer ac = new frmAddCustomer();
            //ac.ShowDialog();

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as Customer;


            var seqNo = cus.CustomerSeqNumber; // FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerSeqNumber");
            var customerId = cus.CustomerId; // FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerId");
            var loanAmount = cus.LoanAmount; // FormGeneral.GetGridCellValue(grid, rowIndex, "LoanAmount");



            var collectedAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "CollectionAmt");

            if (string.IsNullOrEmpty(collectedAmount) == false)
            {
                var txn = new Transaction()
                {
                    AmountReceived = Convert.ToInt32(collectedAmount),
                    CustomerId = customerId,
                    CustomerSequenceNo = seqNo,
                    TransactionId = Transaction.GetNextTransactionId(),
                    Balance = (Transaction.GetBalance(cus) - Convert.ToInt16(collectedAmount)), // TODO: Balance is not updaed correctly eg: 71-104 - 19th july txn.
                    TxnDate = dateTimePicker1.Value
                };

                if (txn.Balance < 0)
                {
                    MessageBox.Show("Balance is less than 0. Please check your amount. Txn Aborted!");
                    LogHelper.WriteLog($"Balance is less than 0. Please check your amount. Txn Aborted!", txn.CustomerId, txn.CustomerSequenceNo);
                    return;
                }
                if (txn.Balance == 0)
                {
                    MessageBox.Show("Good News, This txn will be closed!");
                    LogHelper.WriteLog("Good News, This txn will be closed!", txn.CustomerId, txn.CustomerSequenceNo);
                    Customer.CloseCustomerTxn(cus, false, txn.TxnDate); //  new Customer() { CustomerId = txn.CustomerId, CustomerSeqNumber = txn.CustomerSequenceNo, IsActive = false, ClosedDate = txn.TxnDate });
                }

                txn.IsClosed = (txn.Balance <= 0);

                // Add new Txn.
                var existingTxn = Transaction.GetTransactionForDate(txn);
                if (existingTxn == null || existingTxn.Count == 0)
                {
                    Transaction.AddDailyTransactions(txn);
                }
                else
                {
                    if (existingTxn.First().AmountReceived == 0) // Customer is giving money in the same day fo given date.
                    {
                        Transaction.AddDailyTransactions(txn);
                    }
                    else
                    {
                        txn.TransactionId = existingTxn.First().TransactionId;
                        Transaction.UpdateTransactionDetails(txn);
                    }
                }

                // Update txn Closed Date
                if (txn.IsClosed && txn.Balance == 0)
                {
                    // Update Closed Date
                    Customer.UpdateCustomerClosedDate(
                        new Customer()
                        {
                            CustomerId = txn.CustomerId,
                            CustomerSeqNumber = txn.CustomerSequenceNo,
                            ClosedDate = txn.TxnDate,
                        });
                }
                return;
            }



            //var amountGivenDate = cus.AmountGivenDate; // FormGeneral.GetGridCellValue(grid, rowIndex, "AmountGivenDate");
            //var closedDate = cus.ClosedDate; //  FormGeneral.GetGridCellValue(grid, rowIndex, "ClosedDate");
            //var interest = cus.Interest; // FormGeneral.GetGridCellValue(grid, rowIndex, "Interest");
            //var name = cus.Name; // FormGeneral.GetGridCellValue(grid, rowIndex, "Name");

            // Update Customer Created Date.

            Customer.CorrectCustomerData(cus);

            //Customer.CorrectCustomerData(
            //        new Customer()
            //        {
            //            CustomerId = customerId,
            //            CustomerSeqNumber = Convert.ToInt32(seqNo),
            //            AmountGivenDate = Convert.ToDateTime(amountGivenDate),
            //            ClosedDate = Convert.ToDateTime(closedDate),
            //            Interest = Convert.ToInt32(interest),
            //            LoanAmount = Convert.ToInt32(loanAmount),
            //            Name = name
            //        });
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetCustomers();
            txtSearch.Focus();
            txtSearch.Text = string.Empty;
            rdbActive.Checked = true;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 1) return;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                var seqNo = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "CustomerSeqNumber");
                var customerId = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "CustomerId");
                var isActive = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "IsActive");

                strip.Tag = new Customer() { CustomerSeqNumber = Convert.ToInt32(seqNo), CustomerId = Convert.ToInt32(customerId), IsActive = Convert.ToBoolean(isActive) };

                if (rowIndex >= 0)
                {
                    strip.Items.Add("Delete Customer and Txn").Name = "All";
                    strip.Items.Add("Delete Customer only").Name = "Cus";
                    strip.Items.Add("Delete Txn only").Name = "Txn";
                }

                strip.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));

                strip.ItemClicked += Strip_ItemClicked;

            }

        }

        private void Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var cus = (Customer)((ContextMenuStrip)sender).Tag;

            if (e.ClickedItem.Name == "All")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
                Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "Cus")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "Txn")
            {
                Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["CollectionAmt"];
            dataGridView1.BeginEdit(true);
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (customers == null) return;
            var value = ((KeyValuePair<int, string>)cmbFilters.SelectedItem).Key;
            List<Customer> searchedCustomer;

            if (value == 1)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderByDescending(o => o.LoanAmount).ToList();
            }
            else if (value == 2)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderBy(o => o.CustomerSeqNumber).ToList();
            }
            else if (value == 3)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderBy(o => o.CustomerId).ToList();
            }
            else if (value == 4)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderBy(o => o.Name).ToList();
            }
            else if (value == 5)
            {
                searchedCustomer = customers.Where(w => w.IsActive && w.ReturnDay == DateTime.Today.DayOfWeek).ToList();
            }
            else if (value == 6)
            {
                searchedCustomer = customers.Where(w => w.IsActive && w.ReturnDay == DateTime.Today.AddDays(1).DayOfWeek).ToList();
            }
            else if (value == 7)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderBy(o => o.ReturnDay).ToList();
            }
            else if (value == 8)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderBy(o => o.ReturnType).ToList();
            }
            else if  (value == 9)
            {
                searchedCustomer = customers.Where(w => w.IsActive).OrderByDescending(o => o.CollectionSpotId).ToList();
            }
            else//  (value == 10)
            {
                searchedCustomer = customers.Where(w => w.IsActive && w.ReturnDay == DateTime.Today.AddDays(-1).DayOfWeek).ToList();
            }

            dataGridView1.DataSource = searchedCustomer;
            AdjustColumnOrder();

        }

        private void chkAllColumns_CheckedChanged(object sender, EventArgs e)
        {
            SetColumnVisibility(chkAllColumns.Checked);

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            lblRowCount.Text = $"Row Count: {dataGridView1.Rows.Count.ToString()}";
        }
    }
}
