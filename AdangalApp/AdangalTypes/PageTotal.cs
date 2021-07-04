namespace AdangalApp.AdangalTypes
{
    public class PageTotal
    {
        public int PageNo { get; set; }
        public string ParappuTotal { get; set; }
        public decimal TheervaiTotal { get; set; }
        public string Saagupadi { get; set; }
        public string Tharisu { get; set; }
        public LandType LandType { get; set; }

        public override string ToString()
        {
            return $"pn:{PageNo}-p:{ParappuTotal}-t:{TheervaiTotal}-lt:{LandType}";
        }
    }
}
