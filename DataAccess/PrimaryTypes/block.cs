using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class Block : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.BlockFile;


        public int BlockId { get; set; }

        public int DistrictId { get; set; }

        public string Name { get; set; }

        public int Ondrium { get; set; }

        // TODO: remove nullable
        public int? Population { get; set; }
        public int? Male { get; set; }
        public int? Female { get; set; }
        public int? SC { get; set; }
        public int? Scmale { get; set; }
        public int? ScFemale { get; set; }
        public int? ST { get; set; }
        public int? Stmale { get; set; }
        public int? StFemale { get; set; }

        public static List<Block> GetAll()
        {
            return ReadFileAsObjects<Block>(JsonFilePath);
        }


    }
}
