using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Zonal : BaseClass
    {
        
   

        private static string JsonFilePath = AppConfiguration.ZonalFile;

        public int ZonalId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Headquarters { get; set; }
        public DateTime Established { get; set; }
        public string FormedFrom { get; set; }
        public decimal AreakmSquare { get; set; }
        public int Population { get; set; }
        public int PopulationDensity { get; set; }
        public int? District { get; set; }
        public int Taluk { get; set; }
        public int? Block { get; set; }

        public Zonal(int zonalId, string name)
        {
            ZonalId = zonalId;
            Name = name;


        }


        public static void AddDivision(Zonal newCustomer)
        {
            InsertSingleObjectToListJson(JsonFilePath, newCustomer);
        }

        public static List<Zonal> GetAll()
        {
            return ReadFileAsObjects<Zonal>(JsonFilePath);
        }


        public static void Update(Zonal updatedCustomer)
        {

            try
            {

                List<Zonal> list = ReadFileAsObjects<Zonal>(JsonFilePath);

                var division = list.Where(c => c.ZonalId == updatedCustomer.ZonalId).First();

                division.Name = updatedCustomer.Name;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Zonal GetDivisionDetails(Zonal division)
        {
            try
            {
                List<Zonal> list = ReadFileAsObjects<Zonal>(JsonFilePath);
                return list.Where(c => c.ZonalId == division.ZonalId).FirstOrDefault();
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
                List<Zonal> list = ReadFileAsObjects<Zonal>(JsonFilePath);

                var itemToDelete = list.Where(c => c.ZonalId == customerId).FirstOrDefault();
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
            List<Zonal> list = ReadFileAsObjects<Zonal>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;


            return (list.Max(m => m.ZonalId) + 1);


        }

    }



}
