using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AdangalApp.AdangalTypes;
using Common;
using Common.ExtensionMethod;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tesseract;
using System.ComponentModel;

namespace AdangalApp.AdangalTypes
{
    public static class AdangalConverter
    {
        static LogHelper logHelper;
        static List<string> relationTypesCorrect;
        public static string villagPath; // = @"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\ACHUNTHANVAYAL\ACHUNTHANVAYAL-full.json";
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
        public static void ProcessAdangal(List<KeyValue> list, bool isCorrection = false)
        {
            int minute = DateTime.Now.Minute;

            logHelper = Program.logHelper;
            string districCode = ConfigurationManager.AppSettings["districCode"];
            string talukCode = ConfigurationManager.AppSettings["talukCode"];
            string villageCode = ConfigurationManager.AppSettings["villageCode"];


            //var list = new List<KeyValue>() { 
            //    new KeyValue() { Value = 66, Caption = "2E" } ,
            //    new KeyValue() { Value = 37, Caption = "2E" }
            //};

            try
            {
                int retryCount = 0;
                System.Timers.Timer aTimer = new System.Timers.Timer(10 * 60 * 1000);
                IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    if (isCorrection == false)
                    {
                        if (IsExist(list[i].Value, list[i].Caption)) // && isTest == false)
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
                    txt.SendKeys(list[i].Value.ToString());

                    //enter captcha
                    var text = AdangalConverter.GenerateSnapshot(driver, @"E:\imageTest\");
                    var txtCap = driver.FindElement(By.Name("captcha"));
                    txtCap.SendKeys(text);

                    var subDiv = driver.FindElement(By.Name("subdivNo"));
                    var subDivElement = new SelectElement(subDiv);
                    while (subDivElement.WrappedElement.Text == "Please Select ...")
                    {
                        subDiv = driver.FindElement(By.Name("subdivNo"));
                        subDivElement = new SelectElement(subDiv);
                    }
                    subDivElement.SelectByValue(list[i].Caption);

                    //button click
                    driver.FindElement(By.ClassName("button")).Click();

                    driver = driver.SwitchTo().Window(driver.WindowHandles[1]);
                    Actions action = new Actions(driver);
                    action.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("a").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();
                    action.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("c").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();

                    var copiedText = Clipboard.GetText();

                    if (copiedText.ToLower().Contains("district") == true &&
                        copiedText.ToLower().Contains("government") == false &&
                        copiedText.ToLower().Contains("bhoodan") == false &&
                        copiedText.ToLower().Contains("-") == false)
                    {
                        logHelper.WriteAdangalLog($"Wrong Captcha: {text}: {list[i].Value}-{list[i].Caption}");
                        //MessageBox.Show($"Retry for {list[i].Value}-{list[i].Caption}!");
                        retryCount += 1;
                        if (retryCount <= 2)
                            i -= 1;
                        driver.Close();
                        continue;
                    }
                    retryCount = 0;

                    File.AppendAllText($@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{Program.villageName}\{Program.villageName}-10-1.txt", copiedText);

                    if (copiedText.ToLower().Contains("bhoodan"))
                    {
                        AddOtherLandType(list[i].Value, list[i].Caption, LandType.CLRBhoodanLands);
                        logHelper.WriteAdangalLog($"added bhoodan land - {list[i].Value}-{list[i].Caption}");
                    }
                    else if (copiedText.ToLower().Contains("government"))
                    {
                        AddOtherLandType(list[i].Value, list[i].Caption, LandType.Porambokku);
                        logHelper.WriteAdangalLog($"added govt land - {list[i].Value}-{list[i].Caption}");
                    }
                    else if (copiedText.ToLower().Contains("-"))
                    {
                        AddOtherLandType(list[i].Value, list[i].Caption, LandType.Dash);
                        logHelper.WriteAdangalLog($"added unknown - {list[i].Value}-{list[i].Caption}");
                    }
                    else
                    {
                        TextToAdangal(copiedText, list[i].Value, list[i].Caption, logHelper);
                    }

                    LogMessage($"DONE:[index-{i}]/{list.Count}");

                    driver.Close();

                    if (isCorrection == false)
                    {
                        aTimer.Elapsed += (a, o) =>
                        {
                            bw.DoWork += (s, e) =>
                            {
                                var perc = list.Count.PercentageBtwIntNo(i);
                                var sub = $"{perc}% - {Program.villageName}- DONE:[{i}] out of {list.Count}";
                                AppCommunication.SendAdangalUpdate($"{perc}% - {Program.villageName}- DONE:[{i}] out of {list.Count}");
                            };

                            bw.RunWorkerAsync();

                        };
                    }

                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;
                    aTimer.Start();


                }

                var processed = DataAccess.GetActiveAdangalNew(AdangalConverter.villagPath);
                MessageBox.Show($"Completed: {processed.Count} out of {list.Count}", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LogMessage("ERROR:" + ex.ToString());

            }
        }



        private static void LogMessage(string message)
        {
            try
            {
                if (logHelper != null)
                    logHelper.WriteAdangalLog(message);
            }
            catch (Exception ex)
            {

                MessageBox.Show("erro" + ex.ToString());
            }


        }

        public static void SetGlobalVillagePath(string path)
        {
            villagPath = path;
            General.CreateFileIfNotExist(villagPath);
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
            using (var engine = new TesseractEngine(@"F:\AssemblySimpleApp\AdangalApp\tessdata", "eng", EngineMode.Default))
            {
                Page ocrPage = engine.Process(Pix.LoadFromFile(filePath + "CaptchImage.png"), PageSegMode.AutoOnly);
                captchatext = ocrPage.GetText();
            }

            return captchatext.Trim();
        }

        public static void TextToAdangal(string text, int surveyNo, string subdivNo, LogHelper logHelper)
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
                //bool isVagai = (hecIndex > 3);
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
                    if (DataAccess.AddNewAdangal(adangal, villagPath))
                        addedCount += 1;
                });

                logHelper.WriteAdangalLog($"Added ({addedCount}) out of ({neededData.Count()}) for {surveyNo}-{subdivNo}");

            }
            catch (Exception ex)
            {
                DataAccess.AddNewAdangal(new Adangal()
                { LandStatus = LandStatus.Error, NilaAlavaiEn = surveyNo, UtpirivuEn = subdivNo }, villagPath);
                logHelper.WriteAdangalLog($"ERROR: {surveyNo}-{subdivNo} - {ex.ToString()}");
                //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");

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

            }, villagPath);

        }

        

        public static bool IsExist(int NilaAlavaiEn, string UtpirivuEn)
        {
            return DataAccess.IsAdangalAlreadyExist(NilaAlavaiEn, UtpirivuEn, villagPath);
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
                    logHelper.WriteAdangalLog($"ERROR in names: namerow-{paramNameRow}");
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
}
