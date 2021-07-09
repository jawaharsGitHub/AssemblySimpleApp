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

        static LogHelper logHelper;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool isApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isApp"]);
            string villageName = ConfigurationManager.AppSettings["villName"];
            AdangalConverter.SetGlobalVillagePath($@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}-full.json");
            string districCode = ConfigurationManager.AppSettings["districCode"]; 
            string talukCode = ConfigurationManager.AppSettings["talukCode"];
            string villageCode = ConfigurationManager.AppSettings["villageCode"];

            if (isApp == false)
            {

                var logFolder = $@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}-Log";
                logHelper = new LogHelper("AdangalLog", logFolder, Environment.UserName, villageName);
                logHelper.WriteAdangalLog($"================={DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")}========================");

                //var list = new List<KeyValue>() { new KeyValue() { Value = 50, Caption = "3" } };
                //var isTest = true;

                // Read subdiv file
                var list = DataAccess.GetSubdiv($@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}-subdiv.json");

                try
                {

                    IWebDriver driver = new ChromeDriver();
                    driver.Navigate().GoToUrl("https://eservices.tn.gov.in/eservicesnew/land/chitta.html?lan=en");

                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        if (AdangalConverter.IsExist(list[i].Value, list[i].Caption)) // && isTest == false)
                            continue;

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
                            copiedText.ToLower().Contains("bhoodan") == false)
                        {
                            logHelper.WriteAdangalLog($"Wrong Captcha: {text}: {list[i].Value}-{list[i].Caption}");
                            //MessageBox.Show($"Retry for {list[i].Value}-{list[i].Caption}!");
                            i -= 1;
                            driver.Close();
                            continue;
                        }

                        File.AppendAllText($@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}-10-1.txt", copiedText);

                        
                        if (copiedText.ToLower().Contains("bhoodan"))
                        {
                            AdangalConverter.AddBhoodanData(list[i].Value, list[i].Caption);
                            logHelper.WriteAdangalLog($"added bhoodan land - {list[i].Value}-{list[i].Caption}");
                        }
                        else if (copiedText.ToLower().Contains("government"))
                        {
                            AdangalConverter.AddPorambokkuData(list[i].Value, list[i].Caption);
                            logHelper.WriteAdangalLog($"added govt land - {list[i].Value}-{list[i].Caption}");
                        }
                        else
                        {
                            AdangalConverter.TextToAdangal(copiedText, list[i].Value, list[i].Caption, logHelper);
                        }
                        
                        LogMessage($"DONE:[index-{i}]");


                        driver.Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    LogMessage("ERROR:" + ex.ToString());

                }


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
