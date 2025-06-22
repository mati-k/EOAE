using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    public abstract class Modifier : StatusEffectAction
    {
        [XmlAttribute]
        // Key like freezing , burning, to avoid stacking from same source
        public string Key = "";
    }
}
