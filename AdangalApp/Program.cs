using AdangalApp.AdangalTypes;
using Common;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tesseract;

namespace AdangalApp
{
    static class Program
    {

        //public static LogHelper CommonLogHelper;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //AdangalConverter.GetBloContacts();
            //AdangalConverter.GetForm20s();

            var dataFolder = General.GetDataFolder("data");
            try
            {
                //var di = (from d in DriveInfo.GetDrives().ToList()
                //          where d.Name.ToLower().Contains("c") == false
                //          select d).First().Name;
                //var imgPath = Path.Combine(di, "imageTest\\");
                //string testdataPath = ConfigurationManager.AppSettings["testdataPath"];
                //string captchatext;
                ////reading text from images
                //using (var engine = new TesseractEngine(testdataPath, "eng", EngineMode.Default))
                //{
                //    Page ocrPage = engine.Process(Pix.LoadFromFile(imgPath + "WhatsApp Image 2021-07-13 at 9.03.49 PM.jpeg"), PageSegMode.AutoOnly);
                //    captchatext = ocrPage.GetText();
                //}

                //return captchatext.Trim();

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
