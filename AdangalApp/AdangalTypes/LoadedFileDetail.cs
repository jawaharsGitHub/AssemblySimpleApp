using Common.ExtensionMethod;
using System.Linq;

namespace AdangalApp.AdangalTypes
{
    public class LoadedFileDetail
    {
        public int MaavattamCode { get; set; }
        public string MaavattamName { get; set; }
        public string MaavattamNameTamil { get; set; }

        public int VattamCode { get; set; }
        public string VattamName { get; set; }
        public string VattamNameTamil { get; set; }
        
        public string FirkaName { get; set; }

        public int VillageCode { get; set; }
        public string VillageName { get; set; }
        public string VillageNameTamil { get; set; }

        public string VillageFullName { get  
            {
                return $"{VillageCode} - {VillageName}";
            }
        
        }

        public decimal? InitialPercentage { get; set; }
        public decimal? CorrectedPercentage { get; set; }

        

    }
}
