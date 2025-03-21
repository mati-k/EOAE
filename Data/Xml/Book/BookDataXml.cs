﻿using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Book;

[Serializable]
public class BookDataXml
{
    [XmlAttribute]
    public string ItemName = "";

    // Base hours needed to read the book (can be modified by other factors, like location, attributes, etc.)
    [XmlAttribute]
    public int ReadTime = 0;

    [XmlArray("ReadEffects")]
    [XmlArrayItem("UnlockSpell", typeof(UnlockSpellData))]
    [XmlArrayItem("IncreaseSkill", typeof(IncreaseSkillData))]
    public ReadEffectBase[] ReadEffects = Array.Empty<ReadEffectBase>();

    [XmlArray("ReadRequirements")]
    [XmlArrayItem("PerkRequirement", typeof(PerkRequirementData))]
    [XmlArrayItem("SkillRequirement", typeof(SkillRequirementData))]
    public RequirementBase[] ReadRequirements = Array.Empty<RequirementBase>();
}

[XmlInclude(typeof(UnlockSpellData))]
[XmlInclude(typeof(IncreaseSkillData))]
[Serializable]
public abstract class ReadEffectBase { }

[XmlInclude(typeof(PerkRequirementData))]
[XmlInclude(typeof(SkillRequirementData))]
[Serializable]
public abstract class RequirementBase { }
