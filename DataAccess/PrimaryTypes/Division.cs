using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Division : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.DivisionFile;

        public int DivisionId { get; set; }
        public string Name { get; set; }


        public static void AddDivision(Division newCustomer)
        {
            InsertSingleObjectToListJson(JsonFilePath, newCustomer);
        }

        public static List<Division> GetAll()
        {
            return ReadFileAsObjects<Division>(JsonFilePath);
        }


        public static void Update(Division updatedCustomer)
        {

            try
            {

                List<Division> list = ReadFileAsObjects<Division>(JsonFilePath);

                var division = list.Where(c => c.DivisionId == updatedCustomer.DivisionId).First();

                division.Name = updatedCustomer.Name;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Division GetDivisionDetails(Division division)
        {
            try
            {
                List<Division> list = ReadFileAsObjects<Division>(JsonFilePath);
                return list.Where(c => c.DivisionId == division.DivisionId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void DeleteDivision(int customerId, int sequenceNo)
        {
            try
            {
                List<Division> list = ReadFileAsObjects<Division>(JsonFilePath);

                var itemToDelete = list.Where(c => c.DivisionId == customerId).FirstOrDefault();
                list.Remove(itemToDelete);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetNextId()
        {
            List<Division> list = ReadFileAsObjects<Division>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;


            return (list.Max(m => m.DivisionId) + 1);


        }

    }



}
