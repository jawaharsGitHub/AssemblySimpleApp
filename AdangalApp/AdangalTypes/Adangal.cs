using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdangalApp.AdangalTypes
{
    public class Adangal
    {
        
        public int NilaAlavaiEn { get; set; }
        public string UtpirivuEn { get; set; }
        public string Parappu { get; set; }
        public string Theervai { get; set; }
        public string Anupathaarar
        {
            get
            {
                if (LandType == LandType.Porambokku || LandType == LandType.NansaiAtheenam || LandType == LandType.PunsaiAtheenam)
                    return $"{OwnerName}";
                else
                    return $"{PattaEn} - {(IsVagai ?  OwnerName + " வகை" : OwnerName)}";
            }
        }
        public string OwnerName { get; set; }
        public string LastName { get; set; }

        public bool IsVagai { get; set; }

        public string RegisterDate { get; set; }

        private LandType _landType;
        public LandType LandType
        {
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
                _landType = value;
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
            return $"{NilaAlavaiEn}-{UtpirivuEn}    {Parappu}   {Theervai} ({LandType.ToString()}){Anupathaarar}";
        }

        public int PattaEn { get; set; }

        //public string CorrectNameRow { get; set; }
    }
}
