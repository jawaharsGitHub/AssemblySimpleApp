using Common;
using System;
using System.Collections.Generic;

namespace DataAccess.PrimaryTypes
{
    public class Investment : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.InvestmentFile;

        public InvestmentFrom InvestType { get; set; }
        public int Amount { get; set; }
        public int Interest { get; set; }
        public int CustomerId { get; set; }
        public int CustomerSequenceNo { get; set; }
        public string CreatedDate { get; set; }


        public static void AddInvestment(Investment investment)
        {
            investment.CreatedDate = DateTime.Today.ToLongTimeString();
            InsertSingleObjectToListJson(JsonFilePath, investment);
        }

        public static List<Investment> GetAllInvestmet()
        {
            try
            {
                var list = ReadFileAsObjects<Investment>(JsonFilePath);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum InvestmentFrom
    {
        Jawahar = 1,
        Company,

    }
}
