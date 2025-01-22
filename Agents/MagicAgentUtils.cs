using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
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

    public static float GetHeightAtPoint(Vec2 point, Spell? spell)
    {
        float height = 0;
        Mission.Current.Scene.GetHeightAtPoint(
            point,
            BodyFlags.CommonCollisionExcludeFlagsForCombat,
            ref height
        );

        // Bigger the area, higher the height for better visibility
        height += 0.1f * spell.AreaRange;
        return height;
    }
}
