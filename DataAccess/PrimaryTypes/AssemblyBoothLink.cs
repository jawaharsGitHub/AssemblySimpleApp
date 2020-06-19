using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class AssemblyBoothLink : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.AssemblyBoothLinkFile;


        public int AssemblyNo { get; set; }

        public int BoothNo { get; set; }

        public int PaguthiNo { get; set; }

        public PaguthiType PaguthiType { get; set; }

        public int PanchayatId { get; set; }


        public static List<AssemblyBoothLink> GetAll()
        {
            return ReadFileAsObjects<AssemblyBoothLink>(JsonFilePath);
        }

        public static List<AssemblyBoothLink> GetForAssembly(int assemblyNo)
        {
            var data =  ReadFileAsObjects<AssemblyBoothLink>(JsonFilePath);
            return data.Where(w => w.AssemblyNo == assemblyNo).ToList();
        }

        public static void AddBoothForAssembly(List<AssemblyBoothLink> ablList)
        {

            var list = ReadFileAsObjects<AssemblyBoothLink>(JsonFilePath);

            list.RemoveAll((c) => c.AssemblyNo == ablList.First().AssemblyNo && ablList.Select(s => s.BoothNo).Contains(c.BoothNo));

            InsertObjectsToJson<AssemblyBoothLink>(JsonFilePath, ablList);

        }

    }
}
