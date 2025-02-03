using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class DamageOverTimeEffectData : StatusEffectBase
    {
        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties) { }

        public override void EffectTick(float totalValue, Agent target, Agent caster)
        {
            if (target.IsFadingOut())
            {
                return;
            }

            // ToDo: move damage logic into some simpler to use extension
            var blow = new Blow(caster.Index);
            blow.BaseMagnitude = totalValue;
            blow.InflictedDamage = (int)totalValue;

            var collisionData = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(
                false, false, false, false, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 1, 2, blow.BoneIndex, blow.VictimBodyPart, caster.Monster.MainHandBoneIndex, Agent.UsageDirection.AttackUp, -1, CombatHitResultFlags.NormalHit, 0.5f, 0, 0, 0, 0, 0, 0, 0, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, target.Velocity, Vec3.Zero);

            target.RegisterBlow(blow, collisionData);
        }
    }
}
