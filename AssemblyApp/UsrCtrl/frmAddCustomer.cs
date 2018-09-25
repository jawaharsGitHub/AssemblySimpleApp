using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmAddCustomer : UserControl
    {
        public frmAddCustomer()
        {
            InitializeComponent();
            cmbExistingCustomer.Visible = false;
            LoadCustomerCollectionType();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var nextIds = Customer.GetNextIds();
            var newCustomerId = nextIds.NewCustomerId;
            var nextSeqNo = nextIds.NewCustomerSeqId;
            Customer cus = new Customer();

            if (chkExistingCustomer.Checked)
            {
                cus = (Customer)cmbExistingCustomer.SelectedItem;
                cus.CustomerSeqNumber = nextSeqNo;
                cus.IsExistingCustomer = true;
                newCustomerId = cus.CustomerId;                
                cus.IsActive = false;   //Update Active flag of existing customer.
            }
            else
            {
                var isDuplicateName = Customer.IsDuplicateName(txtName.Text);

                if (isDuplicateName)
                {
                    var msg = $"Customer Name [{txtName.Text}] already exist, Please verify!";
                    MessageBox.Show(msg);
                    lblMessage.Text = msg;
                    return;
                }
                cus.CustomerId = newCustomerId;
                cus.Name = txtName.Text;
                cus.PhoneNumber = txtPhone.Text;
                cus.CustomerSeqNumber = nextSeqNo;
            }

            cus.LoanAmount = Convert.ToInt32(txtLoan.Text);
            cus.Interest = Convert.ToInt32(txtInterest.Text);
            cus.AmountGivenDate = dateTimePicker1.Value;
            cus.ReturnType = (ReturnTypeEnum)cmbReturnType.SelectedItem;
            cus.ReturnDay = (DayOfWeek)cmbReturnDay.SelectedItem;
            cus.CollectionSpotId = cmbCollectionSpot.SelectedValue.ToInt32();

            Customer.AddCustomer(cus);
            txtCustomerNo.Text = newCustomerId.ToString();

            // Add First(Default) Transaction.
            var txn = new Transaction()
            {
                AmountReceived = 0,
                CustomerId = cus.CustomerId,
                CustomerSequenceNo = cus.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = cus.LoanAmount,
                TxnDate = dateTimePicker1.Value
            };

            Transaction.AddTransaction(txn);

            // Add Investment
            Investment.AddInvestment(new Investment()
            {
                Amount = cus.LoanAmount,
                Interest = Convert.ToInt16(txtInterest.Text),
                CustomerId = cus.CustomerId,
                CustomerSequenceNo = cus.CustomerSeqNumber
            });

            var nthTimes = Customer.GetAllCustomer().Where(w => w.CustomerId == cus.CustomerId).Count();

            lblMessage.Text = $"Customer {cus.Name} ({nthTimes}{General.GetDaySuffix(nthTimes)} times) Added Successfully.";
            lblNoteCount.Text = cus.CustomerSeqNumber.ToString();

        }

        private void chkExistingCustomer_CheckedChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.Visible = (sender as CheckBox).Checked;

            if ((sender as CheckBox).Checked)
            {
                // load existing customer
                cmbExistingCustomer.DataSource = Customer.GetAllCustomer().DistinctBy(d => d.CustomerId).OrderBy(o => o.Name).ToList();
                cmbExistingCustomer.DisplayMember = "Name";
                cmbExistingCustomer.ValueMember = "CustomerId";
                txtName.Enabled = txtPhone.Enabled = false;

            }
            else
            {
                txtName.Enabled = txtPhone.Enabled = true;
            }
        }

        private void txtLoan_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoan.Text)) return;

            var loanAmount = Convert.ToInt32(txtLoan.Text);

            var interest = (loanAmount / 100) * 10;
            txtInterest.Text = interest.ToString();

        }

        private void cmbExistingCustomer_TextChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.DroppedDown = false;
        }

        private void LoadCustomerCollectionType()
        {
            cmbReturnType.DataSource = Enum.GetValues(typeof(ReturnTypeEnum));
            cmbReturnDay.DataSource = Enum.GetValues(typeof(DayOfWeek));

            cmbCollectionSpot.DataSource = Customer.GetAllUniqueCustomers();
            cmbCollectionSpot.ValueMember = "CustomerId";
            cmbCollectionSpot.DisplayMember = "Name";


            cmbReturnType.SelectedItem = ReturnTypeEnum.Daily;
            cmbReturnDay.SelectedItem = DayOfWeek.Sunday;
            cmbCollectionSpot.SelectedValue = 0;
        }
    }
}
