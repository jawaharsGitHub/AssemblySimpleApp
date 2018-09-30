using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class District : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.DistrictFile;

        public int DistrictId { get; set; }

        public int ZonalId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Blocks { get; set; }

        public District(int districtId, string name )
        {
            DistrictId = districtId;
            Name = name;
                
        }

        public static List<District> GetAll()
        {
            return ReadFileAsObjects<District>(JsonFilePath);
        }


    }
}
