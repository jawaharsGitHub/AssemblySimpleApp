using Common;
using System.ComponentModel;

namespace AdangalApp.AdangalTypes
{
    public enum LandType
    {
        [Description("நன்செய்")]
        [Order(0)]
        Nansai,

        [Description("புன்செய்")]
        [Order(1)]
        Punsai,

        [Description("மானாவாரி")]
        [Order(2)]
        Maanaavari,

        [Description("புறம்போக்கு")]
        [Order(6)]
        Porambokku,
        [Order(101)]
        PorambokkuError,
        [Order(102)]
        Zero,
        [Order(103)]
        CLRBhoodanLands,
        [Order(104)]
        Dash,
        [Order(105)]
        UnKnown,
        [Description("நன்செய் அனாதீனம்")]
        [Order(3)]
        NansaiAtheenam,

        [Description("புன்செய் அனாதீனம்")]
        [Order(4)]
        PunsaiAtheenam,

        [Description("தென்னை அபி விருத்தி திட்ட ஒதுக்கீடு")]
        [Order(5)]
        ThennaiAbiViruththi,

        //[Description("நத்தம்")]
        //[Order(5)]
        //Naththam,
    }
}
