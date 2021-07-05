using System.ComponentModel;

namespace AdangalApp.AdangalTypes
{
    public enum LandType
    {
        [Description("நன்செய்")]
        Nansai,
        [Description("புன்செய்")]
        Punsai,
        [Description("மானாவாரி")]
        Maanaavari,
        [Description("புறம்போக்கு")]
        Porambokku,
        PorambokkuError,        
        Zero,
        //NameError
    }
}
