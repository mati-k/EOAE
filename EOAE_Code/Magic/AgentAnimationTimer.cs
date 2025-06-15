using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOAE_Code.Magic
{
    public class AgentAnimationTimer
    {
        public float DurationLeft { get; set; }
        public Action OnComplete { get; private set; }

        public AgentAnimationTimer(float durationLeft, Action onComplete)
        {
            DurationLeft = durationLeft;
            OnComplete = onComplete;
        }
    }
}
