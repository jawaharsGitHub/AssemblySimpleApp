using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AdangalApp.AdangalTypes;

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

            bool isApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isApp"]);

            if (isApp == false)
            {
                // Read subdiv file

                var list = DataAccess.GetSubdiv(@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\ACHUNTHANVAYAL\ACHUNTHANVAYAL-subdiv.json");

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

                    AdangalConverter.TextToAdangal(copiedText);

                    driver.Close();

                }

                return;
            }
            else
            {
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
