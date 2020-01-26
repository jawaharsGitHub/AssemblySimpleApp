using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class VoterDetail : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.VoterFile;

        public int Sno { get; set; }
        public string VoterId { get; set; }
        public string CorrectedVoterId { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Address { get; set; }
        public string CorrectedAddress { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int PageNumber { get; set; }
        public string R { get; set; }

        public static List<VoterDetail> GetAll()
        {
            return ReadFileAsObjects<VoterDetail>(JsonFilePath);
        }

    }
}
