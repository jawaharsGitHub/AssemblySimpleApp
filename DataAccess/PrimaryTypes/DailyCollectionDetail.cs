using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class DailyCollectionDetail : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.DailyTxnFile;

        public string Date { get; set; }
        public int? YesterdayAmountInHand { get; set; }
        public decimal? SentFromUSA { get; set; }
        public decimal? InBank { get; set; }
        public decimal? BankTxnOut { get; set; }
        public int? TakenFromBank { get; set; }
        public int? CollectionAmount { get; set; }
        public int? GivenAmount { get; set; }
        public int? Interest { get; set; }
        public int? TodayInHand { get; set; }
        public int? ClosedAccounts { get; set; }
        public int? OpenedAccounts { get; set; }
        public int? TomorrowNeed { get; set; }
        public int? TomorrowDiff { get; set; }
        public string Comments { get; set; }
        public int OtherExpenditire { get; set; }
        public int OtherInvestment { get; set; }


        public static void AddOrUpdateDaily(DailyCollectionDetail dailyCol)
        {


            var list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
            if (list != null && list.Count > 0)
            {
                var data = list.Where(w => w.Date == dailyCol.Date).ToList().FirstOrDefault();

                if (data != null) // Update
                {
                    //data.BankTxnOut = dailyCol.BankTxnOut;
                    //data.ClosedAccounts = dailyCol.ClosedAccounts;
                    //data.CollectionAmount = dailyCol.CollectionAmount;
                    data.Comments = dailyCol.Comments;
                    //data.Date = dailyCol.Date;
                    //data.GivenAmount = dailyCol.GivenAmount;
                    //data.InBank = dailyCol.InBank;
                    //data.Interest = dailyCol.Interest;
                    //data.OpenedAccounts = dailyCol.OpenedAccounts;
                    //data.OtherExpenditire = dailyCol.OtherExpenditire;
                    //data.OtherInvestment = dailyCol.OtherInvestment;
                    //data.SentFromUSA = dailyCol.SentFromUSA;
                    //data.TakenFromBank = dailyCol.TakenFromBank;
                    //data.TodayInHand = dailyCol.TodayInHand;
                    //data.TomorrowDiff = dailyCol.TomorrowDiff;
                    //data.TomorrowNeed = dailyCol.TomorrowNeed;
                    //data.YesterdayAmountInHand = dailyCol.YesterdayAmountInHand;

                    WriteObjectsToFile(list, JsonFilePath);
                }
                else // New (Insert)
                {
                    InsertSingleObjectToListJson(JsonFilePath, dailyCol);

                    // Update In Hand and In Bank amount.
                    var inhand = new InHandAndBank()
                    {
                        Date = dailyCol.Date,
                        InBank = dailyCol.InBank.Value,
                        InHandAmount = dailyCol.TodayInHand.Value
                    };

                    InHandAndBank.AddInHand(inhand, dailyCol.TakenFromBank);

                }

            }


        }
        public static void UpdateDaily(DailyCollectionDetail dailyCol)
        {



        }

        public static DailyCollectionDetail GetDailyTxn(DateTime date, bool isOnLoad)
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
                var dailyTxn = list.Where(c => c.Date == date.ToShortDateString()).FirstOrDefault();

                return dailyTxn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}