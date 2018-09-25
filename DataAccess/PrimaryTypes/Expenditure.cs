using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{

    public class Expenditure : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.ExpenditureFile;
        public string Date { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }


        public static void AddExpenditure(Expenditure expenditure)
        {
            expenditure.Date = DateTime.Today.ToLongTimeString();

            InsertSingleObjectToListJson(AppConfiguration.ExpenditureFile, expenditure);
        }

        public static List<Expenditure> GetAllExpenditure()
        {
            try
            {
                var list = ReadFileAsObjects<Expenditure>(JsonFilePath);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetTotalExpenditure()
        {
            try
            {
                var json = GetAllExpenditure();
                return json == null ? 0 : json.Sum(s => s.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
