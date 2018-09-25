using Common;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace CenturyFinCorpApp
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

            CultureInfo myCI = new CultureInfo("en-GB", false);
            myCI.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

            Thread.CurrentThread.CurrentCulture = myCI;

            var dataFolder = General.GetDataFolder("AssemblyApp\\bin\\Debug", "DataAccess\\Data\\");

            string key = "SourceFolder";
            string value = dataFolder;

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            //if (AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder))
            //{
                //LogHelper.WriteLog($"started application");
                Application.Run(new frmIndexForm());
                
            //}

            //frmPrediction.Predict();

        }

    }

}
