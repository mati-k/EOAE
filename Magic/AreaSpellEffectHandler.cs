using EOAE_Code.Data.Xml;
using EOAE_Code.Extensions;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class AreaSpellEffectHandler : ScriptComponentBehavior
    {
        public Agent Caster { get; set; }
        public AreaEffectData Data { get; set; }
        public float Radius { get; set; }

        private const float TICK_INTERVAL = 1f;
        private float timer = TICK_INTERVAL;
        private float lifeTime = 0;

        protected override void OnTick(float dt)
        {
            base.OnTick(dt);

            lifeTime += dt;
            timer -= dt;

            if (timer <= 0)
            {
                timer = TICK_INTERVAL;
                DealDamage();
            }

            if (lifeTime >= Data.Duration)
            {
                GameEntity.Remove(80);
            }
        }

        private void DealDamage()
        {
            var agents = new MBList<Agent>();
            Mission.Current.GetNearbyAgents(GameEntity.GlobalPosition.AsVec2, Radius, agents);

            foreach (var agent in agents)
            {
                if (agent.IsActive())
                {
                    agent.DealDamage(Caster, Data.DamagePerSecond * TICK_INTERVAL);
                }
            }
        }
    }
}
