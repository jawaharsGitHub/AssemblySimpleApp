using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace AdangalApp
{
    static class Program
    {



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Form20Analysis());
            //Application.Run(new DocxToTxt());

            List<KeyValue> list = new List<KeyValue>() {

                new KeyValue() { Caption = "100", Caption2 = "2B2" },
                 new KeyValue() { Caption = "100", Caption2 = "2A2" },
                 new KeyValue() { Caption = "100", Caption2 = "3B" },

            };


            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");

            for (int i = 0; i <= list.Count - 1; i++)
            {
                driver = driver.SwitchTo().Window(driver.WindowHandles[0]);
                //select district
                var district = driver.FindElement(By.Name("districtCode"));
                var selectElement = new SelectElement(district);
                selectElement.SelectByValue("27");

                //choose rural
                RadioButtons categories = new RadioButtons(driver, driver.FindElements(By.Name("areaType")));
                categories.SelectValue("rural");

                //button click
                driver.FindElement(By.ClassName("button")).Click();

                // select taluk
                var taluk = driver.FindElement(By.Name("talukCode"));
                var talukElement = new SelectElement(taluk);
                talukElement.SelectByValue("11");

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
                vilageElement.SelectByValue("022");

                //enter survey no
                var txt = driver.FindElement(By.Name("surveyNo"));
                txt.SendKeys(list[i].Caption);

                //enter captcha
                var text = GenerateSnapshot(driver, @"E:\imageTest\");
                var txtCap = driver.FindElement(By.Name("captcha"));
                txtCap.SendKeys(text);

                var subDiv = driver.FindElement(By.Name("subdivNo"));
                var subDivElement = new SelectElement(subDiv);
                while (subDivElement.WrappedElement.Text == "Please Select ...")
                {
                    subDiv = driver.FindElement(By.Name("subdivNo"));
                    subDivElement = new SelectElement(subDiv);
                }
                subDivElement.SelectByValue(list[i].Caption2);
               
                //button click
                driver.FindElement(By.ClassName("button")).Click();

                var isCapchaCorrect = (driver.WindowHandles.Count == 2);
                if (isCapchaCorrect == false)
                {
                    i -= 1;
                    continue;
                }

                driver = driver.SwitchTo().Window(driver.WindowHandles[1]);
                Actions action = new Actions(driver);
                action.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("a").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();
                action.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("c").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();

                var copiedText = Clipboard.GetText();

                File.AppendAllText(@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\ACHUNTHANVAYAL\Achun-10-1.txt", copiedText);
                driver.Close();

            }

            return;

            var dataFolder = AdangalConstant.dataPath;
            try
            {
                if (AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder))
                    Application.Run(new vao());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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

        //    public static void LoadImageFromClassAndSrcInfo(IWebDriver webDriver, string Id, string partialSrc, string localFile)
        //    {
        //        IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
        //        string base64string = js.ExecuteScript(@"
        //var c = document.createElement('canvas');
        //var ctx = c.getContext('2d');
        //var img = Array.prototype.filter.call(document.getElementById('" + Id + @"'), ({ src }) => src.includes('" + partialSrc + @"') )[0];
        //c.height=img.naturalHeight;
        //c.width=img.naturalWidth;
        //ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
        //var base64String = c.toDataURL();
        //return base64String;
        //") as string;

        //        var base64 = base64string.Split(',').Last();

        //        using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
        //        {
        //            using (var bitmap = new Bitmap(stream))
        //            {
        //                bitmap.Save(localFile, ImageFormat.Jpeg);
        //            }
        //        }
        //    }

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
