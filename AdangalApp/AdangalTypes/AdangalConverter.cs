using Common;
using Common.ExtensionMethod;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tesseract;
using SnKeys = OpenQA.Selenium.Keys;

namespace AdangalApp.AdangalTypes
{
    public static class AdangalConverter
    {
        //static LogHelper logHelper;
        static List<string> relationTypesCorrect;
        //public static string villagPath; // = @"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\ACHUNTHANVAYAL\ACHUNTHANVAYAL-full.json";
        static AdangalConverter()
        {
            relationTypesCorrect = new List<string>() {
                        "தந்தை",
                        "கணவன்",
                        "காப்பாளர்",
                        "மகன்",
                        "மனைவி",
                        "மகள்"
                    };


        }


        static BackgroundWorker bw = new BackgroundWorker();
        //static BackgroundWorker bwFull = new BackgroundWorker();
        static string testdataPath = "";

        public static void ProcessAdangal(List<KeyValue> list, bool isCorrection = false)
        {
            var lf = vao.loadedFile;
            string districCode = lf.MaavattamCode.ToString();
            string talukCode = lf.VattamCode.ToString().PadLeft(2, '0');
            string villageCode = lf.VillageCode.ToString().PadLeft(3, '0');

            testdataPath = ConfigurationManager.AppSettings["testdataPath"];
            int emailInterval = ConfigurationManager.AppSettings["emailInterval"].ToInt32();
            System.Timers.Timer aTimer = new System.Timers.Timer(emailInterval * 60 * 1000);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();

            var di = (from d in DriveInfo.GetDrives().ToList()
                      where d.Name.ToLower().Contains("c") == false
                      select d).First().Name;
            var imgPath = Path.Combine(di, "imageTest\\");

            int retryCount = 0;
            
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");
            int currentSurvey = 0;
            string currentSubDiv = "";

            //bw.DoWork += (s, e) =>
            //{
            //    var perc = list.Count.PercentageBtwIntNo(i);
            //    var sub = $"{perc}% - {vao.loadedFile.VillageName}- DONE:{i} of {list.Count}";
            //    //var adn = DataAccess.GetActiveAdangal();
            //    //var issueRec = adn.Where(w => w.LandType != LandType.Porambokku && w.LandType != LandType.Dash && w.PattaEn == 0).Count();
            //    string bodyDtr = $"LastItem Procesed: {currentSurvey}-{currentSubDiv}";
            //    AppCommunication.SendAdangalUpdate(sub, bodyDtr);
            //};

            //var startTime = DateTime.Now;
            //var subFirst = $"Started for {vao.loadedFile.VillageName}";
            //AppCommunication.SendAdangalUpdate(subFirst, subFirst);

            for (int i = 0; i <= list.Count - 1; i++)
            {
                try
                {
                    currentSurvey = list[i].Value;
                    currentSubDiv = list[i].Caption;                    

                    //if (isCorrection == false && DateTime.Now >= startTime.AddMinutes(1))
                    //{
                    //    var perc = list.Count.PercentageBtwIntNo(i);
                    //    var sub = $"{perc}% - {vao.loadedFile.VillageName}- DONE:{i} of {list.Count}";
                    //    //var adn = DataAccess.GetActiveAdangal();
                    //    //var issueRec = adn.Where(w => w.LandType != LandType.Porambokku && w.LandType != LandType.Dash && w.PattaEn == 0).Count();
                    //    string bodyDtr = $"LastItem Procesed: {currentSurvey}-{currentSubDiv}";
                    //    AppCommunication.SendAdangalUpdate(sub, bodyDtr);
                    //    startTime = DateTime.Now;

                    //    //aTimer.Elapsed += (a, o) =>
                    //    //{
                    //    //    bw.RunWorkerAsync();
                    //    //};
                    //}

                    

                    //LogEntries logEntries = driver.Manage().Logs.

                    if (isCorrection == false)
                    {
                        if (IsExist(currentSurvey, currentSubDiv))
                            continue;
                    }

                    driver = driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //select district
                    var district = driver.FindElement(By.Name("districtCode"));
                    var selectElement = new SelectElement(district);
                    selectElement.SelectByValue(districCode);

                    //choose rural
                    RadioButtons categories = new RadioButtons(driver, driver.FindElements(By.Name("areaType")));
                    categories.SelectValue("rural");

                    //button click
                    driver.FindElement(By.ClassName("button")).Click();

                    // select taluk
                    var taluk = driver.FindElement(By.Name("talukCode"));
                    var talukElement = new SelectElement(taluk);
                    talukElement.SelectByValue(talukCode);

                    //choose survey
                    RadioButtons pattaCategory = new RadioButtons(driver, driver.FindElements(By.Name("viewOpt")));
                    pattaCategory.SelectValue("sur");

                    //choose survey
                    RadioButtons fmbCategory = new RadioButtons(driver, driver.FindElements(By.Name("viewOption")));
                    fmbCategory.SelectValue("view");

                    // select village
                    var village = driver.FindElement(By.Name("villageCode"));
                    var vilageElement = new SelectElement(village);
                    while (vilageElement.WrappedElement.Text == "Please Select ...")
                    {
                        village = driver.FindElement(By.Name("villageCode"));
                        vilageElement = new SelectElement(village);
                    }
                    vilageElement.SelectByValue(villageCode);

                    //enter survey no
                    var txt = driver.FindElement(By.Name("surveyNo"));
                    txt.SendKeys(currentSurvey.ToString());

                    //enter captcha
                    var text = GenerateSnapshot(driver, imgPath);
                    var txtCap = driver.FindElement(By.Name("captcha"));
                    txtCap.SendKeys(text);

                    var subDiv = driver.FindElement(By.Name("subdivNo"));
                    var subDivElement = new SelectElement(subDiv);
                    int iterationCount = 0;
                    while (subDivElement.WrappedElement.Text == "Please Select ...")
                    {
                        subDiv = driver.FindElement(By.Name("subdivNo"));
                        subDivElement = new SelectElement(subDiv);
                        iterationCount += 1;

                        if (iterationCount == 100 || iterationCount == 200 || iterationCount == 300)
                        {
                            MessageBox.Show("Website not working!!");
                        }
                    }
                    subDivElement.SelectByValue(currentSubDiv);

                    //button click
                    driver.FindElement(By.ClassName("button")).Click();

                    driver = driver.SwitchTo().Window(driver.WindowHandles[1]);
                    Actions action = new Actions(driver);
                    action.KeyDown(SnKeys.Control).SendKeys("a").KeyUp(SnKeys.Control).Build().Perform();
                    action.KeyDown(SnKeys.Control).SendKeys("c").KeyUp(SnKeys.Control).Build().Perform();

                    var copiedText = Clipboard.GetText();

                    vao.LogMessage($"STARTED: {currentSurvey}-{currentSubDiv} [{i}/{list.Count}]");
                    vao.LogMessage($"------------------------------------");

                    if (copiedText.Contains("உரிமையாளர்கள் பெயர்"))
                    {
                        TextToAdangal(copiedText, currentSurvey, currentSubDiv);
                        //vao.logHelper.WriteAdangalLog($"added new record - {currentSurvey}-{currentSubDiv}");
                    }
                    else if (IsHave(copiedText, "government"))
                    {
                        AddOtherLandType(currentSurvey, currentSubDiv, LandType.Porambokku);
                        vao.logHelper.WriteAdangalLog($"added govt land - {currentSurvey}-{currentSubDiv}");
                    }
                    else if (IsHave(copiedText, "bhoodan"))
                    {
                        AddOtherLandType(currentSurvey, currentSubDiv, LandType.CLRBhoodanLands);
                        vao.logHelper.WriteAdangalLog($"added bhoodan land - {currentSurvey}-{currentSubDiv}");
                    }
                    else if (IsDash(copiedText))
                    {
                        AddOtherLandType(currentSurvey, currentSubDiv, LandType.Dash);
                        vao.logHelper.WriteAdangalLog($"added dash - {currentSurvey}-{currentSubDiv}");
                    }
                    else if (copiedText.ToLower().Contains("district") == true &&
                            copiedText.ToLower().Contains("government") == false &&
                            copiedText.ToLower().Contains("bhoodan") == false)
                    {
                        vao.logHelper.WriteAdangalLog($"Wrong Captcha: {text}: {currentSurvey}-{currentSubDiv}");
                        retryCount += 1;
                        if (retryCount <= 2)
                            i -= 1;
                        driver.Close();
                        continue;
                    }
                    else
                    {
                        AddOtherLandType(currentSurvey, currentSubDiv, LandType.UnKnown);
                        vao.logHelper.WriteAdangalLog($"added unknown - {currentSurvey}-{currentSubDiv}");
                    }
                    retryCount = 0;

                    DataAccess.SaveCopiedText($"{currentSurvey}-{currentSubDiv}{Environment.NewLine}{copiedText}");



                    

                    vao.LogMessage($"DONE: {currentSurvey}-{currentSubDiv} [{i}/{list.Count}]");
                    driver.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    vao.LogMessage("ERROR:" + ex.ToString());
                    driver.Close();

                }

            }


            var processed = DataAccess.GetActiveAdangalNew();
            bw.DoWork += (s, e) =>
            {
                AppCommunication.SendAdangalUpdate($"{vao.loadedFile.VillageName} Completed-{processed} of {list.Count}", "");
            };
            bw.RunWorkerAsync();

            MessageBox.Show($"Completed: {processed.Count} out of {list.Count}", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //driver.Close();



        }

        private static bool IsHave(string copiedText, string searchedText)
        {
            return copiedText.ToLower().Contains("district") == true && copiedText.ToLower().Contains(searchedText);
        }

        private static bool IsDash(string copiedText)
        {
            return
                copiedText.Split(Environment.NewLine.ToCharArray()).ToList()
                .Where(w => w.Trim() != "").Select(s => s).ToList()[1].ToString().Trim() == "-";
        }

        public enum FileStatus
        {
            SubDivNotCreated,
            SubDivCreatedButAdangalNotExist,
            PartiallyCreated,
            CompletedWithError,
            Issue
        }
        public static string IsSync()
        {
            FileStatus status = FileStatus.Issue;
            string error = "";
            if (DataAccess.IsSubDivFileExist() == false)
            {
                status = FileStatus.SubDivNotCreated;
            }

            else if (DataAccess.IsAdangalFileExist() == false)
            {
                status = FileStatus.SubDivCreatedButAdangalNotExist;
            }
            else
            {
                var subDivCount = DataAccess.GetSubdiv().Count();
                var activeAdangal = DataAccess.GetActiveAdangal();
                var notSyncCount = activeAdangal.Where(w => w.LandStatus != LandStatus.NoChange).Count();

                if (activeAdangal.Count < subDivCount)
                {
                    status = FileStatus.PartiallyCreated;
                    error = $"created {activeAdangal.Count} out of {subDivCount}";
                }
                else if (activeAdangal.Count == subDivCount && notSyncCount > 0)
                {
                    status = FileStatus.CompletedWithError;
                    error = $"Error Record {notSyncCount}";
                }
            }
            return $"{AdangalConstant.villageName} : {status.ToName()}-{error}";
        }



        public static void SetGlobalVillagePath(string path)
        {
            //villagPath = path;
            //General.CreateFileIfNotExist(villagPath);
        }


        public static string GenerateSnapshot(IWebDriver driver, string filePath)
        {
            var remElement = driver.FindElement(By.Id("captchaImg"));
            Point location = remElement.Location;

            var screenshot = (driver as ChromeDriver).GetScreenshot();
            using (MemoryStream stream = new MemoryStream(screenshot.AsByteArray))
            {
                using (Bitmap bitmap = new Bitmap(stream))
                {
                    RectangleF part = new RectangleF(location.X, location.Y, remElement.Size.Width, remElement.Size.Height);
                    using (Bitmap bn = bitmap.Clone(part, bitmap.PixelFormat))
                    {
                        bn.Save(filePath + "CaptchImage.png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

            string captchatext;
            //reading text from images
            using (var engine = new TesseractEngine(testdataPath, "eng", EngineMode.Default))
            {
                Page ocrPage = engine.Process(Pix.LoadFromFile(filePath + "CaptchImage.png"), PageSegMode.AutoOnly);
                captchatext = ocrPage.GetText();
            }

            return captchatext.Trim();
        }

        public static void TextToAdangal(string text, int surveyNo, string subdivNo)
        {
            try
            {

                var newData = text;

                var splitData = newData.Replace("உரிமையாளர்கள் பெயர்", "$").Split('$');
                var createdTime = splitData[1].Replace("இத் தகவல்கள்", "$").Split('$')[1].Replace("நேரத்தில்", "$").Split('$')[0].Replace("அன்று", "").Trim();
                var pattaEn = splitData[0].Replace("பட்டா எண்", "$").Split('$').ToList()[1].Replace(":", "").Trim();

                var first = splitData[1];

                var sec = first.Replace("குறிப்பு2", "$").Split('$')[0];
                var third = sec.Split(Environment.NewLine.ToCharArray()).Where(w => w.Trim() != "").ToList();

                var hecIndex = third.FindIndex(f => f.Contains("ஹெக்"));
                var totalIndex = third.Count - 1;

                var neededData = third.Skip(hecIndex + 1);
                neededData = neededData.Take(neededData.Count() - 1);

                var firstRowWithName = sec.Replace("புல எண்", "$").Split('$').ToList()[0];
                if (firstRowWithName.Contains("2."))
                {
                    firstRowWithName = firstRowWithName.Replace("2.", "$").Split('$')[0];
                }
                var name = GetOwnerName(firstRowWithName);

                int addedCount = 0;
                neededData.ToList().ForEach(fe =>
                {
                    var rowData = fe.Split('\t').ToList();
                    var adangal = GetAdangalFromCopiedData(rowData, pattaEn, name);
                    adangal.RegisterDate = GetRegisterDate(rowData);
                    adangal.IsVagai = (hecIndex > 3);
                    if (DataAccess.AddNewAdangal(adangal))
                        addedCount += 1;
                });

                vao.LogMessage($"Added ({addedCount}) out of ({neededData.Count()}) for {surveyNo}-{subdivNo}");

            }
            catch (Exception ex)
            {
                DataAccess.AddNewAdangal(new Adangal()
                { LandStatus = LandStatus.Error, LandType = LandType.UnKnown, NilaAlavaiEn = surveyNo, UtpirivuEn = subdivNo });
                vao.LogMessage($"ERROR: {surveyNo}-{subdivNo} - {ex.ToString()}");

            }

        }

        private static string GetRegisterDate(List<string> rowData)
        {
            var DateData = rowData.Last().Split('-');
            var dateCount = DateData.Count();
            return $"{DateData[dateCount - 3].Trim()}-{DateData[dateCount - 2].Trim()}-{DateData[dateCount - 1].Trim()}";
        }

        public static void AddOtherLandType(int survey, string subdiv, LandType LandType)
        {
            DataAccess.AddNewAdangal(new Adangal()
            {
                LandType = LandType,
                NilaAlavaiEn = survey,
                UtpirivuEn = subdiv

            });

        }



        public static bool IsExist(int NilaAlavaiEn, string UtpirivuEn)
        {
            return DataAccess.IsAdangalAlreadyExist(NilaAlavaiEn, UtpirivuEn);
        }

        public static Adangal GetAdangalFromCopiedData(List<string> data, string pattaEn, KeyValue ownerName)
        {
            var adangal = new Adangal();
            try
            {
                // Test for both fullfilled and extend also
                adangal.NilaAlavaiEn = data[0].ToInt32();
                adangal.UtpirivuEn = data[1];

                var (lt, par, thee) = GetLandDetails(data);
                adangal.Parappu = par.Trim().Replace(" ", "").Replace("-", ".");
                adangal.Theervai = thee;
                adangal.LandType = lt;
                adangal.OwnerName = ownerName.Caption.Trim();
                adangal.LastName = ownerName.Caption2.Trim();
                adangal.PattaEn = pattaEn.ToInt32();
                adangal.LandStatus = LandStatus.NoChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return adangal;
        }

        private static (LandType lt, string par, string thee) GetLandDetails(List<string> data)
        {
            LandType ld = LandType.Zero;
            var parappu = "";
            var theervai = "";
            try
            {
                int i = 0;
                if (data[2] != "--" && data[3] != "--")
                {
                    ld = LandType.Punsai;
                    parappu = data[2];
                    theervai = data[3];
                    i += 1;
                }
                if (data[4] != "--" && data[5] != "--")
                {
                    ld = LandType.Nansai;
                    parappu = data[4];
                    theervai = data[5];
                    i += 1;
                }
                if (data[6] != "--" && data[7] != "--")
                {
                    ld = LandType.Maanaavari;
                    parappu = data[6];
                    theervai = data[7];
                    i += 1;
                }
                if (i > 1)
                {
                    ld = LandType.Zero;
                }
            }
            catch (Exception ex)
            {
                //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            }

            return (ld, parappu, theervai);
        }

        public static KeyValue GetOwnerName(string paramNameRow)
        {
            try
            {
                KeyValue kv = new KeyValue();
                var nameRow = paramNameRow;
                //nameRow = nameRow.Split('.').ToList().Where(w => w.Trim() != "").ToList()[1].Trim();
                nameRow = nameRow.Replace("1.", "$").Split('$').ToList().Where(w => w.Trim() != "").ToList()[0].Trim();
                string name = "";
                if (relationTypesCorrect.Any(a => nameRow.Split('\t').ToList().Contains(a))) // have valid names.
                {
                    var delitList = relationTypesCorrect.Intersect(nameRow.Split('\t').ToList()).ToList();

                    if (delitList.Count == 1)
                    {
                        var delimit = delitList[0];
                        //name = nameRow.Replace(delimit, "$").Split('$')[1];
                        var d = nameRow.Replace(delimit, "$").Split('$');
                        kv.Caption = $"{d[1].Replace("\t", "").Replace("-", "")}";
                        kv.Caption2 = $"{d[0].Replace("\t", "").Replace("-", "").Replace(Environment.NewLine, "").Replace("2", "")}";
                    }
                    else
                    {
                        // ERROR!
                        MessageBox.Show("Error!");
                    }
                }
                else
                {
                    vao.LogMessage($"ERROR in names: namerow-{paramNameRow}");
                }

                return kv;
            }
            catch (Exception ex)
            {
                //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
                return null;
            }
        }


    }

    public class RadioButtons
    {
        public RadioButtons(IWebDriver driver, ReadOnlyCollection<IWebElement> webElements)
        {
            Driver = driver;
            WebElements = webElements;
        }

        protected IWebDriver Driver { get; }

        protected ReadOnlyCollection<IWebElement> WebElements { get; }

        public void SelectValue(String value)
        {
            WebElements.Single(we => we.GetAttribute("value") == value).Click();
        }
    }
}
