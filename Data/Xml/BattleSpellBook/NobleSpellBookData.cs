using System.Linq;
using System.Xml.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Data.Xml.BattleSpellBook
{
    public class NobleSpellBookData
    {
        private const int MAX_SPELLS = 5;

        [XmlAttribute]
        public string Culture = "";

        [XmlArray]
        public NobleSpellData[] Spells = new NobleSpellData[0];

        public NobleBattleSpells GetSpellBookForBattle(Hero hero)
        {
            var filtered = Spells
                .Where(spell =>
                    string.IsNullOrEmpty(spell.RequiredPerk)
                    || hero.GetPerkValue(
                        MBObjectManager.Instance.GetObject<PerkObject>(spell.RequiredPerk)
                    )
                )
                .ToList();
            Spells.Randomize();

            return new NobleBattleSpells(Spells.Take(MAX_SPELLS).ToList());
        }
    }
}
