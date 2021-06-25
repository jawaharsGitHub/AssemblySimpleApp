using Common.ExtensionMethod;
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

        private LandType _landType;
        public LandType LandType { 
            get 
            {
                return _landType; 
            } 
            set 
            {
                if (value == LandType.PorambokkuError)
                {
                    LandStatus = LandStatus.Error;
                }
                _landType =  value; 
            }
        }

        public bool IsFullfilled 
        { 
            get 
            {
                return string.IsNullOrEmpty(UtpirivuEn);
            } 
        }

        
        public LandStatus LandStatus { get; set; }


        public override string ToString()
        {
            return $"{NilaAlavaiEn}-{UtpirivuEn}    {Parappu}   {Theervai} ({LandType.ToName()}){Anupathaarar}";
        }
    }
}
