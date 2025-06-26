using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml.Enchantments
{
    [Serializable]
    public class EnchantmentData
    {
        [XmlAttribute]
        public string Name { get; set; } = "";

        [XmlAttribute]
        public string DisplayName { get; set; } = "";

        // For now use preview from some items, in future maybe custom pictures
        [XmlAttribute]
        public string IconItem { get; set; } = "";

        [XmlAttribute]
        public bool UseSlider { get; set; } = false;

        [XmlArray("ItemTypes")]
        [XmlArrayItem("ItemType")]
        public List<ItemObject.ItemTypeEnum> ItemTypes { get; set; } = new();

        [XmlElement("StatusEffectTemplate")]
        public StatusEffectTemplate StatusEffectTemplate { get; set; } = new StatusEffectTemplate();

        public string GetDescription(float value)
        {
            StringBuilder descriptionBuilder = new();
            foreach (var action in StatusEffectTemplate.Effect.Actions)
            {
                descriptionBuilder.AppendLine(action.GetDescription(value));
            }

            return descriptionBuilder.ToString().TrimEnd();
        }
    }
}
