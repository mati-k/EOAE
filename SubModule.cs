﻿using EOAE_Code.BaseGameFixes;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Literature;
using EOAE_Code.Magic;
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

            XmlDataLoader.LoadXmlData<
                SettlementUniqueMilitiaDataXml,
                SettlementUniqueMilitiaManager
            >("custom_militia.xml");
            XmlDataLoader.LoadXmlData<SpellDataXml, SpellManager>("spells.xml");
            XmlDataLoader.LoadXmlData<TroopSpellBookData, TroopSpellBookManager>(
                "troop_spellbooks.xml"
            );
            XmlDataLoader.LoadXmlData<AnimationDurationData, AnimationDurationManager>(
                "animation_durations.xml"
            );
            XmlDataLoader.LoadXmlData<BookDataXml, BookManager>("books.xml");

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
                starter.AddBehavior(new BookCampaignBehavior());
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            mission.AddMissionBehavior(new MagicMissionLogic());
            mission.AddMissionBehavior(new MagicMissionView());
            mission.AddMissionBehavior(new SpellAimView());
            mission.AddMissionBehavior(new SpellSelectorView());
        }
    }
}
