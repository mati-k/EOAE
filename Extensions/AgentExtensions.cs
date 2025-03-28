using System;
using EOAE_Code.Character;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
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

        public static void DealDamage(
            this Agent agent,
            Agent attacker,
            float value,
            SkillObject? skill = null,
            bool shouldAddXp = false
        )
        {
            if (skill != null)
            {
                if (skill == CustomSkills.Instance.Destruction)
                {
                    value *= agent.GetMultiplierForSkill(
                        CustomSkills.Instance.Destruction,
                        CustomSkillEffects.Instance.DestructionDamage
                    );

                    int xpToGive;
                    Campaign.Current.Models.CombatXpModel.GetXpFromHit(
                        (CharacterObject)attacker.Character,
                        (CharacterObject?)attacker.Formation.Captain?.Character,
                        (CharacterObject)agent.Character,
                        null,
                        (int)value,
                        false,
                        CombatXpModel.MissionTypeEnum.Battle,
                        out xpToGive
                    );
                    attacker.AddSkillXp(CustomSkills.Instance.Destruction, xpToGive);
                }
            }

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

        public static void AddSkillXp(this Agent agent, SkillObject skill, float xp)
        {
            var character = agent.Character as CharacterObject;
            if (character != null && character.HeroObject != null)
            {
                character.HeroObject.AddSkillXp(skill, xp);
            }
        }
    }
}
