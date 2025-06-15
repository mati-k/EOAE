using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Book;

[Serializable]
public class UnlockSpellData : ReadEffectBase
{
    [XmlAttribute]
    public string Spell = "";
}
