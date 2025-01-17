using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using HarmonyLib;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.BaseGameFixes
{
    public static class TradeBoundPatch
    {
        private static float tradeBoundDistance;

        public static void Apply(Harmony harmony)
        {
            tradeBoundDistance = Convert.ToSingle(
                File.ReadAllText(
                    ModuleHelper.GetModuleFullPath("EOAE_Code") + "TradeBoundDistance.txt"
                )
            );
            var villageTradeBoundCampaignBehavior = AccessTools.TypeByName(
                "VillageTradeBoundCampaignBehavior"
            );
            var original = AccessTools.Method(
                villageTradeBoundCampaignBehavior,
                "TryToAssignTradeBoundForVillage"
            );
            var transpiler = AccessTools.Method(typeof(TradeBoundPatch), nameof(Transpiler));
            harmony.Patch(original: original, transpiler: new HarmonyMethod(transpiler));
        }

        public static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions
        )
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
    }
}
