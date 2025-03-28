using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Extensions
{
    public static class AgentExtensions
    {
        public static Hero? GetHero(this Agent agent)
        {
            var character = agent?.Character as CharacterObject;

            if (character == null)
            {
                return null;
            }

            return character.HeroObject;
        }

        public static void DealDamage(this Agent agent, Agent attacker, float value)
        {
            // ToDo: move damage logic into some simpler to use extension
            var blow = new Blow(attacker.Index);
            blow.BaseMagnitude = value;
            blow.InflictedDamage = (int)value;

            // todo: lookup over the arguments, extend into option parameters to this method
            var collisionData = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                CombatCollisionResult.StrikeAgent,
                -1,
                1,
                2,
                blow.BoneIndex,
                blow.VictimBodyPart,
                attacker.Monster.MainHandBoneIndex,
                Agent.UsageDirection.AttackUp,
                -1,
                CombatHitResultFlags.NormalHit,
                0.5f,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                Vec3.Up,
                blow.Direction,
                blow.GlobalPosition,
                Vec3.Zero,
                Vec3.Zero,
                agent.Velocity,
                Vec3.Zero
            );

            agent.RegisterBlow(blow, collisionData);
        }

        public static float GetMultiplierForSkill(
            this Agent agent,
            SkillObject skill,
            SkillEffect skillEffect
        )
        {
            ExplainedNumber explainedNumber = new(1f, false, null);
            var character = agent.Character as CharacterObject;

            if (character != null && skill != null)
            {
                int effectiveSkill = character.GetSkillValue(skill);
                SkillHelper.AddSkillBonusForCharacter(
                    skill,
                    skillEffect,
                    character,
                    ref explainedNumber,
                    effectiveSkill,
                    true,
                    0
                );
            }

            return Math.Max(0, explainedNumber.ResultNumber);
        }
    }
}
