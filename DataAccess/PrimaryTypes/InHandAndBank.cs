using Common;
using System;

namespace DataAccess.PrimaryTypes
{
    public class InHandAndBank : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.InHandFile;

        public int InHandAmount { get; set; }
        public decimal InBank { get; set; }
        public string Date { get; set; }
        public int? RealInvestment { get; set; }


        public static void AddInHand(InHandAndBank inHand, int? takenFromBank)
        {
            var existingMoney = GetAllhandMoney();

            inHand.RealInvestment = (existingMoney.RealInvestment + takenFromBank);
            InsertSingleObjectToSingleJson(JsonFilePath, inHand);
        }

        public static InHandAndBank GetAllhandMoney()
        {
            try
            {
                return ReadFileAsSingleObject<InHandAndBank>(JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
