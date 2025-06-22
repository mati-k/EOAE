using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Data.Xml.Enchantments
{
    [Serializable]
    public class EnchantmentData
    {
        [XmlAttribute]
        public string Name = "";

        [XmlAttribute]
        public string DisplayName = "";

        // ToDo: just temporary thing. Need to make some integrated way for status effects that would be for enchantments, spells, potion etc. with easy reuse and compatibility
        [XmlAttribute]
        public string DescriptionKey = "";

        // For now use preview from some items, in future maybe custom pictures
        [XmlAttribute]
        public string IconItem = "";

        [XmlAttribute]
        public bool UseSlider = false;

        [XmlArray("ItemTypes")]
        [XmlArrayItem("ItemType")]
        public List<ItemObject.ItemTypeEnum> ItemTypes = new();

        [XmlElement("StatusEffectTemplate")]
        public StatusEffectTemplate StatusEffectTemplate = new StatusEffectTemplate();

        public TextObject GetDescription(float value)
        {
            return new TextObject(DescriptionKey, null).SetTextVariable("value", value);
        }
    }
}
