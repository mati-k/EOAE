using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class SummonEntityData
    {
        [XmlAttribute]
        public string AgentName = "";

        [XmlAttribute]
        public float Duration = 0;

        [XmlAttribute]
        public float Amount = 0;
    }
}
