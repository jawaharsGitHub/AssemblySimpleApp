using Common;
using AdangalApp.AdangalTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.ExtensionMethod;

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
        public static string SummaryPath = ""; 
        public static string GovtBuildingPath = "";
        public static string MissedAdangalPath = "";

        public static void SetVillageName()
        {
            var vnPath = $"AdangalJson/{AdangalConstant.villageName}/{AdangalConstant.villageName}";

            JsonPath = AppConfiguration.GetDynamicPath($"{vnPath}.json");
            PattaJsonPath = AppConfiguration.GetDynamicPath($"{vnPath}-PattaJsonPath.json");
            WholeLandListJsonPath = AppConfiguration.GetDynamicPath($"{vnPath}-WholeLandListJsonPath.json");
            SubDivPath = AppConfiguration.GetDynamicPath($"{vnPath}-subdiv.json");
            AdangalOriginalPath = AppConfiguration.GetDynamicPath($"{vnPath}-OriginalAdangal.json");
            SummaryPath = AppConfiguration.GetDynamicPath($"{vnPath}-Summary.json");
            GovtBuildingPath = AppConfiguration.GetDynamicPath($"{vnPath}-GovtBuilding.json");
            MissedAdangalPath = AppConfiguration.GetDynamicPath($"{vnPath}-{Environment.UserName}-MissedAdangal.json");

            if (Directory.Exists(Directory.GetParent(JsonPath).FullName) == false)
                Directory.CreateDirectory(Directory.GetParent(JsonPath).FullName);
        }

        private static string GetTablePath(string tableName)
        {
            return AppConfiguration.GetDynamicPath($"database/{tableName}.json");
        }

        public static List<KeyValue> GetSubdiv(string path = null)
        {
            SubDivPath = path;
            var data = ReadFileAsObjects<KeyValue>(SubDivPath);
            return data;
        }

        public static List<LoadedFileDetail> GetLoadedFile()
        {
            LoadedFile = AppConfiguration.GetDynamicPath($"AdangalJson/LoadedFile.json");
            General.CreateFileIfNotExist(LoadedFile);

            var data = ReadFileAsObjects<LoadedFileDetail>(LoadedFile);
            data = data.Where(w => File.Exists(AppConfiguration.GetDynamicPath($"AdangalJson/{w.VillageName}/{w.VillageName}.json"))).ToList();
            data.Insert(0, new LoadedFileDetail() { VillageCode = -1, VillageName = "--select--" });
            return data;
        }

        public static List<ComboDataStr> GetProcessedFiles()
        {
            var JsonFileFolder = AppConfiguration.GetDynamicPath($"AdangalJson");
            var files = (from f in Directory.GetFiles(JsonFileFolder).ToList()
                         select new ComboDataStr()
                         {
                             Value = f.Replace("-subdiv", ""),
                             Display = Path.ChangeExtension(new FileInfo(f).Name, null)
                         }).Distinct().ToList();

            files.Insert(0, new ComboDataStr() { Value = "", Display = "--select--" });
            return files;
        }

        public static List<KeyValue> SubdivToJson(List<KeyValue> subdivData)
        {
            if (File.Exists(SubDivPath) == false)
                WriteObjectsToFile(subdivData, SubDivPath);

            var data = ReadFileAsObjects<KeyValue>(SubDivPath);
            return data;
        }

        public static List<Adangal> AdangalToJson(List<Adangal> adangalData)
        {
            string path = JsonPath;
            General.CreateFileIfNotExist(path);

            WriteObjectsToFile(adangalData, path);

            var data = GetActiveAdangal();
            return data;
        }

        public static void SaveMissedAdangal(Adangal adangalData)
        {
            string path = MissedAdangalPath;
            General.CreateFileIfNotExist(MissedAdangalPath);
            InsertSingleObjectToListJson<Adangal>(path, adangalData);
        }

        public static List<Adangal> GetMissedAdangal(string missedSurveysPath)
        {
            return ReadFileAsObjects<Adangal>(missedSurveysPath);
        }

        public static void SaveSummary(List<Summary> summaryData)
        {
            string path = SummaryPath;
            var totaPunsaiParappu = AdangalFn.GetSumThreeDotNo(summaryData.Skip(1).Take(2).Select(s => s.Parappu).ToList());
            var totalPunsaiPages = summaryData[1].Pakkam.Split('-')[0] + "-" + summaryData[2].Pakkam.Split('-')[1];

            List<Summary> data = null;
            if (File.Exists(path) == false)
            {
                var myFile = File.Create(path);
                myFile.Close();

                data = summaryData;
                data[0].Vibaram = $"மொத்த {data[0].Vibaram}";
                data.Insert(3,
                    new Summary()
                    {
                        Id = -1,
                        Parappu = totaPunsaiParappu,
                        Vibaram = "மொத்த புன்செய் (புன்செய் + மானாவாரி)",
                        Pakkam = totalPunsaiPages
                    });
            }
            else
            {
                data = GetSummary();
                for (int i = 0; i <= 3; i++)
                {
                    data.Where(w => w.Id == i).First().Parappu = summaryData[i].Parappu;
                    data.Where(w => w.Id == i).First().Pakkam = summaryData[i].Pakkam;
                }

                data.Where(w => w.Id == -1).First().Parappu = totaPunsaiParappu;
                data.Where(w => w.Id == -1).First().Pakkam = totalPunsaiPages;
            }

            WriteObjectsToFile(data, path);
        }

        public static List<Summary> GetSummary()
        {
            return ReadFileAsObjects<Summary>(SummaryPath);
        }

        public static List<GovtBuilding> GetGovtBuilding()
        {
            return ReadFileAsObjects<GovtBuilding>(GovtBuildingPath);
        }

        public static void SavePattaList(PattaList pattaList)
        {
            var path = PattaJsonPath;
            General.CreateFileIfNotExist(path);
            WriteObjectsToFile(pattaList, path);
        }

        public static void SaveWholeLandList(List<LandDetail> landDetails)
        {
            var path = WholeLandListJsonPath;
            General.CreateFileIfNotExist(path);
            WriteObjectsToFile(landDetails, path);
        }

        private static bool IsAdangalAlreadyExist(Adangal adangal)
        {
            return GetActiveAdangal()
                .Where(w => w.NilaAlavaiEn == adangal.NilaAlavaiEn && w.UtpirivuEn.Trim() == adangal.UtpirivuEn)
                .Count() == 1;

        }

        public static void SaveAdangalOriginalList(List<Adangal> adangal)
        {
            var path = AdangalOriginalPath;
            General.CreateFileIfNotExist(path);
            WriteObjectsToFile(adangal, path);
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

        public static bool IsAdangalFileExist()
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
            return ReadFileAsObjects<Adangal>(JsonPath).Where(w => w.LandStatus == LandStatus.WrongName).OrderBy(o => o.CorrectNameRow).ToList();
        }

        public static void UpdateOwnerName(Adangal adn, string ownerName)
        {
            var data = ReadFileAsObjects<Adangal>(JsonPath);

            var existingName = data.Where(w => w.PattaEn == adn.PattaEn).First().OwnerName;

            if (adn.PattaEn == 0)
            {
                data.Where(w => w.LandType == LandType.Porambokku &&
                                w.NilaAlavaiEn == adn.NilaAlavaiEn && w.UtpirivuEn == adn.UtpirivuEn).ToList().ForEach(fe =>
                                {
                                    fe.OwnerName = ownerName;
                                    fe.LandStatus = LandStatus.NameEdited;
                                });
            }
            else
            {
                data.Where(w => 
                                w.NilaAlavaiEn == adn.NilaAlavaiEn && w.UtpirivuEn == adn.UtpirivuEn).ToList().ForEach(fe =>
                                {
                                    fe.OwnerName = ownerName;
                                    fe.LandStatus = LandStatus.NameEdited;
                                });
            }

            WriteObjectsToFile(data, JsonPath);
        }

        public static void UpdateLandStatus(Adangal adn)
        {
            List<Adangal> list = GetActiveAdangal();
            var u = list.Where(ld => ld.NilaAlavaiEn == adn.NilaAlavaiEn &&
                              ld.UtpirivuEn == adn.UtpirivuEn).First();
            u.LandStatus = adn.LandStatus;
            u.LandStatus = LandStatus.NameEdited;
            WriteObjectsToFile(list, JsonPath);
        }

        public static void UpdatePattaEN(Adangal adn)
        {
            List<Adangal> list = GetActiveAdangal();
            var u = list.Where(ld => ld.NilaAlavaiEn == adn.NilaAlavaiEn &&
                              ld.UtpirivuEn == adn.UtpirivuEn).First();
            u.PattaEn = adn.PattaEn;
            u.LandStatus = LandStatus.NameEdited;
            WriteObjectsToFile(list, JsonPath);
        }


        public static bool AddNewAdangal(Adangal adangal)
        {
            if (IsAdangalAlreadyExist(adangal) == false)
            {
                InsertSingleObjectToListJson<Adangal>(JsonPath, adangal);
                //SaveMissedAdangal(adangal);
                return true;
            }
            return false;
        }

        public static bool AddNewLoadedFile(LoadedFileDetail loadedFileDetail)
        {
            DeleteLoadedFile(loadedFileDetail);
            InsertSingleObjectToListJson<LoadedFileDetail>(LoadedFile, loadedFileDetail);
            return true;
        }

        public static LoadedFileDetail UpdatePercentage(LoadedFileDetail loadedFileDetail, decimal percentage)
        {
            List<LoadedFileDetail> list = GetLoadedFile();
            var u = list.Where(ld => ld.MaavattamCode == loadedFileDetail.MaavattamCode &&
                              ld.VattamCode == loadedFileDetail.VattamCode &&
                              ld.VillageCode == loadedFileDetail.VillageCode).First();
            u.InitialPercentage = percentage;
            WriteObjectsToFile(list, LoadedFile);
            return u;
        }

        public static LoadedFileDetail UpdateCorrectedPerc(LoadedFileDetail loadedFileDetail, decimal percentage)
        {
            List<LoadedFileDetail> list = GetLoadedFile();
            var u = list.Where(ld => ld.MaavattamCode == loadedFileDetail.MaavattamCode &&
                              ld.VattamCode == loadedFileDetail.VattamCode &&
                              ld.VillageCode == loadedFileDetail.VillageCode).First();
            u.CorrectedPercentage = percentage;
            WriteObjectsToFile(list, LoadedFile);
            return u;
        }

        public static void DeleteLoadedFile(LoadedFileDetail loadedFileDetail)
        {
            var data = ReadFileAsObjects<LoadedFileDetail>(LoadedFile);

            var itemToDelete = data.Where(w => w.MaavattamCode == loadedFileDetail.MaavattamCode &&
                            w.VattamCode == loadedFileDetail.VattamCode &&
                            w.VillageCode == loadedFileDetail.VillageCode).FirstOrDefault();

            if (itemToDelete != null)
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
                var data = ReadFileAsObjects<ComboData>(filePath).OrderBy(o => o.Display).ToList();
                data.Insert(0, new ComboData() { Display = "--select--", Value = -1 });
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
