using Common;
using NTK_Support.AdangalTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NTK_Support
{
    public class DataAccess : BaseClass
    {
        public static List<Adangal> GetAdangal(string fileName)
        {
            var filePath = AppConfiguration.GetDynamicPath($"{fileName}.json");
            var data  = ReadFileAsObjects<Adangal>(filePath);
            return data;
        }

        public static List<Adangal> AdangalToJson(List<Adangal> adangalData, string fileName)
        {
            var filePath = AppConfiguration.GetDynamicPath($"{fileName}.json");

            if (File.Exists(filePath) == false)
            {
                WriteObjectsToFile<Adangal>(adangalData, filePath);
            }
            var data = ReadFileAsObjects<Adangal>(filePath);
            return data;
        }

        public static bool IsAdangalExist(string fileName)
        {
            var filePath = AppConfiguration.GetDynamicPath($"{fileName}.json");
            return File.Exists(filePath);
        }

        public static List<Adangal> SetDeleteFlag(string fileName, List<string> adangalToBeDelete)
        {
            var filePath = AppConfiguration.GetDynamicPath($"{fileName}.json");
            var data = ReadFileAsObjects<Adangal>(filePath);

            adangalToBeDelete.ForEach(fe => {
                data.Where(w => w.NilaAlavaiEn.ToString() == fe.Split('~')[0] && w.UtpirivuEn == fe.Split('~')[1]).First().LandStatus = LandStatus.Deleted;
            });
            WriteObjectsToFile(data, filePath);
            return ReadFileAsObjects<Adangal>(filePath);
        }

        public static List<ComboData> GetDistricts()
        {
            var filePath = AppConfiguration.GetDynamicPath($"database/RevDistrict.json");

            
                return ReadFileAsObjects<ComboData>(filePath);
            
        }

    }
}
