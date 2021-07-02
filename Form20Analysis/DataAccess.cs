using Common;
using NTK_Support.AdangalTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NTK_Support
{
    public class DataAccess : BaseClass
    {

        private static string GetAdangalPath(string fileName)
        {
            return AppConfiguration.GetDynamicPath($"Adangal/{fileName}.json");
        }

        private static string GetdatabasePath(string fileName)
        {
            return AppConfiguration.GetDynamicPath($"database/{fileName}.json");
        }

        public static List<Adangal> GetAdangal(string fileName)
        {
            var filePath = GetAdangalPath(fileName);
            var data = ReadFileAsObjects<Adangal>(filePath);
            return data;
        }

        public static List<KeyValue> GetSubdiv(string fileName)
        {
            var filePath = GetAdangalPath(fileName);
            var data = ReadFileAsObjects<KeyValue>(filePath);
            return data;
        }

        public static List<KeyValue> SubdivToJson(List<KeyValue> subdivData, string fileName)
        {
            var filePath = GetAdangalPath(fileName);

            if (File.Exists(filePath) == false)
            {
                WriteObjectsToFile<KeyValue>(subdivData, filePath);
            }
            //var data = GetSubdiv(filePath);
            var data = ReadFileAsObjects<KeyValue>(filePath);
            return data;
        }

        public static List<Adangal> AdangalToJson(List<Adangal> adangalData, string fileName)
        {
            var filePath = GetAdangalPath(fileName);

            if (File.Exists(filePath) == false)
            {
                WriteObjectsToFile<Adangal>(adangalData, filePath);
            }
            var data = GetActiveAdangal(filePath, false);
            return data;
        }

        public static bool IsAdangalExist(string fileName)
        {
            var filePath = GetAdangalPath(fileName);
            return File.Exists(filePath);
        }

        public static List<Adangal> SetDeleteFlag(string fileName, List<string> adangalToBeDelete)
        {
            var filePath = GetAdangalPath(fileName);
            var data = ReadFileAsObjects<Adangal>(filePath);

            adangalToBeDelete.ForEach(fe =>
            {
                data.Where(w => w.NilaAlavaiEn.ToString() == fe.Split('~')[0] && w.UtpirivuEn == fe.Split('~')[1]).First().LandStatus = LandStatus.Deleted;
            });
            WriteObjectsToFile(data, filePath);
            return GetActiveAdangal(filePath, false);
        }

        public static List<Adangal> GetActiveAdangal(string fileName, bool directCall)
        {
            var filePath = directCall ? GetAdangalPath(fileName) : fileName; // ???
            return ReadFileAsObjects<Adangal>(filePath).Where(w => w.LandStatus != LandStatus.Deleted).ToList();
        }


        public static List<Adangal> GetDeletedAdangal(string fileName, bool directCall)
        {
            var filePath = directCall ? GetAdangalPath(fileName) : fileName; // ???
            return ReadFileAsObjects<Adangal>(filePath).Where(w => w.LandStatus == LandStatus.Deleted).ToList();
        }

        public static List<Adangal> GetErrorAdangal(string fileName, bool directCall)
        {
            var filePath = directCall ? GetAdangalPath(fileName) : fileName; // ???
            return ReadFileAsObjects<Adangal>(filePath).Where(w => w.LandStatus == LandStatus.Error).ToList();
        }

        public static List<Adangal> GetNameIssueAdangal(string fileName, bool directCall)
        {
            var filePath = directCall ? GetAdangalPath(fileName) : fileName; // ???
            return ReadFileAsObjects<Adangal>(filePath).Where(w => w.LandStatus == LandStatus.WrongName).ToList();
        }

        public static List<Adangal> UpdateOwnerName(Adangal adn, string ownerName, string fileName, bool directCall)
        {
            var filePath = directCall ? GetAdangalPath(fileName) : fileName; // ???

            var data = ReadFileAsObjects<Adangal>(filePath);

            var existingName = data.Where(w => w.PattaEn == adn.PattaEn).First().OwnerName;

            data.Where(w => w.LandStatus == LandStatus.WrongName &&
                            //w.PattaEn == adn.PattaEn && 
                            w.OwnerName == existingName).ToList().ForEach(fe => {
                                fe.OwnerName = ownerName;
                                fe.LandStatus = LandStatus.NameEdited;
                            });

            //data.Where(w => w.OwnerName == adn.OwnerName &&
            //                w.PattaEn == adn.pattaEn).ToList().ForEach(fe => {
            //                    fe.OwnerName = ownerName;
            //                    fe.LandStatus = LandStatus.NameEdited;
            //                });

            WriteObjectsToFile(data, filePath);

            return GetNameIssueAdangal(filePath, false);

        }


        public static bool AddNewAdangal(string fileName, Adangal adangal)
        {
            var filePath = GetAdangalPath(fileName);
            InsertSingleObjectToListJson<Adangal>(filePath, adangal);
            return true;
        }
        public static List<ComboData> GetDistricts()
        {
            var filePath = GetdatabasePath("RevDistrict");
            return ReadFileAsObjects<ComboData>(filePath);

        }

        
    }
}
