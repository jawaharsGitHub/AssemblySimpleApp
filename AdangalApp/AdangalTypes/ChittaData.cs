using System;

namespace AdangalApp.AdangalTypes
{
    public class ChittaData
    {

        public int Index { get; set; }

        public int SurveyNo { get; set; }

        public string SurveyNoStr { get { return SurveyNo == 0 ? "" : SurveyNo.ToString(); } }

        public string SubDivNo { get; set; }

        public string Parappu { get; set; }

        public decimal Theervai { get; set; }
        public string TheervaiStr { get { return Theervai == 0 ? "" : Theervai.ToString(); } }

        public int PattaNo { get; set; }

        public string OwnerName { get; set; }

        public string LandType { get; set; }    // N-Nanjai, P- Punjai, M-matravai

        public int PageNumber { get; set; }


        public string PageNumberStr
        {
            get
            {
                return PageNumber == 0 ? "" : PageNumber.ToString();
            }
        }

        public string PageIndex { get; set; }


        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}{3}", PageNumberStr, Parappu, TheervaiStr, Environment.NewLine);
        }

    }
}
