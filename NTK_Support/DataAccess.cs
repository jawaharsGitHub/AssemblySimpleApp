using Common;
using NTK_Support.AdangalTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NTK_Support
{
    public class DataAccess : BaseClass
    {
        public static string JsonPath = "";
        public static string SubDivPath = "";

        public static void SetVillageName()
        {
            JsonPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}.json");
            SubDivPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}-subdiv.json");

            if (Directory.Exists(Directory.GetParent(JsonPath).FullName) == false)
                Directory.CreateDirectory(Directory.GetParent(JsonPath).FullName);
        }

        private static string GetTablePath(string tableName)
        {
            return AppConfiguration.GetDynamicPath($"database/{tableName}.json");
        }

        public static List<KeyValue> GetSubdiv()
        {
            var data = ReadFileAsObjects<KeyValue>(SubDivPath);
            return data;
        }

        public static List<KeyValue> SubdivToJson(List<KeyValue> subdivData)
        {
            if (File.Exists(SubDivPath) == false)
                WriteObjectsToFile<KeyValue>(subdivData, SubDivPath);

            var data = ReadFileAsObjects<KeyValue>(SubDivPath);
            return data;
        }

        public static List<Adangal> AdangalToJson(List<Adangal> adangalData)
        {
            if (File.Exists(JsonPath) == false)
                File.Create(JsonPath).Close();

            WriteObjectsToFile<Adangal>(adangalData, JsonPath);

            var data = GetActiveAdangal();
            return data;
        }

        public static bool IsSubDivExist()
        {
            return File.Exists(SubDivPath);
        }

        public static List<Adangal> SetDeleteFlag(List<string> adangalToBeDelete)
        {
            var data = ReadFileAsObjects<Adangal>(JsonPath);

            adangalToBeDelete.ForEach(fe =>
            {
                data.Where(w => w.NilaAlavaiEn.ToString() == fe.Split('~')[0] && w.UtpirivuEn == fe.Split('~')[1])
                            .First().LandStatus = LandStatus.Deleted;
            });
            WriteObjectsToFile(data, JsonPath);
            return GetActiveAdangal();
        }

        public static bool IsAdangalExist()
        {
            return File.Exists(JsonPath);
        }
        public static List<Adangal> GetActiveAdangal()
        {
            return ReadFileAsObjects<Adangal>(JsonPath).Where(w => w.LandStatus != LandStatus.Deleted).ToList();
        }

        public static List<Adangal> GetDeletedAdangal()
        {
            return ReadFileAsObjects<Adangal>(JsonPath).Where(w => w.LandStatus == LandStatus.Deleted).ToList();
        }

        public static List<Adangal> GetErrorAdangal()
        {
            return ReadFileAsObjects<Adangal>(JsonPath).Where(w => w.LandStatus == LandStatus.Error).ToList();
        }

        public static List<Adangal> GetNameIssueAdangal()
        {
            return ReadFileAsObjects<Adangal>(JsonPath).Where(w => w.LandStatus == LandStatus.WrongName).ToList();
        }

        public static List<Adangal> UpdateOwnerName(Adangal adn, string ownerName)
        {
            var data = ReadFileAsObjects<Adangal>(JsonPath);

            var existingName = data.Where(w => w.PattaEn == adn.PattaEn).First().OwnerName;

            data.Where(w => w.LandStatus == LandStatus.WrongName &&
                            w.OwnerName == existingName).ToList().ForEach(fe =>
                            {
                                fe.OwnerName = ownerName;
                                fe.LandStatus = LandStatus.NameEdited;
                            });

            WriteObjectsToFile(data, JsonPath);

            return GetNameIssueAdangal();

        }


        public static bool AddNewAdangal(Adangal adangal)
        {
            InsertSingleObjectToListJson<Adangal>(JsonPath, adangal);
            return true;
        }
        public static List<ComboData> GetDistricts()
        {
            var filePath = GetTablePath("RevDistrict");
            try
            {
                return ReadFileAsObjects<ComboData>(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
