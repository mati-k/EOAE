using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Book;

[Serializable]
public class SkillRequirementData : RequirementBase
{
    [XmlAttribute]
    public string Skill = "";

    [XmlAttribute]
    public int Level = 0;
}
