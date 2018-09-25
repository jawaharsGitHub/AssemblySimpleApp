using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamilNaduElections
{
    public static class TnAppConfiguration
    {
        public static string CustomerFile { get; } = GetFullPath("CustomerFile");


        private static string GetFullPath(string configKey)
        {
            return Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings[configKey]);

        }
    }
}
