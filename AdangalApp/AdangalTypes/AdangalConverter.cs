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
using System.Media;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;
using Tesseract;
using SnKeys = OpenQA.Selenium.Keys;

namespace AdangalApp.AdangalTypes
{
    public static class AdangalConverter
    {
        static List<string> relationTypesCorrect;
        static readonly SpeechSynthesizer ss;

        static AdangalConverter()
        {
            relationTypesCorrect = ConfigurationManager.AppSettings["relation"].Split('|').ToList();
            ss = new SpeechSynthesizer();
            ss.Volume = 100;
            ss.Rate = -1;
            //ss.Speak("Started Initialse Adangal Converter!");
        }


        private static void SendEmailUpdate(int totalListCount, int processingIndex, int survey, string subdiv)
        {
            var perc = totalListCount.PercentageBtwIntNo(processingIndex);
            var sub = $"{perc}% - {vao.loadedFile.VillageName}- DONE:{processingIndex} of {totalListCount}";
            string bodyDtr = $"LastItem Procesed: {survey}-{subdiv}";
            vao.LogMessage($"{sub}-{bodyDtr}");
            AppCommunication.SendAdangalUpdate(sub, bodyDtr);
            ss.Speak("Email Send please check!");
        }

        static BackgroundWorker bw = new BackgroundWorker();
        //static BackgroundWorker bwFull = new BackgroundWorker();
        static string testdataPath = "";
        static int lastIndexProcessed = 0;
        static List<KeyValue> localList;


        public static bool ProcessAdangal(List<KeyValue> list, int alreadyProcessed = 0, bool isCorrection = false)
        {
            localList = list;

            try
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
                DateTime lastEmailTime = DateTime.Now; // = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                var di = (from d in DriveInfo.GetDrives().ToList()
                          where d.Name.ToLower().Contains("c") == false
                          select d).First().Name;
                var imgPath = Path.Combine(di, "imageTest\\");

                int retryCount = 0;

                IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");
                int currentSurvey = 0;
                string currentSubDiv = "";

                var startTime = DateTime.Now;
                var subFirst = $"Started for {vao.loadedFile.VillageName}";
                AppCommunication.SendAdangalUpdate(subFirst, subFirst);

                int processedCount = 0;
                bool isSubDivFound = true;


                for (int i = lastIndexProcessed + 1; i <= list.Count - 1; i++)
                {
                    try
                    {
                        currentSurvey = list[i].Value;
                        currentSubDiv = list[i].Caption;
                        lastIndexProcessed = i;

                        if (isCorrection == false)
                        {
                            if (IsExist(currentSurvey, currentSubDiv))
                                continue;
                        }


                        try
                        {
                            driver = driver.SwitchTo().Window(driver.WindowHandles[0]);
                        }
                        catch (Exception ex)
                        {
                            driver.Dispose();
                            ss.Speak("Driver Closed! so stopping the process.");
                            AppCommunication.SendAdangalUpdate("Driver issue, so stopping the process.", ex.Message);
                            return false;
                        }

                        //select district
                        var district = driver.FindElement(By.Name("districtCode"));
                        var selectElement = new SelectElement(district);
                        try
                        {
                            selectElement.SelectByValue(districCode);
                        }
                        catch (Exception)
                        {
                            driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");
                            selectElement.SelectByValue(districCode);
                        }


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
                                ss.Speak("Website not working!");

                        }

                        try
                        {
                            subDivElement.SelectByValue(currentSubDiv);
                            isSubDivFound = true;

                        }
                        catch (NoSuchElementException nseEx)
                        {
                            subDivElement.SelectByIndex(0);
                            vao.LogMessage($"ERROR:: {nseEx.ToString()} -  NOT FOUND FOR : {currentSurvey}-{currentSubDiv}");
                            DataAccess.SaveSurveyNotFound(new KeyValue() { Value = currentSurvey, Caption = currentSubDiv });
                            isSubDivFound = false;
                        }

                        //button click
                        driver.FindElement(By.ClassName("button")).Click();
                        driver = driver.SwitchTo().Window(driver.WindowHandles[1]);

                        if (isSubDivFound == false)
                        {
                            driver.Close();
                            continue;

                        }
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
                        processedCount += 1;

                        aTimer.Elapsed += (a, o) =>
                        {
                            if (lastEmailTime == o.SignalTime)
                            {
                                //ss.Speak("Try email again, so ignore!");
                                return;
                            }
                            lastEmailTime = o.SignalTime;
                            SendEmailUpdate(list.Count + alreadyProcessed, i + alreadyProcessed, currentSurvey, currentSubDiv);
                            aTimer.Stop();
                        };
                        aTimer.Start();
                        driver.Close();
                    }
                    catch (Exception ex)
                    {
                        vao.LogMessage("ERROR:" + ex.ToString());
                        ss.SpeakAsync($"Other exception: {ex.Message}");

                        while (General.CheckForInternetConnection() == false)
                        {
                            Thread.Sleep(4000);
                            ss.Speak("Internet not working!");
                        }
                    }
                }

                MessageBox.Show($"Completed: {processedCount} out of {list.Count}", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppCommunication.SendAdangalUpdate($"{vao.loadedFile.VillageName} Completed-{processedCount} of {list.Count()}", "");
                return true;
            }
            catch (Exception ex)
            {

                while (General.CheckForInternetConnection() == false)
                {
                    Thread.Sleep(4000);
                    ss.Speak("No ineternet, please check!");
                }

                ss.Speak($"{ex.Message}, so trying again!");
                //ProcessAdangal(localList);
                return false;

            }
        }

        public static void GetBloContacts()
        {

            try
            {
                string fp = @"F:\AssemblySimpleApp\AssemblyApp\BLO\blo.txt";
                IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://www.elections.tn.gov.in/blo/");
                driver = driver.SwitchTo().Window(driver.WindowHandles[0]);
                var district = driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$ddl_District"));
                var selectElement = new SelectElement(district);
                selectElement.SelectByValue("27");

                List<KeyValue> list = new List<KeyValue>()
                {
                    // new KeyValue() {  Caption = "210", Value = 346, Id = 129  },
                    new KeyValue() {  Caption = "211", Value = 336, Id = 112  },

                      new KeyValue() {  Caption = "209", Value = 302, Id = 1  },
                       new KeyValue() {  Caption = "212", Value = 385, Id = 1  }


                };

                list.ForEach(fe =>
                {
                    DataAccess.SaveText($"{fe.Caption}-{fe.Value}", fp);
                    DataAccess.SaveText($"-------------------", fp);

                    var assembly = driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$ddl_Assembly"));
                    var selectElement2 = new SelectElement(assembly);
                    selectElement2.SelectByValue(fe.Caption);

                    for (int i = fe.Id; i <= fe.Value; i++)
                    {


                        var booth = driver.FindElement(By.Name("ctl00$ContentPlaceHolder1$ddl_Part"));
                        var selectElement3 = new SelectElement(booth);
                        selectElement3.SelectByValue(i.ToString());

                        driver.FindElement(By.ClassName("btn")).Click();

                        //driver = driver.SwitchTo().Window(driver.WindowHandles[1]);
                        Actions action = new Actions(driver);
                        action.KeyDown(SnKeys.Control).SendKeys("a").KeyUp(SnKeys.Control).Build().Perform();
                        action.KeyDown(SnKeys.Control).SendKeys("c").KeyUp(SnKeys.Control).Build().Perform();

                        var copiedText = Clipboard.GetText();

                        var neededData = copiedText.Replace("Mobile", "$").Split('$')[1].Replace("Copyright", "$").Split('$')[0].Trim().Split('\t');
                        var contact = $"{i}) {neededData[neededData.Count() - 2]}-{neededData.Last()}";
                        DataAccess.SaveText(contact, fp);
                    }
                    //MessageBox.Show($"Completed for  {fe.Value}-{ fe.Caption}");

                });

                driver.Close();
            }
            catch (Exception ex)
            {
                vao.LogMessage("ERROR:" + ex.ToString());
                ss.Speak("No ineternet, please check!");

                while (General.CheckForInternetConnection() == false)
                {
                    Thread.Sleep(4000);
                    ss.Speak("No ineternet, please check!");
                }
            }

        }

        public static void GetForm20s()
        {

            try
            {
                string fp = @"F:\AssemblySimpleApp\AssemblyApp\BLO\blo.txt";
                IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://www.elections.tn.gov.in/Form20_TNLA2021.aspx");

                IList<IWebElement> links = driver.FindElements(By.TagName("a"));

                var d = links.Where(element => element.Text.Trim().Split('.').Count() == 2).ToList(); //.Click();

                // your logic with traditional foreach loop
                foreach (var link in d)
                {
                    //if (link.Text == "YouTube")
                    //{
                    driver = driver.SwitchTo().Window(driver.WindowHandles[0]);
                    link.Click();
                    driver = driver.SwitchTo().Window(driver.WindowHandles[1]);

                    //driver.WindowHandles[1].

                    //IWebElement downloadIcon = driver.FindElement(By.TagName("embed"));
                    //String fileAddress = downloadIcon.GetAttribute("src");
                    //driver.g(fileAddress);

                    Actions action = new Actions(driver);
                    //Actions action = new Actions(driver);
                    //action.KeyDown(SnKeys.Tab).KeyUp(SnKeys.Tab).Build().Perform();
                    action.KeyDown(SnKeys.Control).SendKeys("a").KeyUp(SnKeys.Control).Build().Perform();
                    action.KeyDown(SnKeys.Control).SendKeys("c").KeyUp(SnKeys.Control).Build().Perform();

                    var copiedText = Clipboard.GetText();
                    driver.Close();

                }

                driver.Quit();

            }

            catch (Exception ex)
            {
                vao.LogMessage("ERROR:" + ex.ToString());
                ss.Speak("No ineternet, please check!");

                while (General.CheckForInternetConnection() == false)
                {
                    Thread.Sleep(4000);
                    ss.Speak("No ineternet, please check!");
                }
            }

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
                var notSyncCount = activeAdangal.Where(w => w.LandStatus == LandStatus.Error).Count();

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
                else
                {
                    return $"{AdangalConstant.villageName} : Data are synced, please check for other";
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
        private static string FixParappu(string parappuToFix)
        {
            var p = parappuToFix.Trim().Replace(" ", "").Replace("-", ".").Split('.');

            //var p = parappuToFix.Split('.');
            if (p[1].Trim().Length < 2)
            {
                p[1] = p[1].PadLeft(2, '0');
            }

            return $"{p[0]}.{p[1].Trim()}.{p[2]}";
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

                adangal.Parappu = FixParappu(par);
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
                //string name = "";
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
