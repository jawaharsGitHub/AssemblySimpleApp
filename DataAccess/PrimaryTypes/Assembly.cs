using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class Assembly : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.AssemblyFile;


        public int AssemblyNo { get; set; }

        public string Name { get; set; }

        public int Electors { get; set; }

        public string Category { get; set; }

        public static List<Assembly> GetAll()
        {
            return ReadFileAsObjects<Assembly>(JsonFilePath);
        }


    }
}
