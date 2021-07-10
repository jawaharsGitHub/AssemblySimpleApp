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

            string villageName = ConfigurationManager.AppSettings["villName"];
            AdangalConverter.SetGlobalVillagePath($@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}.json");
            var logFolder = $@"F:\AssemblySimpleApp\AdangalApp\data\AdangalJson\{villageName}\{villageName}-Log";
            logHelper = new LogHelper("AdangalLog", logFolder, Environment.UserName, villageName);
            logHelper.WriteAdangalLog($"================={DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")}========================");
            bool isApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isApp"]);

            
            if (isApp == false)
            {
                AdangalConverter.ProcessAdangal(logHelper, villageName);
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
