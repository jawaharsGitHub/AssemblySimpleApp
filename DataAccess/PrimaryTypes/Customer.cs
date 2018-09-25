using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Customer : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.CustomerFile;
        private DayOfWeek? _returnDay;
        private int _collSpot;

        public int CustomerSeqNumber { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int LoanAmount { get; set; }
        public int Interest { get; set; }
        public bool IsExistingCustomer { get; set; }
        public bool IsActive { get; set; }
        public DateTime? AmountGivenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public ReturnTypeEnum ReturnType { get; set; }
        public int CollectionSpotId
        {
            get { return (_collSpot == 0) ? CustomerId : _collSpot; }
            set { _collSpot = value; }
        }
        public DayOfWeek? ReturnDay
        {
            get { return (ReturnType == ReturnTypeEnum.Daily) ? null : _returnDay; }
            set { _returnDay = value; }
        }
        public DateTime? ModifiedDate { get; set; }

        [JsonIgnore]
        public string IdAndName => $"{CustomerId}-{Name}";


        public static void AddCustomer(Customer newCustomer)
        {
            newCustomer.ModifiedDate = null;
            newCustomer.IsActive = true;
            newCustomer.ClosedDate = null;

            InsertSingleObjectToListJson(JsonFilePath, newCustomer);


        }

        public static void CloseCustomerTxn(Customer updatedCustomer, bool isActive, DateTime? closedDate)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber && c.IsActive == true).FirstOrDefault();
                u.IsActive = isActive;
                u.ClosedDate = closedDate;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsDuplicateName(string name)
        {
            try
            {
                var count = (from c in GetAllCustomer()
                             where c.Name.Trim() == name
                             select c).Count();

                return (count > 0);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CorrectCustomerData(Customer updatedCustomer)
        {

            try
            {
                if (updatedCustomer.ClosedDate == DateTime.MinValue)
                {
                    updatedCustomer.ClosedDate = null;
                }

                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var oneCustomers = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).First();

                if (oneCustomers.Name != updatedCustomer.Name)
                {
                    oneCustomers.Name = updatedCustomer.Name;
                }

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();

                u.AmountGivenDate = updatedCustomer.AmountGivenDate; // Done: need to update all fields later
                u.ClosedDate = updatedCustomer.ClosedDate;
                u.Interest = updatedCustomer.Interest;
                u.LoanAmount = updatedCustomer.LoanAmount;
                u.CustomerId = updatedCustomer.CustomerId;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateCustomerClosedDate(Customer updatedCustomer)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();
                u.ClosedDate = updatedCustomer.ClosedDate;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateCustomerReturnType(Customer updatedCustomer)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();
                u.ReturnType = updatedCustomer.ReturnType;
                u.ReturnDay = updatedCustomer.ReturnDay;
                u.CollectionSpotId = updatedCustomer.CollectionSpotId;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Customer GetCustomerDetails(Customer customer)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list.Where(c => c.CustomerId == customer.CustomerId && c.CustomerSeqNumber == customer.CustomerSeqNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Customer GetCustomerDetails(Transaction txn)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list.Where(c => c.CustomerId == txn.CustomerId && c.CustomerSeqNumber == txn.CustomerSequenceNo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Customer> GetAllCustomer()
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Customer> GetAllUniqueCustomers()
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteCustomerDetails(int customerId, int sequenceNo)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSeqNumber == sequenceNo).FirstOrDefault();
                list.Remove(itemToDelete);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static (int NewCustomerId, int NewCustomerSeqId) GetNextIds()
        {
            List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

            if (list == null || list.Count == 0) return (1, 1);

            var newCustomerId = (list.Max(m => m.CustomerId) + 1);

            var newCustomerSeqId = (list.Max(m => m.CustomerSeqNumber) + 1);

            return (newCustomerId, newCustomerSeqId);

        }

        public static int GetNextCustomerSeqNo()
        {
            List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.CustomerSeqNumber).Max() + 1);
        }

        public static CreditReport GetCreditScore(Customer customer)
        {

            //var cus = Customer.GetCustomerDetails(cu);

            var _isClosedTx = (customer.IsActive == false);

            var txns = Transaction.GetTransactionDetails(customer);

            if (txns == null || txns.Count == 0) return null;

            var dataDource = txns;


            //if (byBalance)
            //    dataGridView1.DataSource = dataDource.OrderBy(o => o.Balance).ToList();
            //else if (isDesc)
            //    dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate).ThenBy(t => t.Balance).ToList();
            //else
            //    dataGridView1.DataSource = dataDource.OrderBy(o => o.TxnDate).ToList();

            var lastBalance = txns.Min(m => m.Balance);

            var startDate = dataDource.Select(s => s.TxnDate).Min();
            var lastDate = dataDource.Select(s => s.TxnDate).Max();
            var daysTaken = (lastBalance == 0) ? lastDate.Date.Subtract(startDate).Days + 2 : DateTime.Now.Date.Subtract(startDate).Days + 2;


            //lblStartDate.Text = $"Start Date: {startDate.Date.ToShortDateString()}";
            //lblLastDate.Text = $"Last Date: {lastDate.Date.ToShortDateString()}";

            var expected = (daysTaken * (customer.LoanAmount / 100)) > customer.LoanAmount ? -1 : (daysTaken * (customer.LoanAmount / 100));

            var _balance = _isClosedTx ? 0 : Transaction.GetBalance(customer);
            var actual = customer.LoanAmount - _balance;

            var dayTaken = daysTaken;
            var _expected = expected;
            var _actual = actual;


            //lblNoOfDays.Text = $"Days taken to Return {DaysTaken} (Expected {expected} ACTUAL {actual})";

            //if (expected == -1)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.IndianRed;
            //else if (actual < expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Red;
            //else if (actual > expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Green;
            //else if (actual == expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Honeydew;


            var interestRate = (customer.LoanAmount / customer.Interest) == 12 ? 8.69 : 11.11;


            var percGainPerMonth = Math.Round(((interestRate / daysTaken) * 30), 2); // 8.89% is % of 800 for 9200 for one month.

            //var percentage = percGainPerMonth;
            var interestPerMonth = (percGainPerMonth / 100) * (customer.LoanAmount - customer.Interest);

            //lblPercentageGain.Text = $"{percGainPerMonth.ToString()}% Per Month({(percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest)} Rs/Month)";

            //dateTimePicker1.Value = lastDate.AddDays(1);

            //dataGridView1.Columns["CustomerId"].Visible = false;
            //dataGridView1.Columns["IsClosed"].Visible = false;
            //dataGridView1.Columns["TxnUpdatedDate"].Visible = false;
            //dataGridView1.Columns["CustomerSequenceNo"].Visible = false;

            // Credit Score Calculation.
            // TODO: Need to move as a seperate method - CalculateCreditScore()

            double _creditScore = 0;
            List<DateTime> col = txns.Select(s => s.TxnDate.Date).ToList();
            var _missingLastDate = _isClosedTx ? lastDate : DateTime.Today.Date;


            var range = (Enumerable.Range(0, (int)(_missingLastDate - startDate).TotalDays + 1)
                                  .Select(i => startDate.AddDays(i).Date)).ToList();

            var missingDays = range.Except(col).ToList().Count;


            double perDayAmount = (customer.LoanAmount / 100);

            double perDayValue = (perDayAmount / 100.0);

            _creditScore -= missingDays * perDayValue;  // 1.Missing Days (value = -1)


            _creditScore -= (daysTaken > 100) ? ((daysTaken - 100) * 0.75 * perDayValue) : 0; // 2.Above 100 Days (value = -1.5)


            // 3.Lumb amount (value = +0.75)
            var lumbCount = (from t in txns
                             where t.AmountReceived > (customer.LoanAmount / 100)
                             select ((t.AmountReceived - perDayAmount) / perDayAmount)).ToList();
            _creditScore += (lumbCount.Sum() * 0.75 * perDayValue);


            if (_isClosedTx)    // 4.Number days saved(value = 1)
                _creditScore += (daysTaken < 100) ? ((100 - daysTaken) * 1 * perDayValue) : 0;

            // Credit Score Calculation

            return new CreditReport
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                CreditScore = _creditScore,
                InterestRate = interestRate,
                PercGainPerMonth = percGainPerMonth,
                InterestPerMonth = interestPerMonth,
                DaysTaken = daysTaken,
                MissingDays = missingDays,

            };


        }

        public static double GetCreditScore(int _customerId)
        {
            var creditScores = (from c in Customer.GetAllCustomer()
                                where c.CustomerId == _customerId
                                select GetCreditScore(c)).ToList();

            return creditScores.Average(a => a.CreditScore).RoundPoints();

        }

    }



}
