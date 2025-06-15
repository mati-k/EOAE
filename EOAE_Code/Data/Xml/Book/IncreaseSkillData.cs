using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Book;

[Serializable]
public class IncreaseSkillData : ReadEffectBase
{
    [XmlAttribute]
    public string Skill = "";
}
