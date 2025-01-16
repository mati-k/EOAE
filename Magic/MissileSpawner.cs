using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic;

public class MissileSpawner : ScriptComponentBehavior
{
    public Agent Caster { get; set; }

    public void SpawnMissile(string missileName, Vec3 offset, Vec3 direction, float speed)
    {
        var missileItem = MBObjectManager.Instance.GetObject<ItemObject>(missileName);
        var missionWeapon = new MissionWeapon(missileItem, null, null);
        var position = GameEntity.GlobalPosition + offset;
        var orientation = GameEntity.GetGlobalFrame().rotation;

        Mission.Current.AddCustomMissile(
            Caster,
            missionWeapon,
            position,
            direction,
            orientation,
            speed,
            speed,
            false,
            null
        );
    }
}
