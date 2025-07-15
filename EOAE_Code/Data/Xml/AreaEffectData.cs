using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class AreaEffectData
    {
        [XmlAttribute]
        public string Prefab = "";

        [XmlAttribute]
        public float DamagePerSecond = 0;

        [XmlAttribute]
        public float Duration = 0;

        [XmlAttribute]
        public float Height = 0;
    }
}
