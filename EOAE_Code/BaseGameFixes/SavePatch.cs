using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace EOAE_Code.BaseGameFixes
{
    public class SavePatch : CampaignBehaviorBase
    {
        private Dictionary<string, int> heroRaceMap = new();

        public override void RegisterEvents()
        {
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, OnSave);
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionStart);
        }

        private void OnSessionStart(CampaignGameStarter obj)
        {
            Hero main = Hero.AllAliveHeroes.Find(hero => hero.StringId == "main_hero");
            if (heroRaceMap.Count > 0)
            {
                foreach (var hero in Hero.AllAliveHeroes)
                {
                    if (
                        heroRaceMap.ContainsKey(hero.StringId)
                        && heroRaceMap[hero.StringId] != hero.CharacterObject.Race
                    )
                    {
                        hero.CharacterObject.Race = heroRaceMap[hero.StringId];
                    }
                }
            }
        }

        private void OnSave()
        {
            heroRaceMap = new Dictionary<string, int>();
            Hero main = Hero.AllAliveHeroes.Find(hero => hero.StringId == "main_hero");
            foreach (var hero in Hero.AllAliveHeroes)
            {
                if (!heroRaceMap.ContainsKey(hero.StringId))
                {
                    heroRaceMap.Add(hero.StringId, hero.CharacterObject.Race);
                }
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_heroRaceMap", ref heroRaceMap);
        }
    }
}
