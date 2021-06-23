using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTK_Support.AdangalTypes
{
    public class Adangal
    {
        public int NilaAlavaiEn { get; set; }
        public string UtpirivuEn { get; set; }
        public string Parappu { get; set; }
        public string Theervai { get; set; }
        public string Anupathaarar { get; set; }
        public string OwnerName { get; set; }
        public LandType LandType { get; set; }

        public bool IsFullfilled { get {
                return string.IsNullOrEmpty(UtpirivuEn);
                    } 
        }
        public LandStatus LandStatus { get; set; }
    }
}
