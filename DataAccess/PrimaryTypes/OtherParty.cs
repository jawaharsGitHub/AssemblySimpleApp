using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class OtherPartyData : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.OtherPartyFile;

        public int pno { get; set; }

        public string boothname { get; set; }

        public int bn { get; set; }

        public int total { get; set; }

        public int polled { get; set; }
        public int ntkvote { get; set; }
        public decimal ntkperc { get; set; }
        public decimal ntkAim { get; set; }
        public int dmk { get; set; }
        public int admk { get; set; }
        public int ttv { get; set; }

        public int mnm { get; set; }

        public int boothNO { get; set; }

        public int BoothCount { get; set; }

        public string ooratchi { get; set; }

        public string ondrium { get; set; }

        public static List<OtherPartyData> GetAll()
        {
            return ReadFileAsObjects<OtherPartyData>(JsonFilePath);
        }


    }
}
