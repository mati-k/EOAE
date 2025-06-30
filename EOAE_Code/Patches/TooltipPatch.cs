using EOAE_Code.Data.Managers;
using EOAE_Code.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
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
        string itemId = item.ItemRosterElement.EquipmentElement.Item.StringId;

        if (BookManager.IsBook(itemId))
        {
            BookManager.GetBook(itemId).AddTooltips(__instance);
        }

        var itemObject = MBObjectManager.Instance.GetObject<ItemObject>(itemId);
        if (itemObject.IsEnchanted())
        {
            __instance.AddTooltip(
                "",
                new TextObject("{=aVhgwU2u}Enchantment:").ToString(),
                Color.ConvertStringToColor("#4470F2FF")
            );

            itemObject.GetEnchantment()!.StatusEffect.AddTooltips(__instance);
        }
    }
}
