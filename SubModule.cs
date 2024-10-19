using EOAE_Code.SettlementUniqueMilitia;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;


namespace EOAE_Code
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("EOAE_Code");
        private static float tradeBoundDistance;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            ApplyTradeBoundFix();
            SettlementUniqueMilitiaLoader.LoadSettlements();

            Harmony.PatchAll();
        }

        private void ApplyTradeBoundFix()
        {
            tradeBoundDistance = Convert.ToSingle(File.ReadAllText(ModuleHelper.GetModuleFullPath("EOAE_Code") + "TradeBoundDistance.txt"));
            var villageTradeBoundCampaignBehavior = AccessTools.TypeByName("VillageTradeBoundCampaignBehavior");
            var original = AccessTools.Method(villageTradeBoundCampaignBehavior, "TryToAssignTradeBoundForVillage");
            var transpiler = AccessTools.Method(typeof(SubModule), nameof(Transpiler));
            Harmony.Patch(original: original, transpiler: new HarmonyMethod(transpiler));
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.OperandIs(150))
                {
                    instruction.operand = tradeBoundDistance;
                }

                yield return instruction;
            }
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
    }
}