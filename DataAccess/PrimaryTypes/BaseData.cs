﻿using Common;
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
                                      OndriumName = newGrp.First().OndriumName
                                  }).ToList();

            
            
            return result;
        }

        public static List<BaseData> GetPanchayat(int ondriumId)
        {
            return OndriumForDistrict.Where(w => w.OndriumId == ondriumId).ToList();
        }

        public static List<BaseData> GetAll()
        {
            return ReadFileAsObjects<BaseData>(JsonFilePath);
        }


    }
}