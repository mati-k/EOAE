using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Managers
{
    public class AnimationDurationManager : IDataManager<AnimationDurationData>
    {
        public static Dictionary<string, AnimationDurationData> AnimationDurations = new();

        public void Add(AnimationDurationData item)
        {
            if (!string.IsNullOrEmpty(item.Animation))
            {
                item.AnimationIndexCache = ActionIndexCache.Create(item.Animation);
            }

            AnimationDurations.Add(item.Animation, item);
        }

        public static float GetDuration(string animation)
        {
            AnimationDurations.TryGetValue(animation, out var animationDuration);
            return animationDuration?.Duration ?? 0;
        }

        public static float GetDuration(Spell spell)
        {
            return GetDuration(spell.Animation);
        }

        public static ActionIndexCache GetCacheIndex(string animation)
        {
            AnimationDurations.TryGetValue(animation, out var animationDuration);
            return animationDuration?.AnimationIndexCache;
        }

        public static ActionIndexCache GetCacheIndex(Spell spell)
        {
            return GetCacheIndex(spell.Animation);
        }
    }
}
