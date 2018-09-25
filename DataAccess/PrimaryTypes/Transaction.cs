using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Transaction : BaseClass
    {

        private static string ClosedTxnFilePath = AppConfiguration.ClosedNotesFile;
        private static string JsonFilePath = AppConfiguration.TransactionFile;

        public int CustomerId { get; set; }
        public bool IsClosed { get; set; }
        public int TransactionId { get; set; }
        public int AmountReceived { get; set; }
        public int Balance { get; set; }
        public DateTime? TxnUpdatedDate { get; set; }
        public int CustomerSequenceNo { get; set; }
        public DateTime TxnDate { get; set; }
        [JsonIgnore]
        private Customer _customer;


        public static void AddTransaction(Transaction newTxn)
        {
            newTxn.TxnUpdatedDate = null;

            var jsonFilePath = newTxn.IsClosed ? $"{ClosedTxnFilePath}/{newTxn.CustomerId}/{newTxn.CustomerId}_{newTxn.CustomerSequenceNo}.json" : JsonFilePath;

            InsertSingleObjectToListJson(jsonFilePath, newTxn);
        }

        public static void AddDailyTransactions(Transaction txn)
        {
            InsertSingleObjectToListJson(JsonFilePath, txn);
        }

        public static void AddTransactions(List<Transaction> newTxns)
        {
            InsertObjectsToJson(JsonFilePath, newTxns);
        }

        public static void AddBatchTransactions(List<Transaction> newTxns, string fileName)
        {
            var fullFilePath = Path.Combine(AppConfiguration.DailyBatchFile, fileName);
            if (File.Exists(fullFilePath)) File.Delete(fullFilePath);

            WriteObjectsToFile(newTxns, fullFilePath);
        }


        //TODO: its not yet started using.
        public static bool DeleteTransactionDetails(int customerId, int sequenceNo, bool isActive)
        {

            try
            {
                var filePath = isActive ? JsonFilePath : $"{ClosedTxnFilePath}/{customerId}/{customerId}_{sequenceNo}.json";
                if (isActive)
                {
                    var list = ReadFileAsObjects<Transaction>(filePath);

                    list.RemoveAll(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddClosedTransaction(List<Transaction> closedTxn)
        {
            var customer = closedTxn.First();

            string customerBackupFolderPath = Path.Combine(ClosedTxnFilePath, customer.CustomerId.ToString());

            if (Directory.Exists(customerBackupFolderPath) == false)
            {
                Directory.CreateDirectory(customerBackupFolderPath);
            }

            string backupFileName = Path.Combine(customerBackupFolderPath, $"{customer.CustomerId.ToString()}_{customer.CustomerSequenceNo.ToString()}.json");

            WriteObjectsToFile(closedTxn, backupFileName);
        }

        public static void UpdateTransactionDetails(Transaction updatedTransaction)
        {
            try
            {
                List<Transaction> list = ReadFileAsObjects<Transaction>(AppConfiguration.TransactionFile);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId && c.CustomerSequenceNo == updatedTransaction.CustomerSequenceNo && c.CustomerId == updatedTransaction.CustomerId).FirstOrDefault();
                
                u.Balance = ((u.Balance + u.AmountReceived) - updatedTransaction.AmountReceived); // Very important calculation when corrected collection amount more than 1 time for he same date.
                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnUpdatedDate = DateTime.Today;

                WriteObjectsToFile(list, AppConfiguration.TransactionFile);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CorrectTransactionData(Transaction updatedTransaction)
        {

            try
            {
                var filePath = updatedTransaction.IsClosed ? $"{AppConfiguration.ClosedNotesFile}/{updatedTransaction.CustomerId}/{updatedTransaction.CustomerId}_{updatedTransaction.CustomerSequenceNo}.json" : AppConfiguration.TransactionFile;

                List<Transaction> list = ReadFileAsObjects<Transaction>(filePath);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();

                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnDate = updatedTransaction.TxnDate;
                u.Balance = updatedTransaction.Balance;

                WriteObjectsToFile(list, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Transaction GetTransactionDetail(int txnId)
        {
            try
            {
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;
                return list.Where(c => c.TransactionId == txnId).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionDetails(Customer customer)
        {
            try
            {
                var txnFile = (customer.IsActive == false) ? $"{ClosedTxnFilePath}/{customer.CustomerId}/{customer.CustomerId}_{customer.CustomerSeqNumber}.json" : JsonFilePath;

                if (File.Exists(txnFile))
                {
                    var list = ReadFileAsObjects<Transaction>(txnFile);
                    if (list == null) return null;
                    return list.Where(c => c.CustomerId == customer.CustomerId && c.CustomerSequenceNo == customer.CustomerSeqNumber).OrderBy(o => o.TxnDate.Date).ThenByDescending(t => t.Balance).ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionForDate(Transaction txn)
        {
            try
            {
                //TODO: need to do it for closed txn also?
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;

                return list.Where(c => c.CustomerId == txn.CustomerId && c.CustomerSequenceNo == txn.CustomerSequenceNo && c.TxnDate.Date == txn.TxnDate.Date).OrderByDescending(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionForDate(DateTime txnDate)
        {
            try
            {
                //TODO: need to do it for closed txn also?
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

                if (list == null) return null;

                return list.Where(c => c.TxnDate.Date == txnDate.Date).OrderByDescending(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Transaction> GetClosedTransactionForDate(DateTime txnDate)
        {
            try
            {
                //TODO: need to do it for closed txn also?
                var result = new List<Transaction>();

                foreach (var folder in Directory.GetDirectories(ClosedTxnFilePath))
                {
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        var list = ReadFileAsObjects<Transaction>(file);
                        var data = list.Where(w => w.TxnDate.Date == txnDate.Date).OrderByDescending(o => o.TransactionId).ToList();

                        result.AddRange(data);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Transaction> GetTransactionCount(int customerId, int sequenceNo)
        {
            try
            {
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;
                return list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == c.CustomerSequenceNo).OrderBy(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteTransactionDetails(int customerId, int sequenceNo)
        {
            try
            {
                //TODO: Need to implement delete txn for closed txns also.
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).ToList();
                list.RemoveAll((c) => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetNextTransactionId()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.TransactionId).Max() + 1);

        }

        public static int GetBalance(Customer customer)
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);
            if (list == null || list.Count == 0) return customer.LoanAmount - 0;

            var paidAmount = (list.Where(s => s.CustomerSequenceNo == customer.CustomerSeqNumber && s.CustomerId == customer.CustomerId).Sum(s => s.AmountReceived));
            return (customer.LoanAmount - paidAmount);

        }

        public static List<Transaction> GetActiveTransactions()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.Balance).First());
            });

            return result;


        }

        public static dynamic GetTransactionsToBeClosedSoon(int top)
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            var customers = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();


            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.Balance).First());
            });

            var data = (from c in customers
                        join t in result on
                        new { CustomerSeqNumber = c.CustomerSeqNumber, c.CustomerId } equals new { CustomerSeqNumber = t.CustomerSequenceNo, t.CustomerId }

                        select new
                        {
                            RunningDays = (DateTime.Now - c.AmountGivenDate).Value.Days,
                            c.Name,
                            c.LoanAmount,
                            t.Balance,
                            CreditScore = Customer.GetCreditScore(c.CustomerId),
                            NeedToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? 0 : ((c.AmountGivenDate.Value.AddDays(100) - DateTime.Today.Date).Days),
                            DaysToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? (100 - (DateTime.Now - c.AmountGivenDate.Value.Date).Days) : (t.Balance / (c.LoanAmount / 100)),
                            c.AmountGivenDate,
                            c.CustomerSeqNumber,
                        }).OrderBy(o => o.NeedToClose).Take(top).ToList();

            return data;


        }

        public static dynamic GetTransactionsNotGivenForFewDays()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            var customers = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();


            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.TxnDate).Last());
            });

            var data = (from c in customers
                        join t in result on c.CustomerSeqNumber equals t.CustomerSequenceNo
                        select new
                        {
                            c.Name,
                            c.LoanAmount,
                            t.Balance,
                            CreditScore = Customer.GetCreditScore(c.CustomerId),
                            NotGivenFor = (DateTime.Now.Date - t.TxnDate.Date).TotalDays,
                            LastTxnDate = t.TxnDate,
                            c.AmountGivenDate,
                            c.CustomerSeqNumber                            
                        }).Where(w => w.NotGivenFor > 2).OrderByDescending(o => o.NotGivenFor).ToList();

            return data;
        }

        public static (int actual, int includesProfit) GetAllOutstandingAmount()
        {
            try
            {
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

                if (list == null)
                {
                    var customersOutstanding = (Customer.GetAllCustomer() ?? new List<Customer>()).Where(w => w.IsActive);
                    return (customersOutstanding.Sum(s => s.LoanAmount - s.Interest), customersOutstanding.Sum(s => s.LoanAmount));
                }

                // This needs to validate.
                var outsideMoney = (from L in list

                                    group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                    select newGroup.ToList().OrderBy(w => w.Balance).First()).ToList(); //.Sum(s => s.Balance);

                // this is useful to calculate all external balances
                var result = (from c in Customer.GetAllCustomer()
                              join t in outsideMoney on new { CustomerSeqNumber = c.CustomerSeqNumber, c.CustomerId } equals new
                              {
                                  CustomerSeqNumber = t.CustomerSequenceNo,
                                  t.CustomerId
                              }
                              orderby c.AmountGivenDate
                              select new {
                                  c.Name,
                                  c.AmountGivenDate,
                                  c.CustomerSeqNumber,
                                  c.CustomerId,
                                  t.Balance,
                                  c.LoanAmount,
                                  c.Interest,
                                  t.TransactionId }).ToList();


                return (result.Sum(s => s.Balance - s.Interest), result.Sum(s => s.Balance));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetClosedTxn()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            return list.Where(w => w.Balance == 0).Count();
        }

        public static List<Transaction> GetDailyCollectionDetails(DateTime inputDate)
        {

            // Get from Ongoing Transcations
            var txnFile = Path.Combine(AppConfiguration.DailyBatchFile, inputDate.ToString("dd-MM-yyyy")); // TODO: Ned to fix this as data is not showing till run daily txn batch // new approach
            if (File.Exists(txnFile) == false) return new List<Transaction>();

            var list = ReadFileAsObjects<Transaction>(txnFile);

            if (list == null) return null;

            var fromActiveTxn = list.Where(c => (c.TxnDate.Date == inputDate.Date)).ToList(); // && c.AmountReceived > 0

            return fromActiveTxn;
        }

        public static List<Transaction> GetDailyCollectionDetails_V0(DateTime inputDate)
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);
            if (list == null) return null;
            var fromActiveTxn = list.Where(c => c.TxnDate.Date == inputDate.Date).ToList(); //  && c.AmountReceived > 0

            //Get from Closed Transactions
            var fromClosedTxn = ProcessDirectory(ClosedTxnFilePath, inputDate);

            fromActiveTxn.AddRange(fromClosedTxn);

            return fromActiveTxn;
        }

        private static List<Transaction> ProcessDirectory(string targetDirectory, DateTime inputDate)
        {
            List<Transaction> result = new List<Transaction>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                var list = ReadFileAsObjects<Transaction>(fileName);
                // if (list == null) return null;
                var data = list.Where(c => c.TxnDate.Date == inputDate.Date);
                if (data != null && data.Count() > 0)
                    result.AddRange(data);
            }


            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                result.AddRange(ProcessDirectory(subdirectory, inputDate));

            return result;
        }

    }

}
