﻿using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class TroopSpellData
    {
        [XmlAttribute]
        public string Spell = "";

        [XmlAttribute]
        public float Weight = 0;
    }
}
