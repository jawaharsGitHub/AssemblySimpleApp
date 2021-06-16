using Common.ExtensionMethod;
using System.Linq;

namespace NTK_Support.AdangalTypes
{
    public class LandDetail
    {
        public int PattaEn { get; set; }

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
                if (PulaEn != null && PulaEn.Split('-').Count() == 2)
                    return PulaEn.Split('-')[1];
                else
                    return "-";
            }
        }


        // நன்செய் பரப்பு 
        public string nansaiParappu { get; set; }


        // நன்செய் தீர்வை  
        public string nansaiTheervai { get; set; }

        // புன்செய் பரப்பு 
        public string punsaiParappu { get; set; }

        // புன்செய் தீர்வை 
        public string punsaiTheervai { get; set; }

        // மானாவரி பரப்பு
        public string maanavariParappu { get; set; }

        // மானாவரி தீர்வை
        public string maanavariTheervai { get; set; }

        public LandType LandType
        {

            get
            {

                int i = 0;
                LandType ld = LandType.Other;

                if (nansaiTheervai != "0" && nansaiTheervai != "0.00")
                {
                    i += 1;
                    ld = LandType.Nansai;
                }

                if (punsaiTheervai != "0" && punsaiTheervai != "0.00")
                {
                    i += 1;
                    ld = LandType.Punsai;
                }

                if (maanavariTheervai != "0" && maanavariTheervai != "0.00")
                {
                    i += 1;
                    ld = LandType.Maanaavari;
                }

                if (i > 1)
                {
                    ld = LandType.Other;
                }

                return ld;
            }

        }

    }
}
