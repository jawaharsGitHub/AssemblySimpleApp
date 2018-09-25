using System;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmDataCorrection : UserControl
    {
        public frmDataCorrection()
        {
            InitializeComponent();
        }

        private void btnAddCusIntoTxn_Click(object sender, EventArgs e)
        {
            try
            {
                // Update Active Txn
                //var jsonTxn = File.ReadAllText(AppConfiguration.TransactionFile);

                //List<Transaction> txnlist = JsonConvert.DeserializeObject<List<Transaction>>(jsonTxn);

                //var jsonCus = File.ReadAllText(AppConfiguration.CustomerFile);
                //List<Customer> cuslist = JsonConvert.DeserializeObject<List<Transaction>>(jsonCus);


                //txnlist.ForEach(t => {

                //    u.tx = updatedTransaction.AmountReceived;
                //    u.TxnUpdatedDate = DateTime.Today;


                //});
                ////var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();



                //string updatedString = JsonConvert.SerializeObject(list, Formatting.Indented);


                //File.WriteAllText(AppConfiguration.TransactionFile, updatedString);

                // Update Closed Txn

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
