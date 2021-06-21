using Common;
using NTK_Support.AdangalTypes;
using System.Collections.Generic;
using System.IO;

namespace NTK_Support
{
    public class DataAccess : BaseClass
    {
        public static List<Adangal> GetAdangal(int districtCode, int talukCode, int villageCode, List<Adangal> adangalData)
        {
            var filePath = AppConfiguration.GetDynamicPath($"{districtCode}/{talukCode}/{villageCode}.json");

            if(File.Exists(filePath))
            {
                WriteObjectsToFile<Adangal>(adangalData, filePath);
            }
            else
            {

            }

            var data  = ReadFileAsObjects<Adangal>(filePath);


            return null;
        }

        public static List<ComboData> GetDistricts()
        {
            var filePath = AppConfiguration.GetDynamicPath($"database/RevDistrict.json");

            
                return ReadFileAsObjects<ComboData>(filePath);
            
        }

    }
}
