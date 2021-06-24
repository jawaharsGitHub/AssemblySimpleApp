using System.Collections.Generic;

namespace NTK_Support.AdangalTypes
{
    public class PattaList : List<Patta>
    {

        public void AddAndUpdatePattaAndOwnerNameinList(Patta item, PattaType pattaType, List<string> fullData)
        {
            item.UpdatePatta(pattaType, fullData);
            base.Add(item);

            if (item.landDetails == null)
            {
                item.landDetails = new List<LandDetail>()
                {
                    new LandDetail() { PattaEn = item.PattaEn }
                };
            }
            else
            {
                item.landDetails.ForEach(ld => {

                    ld.PattaEn = item.PattaEn;
                    ld.OwnerName = item.isVagai ? $"{item.PattaTharar} வகை" : item.PattaTharar;
                    }) ;
            }

        }
    }
}
