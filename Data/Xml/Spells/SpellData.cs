using EOAE_Code.Character;
using System;
using System.Xml.Serialization;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    [XmlInclude(typeof(MissileSpellData))]
    [XmlInclude(typeof(HealSelfSpellData))]
    [XmlInclude(typeof(HealGroupSpellData))]
    [XmlInclude(typeof(SummonSpellData))]
    [XmlInclude(typeof(BombardSpellData))]
    [XmlInclude(typeof(AreaSpellData))]
    public class SpellData
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
        public string Icon = "";

        [XmlAttribute]
        public string Animation = "";

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
