using Common;
using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTK_Support.AdangalTypes
{
    public class FinalReport
    {
        public FinalReport(List<Patta> PattaListCtr)
        {
            PattaList = PattaListCtr;
            KeyValue singleData = null;
            CountData = new List<KeyValue>();
            GroupedData = new List<object>();

            int processedCount = 0;
            foreach (PattaType rt in Enum.GetValues(typeof(PattaType)))
            {
                singleData = new KeyValue();
                singleData.Value = PattaList.Count(c => c.PattaType == rt);
                processedCount += singleData.Value;
                singleData.Caption = Enum.GetName(typeof(PattaType), rt);
                singleData.Id = (int)rt;
                var lst = PattaList.Where(c => c.PattaType == rt).ToList();

                singleData.CaptionData = lst.Select(s => s.FullData).ToList();
                GroupedData.Add(lst);
                CountData.Add(singleData);
            }

            IsFullProcessed = (PattaList.Count == processedCount);

            NotProcessedData = (PattaList.Count - processedCount);

            CountData.Add(new KeyValue("Total Record", PattaList.Count));
            CountData.Add(new KeyValue("Not Processed", NotProcessedData));
        }
        public List<KeyValue> CountData { get; set; }

        public List<Patta> PattaList { get; set; }

        public bool IsFullProcessed { get; set; }

        public int NotProcessedData { get; set; }

        public List<object> GroupedData { get; set; }
        public override string ToString()
        {
            if (PattaList == null || PattaList.Count == 0)
                return "";

            return CountData.Select(s => s.ToString()).ToList().ListToString();
        }

    }
}
