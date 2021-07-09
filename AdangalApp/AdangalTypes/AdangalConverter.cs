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

namespace AdangalApp.AdangalTypes
{
    public static class AdangalConverter
    {
        static List<string> relationTypesCorrect;
        public static string villagPath; // = @"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\ACHUNTHANVAYAL\ACHUNTHANVAYAL-full.json";
        static AdangalConverter()
        {
            relationTypesCorrect = new List<string>() {
                        "தந்தை",
                        "கணவன்",
                        "காப்பாளர்",
                        "மகன்",
                        "மனைவி"
                    };

            
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
                bool isVagai = (hecIndex > 3);
                var totalIndex = third.Count - 1;

                var neededData = third.Skip(hecIndex + 1);
                neededData = neededData.Take(neededData.Count() - 1);

                var names = GetOwnerName(sec.Replace("புல எண்", "$").Split('$').ToList()[0]);

                int addedCount = 0;
                neededData.ToList().ForEach(fe =>
                {
                    var rowData = fe.Split('\t').ToList();
                    var adangal = GetAdangalFromCopiedData(rowData, pattaEn, names);
                    adangal.RegisterDate = createdTime;
                    adangal.IsVagai = isVagai;
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

        public static void AddPorambokkuData(int survey, string subdiv)
        {
            DataAccess.AddNewAdangal(new Adangal()
            {
                LandType = LandType.Porambokku,
                NilaAlavaiEn = survey,
                UtpirivuEn = subdiv

            }, villagPath);

        }

        public static void AddBhoodanData(int survey, string subdiv)
        {
            DataAccess.AddNewAdangal(new Adangal()
            {
                LandType = LandType.CLRBhoodanLands,
                NilaAlavaiEn = survey,
                UtpirivuEn = subdiv

            }, villagPath);

        }

        

        public static bool IsExist(int NilaAlavaiEn, string UtpirivuEn)
        {
            return DataAccess.IsAdangalAlreadyExist(NilaAlavaiEn, UtpirivuEn, villagPath);
        }

        private static Adangal GetAdangalFromCopiedData(List<string> data, string pattaEn, KeyValue ownerName)
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
                //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
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

        private static KeyValue GetOwnerName(string nameRow)
        {
            try
            {
                KeyValue kv = new KeyValue();

                nameRow = nameRow.Split(' ').ToList().Where(w => w.Trim() != "").ToList()[1];
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
                        kv.Caption2 = $"{d[0].Replace("\t", "").Replace("-", "")}";
                    }
                    else
                    {
                        // ERROR!
                        MessageBox.Show("Error!");
                    }
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
