using Common.ExtensionMethod;
using System.Linq;

namespace NTK_Support.AdangalTypes
{
    public class LandDetail
    {
        public int PattaEn { get; set; }

        public string OwnerName { get; set; }

        public string PulaEn { get; set; }

        public int SurveyNo
        {
            get
            {
                return PulaEn == null ? -1 : PulaEn.Split('-')[0].ToInt32();
            }
        }

        public string Subdivision
        {
            get
            {
                if (PulaEn != null && PulaEn.Split('-').Count() == 2 && PulaEn.Split('-')[1].Trim() != "")
                    return PulaEn.Split('-')[1];
                else
                    return "-";
            }
        }


        // நன்செய் பரப்பு 
        public string NansaiParappu { get; set; }


        // நன்செய் தீர்வை  
        public string NansaiTheervai { get; set; }

        // புன்செய் பரப்பு 
        public string PunsaiParappu { get; set; }

        // புன்செய் தீர்வை 
        public string PunsaiTheervai { get; set; }

        // மானாவரி பரப்பு
        public string MaanavariParappu { get; set; }

        // மானாவரி தீர்வை
        public string MaanavariTheervai { get; set; }

        private LandType _landType;
        public LandType LandType
        {
            get
            {
                int i = 0;
                _landType = LandType.Zero;

                if (NansaiTheervai != "0" && NansaiTheervai != "0.00")
                {
                    i += 1;
                    _landType = LandType.Nansai;
                }

                if (PunsaiTheervai != "0" && PunsaiTheervai != "0.00")
                {
                    i += 1;
                    _landType = LandType.Punsai;
                }

                if (MaanavariTheervai != "0" && MaanavariTheervai != "0.00")
                {
                    i += 1;
                    _landType = LandType.Maanaavari;
                }

                if (i > 1)
                {
                    _landType = LandType.Zero;
                }

                return _landType;
            }

            set
            {
                _landType = value; 
            }

        }

        public string Parappu
        {
            get
            {
                if (LandType == LandType.Nansai) return NansaiParappu.Replace("-",".");
                else if (LandType == LandType.Punsai) return PunsaiParappu.Replace("-", ".");
                else if (LandType == LandType.Maanaavari) return MaanavariParappu.Replace("-", ".");
                else return "-";
            }
        }

        public string Theervai
        {
            get
            {
                if (LandType == LandType.Nansai) return NansaiTheervai;
                else if (LandType == LandType.Punsai) return PunsaiTheervai;
                else if (LandType == LandType.Maanaavari) return MaanavariTheervai;
                else return "-";
            }
        }

        public string Anupathaarar
        {
            get
            {
                return $"{PattaEn} - {OwnerName}";
            }
        }

        public LandStatus LandStatus { get; set; }

        public string CorrectNameRow { get; set; }
    }
}
