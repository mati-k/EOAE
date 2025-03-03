using System;
using EOAE_Code.Character;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class AgentMana
    {
        private const float BASE_MAX_MANA = 100;
        private const float BASE_MANA_REGEN = 5;

        private const float MAX_MANA_PER_LEVEL = 5;
        private const float MANA_REGEN_PER_LEVEL = 1;

        public float MaxMana { get; private set; } = BASE_MAX_MANA;
        public float CurrentMana { get; private set; } = BASE_MAX_MANA;
        public float ManaRegen { get; private set; } = BASE_MANA_REGEN;

        public AgentMana(Agent agent)
        {
            if (agent.IsHero)
            {
                Hero hero = ((CharacterObject)agent.Character).HeroObject;

                if (hero != null)
                {
                    int magic = hero.GetAttributeValue(Attributes.Instance.Magic);

                    MaxMana += MAX_MANA_PER_LEVEL * magic;
                    ManaRegen += MANA_REGEN_PER_LEVEL * magic;

                    CurrentMana = MaxMana;
                }
            }
        }

        public void ManaRegenTick(float dt)
        {
            CurrentMana = Math.Min(CurrentMana + ManaRegen * dt, MaxMana);
        }

        public void Consume(float amount)
        {
            CurrentMana = Math.Max(CurrentMana - amount, 0);
        }
    }
}
