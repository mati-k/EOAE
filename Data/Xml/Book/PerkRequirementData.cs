using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Book;

[Serializable]
public class PerkRequirementData : RequirementBase
{
    [XmlAttribute]
    public string Perk = "";
}
