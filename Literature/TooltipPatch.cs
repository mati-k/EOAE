using EOAE_Code.Data.Managers;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;

namespace EOAE_Code.Literature;

[HarmonyPatch]
public class TooltipPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemMenuVM), "SetItem")]
    public static void AddBookProgressProperty(
        SPItemVM item,
        ItemVM comparedItem,
        BasicCharacterObject character,
        int alternativeUsageIndex,
        ItemMenuVM __instance
    )
    {
        if (!BookManager.IsBook(item.StringId))
            return;

        BookManager.GetBook(item.StringId).AddTooltips(__instance);
    }
}
