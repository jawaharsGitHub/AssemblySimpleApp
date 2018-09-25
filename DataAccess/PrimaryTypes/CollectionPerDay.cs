using Common;

namespace DataAccess.PrimaryTypes
{
    public class CollectionPerDay : BaseClass
    {

        public string Date { get; set; }
        public int ActualCollection { get; set; }
        public int ExpectedCollection { get; set; }
    }
}
