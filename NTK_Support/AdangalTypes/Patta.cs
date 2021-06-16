using Common.ExtensionMethod;
using System;
using System.Collections.Generic;

namespace NTK_Support.AdangalTypes
{
    public class Patta
    {

        public int PattaEn { get; set; }

        public List<LandDetail> landDetails { get; set; }

        public PattaType PattaType { get; set; }

        public bool isVagai { get; set; }

        public string PattaTharar { get; set; }

        public string FullData { get; set; }


        public void UpdatePatta(PattaType pattaType, List<string> fullData)
        {
            PattaType = pattaType;
            FullData = fullData.ListToString();
        }

        public override string ToString()
        {
            return $"Patta En:{PattaEn} PattaType: {Enum.GetName(typeof(PattaType), PattaType)} IsVagai: {Convert.ToInt32(isVagai)} land: {landDetails.Count}";
        }

    }
}
