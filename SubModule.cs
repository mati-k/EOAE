using EOAE_Code.BaseGameFixes;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Magic;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;


namespace EOAE_Code
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("EOAE_Code");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        
            SettlementUniqueMilitiaLoader.LoadSettlements();
            SpellLoader.LoadSpells();

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
            if (Game.Current.GameType is Campaign && starterObject is CampaignGameStarter)
            {
                var starter = starterObject as CampaignGameStarter;
                starter.AddBehavior(new SavePatch());
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            mission.AddMissionBehavior(new MagicMissionLogic());
            mission.AddMissionBehavior(new MagicMissionView());
        }
    }
}