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

    public static void TextToAdangal(string text)
    {
        try
        {

            var newData = text;

            var splitData = newData.Replace("உரிமையாளர்கள் பெயர்", "$").Split('$');
            var pattaEn = splitData[0].Replace("பட்டா எண்", "$").Split('$').ToList()[1].Replace(":", "").Trim();

            var first = splitData[1];

            var sec = first.Replace("குறிப்பு2", "$").Split('$')[0];
            var third = sec.Split(Environment.NewLine.ToCharArray()).Where(w => w.Trim() != "").ToList();

            var hecIndex = third.FindIndex(f => f.Contains("ஹெக்"));
            var totalIndex = third.Count - 1;

            var neededData = third.Skip(hecIndex + 1);
            neededData = neededData.Take(neededData.Count() - 1);

            var name = GetOwnerName(sec.Replace("புல எண்", "$").Split('$').ToList()[0]);

            //var surveysubdiv = cmbItemToBeAdded.SelectedItem.ToString().Split('~').ToList();

            int addedCount = 0;
            neededData.ToList().ForEach(fe =>
            {
                var rowData = fe.Split('\t').ToList();
                var adangal = GetAdangalFromCopiedData(rowData, pattaEn, name);

                //if (notInPdfToBeAdded.Contains($"{adangal.NilaAlavaiEn}~{adangal.UtpirivuEn}"))
                //{
                DataAccess.AddNewAdangal(adangal);
                //DataAccess.SaveMissedAdangal(adangal);
                addedCount += 1;
                //    LogMessage($"Added new land {adangal.ToString()}");
                //  }
            });

            //MessageBox.Show($"added {addedCount} land details");
            //button2_Click_1(null, null);

            //txtAddNewSurvey.Clear();
        }
        catch (Exception ex)
        {
            //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
        }
    }

    private static Adangal GetAdangalFromCopiedData(List<string> data, string pattaEn, string ownerName)
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
            //adangal.Anupathaarar = $"{pattaEn}-{ownerName}";
            adangal.OwnerName = ownerName;
            adangal.PattaEn = pattaEn.ToInt32();
            adangal.LandStatus = LandStatus.Added;
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

    private static string GetOwnerName(string nameRow)
    {
        try
        {
            nameRow = nameRow.Split(' ').ToList().Where(w => w.Trim() != "").ToList()[1];
            string name = "";
            if (relationTypesCorrect.Any(a => nameRow.Split('\t').ToList().Contains(a))) // have valid names.
            {
                var delitList = relationTypesCorrect.Intersect(nameRow.Split('\t').ToList()).ToList();

                if (delitList.Count == 1)
                {
                    var delimit = delitList[0];
                    name = nameRow.Replace(delimit, "$").Split('$')[1];
                    var d = nameRow.Replace(delimit, "$").Split('$');
                    name = $"{d[1]}";
                }
                else
                {
                    // ERROR!
                    MessageBox.Show("Error!");
                }
            }

            return name.Trim().Replace("\t", "").Replace("-", "");
        }
        catch (Exception ex)
        {
            //LogError($"Error @ {MethodBase.GetCurrentMethod().Name} - {ex.ToString()}");
            return null;
        }
    }


}
}
