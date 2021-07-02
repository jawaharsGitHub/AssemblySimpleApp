using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTK_Support
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

            var dataFolder = General.GetDataFolder("NTK_Support\\json\\");
            MessageBox.Show(dataFolder);

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
