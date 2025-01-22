using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace EOAE_Code.Extensions;

public static class ItemMenuVMExtensions
{
    public static void AddTooltip(this ItemMenuVM instance, string label, string value, Color color)
    {
        AccessTools
            .Method(typeof(ItemMenuVM), "CreateColoredProperty")
            .Invoke(
                instance,
                new object[]
                {
                    instance.TargetItemProperties,
                    label,
                    value,
                    color,
                    0,
                    null,
                    TooltipProperty.TooltipPropertyFlags.None
                }
            );
    }
}
