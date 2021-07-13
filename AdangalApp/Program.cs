using Common;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdangalApp
{
    static class Program
    {

        public static LogHelper CommonLogHelper;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dataFolder = General.GetDataFolder("data");
            try
            {
                if (AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder))
                {
                    AppConfiguration.AddOrUpdateAppSettings("testdataPath", General.GetDataFolder("tessdata"));
                    //CommonLogHelper = new LogHelper("commonLog", dataFolder, Environment.UserName);
                    //CommonLogHelper.WriteAdangalLog($"================={DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")}========================");
                    Application.Run(new vao());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }


}
