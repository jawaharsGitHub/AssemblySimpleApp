using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TamilNaduElections
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

            var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "TamilNaduElections\\DB\\");

            AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder);


            Application.Run(new Form1());
        }
    }
	
	
	/*TODOs*/
	
	// TODO: Urupinar serkai
	// TODO: Kodi Edru Vizha
	// TODO: Free Doctor Checkup
	// TODO: Free Vetnery Doctor Visit.
	// TODO: Vilaiyattu Potikal
	// TODO: Be a friends with other party members also.
	// TODO: Overall -- need a good marketing startegy.
	// TODO: COllecting/Coordinating frreign workers. [Whats app group]
	// TODO: Responsibilities --> Coordinating all party work and members.
	
	
}
