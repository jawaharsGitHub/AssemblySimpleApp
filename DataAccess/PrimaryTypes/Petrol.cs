using Common;
using System;

namespace DataAccess.PrimaryTypes
{
    public class Petrol : BaseClass
    {

        public static string JsonFilePath = AppConfiguration.PetrolFile;

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public decimal PetrolPrice { get; set; }

        public decimal Volume { get; set; }

        public int Speedometer { get; set; }

    }
}
