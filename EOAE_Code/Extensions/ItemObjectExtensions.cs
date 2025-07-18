﻿using System.Runtime.CompilerServices;
using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Enchanting;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace EOAE_Code.Extensions;

public static class ItemObjectExtensions
{
    private static readonly ConditionalWeakTable<ItemObject, MissileEffectHolder> _missileEffects =
        new();

    private class MissileEffectHolder
    {
        public StatusEffect StatusEffect { get; set; } = null!;
    }

    public static StatusEffect? GetMissileEffect(this ItemObject instance)
    {
        if (instance == null)
        {
            return null;
        }

        return !_missileEffects.TryGetValue(instance, out var value) ? null : value.StatusEffect;
    }

    public static void SetMissileEffect(this ItemObject instance, StatusEffect value)
    {
        _missileEffects.GetOrCreateValue(instance).StatusEffect = value;
    }

    public static bool IsEnchanted(this ItemObject itemObject)
    {
        var enchantingCampaignBehavior =
            Campaign.Current.GetCampaignBehavior<EnchantingCampaignBehavior>();
        return enchantingCampaignBehavior.IsItemEnchanted(itemObject);
    }

    public static EnchantedItem? GetEnchantment(this ItemObject itemObject)
    {
        var enchantingCampaignBehavior =
            Campaign.Current.GetCampaignBehavior<EnchantingCampaignBehavior>();
        return enchantingCampaignBehavior.GetItemEnchantment(itemObject);
    }
}
