using EOAE_Code.Data.Managers;
using EOAE_Code.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Patches;

[HarmonyPatch]
public class TooltipPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemMenuVM), "SetItem")]
    public static void AddSpecialTooltips(
        SPItemVM item,
        ItemVM comparedItem,
        BasicCharacterObject character,
        int alternativeUsageIndex,
        ItemMenuVM __instance
    )
    {
        if (BookManager.IsBook(item.StringId))
        {
            BookManager.GetBook(item.StringId).AddTooltips(__instance);
        }

        var itemObject = MBObjectManager.Instance.GetObject<ItemObject>(item.StringId);
        if (itemObject.IsEnchanted())
        {
            itemObject.GetEnchantment()!.Enchantment.AddTooltips(__instance);
        }
    }
}
