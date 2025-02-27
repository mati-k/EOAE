using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class StatusEffectParticleData
    {
        [XmlAttribute]
        public string StatusEffectKey = "";

        [XmlAttribute]
        public string ParticlePrefab = "";
    }
}
