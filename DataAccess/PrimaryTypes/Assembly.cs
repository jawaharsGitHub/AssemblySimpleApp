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

        public int SNo { get; set; }

        public int AssemblyNo { get; set; }

        public int? DistrictId { get; set; }

        public string AssemblyName { get; set; }

        public int Electors { get; set; }

        public string Category { get; set; }

        public string AssemblyFullName { get { return $"{AssemblyNo}-{AssemblyName}"; } }


        public Assembly()
        {

        }
        public Assembly(int assemblyId, string name)
        {

            AssemblyNo = assemblyId;
            AssemblyName = name;

        }

        public static List<Assembly> GetAll()
        {
            return ReadFileAsObjects<Assembly>(JsonFilePath);
        }


    }
}
