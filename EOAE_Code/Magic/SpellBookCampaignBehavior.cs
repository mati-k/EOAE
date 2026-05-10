using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Consts;
using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace EOAE_Code.Magic
{
    public class SpellBookCampaignBehavior : CampaignBehaviorBase
    {
        // Dictionary<heroName, List<spellName>>
        private Dictionary<Hero, List<string>> savedSpellBooks = new();
        private Dictionary<Hero, List<Spell>> heroSpellBooks = new();

        public Dictionary<Hero, List<Spell>> HeroSpellBooks => heroSpellBooks;

        public void SaveHeroSpellBook(Hero hero, List<Spell> spells)
        {
            heroSpellBooks[hero] = spells;
        }

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                SaveSpellBooks();
                dataStore.SyncData("_savedSpellBooks", ref savedSpellBooks);
            }
            else if (dataStore.IsLoading)
            {
                dataStore.SyncData("_savedSpellBooks", ref savedSpellBooks);
                LoadSavedSpellBooks();
            }
        }

        private void SaveSpellBooks()
        {
            savedSpellBooks.Clear();
            savedSpellBooks = heroSpellBooks.ToDictionary(
                keyValue => keyValue.Key,
                keyValue => keyValue.Value.Select(spell => spell?.ItemName ?? "").ToList()
            );
        }

        private void LoadSavedSpellBooks()
        {
            heroSpellBooks.Clear();
            foreach (var hero in savedSpellBooks.Keys)
            {
                if (!heroSpellBooks.ContainsKey(hero))
                {
                    heroSpellBooks[hero] = new List<Spell>();
                }
                foreach (var spellName in savedSpellBooks[hero])
                {
                    if (string.IsNullOrEmpty(spellName))
                    {
                        heroSpellBooks[hero].Add(null);
                    }
                    else
                    {
                        var spell = SpellManager.GetSpellFromItem(spellName);
                        heroSpellBooks[hero].Add(spell);
                    }
                }
            }
        }
    }
}

public class SpellBookSaveDefiner : SaveableTypeDefiner
{
    public SpellBookSaveDefiner()
        : base(ModuleConsts.SaveId) { }

    protected override void DefineContainerDefinitions()
    {
        ConstructContainerDefinition(typeof(Dictionary<Hero, List<string>>));
    }
}
