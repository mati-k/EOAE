using System.Collections.Generic;
using EOAE_Code.Consts;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace EOAE_Code.Enchanting
{
    public class EnchantmentSaving : SaveableTypeDefiner
    {
        public EnchantmentSaving()
            : base(ModuleConsts.SaveId) { }

        protected override void DefineClassTypes()
        {
            base.AddClassDefinition(typeof(EnchantedItem), 3);
        }

        protected override void DefineContainerDefinitions()
        {
            base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, EnchantedItem>));
        }
    }
}
