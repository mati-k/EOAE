﻿using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class HealSelfSpellData : SpellData
    {
        [XmlAttribute]
        public float HealValue = 0;
    }
}
