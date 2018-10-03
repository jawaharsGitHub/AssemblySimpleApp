using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class VoteData : BaseClass
    {
        private static string JsonFilePath = AppConfiguration.VoteDataFile;

        public int SNo { get; set; }

        public int Rank { get; set; }

        public int ACno { get; set; }

        public string CandidateName { get; set; }

        public string PartyAbbreviation { get; set; }

        public int VotesPolled { get; set; }

        public decimal PercVotesPolled { get; set; }

        public static List<VoteData> GetAll()
        {
            return ReadFileAsObjects<VoteData>(JsonFilePath);
        }


    }
}
