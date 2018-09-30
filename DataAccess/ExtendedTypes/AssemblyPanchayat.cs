using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class AssemblyPanchayat
    {


        //public int SNo { get; set; }

        public int PanchayatId { get; set; }

        public string Name { get; set; }

        public int AssemblyNo { get; set; }

        public string AssemblyName { get; set; }

        public int BlockId { get; set; }

        
        // TODO: need to remove nullable types.
        public int? Population { get; set; }
        public int? Male { get; set; }
        public int? Female { get; set; }
        public int? SC { get; set; }
        public int? Scmale { get; set; }
        public int? ScFemale { get; set; }
        public int? ST { get; set; }
        public int? Stmale { get; set; }
        public int? StFemale { get; set; }

    }
}
