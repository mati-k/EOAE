using EOAE_Code.BaseGameFixes;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Literature;
using EOAE_Code.Magic;
using EOAE_Code.Magic.StatusEffect;
using EOAE_Code.Models;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("EOAE_Code");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            XmlDataLoader.LoadXmlDataList<
                SettlementUniqueMilitiaDataXml,
                SettlementUniqueMilitiaManager
            >("custom_militia.xml");
            XmlDataLoader.LoadXmlDataCustomRoot<SpellData, SpellManager, SpellListData>("spells.xml");
            XmlDataLoader.LoadXmlDataList<TroopSpellBookData, TroopSpellBookManager>(
                "troop_spellbooks.xml"
            );
            XmlDataLoader.LoadXmlDataList<AnimationDurationData, AnimationDurationManager>(
                "animation_durations.xml"
            );
            XmlDataLoader.LoadXmlDataList<BookDataXml, BookManager>("books.xml");

            TradeBoundPatch.Apply(Harmony);
            Harmony.PatchAll();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            if (Game.Current.GameType is Campaign && starterObject is CampaignGameStarter starter)
            {
                starter.AddBehavior(new SavePatch());
                starter.AddBehavior(new LiteratureCampaignBehavior());
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            mission.AddMissionBehavior(new MagicMissionLogic());
            mission.AddMissionBehavior(new MagicMissionView());
            mission.AddMissionBehavior(new SpellAimView());
            mission.AddMissionBehavior(new SpellSelectorView());
            mission.AddMissionBehavior(new StatusEffectLogic());
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            gameStarterObject.AddModel(new AgentStatusEffectModel());
        }
    }
}
