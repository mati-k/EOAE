using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EOAE_Code.Data.Xml.Enchantments
{
    [Serializable]
    public class EnchantmentData
    {
        [XmlAttribute]
        public string Name { get; set; } = "";

        [XmlAttribute]
        public string DisplayName { get; set; } = "";

        // ToDo: just temporary thing. Need to make some integrated way for status effects that would be for enchantments, spells, potion etc. with easy reuse and compatibility
        [XmlAttribute]
        public string DescriptionKey { get; set; } = "";

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

        public TextObject GetDescription(float value)
        {
            return new TextObject(DescriptionKey, null).SetTextVariable("value", value);
        }

        public void AddTooltips(ItemMenuVM itemMenuVM)
        {
            itemMenuVM.AddTooltip(
                "",
                new TextObject("{=aVhgwU2u}Enchantment:").ToString(),
                Color.ConvertStringToColor("#4470F2FF")
            );

            itemMenuVM.AddTooltip("", new TextObject(DisplayName).ToString(), Color.Black);

            if (!string.IsNullOrEmpty(DescriptionKey))
            {
                itemMenuVM.AddTooltip(
                    "",
                    new TextObject(DescriptionKey)
                        .SetTextVariable("value", StatusEffectTemplate.Scale)
                        .ToString(),
                    Color.Black
                );
            }
        }
    }
}
