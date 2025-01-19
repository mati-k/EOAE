using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Agents;

public class SummonedAgentComponent : AgentComponent
{
    public readonly Agent Caster;
    private float lifespan;

    public SummonedAgentComponent(Agent agent, Agent caster, float lifespan)
        : base(agent)
    {
        Caster = caster;
        this.lifespan = lifespan;
    }

    public override void OnTickAsAI(float dt)
    {
        base.OnTickAsAI(dt);

        lifespan -= dt;
        if (lifespan <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        var blow = new Blow(Agent.Index);
        blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, 0);
        blow.DamageType = DamageTypes.Blunt;
        blow.BaseMagnitude = 10000f;
        blow.WeaponRecord.WeaponClass = WeaponClass.Undefined;
        blow.GlobalPosition = Agent.Position;
        blow.DamagedPercentage = 1f;
        Agent.Die(blow, Agent.KillInfo.Invalid);
    }
}
