using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class BaseData : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.BaseDataFile;

        public int DistrictNoId { get; set; }

        public string DistrictName { get; set; }

        public int OndriumId { get; set; }

        public string OndriumName { get; set; }

        public int PanchayatId { get; set; }

        public string PanchayatName { get; set; }

        [JsonIgnore]
        public string OndriumFullName { get { return $"{OndriumId}-{OndriumName}"; } }
        
        public static List<BaseData> GetBaseData(int districtId)
        {
            var baseDataForDis = GetAll().Where(w => w.DistrictNoId == districtId).ToList();

            return baseDataForDis;
            //AssemblyNo = assemblyId;
            //AssemblyName = name;

        }

        public static List<BaseData> GetAll()
        {
            return ReadFileAsObjects<BaseData>(JsonFilePath);
        }


    }
}
