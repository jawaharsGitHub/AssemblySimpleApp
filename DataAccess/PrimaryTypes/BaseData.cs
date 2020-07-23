using Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class BaseData : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.BaseDataFile;

        private static List<BaseData> OndriumForDistrict;
        //private static List<BaseData> PanchayatForOndrium;
        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int OndriumId { get; set; }

        public string OndriumName { get; set; }

        public int PanchayatId { get; set; }

        public string PanchayatName { get; set; }

        public PaguthiType PaguthiType { get; set; }

        public int AssemblyId { get; set; }

        //public int BoothId { get; set; }

        [JsonIgnore]
        public string OndriumFullName { get { return $"{OndriumId}-{OndriumName}"; } }

        public static List<BaseData> GetOndrium(int districtId)
        {
            OndriumForDistrict = GetAll().Where(w => w.DistrictId == districtId).ToList();

            var result = (from bd in OndriumForDistrict
                          where bd.DistrictId == districtId
                          group bd by bd.OndriumId into newGrp
                          select new BaseData()
                          {
                              OndriumId = newGrp.Key,
                              OndriumName = newGrp.First().OndriumName,
                              PanchayatId = newGrp.First().PanchayatId,
                              PanchayatName = newGrp.First().PanchayatName
                          }).ToList();



            return result;
        }

        public static List<BaseData> GetPaguthiForAssembly(int assemblyId)
        {
            OndriumForDistrict = GetAll().Where(w => w.AssemblyId == assemblyId).ToList();

            var result = (from bd in OndriumForDistrict
                          where bd.AssemblyId == assemblyId
                          group bd by bd.OndriumId into newGrp
                          select new BaseData()
                          {
                              OndriumId = newGrp.Key,
                              OndriumName = newGrp.First().OndriumName,
                              PanchayatId = newGrp.First().PanchayatId,
                              PanchayatName = newGrp.First().PanchayatName,
                              PaguthiType = newGrp.First().PaguthiType,
                          }).ToList();



            return result;
        }

        public static List<BaseData> GetPanchayat(int ondriumId)
        {
            return OndriumForDistrict.Where(w => w.OndriumId == ondriumId).ToList();
        }

        public static string GetPanchayatName(int ondriumId, int panchayatId)
        {
            //var df = OndriumForDistrict.Where(w => w.OndriumId == ondriumId && w.PanchayatId == panchayatId).ToList();
            return OndriumForDistrict.Where(w => w.OndriumId == ondriumId && w.PanchayatId == panchayatId).First().PanchayatName;
        }

        public static List<BaseData> GetAll()
        {
            return ReadFileAsObjects<BaseData>(JsonFilePath);
        }

        public static void SaveAll()
        {
            var data = ReadFileAsObjects<BaseData>(JsonFilePath);
            WriteObjectsToFile<BaseData>(data, JsonFilePath);
        }

        public static void UpdateBooth(int districtNo, int assemblyNo, int ondriumNo, List<int> panchayatNo)
        {
            var allData = GetAll();

            allData
                .Where(w => w.DistrictId == districtNo &&  w.OndriumId == ondriumNo && panchayatNo.Contains(w.PanchayatId)).ToList()
                .ForEach(a => a.AssemblyId = assemblyNo);

            WriteObjectsToFile<BaseData>(allData, JsonFilePath);


        }

        public override string ToString()
        {
            return $"{OndriumName} - {PanchayatName}";
        }


    }


    
}
