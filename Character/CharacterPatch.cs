using HarmonyLib;
using TaleWorlds.Core;

namespace EOAE_Code.Character
{
    [HarmonyPatch]
    public class CharacterPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game), "InitializeDefaultGameObjects")]
        public static void LoadSkills()
        {
            Attributes.Instance.Initialize();
            CustomSkills.Instance.Initialize();
            CustomPerks.Instance.Initialize();
        }
    }
}
