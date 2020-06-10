using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class BoothDetail
    {
        public string PartNo { get; set; }

        public string PartPlaceName { get; set; }
        public string PartLocationAddress { get; set; }

        public string Type { get; set; }

        public string AssemblyName { get; set; }

        public string ParlimentNo { get; set; }

        public string ParlimentName { get; set; }

        public string AssemblyNo { get; set; }

        public string EligibilityDay { get; set; }

        public string ReleaseDate { get; set; }

        public string MainCityOrVillage { get; set; }

        public string Zone { get; set; }

        public string Birga { get; set; }

        public string PoliceStation { get; set; }

        public string Taluk { get; set; }

        public string District { get; set; }

        public int Pincode { get; set; }

        public int StartNo { get; set; }
        public int EndNo { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int ThirdGender { get; set; }

        public int TotalVoters { get; set; }

        //// Last Page Data

        //// Base
        //public int BaseMale { get; set; }
        //public int BaseFemale { get; set; }
        //public int BaseThirdGender { get; set; }
        //public int BaseTotalVoters { get; set; }


        //// ADD
        //public int AddMale { get; set; }
        //public int AddFemale { get; set; }
        //public int AddThirdGender { get; set; }
        //public int AddTotalVoters { get; set; }

        //// DELETE
        //public int DeleteMale { get; set; }
        //public int DeleteFemale { get; set; }
        //public int DeleteThirdGender { get; set; }
        //public int DeleteTotalVoters { get; set; }

        //// TOTAL
        //public int TotalMale { get; set; }
        //public int TotalFemale { get; set; }
        //public int TotalThirdGender { get; set; }
        //public int TotalTotalVoters { get; set; }


    }
}
