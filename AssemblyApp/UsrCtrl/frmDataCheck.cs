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
    public partial class frmDataCheck : UserControl
    {
        public frmDataCheck()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();

        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "Check MinBalance And AmountReceived"),
                   new KeyValuePair<int, string>(2, "Check EachAndEvery Txn"),
               };

            return myKeyValuePair;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                MinBalanceAndReceivedAmount();
            }
            else if (value == 2)
            {
                CheckEachTxn();
            }


        }

        private void MinBalanceAndReceivedAmount()
        {
            var details = (from c in Customer.GetAllCustomer()
                           select new
                           {
                               c.CustomerId,
                               c.CustomerSeqNumber,
                               c.Name,
                               c.LoanAmount,
                               txn = Transaction.GetTransactionDetails(c)
                           });

            var data = (from t in details
                        select new
                        {
                            t.CustomerId,
                            t.CustomerSeqNumber,
                            t.Name,
                            t.LoanAmount,
                            TotalReceived = t.txn.Sum(s => s.AmountReceived),
                            MinBalance = t.txn.Min(m => m.Balance),
                            IsCorrect = ((t.LoanAmount - t.txn.Sum(s => s.AmountReceived)) == t.txn.Min(m => m.Balance))
                        }
                        );



            dataGridView1.DataSource = data.Where(w => w.IsCorrect == false).ToList();

        }

        private void CheckEachTxn()
        {
            Transaction nextTxn;

            var data = new List<LogData>();

            var details = (from c in Customer.GetAllCustomer()//.Where(w => w.CustomerSeqNumber == 1)
                           select new
                           {
                               c.CustomerId,
                               c.CustomerSeqNumber,
                               c.Name,
                               c.AmountGivenDate,
                               c.LoanAmount,
                               txn = Transaction.GetTransactionDetails(c),
                               c.IsActive
                           }).ToList();

            details.ForEach(f =>
            {

                if (f.txn.Count() == 0)
                {
                    data.Add(new LogData()
                    {
                        LogType = "NO-TXN",
                        CustomerId = f.CustomerId,
                        CusSeqNo = f.CustomerSeqNumber,
                        Name = f.Name,
                        IsActive = f.IsActive
                    });

                }
                else
                {

                    var firstItem = f.txn.First();

                    if (firstItem.AmountReceived > 0)
                    {
                        data.Add(new LogData()
                        {
                            LogType = "NO-FIRST-TXN",
                            TxnDate = firstItem.TxnDate.Date,
                            TxnId = firstItem.TransactionId,
                            CusSeqNo = f.CustomerSeqNumber,
                            CustomerId = f.CustomerId,
                            Name = f.Name,
                            IsActive = f.IsActive
                        });
                    }

                    f.txn.Where(w => w.AmountReceived == 0 && f.AmountGivenDate.Value.Date != w.TxnDate.Date).ToList().ForEach(fe =>
                    {
                        data.Add(new LogData()
                        {
                            LogType = "DUMMY-TXN",
                            TxnDate = fe.TxnDate.Date,
                            TxnId = fe.TransactionId,
                            CusSeqNo = f.CustomerSeqNumber,
                            CustomerId = f.CustomerId,
                            Name = f.Name,
                            IsActive = f.IsActive
                        });
                    });


                    if (firstItem.TxnDate.Date != f.AmountGivenDate.Value.Date)
                    {
                        data.Add(new LogData()
                        {
                            LogType = "GIVEN_DATE_NOT_CORRECT",
                            TxnDate = firstItem.TxnDate.Date,
                            TxnId = firstItem.TransactionId,
                            CusSeqNo = f.CustomerSeqNumber,
                            CustomerId = f.CustomerId,
                            Name = f.Name,
                            IsActive = f.IsActive
                        });
                    }


                    if (firstItem.Balance != f.LoanAmount)
                    {
                        data.Add(new LogData()
                        {
                            LogType = "LOAN-NOT-CORRECT",
                            TxnDate = firstItem.TxnDate.Date,
                            TxnId = firstItem.TransactionId,
                            CusSeqNo = f.CustomerSeqNumber,
                            CustomerId = f.CustomerId,
                            Name = f.Name,
                            IsActive = f.IsActive
                        });


                    }

                    f.txn.ForEach(t =>
                    {

                        if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() > 1)
                        {
                            //data.Add(new LogData()
                            //{
                            //    LogType = ">1-TXN",
                            //    TxnDate = t.TxnDate.Date,
                            //    TxnId = t.TransactionId,
                            //    CusSeqNo = f.CustomerSeqNumber,
                            //    CustomerId = f.CustomerId,
                            //    Name = f.Name
                            //});

                        }

                        else if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() == 0)
                        {
                            data.Add(new LogData()
                            {
                                LogType = "ZERO-TXN",
                                TxnDate = t.TxnDate.Date,
                                TxnId = t.TransactionId,
                                CusSeqNo = f.CustomerSeqNumber,
                                CustomerId = f.CustomerId,
                                Name = f.Name,
                                IsActive = f.IsActive
                            });

                        }

                        else if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() == 1)
                        {
                            nextTxn = f.txn.NextOf(t);

                            if (nextTxn != null && nextTxn.Balance != (t.Balance - nextTxn.AmountReceived))
                            {
                                data.Add(new LogData()
                                {
                                    LogType = "NEED-TO-VERIFY",
                                    TxnDate = nextTxn.TxnDate.Date,
                                    TxnId = nextTxn.TransactionId,
                                    CusSeqNo = f.CustomerSeqNumber,
                                    CustomerId = f.CustomerId,
                                    Name = f.Name,
                                    IsActive = f.IsActive
                                });

                            }


                        }

                    });
                }
            });

            dataGridView2.DataSource = (from d in data
                                        where d.LogType == "NEED-TO-VERIFY"
                                        group d by new { d.Name, d.CusSeqNo } into newGroup
                                        select new { newGroup.Key.Name, newGroup.Key.CusSeqNo }).ToList();

            dataGridView1.DataSource = data.OrderBy(o => o.IsActive).ToList();

        }

    }

    public class LogData
    {
        public int TxnId { get; set; }
        public DateTime TxnDate { get; set; }
        public int CustomerId { get; set; }
        public int CusSeqNo { get; set; }
        public string Name { get; set; }
        public string LogType { get; set; }
        public bool IsActive { get; set; }
    }
}
