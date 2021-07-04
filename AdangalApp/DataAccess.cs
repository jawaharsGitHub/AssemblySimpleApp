using Common;
using AdangalApp.AdangalTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdangalApp
{
    public class DataAccess : BaseClass
    {
        public static string JsonPath = "";
        public static string SubDivPath = "";
        public static string LoadedFile = "";
        public static string PattaJsonPath = "";
        public static string WholeLandListJsonPath = "";
        public static string AdangalOriginalPath = "";

        public static void SetVillageName()
        {
            JsonPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}.json");
            PattaJsonPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}-PattaJsonPath.json");
            WholeLandListJsonPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}-WholeLandListJsonPath.json");
            SubDivPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}-subdiv.json");
            AdangalOriginalPath = AppConfiguration.GetDynamicPath($"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}-OriginalAdangal.json");


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

        public static List<LoadedFileDetail> GetLoadedFile()
        {
            
            LoadedFile = AppConfiguration.GetDynamicPath($"AdangalJson/LoadedFile.json");


            List<LoadedFileDetail> data = new List<LoadedFileDetail>();

            if (File.Exists(LoadedFile) == false)
            {
                File.Create(LoadedFile).Close();
            }
            else
            {
                data = ReadFileAsObjects<LoadedFileDetail>(LoadedFile);
                data = data.Where(w => File.Exists(AppConfiguration.GetDynamicPath($"AdangalJson/{w.VillageName}/{w.VillageName}.json"))).ToList();
            }

            data.Insert(0, new LoadedFileDetail() { VillageCode = -1, VillageName = "--select--" });
            return data;
        }

        public static List<ComboDataStr> GetProcessedFiles()
        {
            var JsonFileFolder = AppConfiguration.GetDynamicPath($"AdangalJson");
            var files = (from f in Directory.GetFiles(JsonFileFolder).ToList()
                        select new ComboDataStr() { 
                             Value = f.Replace("-subdiv", ""),
                             Display = Path.ChangeExtension(new FileInfo(f).Name, null)
                        }).Distinct().ToList();

             files.Insert(0, new ComboDataStr() { Value = "", Display = "--select--" });
            return files;
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

        public static void SavePattaList(PattaList pattaList)
        {
            if (File.Exists(PattaJsonPath) == false)
                File.Create(PattaJsonPath).Close();

            WriteObjectsToFile<PattaList>(pattaList, PattaJsonPath);
        }

        public static void SaveWholeLandList(List<LandDetail> landDetails)
        {
            if (File.Exists(WholeLandListJsonPath) == false)
                File.Create(WholeLandListJsonPath).Close();

            WriteObjectsToFile<LandDetail>(landDetails, WholeLandListJsonPath);
        }

        public static void SaveAdangalOriginalList(List<Adangal> adangal)
        {
            if (File.Exists(AdangalOriginalPath) == false)
                File.Create(AdangalOriginalPath).Close();

            WriteObjectsToFile<Adangal>(adangal, AdangalOriginalPath);
        }

        public static PattaList GetPattaList()
        {
            return ReadFileAsList<PattaList>(PattaJsonPath);
        }

        public static List<LandDetail> GetWholeLandList()
        {
            return ReadFileAsObjects<LandDetail>(WholeLandListJsonPath);
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

        public static bool AddNewLoadedFile(LoadedFileDetail loadedFileDetail)
        {
            DeleteLoadedFile(loadedFileDetail);
            InsertSingleObjectToListJson<LoadedFileDetail>(LoadedFile, loadedFileDetail);
            return true;
        }

        public static void DeleteLoadedFile(LoadedFileDetail loadedFileDetail)
        {
            var data = ReadFileAsObjects<LoadedFileDetail>(LoadedFile);

                var itemToDelete = data.Where(w => w.MaavattamCode == loadedFileDetail.MaavattamCode &&
                                w.VattamCode == loadedFileDetail.VattamCode &&
                                w.VillageCode == loadedFileDetail.VillageCode).FirstOrDefault();

                if(itemToDelete != null)
                {
                    data.Remove(itemToDelete);
                    WriteObjectsToFile(data, LoadedFile);
                }
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
