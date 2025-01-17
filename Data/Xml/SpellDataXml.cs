using System;
using System.Xml.Serialization;
using EOAE_Code.Character;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class SpellDataXml
    {
        [XmlAttribute]
        public string Name = "";

        [XmlAttribute]
        public string ItemName = "";

        [XmlAttribute("School")]
        public string SchoolName = "";

        [XmlAttribute]
        public int Cost = 0;

        [XmlAttribute]
        public string Effect = "";

        [XmlAttribute]
        public float EffecValue = 0;

        [XmlAttribute]
        public float Range = 0;

        [XmlAttribute]
        public float AreaRange = 0;

        [XmlAttribute]
        public bool AreaAim = false;

        [XmlAttribute]
        public string AreaAimPrefab = "";

        [XmlAttribute]
        public string Icon = "";

        [XmlElement]
        public SummonSpellData? SummonSpellData = null;

        [XmlIgnore]
        public SkillObject School
        {
            get
            {
                switch (SchoolName)
                {
                    case "Destruction":
                        return CustomSkills.Instance.Destruction;
                    case "Restoration":
                        return CustomSkills.Instance.Restoration;
                    case "Conjuration":
                        return CustomSkills.Instance.Conjuration;
                }

                return CustomSkills.Instance.Destruction;
            }
        }
    }
}
