using System.Collections.Generic;
using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;

namespace EOAE_Code.Data.Managers
{
    public class StatusEffectParticleManager : IDataManager<StatusEffectParticleData>
    {
        public static Dictionary<string, string> StatusEffectPrefabs = new();

        public void Add(StatusEffectParticleData item)
        {
            StatusEffectPrefabs.Add(item.StatusEffectKey, item.ParticlePrefab);
        }
    }
}
