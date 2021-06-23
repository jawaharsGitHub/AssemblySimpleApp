﻿using Common;
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
            var data  = ReadFileAsObjects<Adangal>(filePath);
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
            var data = GetActiveAdangal(filePath);
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

            adangalToBeDelete.ForEach(fe => {
                data.Where(w => w.NilaAlavaiEn.ToString() == fe.Split('~')[0] && w.UtpirivuEn == fe.Split('~')[1]).First().LandStatus = LandStatus.Deleted;
            });
            WriteObjectsToFile(data, filePath);
            return GetActiveAdangal(filePath);
        }

        public static List<Adangal> GetActiveAdangal(string filePath)
        {
            return ReadFileAsObjects<Adangal>(filePath).Where(w => w.LandStatus != LandStatus.Deleted).ToList();
        }
        public static List<ComboData> GetDistricts()
        {
            var filePath = GetdatabasePath("RevDistrict");

            
                return ReadFileAsObjects<ComboData>(filePath);
            
        }

    }
}
