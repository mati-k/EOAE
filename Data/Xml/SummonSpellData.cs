using System;
using System.Xml.Serialization;
using EOAE_Code.Character;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class SummonSpellData
    {
        [XmlAttribute]
        public string AgentName = "";

        [XmlAttribute]
        public float Duration = 0;

        [XmlAttribute]
        public float Amount = 0;
    }
}
