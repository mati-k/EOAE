using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    [HarmonyPatch]
    public static class SkillPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game), "InitializeDefaultGameObjects")]
        public static void LoadSkills()
        {
            Attributes.Instance.Initialize();
            Skills.Instance.Initialize();
        }
    }
}
