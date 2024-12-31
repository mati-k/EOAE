using EOAE_Code.Character;
using EOAE_Code.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public abstract class Spell
    {
        public string Name { get; private set; }
        public string ItemName { get; private set; }
        public int Cost { get; private set; }
        public SkillObject School { get; private set; }
        public abstract bool IsThrown { get; }

        public Spell(SpellDataXml data) 
        {
            Name = data.Name;
            ItemName = data.ItemName;
            Cost = data.Cost;

            switch (data.SchoolName)
            {
                case "Destruction":
                    School = CustomSkills.Instance.Destruction;
                break;
                case "Restoration":
                    School = CustomSkills.Instance.Restoration;
                    break;
                case "Conjuration":
                    School = CustomSkills.Instance.Conjuration;
                    break;
                default:
                    School = CustomSkills.Instance.Destruction;
                    break;
            }
        }

        public abstract void Cast(Agent caster);
    }
}
