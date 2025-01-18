using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Agents;

public static class MagicAgentUtils
{
    public static void EquipSpell(this Agent agent, Spell spell)
    {
        var spellObject = MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName);
        var spellWeapon = new MissionWeapon(spellObject, null, null);

        agent.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref spellWeapon);
        agent.TryToWieldWeaponInSlot(
            EquipmentIndex.ExtraWeaponSlot,
            Agent.WeaponWieldActionType.Instant,
            false
        );
    }

    public static Spell? GetEquippedSpell(this Agent agent)
    {
        var itemName = agent.WieldedWeapon.Item?.StringId;
        if (itemName == null || !SpellManager.IsSpell(itemName))
            return null;

        return SpellManager.GetSpellFromItem(itemName);
    }
}
