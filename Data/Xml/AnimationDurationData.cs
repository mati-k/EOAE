using System;
using System.Xml.Serialization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class AnimationDurationData
    {
        [XmlAttribute]
        public string Animation = "";
        [XmlAttribute]
        public float Duration = 0;
        [XmlIgnore]
        public ActionIndexCache AnimationIndexCache = ActionIndexCache.act_none;
    }
}
